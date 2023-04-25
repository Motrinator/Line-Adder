using LineSumator.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSumator.Tests
{
    [TestClass]
    public class EnumeratorHelperTest
    {
        [TestMethod]
        public void AreEqualsTest()
        {
            Assert.IsTrue(new[] { "1", "2", "3" }.AreEqual(new[] { "1", "2", "3" }));
        }

        [TestMethod]
        public void AreNotEqualsForActualTest()
        {
            Assert.IsFalse(new[] { "1", "2", "3", "4" }.AreEqual(new[] { "1", "2", "3" }));
        }

        [TestMethod]
        public void AreNotEqualsForExpectedTest()
        {
            Assert.IsFalse(new[] { "1", "2", "3" }.AreEqual(new[] { "1", "2", "3", "4" }));
        }

        [TestMethod]
        public void AreNotEqualsOrderForActualTest()
        {
            Assert.IsFalse(new[] { "2", "1", "3" }.AreEqual(new[] { "1", "2", "3" }));
        }

        [TestMethod]
        public void AreNotEqualsOrderForExpectedTest()
        {
            Assert.IsFalse(new[] { "1", "2", "3" }.AreEqual(new[] { "2", "1", "3" }));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ActualNullCheck()
        {
            IEnumerable<string>? strings = null;

            Assert.IsFalse(strings!.AreEqual(new[] { "1", "2", "3" }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpectedNullCheck()
        {
            Assert.IsFalse(new[] { "1", "2", "3" }.AreEqual(null));
        }
    }
}
