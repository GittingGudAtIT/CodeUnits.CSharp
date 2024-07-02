﻿using CodeUnits.CSharp.Implementation.Attributes;
using CodeUnits.CSharp.Implementation.Common;
using CodeUnits.CSharp.Implementation.Members.Minor;
using System;
using System.Collections.Generic;
using static CodeUnits.CSharp.Generated.CSharpParser;

namespace CodeUnits.CSharp.Implementation.Members.Types
{
    public sealed class EnumDefinition : TypeDefinition, IEnum
    {
        internal EnumDefinition(
            string name,
            AccessModifier accessModifier,
            bool hasNewModifier,
            IReadOnlyList<AttributeGroup> attributeGroups,
            IReadOnlyList<EnumMemberDefinition> members,
            TypeUsage baseType)

            : base(
                  name: name,
                  accessModifier: accessModifier,
                  hasNewModifier: hasNewModifier,
                  attributeGroups: attributeGroups)
        {
            foreach (var member in members)
                member.ContainingType = this;

            Members = members;
            BaseType = baseType;
        }

        public IReadOnlyList<EnumMemberDefinition> Members { get; }

        public override TypeKind TypeKind { get; } = TypeKind.Enum;

        public TypeUsage BaseType { get; }

        internal static EnumDefinition FromContext(Enum_definitionContext context, CommonDefinitionInfo commonInfo)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var modifiers = Modifiers.OfEnum(commonInfo.Modifiers);
            var baseType = context.enum_base()?.type_() is null
                ? null
                : new TypeUsage(context.enum_base().type_());

            return new EnumDefinition(
                name: context.identifier().GetText(),
                accessModifier: modifiers.AccessModifier,
                hasNewModifier: modifiers.HasNewModifier,
                attributeGroups: commonInfo.AttributeGroups,
                members: MemberDefinitions.FromContext(context.enum_body()),
                baseType: baseType);
        }
    }
}