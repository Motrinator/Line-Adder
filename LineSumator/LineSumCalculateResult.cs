namespace LineSumator
{
    public readonly struct LineSumCalculateResult
    {
        public readonly int? LineWithMaxSum;
        public readonly IReadOnlyList<int> LinesWithErrors;

        public LineSumCalculateResult(int? lineWithMaxSum, IReadOnlyList<int> linesWithErrors)
        {
            LineWithMaxSum = lineWithMaxSum;
            LinesWithErrors = linesWithErrors;
        }
    }
}