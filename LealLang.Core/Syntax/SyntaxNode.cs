using System.Reflection;

namespace LealLang.Core.Syntax;

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
				var child = (SyntaxNode?)property.GetValue(this);
				if (child != null)
					yield return child;
			}
			else if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(property.PropertyType))
			{
				var children = (IEnumerable<SyntaxNode>?)property.GetValue(this) ?? [];
				foreach (var child in children)
					yield return child;
			}
		}
	}

	public void WriteTo(TextWriter writer) => PrintNode(writer, this);

	private static void PrintNode(TextWriter writer, SyntaxNode node, string indentation = "", bool isLastOne = true)
	{
		var mark = isLastOne ? "└───" : "├───";

		writer.Write($"{indentation}{mark}");
		writer.Write(node.Kind);

		if (node is SyntaxToken token && token.Value != null)
			writer.Write($" {token.Value}");

		writer.WriteLine();

		indentation += isLastOne ? "    " : "│   ";
		var lastChild = node.GetChildren().LastOrDefault();

		node.GetChildren().ToList().ForEach(child => PrintNode(writer, child, indentation, child == lastChild));
	}

	public override string ToString()
	{
		using var writer = new StringWriter();
		WriteTo(writer);
		return writer.ToString();
	}
}