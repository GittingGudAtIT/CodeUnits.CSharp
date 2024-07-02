﻿namespace CodeUnits.CSharp
{
    public interface IField : IMember
    {
        ITypeUsage Type { get; }

        bool IsStatic { get; }

        bool IsReadonly { get; }

        bool IsNew { get; }

        ICodeFragment DefaultValue { get; }

        bool HasNewModifier { get; }
    }
}
