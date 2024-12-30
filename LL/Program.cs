
using System.Collections.Immutable;
using System.Text;
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
		var content = new StringBuilder();
		var variables = new Dictionary<VariableSymbol, object?>();

		while (true)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write(content.Length > 0 ? "⁞ " : "» ");
			var input = Console.ReadLine() ?? string.Empty;
			Console.ResetColor();
			
			var isBlank = string.IsNullOrEmpty(input);

			if (content.Length <= 0)
			{
				if (input == "#exit")
				{
					break;
				}
				else if (input == "#cls")
				{
					Console.Clear();
					continue;
				}
				else if (input == "#showTree")
				{
					showTree = !showTree;
					Console.WriteLine($"{(showTree ? "showing" : "hiding")} parsed trees");
					continue;
				}
				else if (input == "#showVar")
				{
					foreach (var v in variables)
						Console.WriteLine($"[<{v.Key.Type}>]{v.Key.Name}, {v.Value}");
					continue;
				}
			}

			content.AppendLine(input);
			var text = content.ToString();
			var syntaxTree = SyntaxTree.Parse(text);

			if (!isBlank && syntaxTree.Diagnostics.Any())
				continue;

			if (showTree)
				syntaxTree.Root.WriteTo(Console.Out);

			var compilation = new Compilation(syntaxTree);
			var result = compilation.Evaluate(variables);

			if (!ValidateAndDisplayErrors(syntaxTree.SourceText, [.. result.Diagnostics]))
			{
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.WriteLine(result.Value);
				Console.ResetColor();
			}

			content = new StringBuilder();
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
			Console.WriteLine($"({lineNumber}, {character}): {diagnostic}");
			Console.ResetColor();

			var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
			var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);
			var prefix = sourceText.ToString(prefixSpan);
			var error = sourceText.ToString(diagnostic.Span);
			var suffix = sourceText.ToString(suffixSpan);

			Console.Write($"    {prefix}");

			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(error);
			Console.ResetColor();

			Console.WriteLine(suffix);
		}

		return !diagnostics.IsEmpty;
	}
}