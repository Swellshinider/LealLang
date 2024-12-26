using LealLang.Core.Analyzer.Diagnostics;

namespace LealLang.Core.Analyzer.Syntax;

internal sealed class Lexer
{
	private readonly DiagnosticManager _diagnostics = new();
	private readonly string _text;

	private SyntaxKind _kind;
	private int _position;
	private int _start;
	private object? _value;

	public Lexer(string text)
	{
		_text = text;
		_position = 0;
	}

	internal DiagnosticManager Diagnostics => _diagnostics;

	private char Current => Peek(0);

	private char LookNext => Peek(1);

	private char Peek(int offset)
	{
		var index = _position + offset;
		return index >= _text.Length ? '\0' : _text[index];
	}

	private void Advance(int quantity = 1) => _position += quantity;

	private string GetText(int offset = 0) => _text[_start..(_position + offset)];

	public SyntaxToken Lex()
	{
		_value = null;
		_start = _position;

		_kind = Current switch
		{
			'+' => SyntaxKind.PlusToken,
			'-' => SyntaxKind.MinusToken,
			'*' => SyntaxKind.StarToken,
			'/' => SyntaxKind.SlashToken,
			'=' when LookNext == '=' => SyntaxKind.EqualsEqualsToken,
			'=' => SyntaxKind.EqualsToken,
			'!' when LookNext == '=' => SyntaxKind.NotEqualsToken,
			'!' => SyntaxKind.NotToken,
			'<' when LookNext == '=' => SyntaxKind.LessThanOrEqualToken,
			'<' => SyntaxKind.LessThanToken,
			'>' when LookNext == '=' => SyntaxKind.GreaterThanOrEqualToken,
			'>' => SyntaxKind.GreaterThanToken,
			'|' when LookNext == '|' => SyntaxKind.PipePipeToken,
			'|' => SyntaxKind.PipeToken,
			'&' when LookNext == '&' => SyntaxKind.AmpersandAmpersandToken,
			'&' => SyntaxKind.AmpersandToken,
			'(' => SyntaxKind.OpenParenthesisToken,
			')' => SyntaxKind.CloseParenthesisToken,
			'0' or
			'1' or
			'2' or
			'3' or
			'4' or
			'5' or
			'6' or
			'7' or
			'8' or
			'9' => ReadLiteralNumbers(),

			' ' or
			'\t' or
			'\n' or
			'\r' => ReadWhitespaces(),

			'\0' => SyntaxKind.EndOfFileToken,
			_ => ReadOtherCharacters()
		};

		Advance(_kind.GetAdvanceQuantity());

		return new(_kind, _start, _kind == SyntaxKind.EndOfFileToken ? "\0" : GetText(), _value);
	}

	private SyntaxKind ReadLiteralNumbers()
	{
		while (char.IsDigit(Current))
			Advance();

		var integerText = GetText();

		if (!int.TryParse(integerText, out var value))
			_diagnostics.ReportInvalidType(_start, _position, integerText, typeof(int));

		_value = value;
		return SyntaxKind.LiteralToken;
	}

	private SyntaxKind ReadWhitespaces()
	{
		while (char.IsWhiteSpace(Current))
			Advance();

		return SyntaxKind.WhitespaceToken;
	}

	private SyntaxKind ReadOtherCharacters()
	{
		if (char.IsLetter(Current))
		{
			while (char.IsLetterOrDigit(Current))
				Advance();

			var identifierText = GetText();
			return identifierText.GetKeywordKind();
		}
		
		if (char.IsWhiteSpace(Current))
			return ReadWhitespaces();

		_diagnostics.ReportBadToken(_start, _position, GetText(1));
		return SyntaxKind.BadToken;
	}
}