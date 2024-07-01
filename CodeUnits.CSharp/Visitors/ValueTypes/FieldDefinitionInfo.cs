﻿using System.Collections.Generic;

namespace CodeUnits.CSharp.Visitors.ValueTypes
{
    internal readonly struct FieldDefinitionInfo
    {
        public FieldDefinitionInfo(CommonDefinitionInfo commonInfo, TypeUsage type)
        {
            Modifiers = Common.Modifiers.OfField(commonInfo.Modifiers);
            AttributeGroups = commonInfo.AttributeGroups;
            Type = type;
        }

        public (AccessModifier AccessModifier, bool IsReadonly, bool HasNewModifier, bool IsStatic) Modifiers { get; }

        public List<AttributeGroup> AttributeGroups { get; }

        public TypeUsage Type { get; }
    }
}
