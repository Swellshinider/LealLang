
using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class Parser
{
	private readonly List<SyntaxToken> _tokens = [];
	private int _position = 0;

	public Parser(string text)
	{
		var lexer = new Lexer(text);
		SyntaxToken token;

		do
		{
			token = lexer.Lex();

			if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken)
				_tokens.Add(token);

		} while (token.Kind != SyntaxKind.EndOfFileToken);
	}

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

		// TODO: Error handling
		return new(kind, Current.Position, null, null);
	}

	public ExpressionSyntax Parse()
	{
		return ParseBinaryExpression();
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
			left = ParseNumberExpression();

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
		return new NumberExpressionSyntax(token);
	}
}