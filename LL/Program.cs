using LealLang.Core.Analyzer.Syntax;

namespace LL;

public static class Program
{
	public static void Main(string[] args)
	{
		Console.Title = "LealLang: Interactive Console";
		var showTree = false;
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

			if (input == "#showTree")
			{
				showTree = !showTree;
				Console.WriteLine($"{(showTree ? "showing" : "hiding")} parsed trees");
				continue;
			}

			var diagnostics = new List<string>();
			var parser = new Parser(input, diagnostics);
			diagnostics = parser.Diagnostics;
			var syntaxTree = parser.Parse();

			if (showTree)
				syntaxTree.WriteTo(Console.Out);

			if (diagnostics.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				diagnostics.ForEach(Console.WriteLine);
				Console.ResetColor();
				continue;
			}

			var evaluator = new Evaluator(syntaxTree);
			var result = evaluator.Evaluate();
			Console.WriteLine(result);
		}
	}
}