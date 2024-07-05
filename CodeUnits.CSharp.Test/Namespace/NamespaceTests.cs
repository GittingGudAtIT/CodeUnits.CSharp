﻿namespace CodeUnits.CSharp.Test.Namespace
{
    [TestFixture]
    internal class NamespaceTests
    {
        [Test]
        public void ParsingEmptyString_DoesNotThrowAnyException()
        {
            Assert.DoesNotThrow(() => CodeUnit.FromString(""));
        }

        [Test]
        public void ACodeUnits_ParentNamespace_IsAlwaysNull()
        {
            var result = CodeUnit.FromString("");
            Assert.That(result.ContainingNamespace, Is.Null);
        }

        [Test]
        public void ParsingCode_WithOneNamespace_DoesReturnCodeUnitWithThisNamespace()
        {
            var nsName = "XY";
            var ns = $"namespace {nsName} {{ }}";

            var cu = CodeUnit.FromString(ns);
            var result = cu.Namespaces().First();

            Assert.Multiple(() =>
            {
                Assert.That(result.ContainingNamespace, Is.EqualTo(cu));
                Assert.That(result.Name, Is.EqualTo(nsName));
                TreeNodeAssert.LinksAreValid(result);
            });
        }

        [Test]
        public void ParsingCode_WithStaticUsingDirective_DoesReturnCodeUnitWithStaticUsingDirective()
        {
            var ns = "XY.AB";
            var myClass = "MyClass";
            var code = $"using static {ns}.{myClass};";

            var cu = CodeUnit.FromString(code);
            var result = cu.UsingDirectives
                .OfType<IUsingStaticDirective>()
                .First();

            Assert.Multiple(() =>
            {
                Assert.That(result.ContainingNamespace, Is.EqualTo(cu));
                Assert.That(result.Type.FormatedText, Is.EqualTo($"{ns}.{myClass}"));
                Assert.That(result.IsKind(UsingDirectiveKind.Static), Is.True);
                TreeNodeAssert.LinksAreValid(result);
            });
        }

        [Test]
        public void ParsingCode_WithUsingNamespaceDirective_DoesReturnCodeUnitWithUsingNamespaceDirective()
        {
            var ns = "XY.AB";
            var code = $"using {ns};";

            var cu = CodeUnit.FromString(code);
            var result = cu.UsingDirectives
                .OfType<IUsingNamespaceDirective>()
                .First();

            Assert.Multiple(() =>
            {
                Assert.That(result.ContainingNamespace, Is.EqualTo(cu));
                Assert.That(result.Namespace, Is.EqualTo(ns));
                Assert.That(result.IsKind(UsingDirectiveKind.Namespace), Is.True);
                TreeNodeAssert.LinksAreValid(result);
            });
        }

        [Test]
        public void ParsingCode_WithUsingAliasDirective_DoesReturnCodeUnitWithUsingAliasDirective()
        {
            var ns = "XY.AB";
            var type = "Type";
            var code = $"using {type} = {ns}.{type};";

            var cu = CodeUnit.FromString(code);
            var result = cu.UsingDirectives
                .OfType<IUsingAliasDirective>()
                .First();

            Assert.Multiple(() =>
            {
                Assert.That(result.ContainingNamespace, Is.EqualTo(cu));
                Assert.That(result.Type.FormatedText, Is.EqualTo($"{ns}.{type}"));
                Assert.That(result.Alias, Is.EqualTo(type));
                Assert.That(result.IsKind(UsingDirectiveKind.Alias), Is.True);
                TreeNodeAssert.LinksAreValid(result);
            });
        }

        [Test]
        public void ParsingCode_WithExternAliasDirective_DoesReturnCodeUnitWithExternAlias()
        {
            var aliasName = "XY";
            var code = $"extern alias {aliasName};";

            var cu = CodeUnit.FromString(code);
            var result = cu.ExternAliases[0];

            Assert.Multiple(() =>
            {
                Assert.That(result.ContainingNamespace, Is.EqualTo(cu));
                Assert.That(result.Name, Is.EqualTo(aliasName));
                TreeNodeAssert.LinksAreValid(result);
            });
        }
    }
}
