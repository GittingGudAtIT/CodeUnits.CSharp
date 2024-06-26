﻿using System.Collections.Generic;
namespace CodeUnits.CSharp
{
    public sealed class ConstraintDefinition
    {
        internal ConstraintDefinition(GenericTypeArgumentDefinition targetedTypeArgument, IReadOnlyList<ConstraintClause> clauses)
        {
            TargetedTypeArgument = targetedTypeArgument;
            Clauses = clauses;
        }

        public GenericTypeArgumentDefinition TargetedTypeArgument { get; }

        public IReadOnlyList<ConstraintClause> Clauses { get; }
    }
}
