using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Tests.Analyzer.Syntax;

internal sealed class AssertingExpression : IDisposable
{
	private readonly List<SyntaxNode> _nodeList;
	private bool _hasErrors;
	private int _position = -1;

	public AssertingExpression(SyntaxNode node)
	{
		_nodeList = TraverseSyntaxTree(node).ToList();
	}
	
	private SyntaxNode Current => _nodeList[_position];
	private SyntaxNode NextNode => _nodeList[++_position];

	private static IEnumerable<SyntaxNode> TraverseSyntaxTree(SyntaxNode node)
	{
		var stack = new Stack<SyntaxNode>();
		stack.Push(node);

		while (stack.Count > 0)
		{
			var n = stack.Pop();
			yield return n;

			foreach (var child in n.GetChildren().Reverse())
				stack.Push(child);
		}
	}

	public void AssertNode(SyntaxKind kind)
	{
		try
		{
			Assert.NotNull(NextNode);
			Assert.Equal(kind, Current.Kind);
			Assert.IsNotType<SyntaxToken>(Current);
		}
		catch
		{
			_hasErrors = true;
		}
	}

	public void AssertToken(SyntaxKind kind, string text)
	{
		try
		{
			Assert.NotNull(NextNode);
			Assert.Equal(kind, Current.Kind);
			var token = Assert.IsType<SyntaxToken>(Current);
			Assert.Equal(text, token.Text);
		}
		catch
		{
			_hasErrors = true;
		}
	}

	public void Dispose()
	{
		if (!_hasErrors)
			Assert.False(_position >= _nodeList.Count);
			
		_nodeList.Clear();
	}
}