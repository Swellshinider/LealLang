
namespace LealLang.Core.Analyzer.Syntax;

public sealed class Lexer
{
	private readonly string _text;
	private int _position;

	public Lexer(string text, List<string> diagnostics)
	{
		_text = text;
		_position = 0;
		Diagnostics = diagnostics;
	}

	public List<string> Diagnostics { get; }

	private char Current => Peek(0);

	private char LookNext => Peek(1);

	private char Peek(int offset)
	{
		var index = _position + offset;
		return index >= _text.Length ? '\0' : _text[index];
	}

	private void Advance(int quantity = 1) => _position += quantity;

	private string GetText(int start) => _text[start.._position];

	public SyntaxToken Lex()
	{
		var start = _position;

		if (Current == '\0')
			return new(SyntaxKind.EndOfFileToken, start, "\0");

		if (char.IsDigit(Current))
		{
			while (char.IsDigit(Current))
				Advance();

			var text = GetText(start);

			if (!int.TryParse(text, out var value))
				Diagnostics.Add($"The number '{text}' is not a valid Int32.");

			return new(SyntaxKind.LiteralToken, start, text, value);
		}

		if (char.IsWhiteSpace(Current))
		{
			while (char.IsWhiteSpace(Current))
				Advance();

			return new(SyntaxKind.WhitespaceToken, start, GetText(start));
		}

		if (char.IsLetter(Current))
		{
			while (char.IsLetterOrDigit(Current))
				Advance();

			var text = GetText(start);
			var literalKind = text.GetKeywordKind();
			return new(literalKind, start, text);
		}

		var kind = ReadIsolatedTokens();

		if (kind == SyntaxKind.BadToken)
			Diagnostics.Add($"'{GetText(start)}' is not a valid token.");

		return new(kind, start, GetText(start));
	}

	private SyntaxKind ReadIsolatedTokens()
	{
		var kind = Current switch
		{
			'+' => SyntaxKind.PlusToken,
			'-' => SyntaxKind.MinusToken,
			'*' => SyntaxKind.StarToken,
			'/' => SyntaxKind.SlashToken,
			'=' when LookNext == '=' => SyntaxKind.EqualsEqualsToken,
			'=' => SyntaxKind.EqualsToken,
			'!' when LookNext == '=' => SyntaxKind.ExclamationEqualsToken,
			'!' => SyntaxKind.ExclamationToken,
			'|' when LookNext == '|' => SyntaxKind.PipePipeToken,
			'|' => SyntaxKind.PipeToken,
			'&' when LookNext == '&' => SyntaxKind.AmpersandAmpersandToken,
			'&' => SyntaxKind.AmpersandToken,
			'(' => SyntaxKind.OpenParenthesisToken,
			')' => SyntaxKind.CloseParenthesisToken,
			_ => SyntaxKind.BadToken
		};

		Advance(kind.GetAdvanceQuantity());
		return kind;
	}
}