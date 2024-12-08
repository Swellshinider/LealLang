using LealLang.Core.Syntax;
using LealLang.Core.Syntax.Expressions;

namespace LealLang.Core.Analyzer;

public sealed class Parser
{
	private int _position;
	private readonly List<SyntaxToken> _tokens;

	public Parser(List<SyntaxToken> tokens)
	{
		_tokens = tokens;
		_position = 0;
	}

	public ExpressionSyntax Parse() => ParseExpression();

	private SyntaxToken Current => Peek(0);

	private SyntaxToken NextToken()
	{
		var current = Current;
		_position++;
		return current;
	}

	private SyntaxToken Peek(int offset)
	{
		var index = _position + offset;

		if (index >= _tokens.Count)
			return _tokens[^1];

		return _tokens[index];
	}

	private SyntaxToken MatchToken(SyntaxKind kind)
	{
		var current = Current;

		if (current.Kind == kind)
			return NextToken();

		// TODO: Error handling
		return new SyntaxToken(kind, Current.Position, null, null);
	}

	private ExpressionSyntax ParseExpression()
	{
		return ParseBinaryExpression();
	}

	private ExpressionSyntax ParseBinaryExpression(int parentPrecedenceValue = 0)
	{
		ExpressionSyntax left;

		var unaryPrecedence = Current.Kind.GetUnaryPrecedence();

		if (unaryPrecedence != 0 && unaryPrecedence >= parentPrecedenceValue)
		{
			var operatorToken = NextToken();
			var operand = ParseBinaryExpression(unaryPrecedence);
			left = new UnaryExpressionSyntax(operatorToken, operand);
		}
		else
		{
			left = ParsePrimaryExpression();
		}

		while (true)
		{
			var precedence = Current.Kind.GetBinaryPrecedence();
			if (precedence == 0 || precedence <= parentPrecedenceValue)
				break;

			var operatorToken = NextToken();
			var right = ParseBinaryExpression(precedence);
			left = new BinaryExpressionSyntax(left, operatorToken, right);
		}

		return left;
	}

	private ExpressionSyntax ParsePrimaryExpression() => Current.Kind switch
	{
		SyntaxKind.NumberToken => ParseNumberLiteralExpression(),
		_ => throw new Exception($"Unexpected token {Current.Kind}")
	};

	private LiteralExpressionSyntax ParseNumberLiteralExpression()
	{
		var numberToken = MatchToken(SyntaxKind.NumberToken);
		return new LiteralExpressionSyntax(numberToken);
	}
}