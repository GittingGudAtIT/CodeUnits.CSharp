﻿namespace CodeUnits.CSharp.Test.Types.Interface
{
    internal class InterfaceTests
    {
        [Test]
        public void InterfaceIsCorrectlyLinkedAtNamespace()
        {
            var code = "interface IIface {}";
            TypeLinks.Test<IInterface>(code, TypeKind.Interface);
        }

        [Test]
        [TestCaseSource(typeof(InterfaceResources), nameof(InterfaceResources.ModifierConfigs))]
        public void ModifierTest(string code, string propertyName, object expectedValue)
        {
            Modifier.Test<IInterface>(code, propertyName, expectedValue);
        }

        [Test]
        [TestCaseSource(typeof(InterfaceResources), nameof(InterfaceResources.GenericTypeParameters))]
        public void TypeArgumentTest(string code, string expectedName, Variance expectedVariance)
        {
            TypeArgument.Test<IInterface>(code, expectedName, expectedVariance, t => t.GenericTypeArguments[0]);
        }

        [Test]
        [TestCaseSource(typeof(InterfaceResources), nameof(InterfaceResources.Inheritance))]
        public void InheritanceTest(string code, string[] expectedTypes)
        {
            var result = CodeUnit.FromString(code)
                .Types().OfType<IInterface>()
                .Single().Inheritance;

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(expectedTypes.Length));
                for (var i = 0; i < result.Count; i++)
                {
                    Assert.That(result[i].FormatedText, Is.EqualTo(expectedTypes[i]));
                    TreeNodeAssert.LinksAreValid(result[i]);
                }
            });
        }
    }
}
