﻿
namespace CodeUnits.CSharp
{
    public enum ConstraintKind 
    { 
        Constructor, 
        OfType,
        Class,
        ClassNullable,
        Struct, 
        Unmanaged,
        NotNull, 
        Default 
    }

    public sealed class ConstraintClause
    {
        internal ConstraintClause(ConstraintKind kind, TypeUsage value = null)
        {
            Kind = kind;
            Value = value;
        }

        public ConstraintKind Kind { get; }

        public TypeUsage Value { get; }

        public bool IsKind(ConstraintKind kind) => Kind == kind;
    }
}
