﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeUnits.CSharp.Implementation.Attributes;
using static CodeUnits.CSharp.Generated.CSharpParser;

namespace CodeUnits.CSharp.Implementation.Parameters
{
    public sealed class ParameterDefinition : IParameter
    {
        internal ParameterDefinition(TypeUsage type, string name, ParameterModifier modifier, IReadOnlyList<AttributeGroup> attributes, bool isParamsArray = false, CodeFragment defaultValue = null)
        {
            Type = type;
            Name = name;
            IsParamsArray = isParamsArray;
            DefaultValue = defaultValue;
            AttributeGroups = attributes;
            Attributes = attributes.SelectMany(a => a.Attributes).ToArray();
            IsOptional = defaultValue == null;
            Modifier = modifier;
        }

        public ITypeUsage Type { get; }

        public string Name { get; }

        public ParameterModifier Modifier { get; }

        public IReadOnlyList<IAttributeGroup> AttributeGroups { get; }

        public IReadOnlyList<IAttributeUsage> Attributes { get; }

        public ICodeFragment DefaultValue { get; }

        public bool IsParamsArray { get; }

        public bool IsOptional { get; }

        public static ParameterDefinition FromContext(Arg_declarationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return new ParameterDefinition(
                    type: new TypeUsage(context.type_()),
                    name: context.identifier().GetText(),
                    modifier: ParameterModifier.None,
                    attributes: Array.Empty<AttributeGroup>(),
                    isParamsArray: false,
                    defaultValue: null);
        }
    }
}
