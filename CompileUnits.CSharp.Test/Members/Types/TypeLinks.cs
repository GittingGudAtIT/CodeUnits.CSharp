﻿namespace CompileUnits.CSharp.Test.Members.Types
{
    internal class TypeLinks
    {
        public static void Test<T>(string code, TypeKind typeKind) where T : IType
        {
            var cu = CompileUnit.FromString(code);
            var result = cu.Types().OfType<T>().Single();

            Assert.Multiple(() =>
            {
                Assert.That(result.ContainingNamespace, Is.EqualTo(cu));
                Assert.That(result.ContainingType, Is.Null);
                Assert.That(result.TypeKind, Is.EqualTo(typeKind));
                TreeNodeAssert.LinksAreValid(result);
            });
        }
    }
}
