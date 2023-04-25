using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace LineSumator.Tests
{
    [TestClass]
    public class LineSumatorTest
    {
        [TestMethod]
        public void CheckAllValidValues()
        {
            var result = GetResultFromString(
                "0.1,0.2,0.3\n" +
                "1.4,1.5,1.6\n" +
                "2.7,2.8,2.8"
            );

            Assert.AreEqual(result.LineWithMaxSum, 3);
            Assert.AreEqual(result.LinesWithErrors.Count, 0);
        }

        [TestMethod]
        public void CheckTrim()
        {
            var result = GetResultFromString(
                 " 0.1 ,\t0.2 ,\t\t0.3\n" +
                 "1.4, 1.5  ,1.6\n" +
                 "\t  2.7 ,2.8, 2.8   "
             );

            Assert.AreEqual(result.LineWithMaxSum, 3);
            Assert.AreEqual(result.LinesWithErrors.Count, 0);
        }

        [TestMethod]
        public void CheckOverflowInNumber()
        {
            var result = GetResultFromString(
                "0.1,0.2,99999999999999999999999999999\n" +
                "1.4,1.5,1.6\n" +
                "2.7,2.8,2.9"
            );

            Assert.AreEqual(result.LineWithMaxSum, 3);
            Assert.AreEqual(result.LinesWithErrors.Count, 1);
            Assert.AreEqual(result.LinesWithErrors[0], 1);
        }

        [TestMethod]
        public void CheckOverflowInSum()
        {
            var result = GetResultFromString(
                "0.1,0.2,0.3\n" +
                "1.4,1.5,1.6\n" +
                "2.7,2.8,2.9," + (decimal.MaxValue)
            );

            Assert.AreEqual(result.LineWithMaxSum, 2);
            Assert.AreEqual(result.LinesWithErrors.Count, 1);
            Assert.AreEqual(result.LinesWithErrors[0], 3);
        }

        [TestMethod]
        public void CheckOnlySignInNumber()
        {
            var result = GetResultFromString(
                "0.1,0.2,1212\n" +
                "1.4,1.5,1.6\n" +
                "-\n" +
                "-, 1212, 2121\n" +
                "1212, -, 1221\n" +
                "1212, 2121, -\n" +
                "1212, 2121, +121"
            );

            Assert.AreEqual(result.LineWithMaxSum, 1);
            CollectionAssert.AreEquivalent(result.LinesWithErrors, new[] { 3, 4, 5, 6, 7 });
        }

        [TestMethod]
        public void CheckEmptyLines()
        {
            var result = GetResultFromString(
                "0.1,0.2,1214,\n" +
                "1.4,1.5,1.6\n" +
                "\n" +
                "  \t"
            );

            Assert.AreEqual(result.LineWithMaxSum, 2);
            CollectionAssert.AreEquivalent(result.LinesWithErrors, new[] { 1, 3, 4 });
        }

        [TestMethod]
        public void OmitZeros()
        {
            var result = GetResultFromString(
                "0.1,0.2,1214\n" +
                ".4,1.5,1.6\n" +
                "2.4,-.5,1.6\n" +
                "2.4,5,.6\n" +
                "2.4,1,12."
            );

            Assert.AreEqual(result.LineWithMaxSum, 1);
            CollectionAssert.AreEquivalent(result.LinesWithErrors, new[] { 2, 3, 4, 5 });
        }

        [TestMethod]
        public void AllIncorrectValues()
        {
            var result = GetResultFromString(
                "\n" +
                ".6\n" +
                "-.5,1.6\n" +
                "2.4,5,.6\n" +
                "2.4,1,12."
            );

            Assert.AreEqual(result.LineWithMaxSum, null);
            CollectionAssert.AreEquivalent(result.LinesWithErrors, new[] { 1, 2, 3, 4, 5 });
        }

        [TestMethod]
        public void TwoMaxResults()
        {
            var result = GetResultFromString(
                "1, 2, 3, 4, 5\n" +
                "-1, -2, -3, -4, -5\n" +
                "1, 2, 3, 4, 5\n" +
                "-1, -2, -3, -4, -5"
            );

            Assert.AreEqual(result.LineWithMaxSum, 3);
            Assert.AreEqual(result.LinesWithErrors.Count, 0);
        }

        private static LineSumCalculateResult GetResultFromString(string text)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(text));
            using var stream = new StreamReader(memoryStream);

            var result = LineSum.Calculate(stream);

            return result;
        }
    }
}