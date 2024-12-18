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

			var parser = new Parser(input);
			var syntaxTree = parser.Parse();
			
			if (showTree)
				syntaxTree.WriteTo(Console.Out);
				
			var evaluator = new Evaluator(syntaxTree);
			var result = evaluator.Evaluate();
			Console.WriteLine(result);
		}	
	}
}