﻿namespace CompileUnits.CSharp.Test.Members.Types
{
    internal static class TypeArgument
    {
        public static void Test<T>(string code, string expectedName, Variance expectedVariance, Func<T, IGenericTypeParameter> getParameter)
        {
            var cu = CompileUnit.FromString(code);
            var del = cu.Types().OfType<T>().Single();
            var result = getParameter(del);

            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo(expectedName));
                Assert.That(result.Variance, Is.EqualTo(expectedVariance));
                TreeNodeAssert.LinksAreValid(result);
            });
        }
    }
}

