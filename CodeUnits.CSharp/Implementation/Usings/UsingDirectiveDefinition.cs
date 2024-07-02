﻿namespace CodeUnits.CSharp.Implementation.Usings
{
    public abstract class UsingDirectiveDefinition : IUsingDirective
    {
        protected private UsingDirectiveDefinition(string value)
        {
            Value = value;
        }

        public INamespace ContainingNamespace { get; internal set; }

        public string Value { get; }

        public abstract UsingDirectiveKind Kind { get; }
    }
}
