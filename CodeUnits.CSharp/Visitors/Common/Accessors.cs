﻿using CodeUnits.CSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using static CodeUnits.CSharp.Generated.CSharpParser;

namespace CodeUnits.CSharp.Visitors.Common
{
    internal static class Accessors
    {
        public static (AccessorDefinition Getter, AccessorDefinition Setter) FromContext(Indexer_declarationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.throwable_expression() != null)
                return (GetterFromArrowFunction(context.throwable_expression()), null);
            return AccessorsFromAccessorsDeclaration(context.accessor_declarations());
        }

        public static (AccessorDefinition Getter, AccessorDefinition Setter) FromContext(Property_declarationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.throwable_expression() != null)
                return (GetterFromArrowFunction(context.throwable_expression()), null);
            return AccessorsFromAccessorsDeclaration(context.accessor_declarations());
        }

        public static (AccessorDefinition Add, AccessorDefinition Remove) FromContext(Event_accessor_declarationsContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var attributes = AttributeGroups.FromContext(context.attributes());
            if(context.remove_accessor_declaration() != null)
            {
                var add = new AccessorDefinition(
                    name: "add",
                    accessModifier: AccessModifier.None,
                    attributeGroups: attributes,
                    body: GetBody(context.block()),
                    kind: AccessorKind.Add);

                return (add, RemoveFromAccessorDeclaration(context.remove_accessor_declaration()));
            }
            else
            {
                var remove = new AccessorDefinition(
                    name: "remove",
                    accessModifier: AccessModifier.None,
                    attributeGroups: attributes,
                    body: GetBody(context.block()),
                    kind: AccessorKind.Remove);

                return (AddFromAccessorDeclaration(context.add_accessor_declaration()), remove);
            }
        }

        private static AccessorDefinition RemoveFromAccessorDeclaration(Remove_accessor_declarationContext context)
        {
            return FromAnyEventAccessor(context.attributes(), context.block(), AccessorKind.Remove);
        }

        private static AccessorDefinition AddFromAccessorDeclaration(Add_accessor_declarationContext context)
        {
            return FromAnyEventAccessor(context.attributes(), context.block(), AccessorKind.Add);
        }

        private static AccessorDefinition FromAnyEventAccessor(AttributesContext attributesContext, BlockContext blockContext, AccessorKind accessorKind)
        {
            var name = accessorKind == AccessorKind.Add
                ? "add"
                : "remove";
            var attributes = AttributeGroups.FromContext(attributesContext);
            var body = GetBody(blockContext);

            return new AccessorDefinition(
                name: name,
                accessModifier: AccessModifier.None,
                attributeGroups: attributes,
                body: body,
                kind: accessorKind);
        }

        private static AccessorDefinition GetterFromArrowFunction(Throwable_expressionContext context)
        {
            var arrow = new TerminalSymbol(TerminalSymbolKind.RightArrow, "=>");
            var semiColon = new TerminalSymbol(TerminalSymbolKind.Semicolon, ";");
            var tokens = Symbols.FromNode(context)
                .Prepend(arrow)
                .Append(semiColon)
                .ToArray();

            return new AccessorDefinition(
                name:            "get",
                accessModifier:  AccessModifier.None,
                attributeGroups: Array.Empty<AttributeGroup>(),
                body:            new Code(tokens),
                kind:            AccessorKind.Getter);
        }

        private static (AccessorDefinition Getter, AccessorDefinition Setter) AccessorsFromAccessorsDeclaration(Accessor_declarationsContext context)
        {
            AccessorDefinition getter;
            AccessorDefinition setter;

            var attributes = AttributeGroups.FromContext(context.attrs);
            var modifier = GetAccessModifier(context.mods);
            var body = GetBody(context.accessor_body()?.block());

            if(context.GET() != null)
            {
                getter = new AccessorDefinition(
                    name:            "get",
                    accessModifier:  modifier,
                    attributeGroups: attributes,
                    body:            body,
                    kind:            AccessorKind.Getter);
                setter = SetterFromSetAccessorContext(context.set_accessor_declaration());
            }
            else
            {
                setter = new AccessorDefinition(
                    name:            "set",
                    accessModifier:  modifier,
                    attributeGroups: attributes,
                    body:            body,
                    kind:            AccessorKind.Setter);
                getter = GetterFromGetAccessorContext(context.get_accessor_declaration());
            }
            return (getter, setter);
        }

        private static AccessorDefinition SetterFromSetAccessorContext(Set_accessor_declarationContext context)
        {
            if(context == null)
                return null;

            return new AccessorDefinition(
                name:            "set",
                accessModifier:  GetAccessModifier(context.accessor_modifier()),
                attributeGroups: AttributeGroups.FromContext(context.attributes()),
                body:            GetBody(context.accessor_body()?.block()),
                kind:            AccessorKind.Setter);
        }

        private static AccessorDefinition GetterFromGetAccessorContext(Get_accessor_declarationContext context)
        {
            if (context == null)
                return null;

            return new AccessorDefinition(
                name:            "get",
                accessModifier:  GetAccessModifier(context.accessor_modifier()),
                attributeGroups: AttributeGroups.FromContext(context.attributes()),
                body:            GetBody(context.accessor_body()?.block()),
                kind:            AccessorKind.Getter);
        }

        private static Code GetBody(BlockContext context)
        {
            return context == null
                ? null
                : new Code(context);
        }

        private static AccessModifier GetAccessModifier(Accessor_modifierContext context)
        {
            AccessModifier modifier = AccessModifier.None;
            if (context != null)
            {
                var modifiers = new List<string>();
                for (int i = 0; i < context.ChildCount; i++)
                    modifiers.Add(context.GetChild(i).GetText());
                modifier = Modifiers.Accessibility(modifiers);
            }
            return modifier;
        }
    }
}
