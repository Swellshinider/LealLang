namespace LL;

public static class Program
{
	public static void Main(string[] args)
	{
		if (args.Length != 1)
		{
			Console.WriteLine("Usage: LL <input-file>");
			return;
		}
		
		var lines = ReadFile(args[0]).ToList();
		var fileContent = string.Join("", lines);
		Console.WriteLine(fileContent);
	}
	
	private static IEnumerable<string> ReadFile(string fileName) 
	{
		var path = Path.Combine(Environment.CurrentDirectory, fileName);
		
		if (!File.Exists(path))
			throw new FileNotFoundException($"File {fileName} not found in the current directory");
		
		return File.ReadLines(path);
	}
}