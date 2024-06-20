﻿using Antlr4.Runtime;
using CodeUnits.CSharp.Generated;

namespace CodeUnits.CSharp.Base
{
    public abstract class CSharpParserBase : Parser
    {
        public CSharpParserBase(ITokenStream input)
            : base(input)
        { }

        protected bool IsLocalVariableDeclaration()
        {
            if (!(Context is CSharpParser.Local_variable_declarationContext localVarDecl))
                return true;
            var localVariableType = localVarDecl.local_variable_type();
            if (localVariableType == null)
                return true;
            if (localVariableType.GetText() == "var")
                return false;
            return true;
        }
    }
}