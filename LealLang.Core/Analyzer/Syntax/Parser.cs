
using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class Parser
{
	private readonly List<SyntaxToken> _tokens = [];
	private int _position = 0;

	public Parser(string text, List<string> diagnostics)
	{
		var lexer = new Lexer(text, diagnostics);
		SyntaxToken token;

		do
		{
			token = lexer.Lex();

			if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken)
				_tokens.Add(token);

		} while (token.Kind != SyntaxKind.EndOfFileToken);
		Diagnostics = lexer.Diagnostics;
	}

	public List<string> Diagnostics { get; }
	
	private SyntaxToken Current => Peek(0);

	private SyntaxToken Peek(int offset)
	{
		var index = _position + offset;
		return index >= _tokens.Count ? _tokens[^1] : _tokens[index];
	}

	private SyntaxToken NextToken()
	{
		var current = Current;
		_position++;
		return current;
	}

	private SyntaxToken Match(SyntaxKind kind)
	{
		if (Current.Kind == kind)
			return NextToken();

		Diagnostics.Add($"Invalid token '{Current.Kind}' at '{Current.Position}', expected: '{kind}'");
		return new(kind, Current.Position, null, null);
	}

	public ExpressionSyntax Parse() => ParseExpression();

	private ExpressionSyntax ParseExpression() => Current.Kind switch
	{
		SyntaxKind.NumberExpression => ParseNumberExpression(),
		_ => ParseBinaryExpression(),
	};

	private ExpressionSyntax ParsePrimaryExpression() => Current.Kind switch
	{
		SyntaxKind.OpenParenthesisToken => ParseParenthesisExpression(),
		_ => ParseNumberExpression()
	};

	private ParenthesizedExpressionSyntax ParseParenthesisExpression()
	{
		var openParenthesisToken = Match(SyntaxKind.OpenParenthesisToken);
		var expression = ParseExpression();
		var closeParenthesisToken = Match(SyntaxKind.CloseParenthesisToken);

		return new ParenthesizedExpressionSyntax(openParenthesisToken, expression, closeParenthesisToken);
	}

	private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
	{
		ExpressionSyntax left;
		var unaryOperatorPrecedence = Current.Kind.GetUnaryPrecedence();

		if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
		{
			var operatorToken = NextToken();
			var operand = ParseBinaryExpression(unaryOperatorPrecedence);
			left = new UnaryExpressionSyntax(operatorToken, operand);
		}
		else
			left = ParsePrimaryExpression();

		while (true)
		{
			var precedence = Current.Kind.GetBinaryPrecedence();
			if (precedence == 0 || precedence <= parentPrecedence)
				break;

			var operatorToken = NextToken();
			var right = ParseBinaryExpression(precedence);

			left = new BinaryExpressionSyntax(left, operatorToken, right);
		}

		return left;
	}

	private ExpressionSyntax ParseNumberExpression()
	{
		var token = Match(SyntaxKind.NumberToken);
		return new LiteralExpressionSyntax(token);
	}
}