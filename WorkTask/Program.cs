using LineSumator;

string path;

if (args.Length > 0)
{
    path = args[0];
    Console.WriteLine("Path loaded from args: {0}", path);
}
else
{
    Console.Write("Enter path to file: ");
    path = Console.ReadLine() ?? throw new InvalidOperationException();
}

if (!File.Exists(path))
{
    Console.WriteLine("File does not exist.");

    return;
}

if (Path.GetExtension(path) != ".txt")
{
    Console.WriteLine("Path must have \"txt\" format.");

    return;
}

using var streamReader = new StreamReader(path);
var result = LineSum.Calculate(streamReader);

Console.WriteLine("Line number with maximum sum: " + result.LineWithMaxSum ?? "Line with maximum sum does not found.");
Console.WriteLine("Lines with error: {0}", string.Join(", ", result.LinesWithErrors));
