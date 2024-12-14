using LealLang.Core.Analyzer.Syntax;

namespace LL;

public static class Program
{
	public static void Main(string[] args)
	{
		Console.Title = "LealLang: Interactive Console";
		while (true)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("> ");
			var input = Console.ReadLine() ?? string.Empty;
			Console.ResetColor();

			if (input == "#exit")
				break;
				
			if (input == "#cls")
			{
				Console.Clear();
				continue;
			}

			var parser = new Parser(input);
			var syntaxTree = parser.Parse();
			syntaxTree.WriteTo(Console.Out);
		}	
	}
}