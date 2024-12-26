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

		if (char.IsDigit(Current))
			ReadNumberToken();
		else if (char.IsWhiteSpace(Current))
			ReadWhiteSpaceToken();
		else if (char.IsLetter(Current))
			ReadIdentifierToken();
		else
		{
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
				
				'\0' => SyntaxKind.EndOfFileToken,
				_ => BadTokenDetected()
			};

			Advance(_kind.GetAdvanceQuantity());
		}

		return new(_kind, _start, _kind == SyntaxKind.EndOfFileToken ? "\0" : GetText(), _value);
	}

	private void ReadNumberToken()
	{
		while (char.IsDigit(Current))
			Advance();

		var integerText = GetText();

		if (!int.TryParse(integerText, out var value))
			_diagnostics.ReportInvalidType(_start, _position, integerText, typeof(int));

		_value = value;
		_kind = SyntaxKind.LiteralToken;
	}

	private void ReadWhiteSpaceToken()
	{
		while (char.IsWhiteSpace(Current))
			Advance();

		_kind = SyntaxKind.WhitespaceToken;
	}

	private void ReadIdentifierToken()
	{
		while (char.IsLetterOrDigit(Current))
			Advance();

		var identifierText = GetText();
		_kind = identifierText.GetKeywordKind();
	}

	private SyntaxKind BadTokenDetected()
	{
		_diagnostics.ReportBadToken(_start, _position, GetText(1));
		return SyntaxKind.BadToken;
	}
}