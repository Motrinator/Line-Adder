using System.Text;

namespace LineSumator.Tests
{
    [TestClass]
    public class LineSumatorTest
    {
        [TestMethod]
        public void CheckAllValidValues()
        {
            using var stream = GetStreamFromString(
                "0.1,0.2,0.3\n" +
                "1.4,1.5,1.6\n" +
                "2.7,2.8,2.8"
            );

            var result = LineSum.Calculate(stream);

            Assert.AreEqual(result.LineWithMaxSum, 3);
            Assert.AreEqual(result.LinesWithErrors.Count, 0);
        }

        [TestMethod]
        public void CheckTrim()
        {
            using var stream = GetStreamFromString(
                 " 0.1 ,\t0.2 ,\t\t0.3\n" +
                 "1.4, 1.5  ,1.6\n" +
                 "\t  2.7 ,2.8, 2.8   "
             );

            var result = LineSum.Calculate(stream);

            Assert.AreEqual(result.LineWithMaxSum, 3);
            Assert.AreEqual(result.LinesWithErrors.Count, 0);
        }

        [TestMethod]
        public void CheckOverflow()
        {
            using var stream = GetStreamFromString(
                "0.1,0.2,99999999999999999999999999999\n" +
                "1.4,1.5,1.6\n" +
                "2.7," + (decimal.MaxValue - 1)
            );

            var result = LineSum.Calculate(stream);

            Assert.AreEqual(result.LineWithMaxSum, 2);
            Assert.AreEqual(result.LinesWithErrors.Count, 2);
            Assert.AreEqual(result.LinesWithErrors[0], 1);
            Assert.AreEqual(result.LinesWithErrors[1], 3);
        }

        [TestMethod]
        public void Test()
        {
            using var stream = GetStreamFromString(
                "0.1,0.2,1212,\n" +
                "1.4,1.5,1.6\n" +
                "-"
            );

            var result = LineSum.Calculate(stream);

            Assert.AreEqual(result.LineWithMaxSum, 1);
            Assert.AreEqual(result.LinesWithErrors.Count, 1);
        }

        private static StreamReader GetStreamFromString(string text)
        {
            return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(text)));
        }
    }
}