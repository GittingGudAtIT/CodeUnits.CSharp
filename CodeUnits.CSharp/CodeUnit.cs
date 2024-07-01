﻿using Antlr4.Runtime;
using CodeUnits.CSharp.Generated;
using CodeUnits.CSharp.Listeners;
using CodeUnits.CSharp.Visitors;
using System;
using System.Collections.Generic;
using System.IO;

namespace CodeUnits.CSharp
{
    public sealed class CodeUnit: NamespaceDefinition
    {
        private CodeUnit(string projectDefaultNamespace,
            IReadOnlyList<UsingDirectiveDefinition> directives, IReadOnlyList<NamespaceDefinition> namespaces,
            IReadOnlyList<TypeDefinition> types, IReadOnlyList<ExternAliasDefinition> externAliases)

            : base(name:         projectDefaultNamespace,
                  directives:    directives, 
                  namespaces:    namespaces, 
                  types:         types, 
                  externAliases: externAliases)
        { }

        public static CodeUnit FromFile(string fileName, string projectDefaultNamespace)
        {
            return FromStream(new AntlrInputStream(File.OpenRead(fileName)), projectDefaultNamespace);
        }

        public static CodeUnit FromText(string text, string projectDefaultNamespace)
        {
            return FromStream(new AntlrInputStream(text), projectDefaultNamespace);
        }

        private static CodeUnit FromStream(ICharStream stream, string projectDefaultNamespace)
        {
            var lexer = new CSharpLexer(stream);
            var parser = new CSharpParser(new CommonTokenStream(lexer));

            var listener = new ThrowExceptionListener();
            parser.AddErrorListener(listener);

            var visitor = new NamespaceVisitor();
            var ns = visitor.VisitCompilation_unit(parser.compilation_unit())
                ?? throw new InvalidOperationException("something went wrong during parsing");

            return new CodeUnit(projectDefaultNamespace, 
                ns.UsingDirectives, ns.Namespaces, ns.Types, ns.ExternAliases);
        }
    }
}
