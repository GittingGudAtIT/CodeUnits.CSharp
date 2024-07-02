﻿using CodeUnits.CSharp.Implementation.Attributes;
using CodeUnits.CSharp.Implementation.Common;
using CodeUnits.CSharp.Implementation.Members.Types.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using static CodeUnits.CSharp.Generated.CSharpParser;

namespace CodeUnits.CSharp.Implementation.Members.Types
{
    public sealed class ClassDefinition : StructuredTypeDefinition
    {
        private ClassDefinition(
            string name,
            AccessModifier accessModifier,
            bool hasNewModifier,
            IReadOnlyList<AttributeGroup> attributeGroups,
            IReadOnlyList<MemberDefinition> members,
            IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments,
            IReadOnlyList<ConstraintDefinition> constraints,
            bool isRecord,
            bool isStatic,
            bool isSealed,
            bool isAbstract)

            : base(
                  name: name,
                  accessModifier: accessModifier,
                  hasNewModifier: hasNewModifier,
                  attributeGroups: attributeGroups,
                  members: members,
                  genericTypeArguments: genericTypeArguments,
                  constraints: constraints)
        {
            IsStatic = isStatic;
            IsRecord = isRecord;
            IsSealed = isSealed;
            IsAbstract = isAbstract;
            Destructor = members.OfType<DestructorDefinition>()
                .SingleOrDefault();
        }

        public override TypeKind TypeKind { get; } = TypeKind.Class;

        public bool IsStatic { get; }

        public bool IsRecord { get; }

        public bool IsSealed { get; }

        public bool IsAbstract { get; }

        public DestructorDefinition Destructor { get; }

        internal static ClassDefinition FromContext(Class_definitionContext context, CommonDefinitionInfo commonInfo)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var modifiers = Modifiers.OfClass(commonInfo.Modifiers);
            var isRecord = context.RECORD() != null;
            var genericTypeArguments = GenericTypeArgumentDefinitions.FromContext(context.type_parameter_list());

            return new ClassDefinition(
                name: context.identifier().GetText(),
                accessModifier: modifiers.AccessModifier,
                hasNewModifier: modifiers.HasNewModifier,
                attributeGroups: commonInfo.AttributeGroups,
                members: MemberDefinitions.FromContext(context.class_body()),
                genericTypeArguments: genericTypeArguments,
                constraints: ConstraintDefinitions.FromContext(context.type_parameter_constraints_clauses(), genericTypeArguments),
                isRecord: isRecord,
                isStatic: modifiers.IsStatic,
                isSealed: modifiers.IsSealed,
                isAbstract: modifiers.IsAbstract);
        }
    }
}