﻿
using System.Collections.Generic;
using CodeUnits.CSharp.Implementation.Attributes;
using static CodeUnits.CSharp.Generated.CSharpParser;

namespace CodeUnits.CSharp.Implementation.Members.Types.Generics
{
    internal static class GenericTypeArgumentDefinitions
    {
        public static List<GenericTypeArgumentDefinition> FromContext(Variant_type_parameter_listContext context)
        {
            var args = new List<GenericTypeArgumentDefinition>();
            if (context is null)
                return args;

            foreach (var argContext in context.variant_type_parameter())
            {
                args.Add(new GenericTypeArgumentDefinition(
                    argContext.identifier().GetText(),
                    GetVariance(argContext.variance_annotation()),
                    AttributeGroups.FromContext(argContext.attributes())));
            }
            return args;
        }

        public static List<GenericTypeArgumentDefinition> FromContext(Type_parameter_listContext context)
        {
            var args = new List<GenericTypeArgumentDefinition>();
            if (context is null)
                return args;

            foreach (var argContext in context.type_parameter())
            {
                args.Add(new GenericTypeArgumentDefinition(
                    argContext.identifier().GetText(),
                    Variance.None,
                    AttributeGroups.FromContext(argContext.attributes())));
            }
            return args;
        }

        private static Variance GetVariance(Variance_annotationContext context)
        {
            var s = context?.GetText();
            if (s == "in")
                return Variance.In;
            if (s == "out")
                return Variance.Out;
            return Variance.None;
        }
    }
}
