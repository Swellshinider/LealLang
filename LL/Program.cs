
using System.Collections.Immutable;
using LealLang.Core.Analyzer;
using LealLang.Core.Analyzer.Binding;
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

			var parser = new Parser(input, []);
			var syntaxTree = parser.Parse();

			if (CheckAndPrintError([.. parser.Diagnostics]))
				continue;

			if (showTree)
				syntaxTree.WriteTo(Console.Out);

			var binder = new Binder();
			var boundExpression = binder.BindExpression(syntaxTree);

			if (CheckAndPrintError([.. binder.Diagnostics]))
				continue;
				
			var evaluator = new Evaluator(boundExpression!);
			var result = evaluator.Evaluate();
			Console.WriteLine(result);
		}
	}

	private static bool CheckAndPrintError(ImmutableArray<string> diagnostics)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		if (!diagnostics.IsEmpty)
			diagnostics.ToList().ForEach(Console.WriteLine);

		Console.ResetColor();
		return !diagnostics.IsEmpty;
	}
}