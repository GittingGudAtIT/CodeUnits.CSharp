﻿using System.Collections.Generic;

namespace CodeUnits.CSharp
{
    public sealed class StructDefinition: StructuredTypeDefinition
    {
        internal StructDefinition(
            string name,
            AccessModifier accessModifier,
            bool hasNewModifier,
            IReadOnlyList<AttributeGroup> attributeGroups,
            IReadOnlyList<MemberDefinition> members,
            IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments,
            IReadOnlyList<ConstraintDefinition> constraints,
            bool isRecord, bool isReadonly)

            : base(name, accessModifier, hasNewModifier, attributeGroups, 
                  members, genericTypeArguments, constraints)
        {
            IsRecord = isRecord;
            IsReadonly = isReadonly;
        }

        public override TypeKind TypeKind { get; } = TypeKind.Struct;

        public bool IsReadonly { get; }

        public bool IsRecord { get; }
    }
}
