using Lansky.SharpMutator.ExampleSut;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lansky.SharpMutator.ExampleTest
{
    [TestClass]
    public class SomeClassTest
    {
        [TestMethod]
        public void MaxReturnsBiggerFirstArgument()
        {
            var maxResult = SomeClass.Max(5, 2);
            Assert.AreEqual(5, maxResult);
        }

        [TestMethod]
        public void MaxReturnsBiggerSecondArgument()
        {
            var maxResult = SomeClass.Max(2, 5);
            Assert.AreEqual(5, maxResult);
        }

        [TestMethod]
        public void MaxReturnsFromSameArguments()
        {
            var maxResult = SomeClass.Max(6, 6);
            Assert.AreEqual(6, maxResult);
        }

        [TestMethod]
        public void SumSums()
        {
            var sum = SomeClass.Adder(2, 3);
            Assert.AreEqual(5, sum);
        }
    }
}
