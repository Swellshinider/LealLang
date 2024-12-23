using LealLang.Core.Analyzer.Diagnostics;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class Lexer
{
	private readonly DiagnosticManager _diagnostics = new();
	private readonly string _text;
	private int _position;

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

			var integerText = GetText(start);

			if (!int.TryParse(integerText, out var value))
				_diagnostics.ReportInvalidType(start, _position, integerText, typeof(int));

			return new(SyntaxKind.LiteralToken, start, integerText, value);
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

			var literalText = GetText(start);
			var literalKind = literalText.GetKeywordKind();
			return new(literalKind, start, literalText);
		}

		var kind = Current switch
		{
			'+' => SyntaxKind.PlusToken,
			'-' => SyntaxKind.MinusToken,
			'*' => SyntaxKind.StarToken,
			'/' => SyntaxKind.SlashToken,
			'=' when LookNext == '=' => SyntaxKind.EqualsEqualsToken,
			'=' => SyntaxKind.EqualsToken,
			'!' when LookNext == '=' => SyntaxKind.NotEqualsToken,
			'!' => SyntaxKind.NotToken,
			'|' when LookNext == '|' => SyntaxKind.PipePipeToken,
			'|' => SyntaxKind.PipeToken,
			'&' when LookNext == '&' => SyntaxKind.AmpersandAmpersandToken,
			'&' => SyntaxKind.AmpersandToken,
			'(' => SyntaxKind.OpenParenthesisToken,
			')' => SyntaxKind.CloseParenthesisToken,
			_ => SyntaxKind.BadToken
		};

		Advance(kind.GetAdvanceQuantity());
		var text = GetText(start);

		if (kind == SyntaxKind.BadToken)
			_diagnostics.ReportBadToken(start, _position, text);

		return new(kind, start, text);
	}
}