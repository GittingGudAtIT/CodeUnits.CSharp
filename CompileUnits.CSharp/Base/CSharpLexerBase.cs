﻿using Antlr4.Runtime;
using CompileUnits.CSharp.Generated;
using System.Collections.Generic;

namespace CompileUnits.CSharp.Base
{
    public abstract class CSharpLexerBase : Lexer
    {
        protected CSharpLexerBase(ICharStream input)
            : base(input) 
        { }

        protected int interpolatedStringLevel;
        protected Stack<bool> interpolatedVerbatiums = new Stack<bool>();
        protected Stack<int> curlyLevels = new Stack<int>();
        protected bool verbatium;

        protected void OnInterpolatedRegularStringStart()
        {
            interpolatedStringLevel++;
            interpolatedVerbatiums.Push(false);
            verbatium = false;
        }

        protected void OnInterpolatedVerbatiumStringStart()
        {
            interpolatedStringLevel++;
            interpolatedVerbatiums.Push(true);
            verbatium = true;
        }

        protected void OnOpenBrace()
        {
            if (interpolatedStringLevel > 0)
            {
                curlyLevels.Push(curlyLevels.Pop() + 1);
            }
        }

        protected void OnCloseBrace()
        {
            if (interpolatedStringLevel > 0)
            {
                curlyLevels.Push(curlyLevels.Pop() - 1);
                if (curlyLevels.Peek() == 0)
                {
                    curlyLevels.Pop();
                    Skip();
                    PopMode();
                }
            }
        }

        protected void OnColon()
        {
            if (interpolatedStringLevel > 0 && _mode != CSharpLexer.INTERPOLATION_FORMAT)
            {
                Mode(CSharpLexer.INTERPOLATION_FORMAT);
            }
        }

        protected void OpenBraceInside()
        {
            curlyLevels.Push(1);
        }

        protected void OnDoubleQuoteInside()
        {
            interpolatedStringLevel--;
            interpolatedVerbatiums.Pop();
            verbatium = interpolatedVerbatiums.Count > 0 && interpolatedVerbatiums.Peek();
        }

        protected void OnCloseBraceInside()
        {
            curlyLevels.Pop();
        }

        protected bool IsRegularCharInside()
        {
            return !verbatium;
        }

        protected bool IsVerbatiumDoubleQuoteInside()
        {
            return verbatium;
        }
    }
}