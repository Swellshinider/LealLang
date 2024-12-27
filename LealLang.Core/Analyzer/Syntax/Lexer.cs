using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Syntax;

internal sealed class Lexer
{
	private readonly DiagnosticManager _diagnostics = new();
	private readonly SourceText _text;

	private SyntaxKind _kind;
	private int _start;
	private int _position;
	private object? _value;

	public Lexer(SourceText text)
	{
		_text = text;
	}

	public DiagnosticManager Diagnostics => _diagnostics;

	private char Current => Peek(0);

	private char Lookahead => Peek(1);

	private char Peek(int offset)
	{
		var index = _position + offset;

		if (index >= _text.Length)
			return '\0';

		return _text[index];
	}

	public SyntaxToken Lex()
	{
		_start = _position;
		_kind = SyntaxKind.BadToken;
		_value = null;

		switch (Current)
		{
			case '\0': _kind = SyntaxKind.EndOfFileToken; break;
			case '+': HandleSingleCharacterToken(SyntaxKind.PlusToken); break;
			case '-': HandleSingleCharacterToken(SyntaxKind.MinusToken); break;
			case '*': HandleSingleCharacterToken(SyntaxKind.StarToken); break;
			case '/': HandleSingleCharacterToken(SyntaxKind.SlashToken); break;
			case '(': HandleSingleCharacterToken(SyntaxKind.OpenParenthesisToken); break;
			case ')': HandleSingleCharacterToken(SyntaxKind.CloseParenthesisToken); break;
			case '&': HandleDoubleCharacterToken('&', SyntaxKind.AmpersandAmpersandToken); break;
			case '|': HandleDoubleCharacterToken('|', SyntaxKind.PipePipeToken); break;
			case '=': HandleCharacterPair('=', SyntaxKind.EqualsToken, SyntaxKind.EqualsEqualsToken); break;
			case '>': HandleCharacterPair('=', SyntaxKind.GreaterThanToken, SyntaxKind.GreaterThanOrEqualToken); break;
			case '<': HandleCharacterPair('=', SyntaxKind.LessThanToken, SyntaxKind.LessThanOrEqualToken); break;
			case '!': HandleCharacterPair('=', SyntaxKind.NotToken, SyntaxKind.NotEqualsToken); break;
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9': ReadNumberToken(); break;
			case ' ': case '\t': case '\n': case '\r': ReadWhiteSpace(); break;
			default: HandleDefaultCase(); break;
		}

		var length = _position - _start;
		var text = _kind.GetText();
		text ??= _text.ToString(_start, length);

		return new SyntaxToken(_kind, _start, text, _value);
	}

	private void HandleSingleCharacterToken(SyntaxKind kind)
	{
		_kind = kind;
		_position++;
	}

	private void HandleDoubleCharacterToken(char expected, SyntaxKind kind)
	{
		if (Lookahead == expected)
		{
			_kind = kind;
			_position += 2;
		}
	}

	private void HandleCharacterPair(char nextExpected, SyntaxKind firstKind, SyntaxKind secondKind)
	{
		_position++;
		if (Current == nextExpected)
		{
			_kind = secondKind;
			_position++;
		}
		else
		{
			_kind = firstKind;
		}
	}

	private void HandleDefaultCase()
	{
		if (char.IsLetter(Current))
		{
			ReadIdentifierOrKeyword();
			return;
		}

		if (char.IsWhiteSpace(Current))
		{
			ReadWhiteSpace();
			return;
		}

		_kind = SyntaxKind.BadToken;
		_position++;
		_diagnostics.ReportBadToken(_start, Current);
	}

	private void ReadNumberToken()
	{
		while (char.IsDigit(Current))
			_position++;

		var length = _position - _start;
		var text = _text.ToString(_start, length);

		if (!int.TryParse(text, out var value))
			_diagnostics.ReportInvalidType(_start, _position, text, typeof(int));

		_value = value;
		_kind = SyntaxKind.LiteralToken;
	}

	private void ReadWhiteSpace()
	{
		while (char.IsWhiteSpace(Current))
			_position++;

		_kind = SyntaxKind.WhitespaceToken;
	}

	private void ReadIdentifierOrKeyword()
	{
		while (char.IsLetter(Current))
			_position++;

		var length = _position - _start;
		var text = _text.ToString(_start, length);
		_kind = text.GetKeywordKind();
	}
}