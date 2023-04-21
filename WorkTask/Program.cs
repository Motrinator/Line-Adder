using LineSumator;

var fileStrings = File.OpenText("D:\\Test.txt");

var result = LineSum.Calculate(fileStrings);
Console.WriteLine(result.LineWithMaxSum);
Console.WriteLine(string.Join(", ", result.LinesWithErrors));