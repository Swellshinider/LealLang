
using System.Collections.Immutable;
using LealLang.Core.Analyzer;
using LealLang.Core.Analyzer.Binding;
using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Text;

namespace LL;

public static class Program
{
	public static void Main(string[] args)
	{
		var showTree = false;
		var variables = new Dictionary<VariableSymbol, object?>();

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

			if (input == "#showVar")
			{
				Console.WriteLine();
				foreach (var v in variables)
					Console.WriteLine($"{v.Key}->{v.Value}");
				continue;
			}

			var syntaxTree = SyntaxTree.Parse(input);

			if (showTree)
				syntaxTree.RootExpression.WriteTo(Console.Out);

			var compilation = new Compilation(syntaxTree);
			var result = compilation.Evaluate(variables);

			if (!ValidateAndDisplayErrors(syntaxTree.SourceText, [.. result.Diagnostics]))
				Console.WriteLine(result.Value);
		}
	}

	private static bool ValidateAndDisplayErrors(SourceText sourceText, ImmutableArray<Diagnostic> diagnostics)
	{
		foreach (var diagnostic in diagnostics)
		{
			var lineIndex = sourceText.GetLineIndex(diagnostic.Span.Start);
			var line = sourceText.Lines[lineIndex];
			var lineNumber = lineIndex + 1;
			var character = diagnostic.Span.Start - line.Start;

			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write($"({lineNumber}, {character}): ");
			Console.WriteLine(diagnostic);
			Console.ResetColor();

			var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
			var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);
			var prefix = sourceText.ToString(prefixSpan);
			var error = sourceText.ToString(diagnostic.Span);
			var suffix = sourceText.ToString(suffixSpan);

			Console.Write("    ");
			Console.Write(prefix);

			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(error);
			Console.ResetColor();

			Console.WriteLine(suffix);
		}

		return !diagnostics.IsEmpty;
	}
}