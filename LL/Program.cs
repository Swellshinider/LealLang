
using System.Collections.Immutable;
using LealLang.Core.Analyzer;
using LealLang.Core.Analyzer.Binding;
using LealLang.Core.Analyzer.Diagnostics;
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

			var syntaxTree = SyntaxTree.Parse(input);
			var compilation = new Compilation(syntaxTree);
			var result = compilation.Evaluate();

			if (!ValidateAndDisplayErrors(input, [.. result.Diagnostics]))
				Console.WriteLine(result.Value);
		}
	}

	private static bool ValidateAndDisplayErrors(string text, ImmutableArray<Diagnostic> diagnostics)
	{
		foreach (var diagnostic in diagnostics) 
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine(diagnostic);
			Console.ResetColor();
			
			var part1 = text[..diagnostic.Span.Start];
			var error = text.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
			var part2 = text[diagnostic.Span.End..];
			
			Console.Write("    " + part1);
			
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(error);
			Console.ResetColor();
			
			Console.WriteLine(part2);
		}
		
		return !diagnostics.IsEmpty;
	}
}