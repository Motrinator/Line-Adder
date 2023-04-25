using System.Collections.ObjectModel;

namespace LineSumator
{
    public readonly ref struct LineSumCalculateResult
    {
        public readonly int? LineWithMaxSum;
        public readonly ReadOnlyCollection<int> LinesWithErrors;

        public LineSumCalculateResult(int? lineWithMaxSum, ReadOnlyCollection<int> linesWithErrors)
        {
            LineWithMaxSum = lineWithMaxSum;
            LinesWithErrors = linesWithErrors;
        }
    }
}