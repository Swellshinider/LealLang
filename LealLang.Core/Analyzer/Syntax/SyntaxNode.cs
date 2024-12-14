using System.Reflection;
using System.Text;

namespace LealLang.Core.Analyzer.Syntax;

public abstract class SyntaxNode
{
	public abstract SyntaxKind Kind { get; }

	public IEnumerable<SyntaxNode> GetChildren()
	{
		var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

		foreach (var property in properties)
		{
			if (typeof(SyntaxNode).IsAssignableFrom(property.PropertyType))
			{
				var child = (SyntaxNode)property.GetValue(this)!;

				if (child != null)
					yield return child;
			}

			if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(property.PropertyType))
			{
				var children = (IEnumerable<SyntaxNode>)property.GetValue(this)!;

				foreach (var child in children)
					if (child != null)
						yield return child;
			}
		}
	}

	public void WriteTo(TextWriter writer) => PrintNode(writer, this);

	public static void PrintNode(TextWriter writer, SyntaxNode node, string indentation = "", bool lastChild = false)
	{
		var toConsole = writer == Console.Out;
		var mark = lastChild ? "└───" : "├───";
		writer.Write($"{indentation}{mark}");

		if (toConsole)
			Console.ForegroundColor = GetColorNode(node);

		writer.Write(node.Kind);

		if (node is SyntaxToken st && st.Value != null)
			writer.Write($" {st.Value}");

		if (toConsole)
			Console.ResetColor();

		writer.WriteLine();

		indentation += lastChild ? "    " : "│   ";
		var last = node.GetChildren().LastOrDefault();

		foreach (var child in node.GetChildren())
			PrintNode(writer, child, indentation, last == child);
	}

	private static ConsoleColor GetColorNode(SyntaxNode node) => node.Kind switch
	{
		SyntaxKind.NumberToken => ConsoleColor.Cyan,

		SyntaxKind.PlusToken or
		SyntaxKind.MinusToken or
		SyntaxKind.StarToken or
		SyntaxKind.SlashToken => ConsoleColor.Magenta,

		SyntaxKind.OpenParenthesisToken or
		SyntaxKind.CloseParenthesisToken => ConsoleColor.Red,

		SyntaxKind.UnaryExpression or
		SyntaxKind.BinaryExpression => ConsoleColor.Yellow,

		_ => ConsoleColor.DarkBlue,
	};

	public override string ToString()
	{
		using var writer = new StringWriter();
		WriteTo(writer);
		return writer.ToString();
	}
}