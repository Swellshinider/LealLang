
using System.Collections.Immutable;
using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Syntax.Expressions;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Syntax;

internal sealed class Parser
{
	private readonly DiagnosticManager _diagnostics = new();
	private readonly ImmutableArray<SyntaxToken> _tokens;
    private readonly SourceText _sourceText;
	private int _position = 0;

	public Parser(SourceText sourceText)
	{
		var tokens = new List<SyntaxToken>();
		var lexer = new Lexer(sourceText);
		SyntaxToken token;

		do
		{
			token = lexer.Lex();

			if (token.Kind != SyntaxKind.WhitespaceToken &&
				token.Kind != SyntaxKind.BadToken)
				tokens.Add(token);

		} while (token.Kind != SyntaxKind.EndOfFileToken);

		_tokens = [.. tokens];
		_diagnostics.AddRange(lexer.Diagnostics);
        _sourceText = sourceText;
    }

	public DiagnosticManager Diagnostics => _diagnostics;

	private SyntaxToken Current => Peek(0);
	private SyntaxToken LookNext => Peek(1);

    private SyntaxToken Peek(int offset)
	{
		var index = _position + offset;
		return index >= _tokens.Length ? _tokens[^1] : _tokens[index];
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

		Diagnostics.ReportTokenDidNotMatched(Current.Span, Current.Kind, kind);
		return new(kind, Current.Position, null, null);
	}

	public SyntaxTree Parse()
	{
		var expression = ParseExpression();
		var endOfFileToken = Match(SyntaxKind.EndOfFileToken);
		return new SyntaxTree(_sourceText, Diagnostics, expression, endOfFileToken);
	}

	private ExpressionSyntax ParseExpression() => Current.Kind switch
	{
		SyntaxKind.LiteralExpression => ParsePrimaryExpression(),
		_ => ParseAssignmentExpression(),
	};

	private ExpressionSyntax ParseAssignmentExpression()
	{
		if (Current.Kind == SyntaxKind.IdentifierToken &&
			LookNext.Kind == SyntaxKind.EqualsToken)
		{
			var identifierToken = NextToken();
			var equalsToken = NextToken();
			var right = ParseAssignmentExpression();
			return new AssignmentExpressionSyntax(identifierToken, equalsToken, right);
		}

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

	private ExpressionSyntax ParsePrimaryExpression() => Current.Kind switch
	{
		SyntaxKind.OpenParenthesisToken => ParseParenthesisExpression(),
		SyntaxKind.IdentifierToken => ParseNameExpression(),
		SyntaxKind.TrueKeyword or SyntaxKind.FalseKeyword => ParseBooleanExpression(),
		_ => ParseNumberExpression()
	};

	private ParenthesizedExpressionSyntax ParseParenthesisExpression()
	{
		var openParenthesisToken = Match(SyntaxKind.OpenParenthesisToken);
		var expression = ParseExpression();
		var closeParenthesisToken = Match(SyntaxKind.CloseParenthesisToken);

		return new ParenthesizedExpressionSyntax(openParenthesisToken, expression, closeParenthesisToken);
	}

	private NameExpressionSyntax ParseNameExpression()
	{
		var identifierToken = Match(SyntaxKind.IdentifierToken);
		return new NameExpressionSyntax(identifierToken);
	}

	private LiteralExpressionSyntax ParseNumberExpression()
	{
		var token = Match(SyntaxKind.LiteralToken);
		return new LiteralExpressionSyntax(token);
	}

	private LiteralExpressionSyntax ParseBooleanExpression()
	{
		var value = Current.Kind == SyntaxKind.TrueKeyword;
		var keywordToken = value ? Match(SyntaxKind.TrueKeyword)
								 : Match(SyntaxKind.FalseKeyword);
		return new LiteralExpressionSyntax(keywordToken, value);
	}
}