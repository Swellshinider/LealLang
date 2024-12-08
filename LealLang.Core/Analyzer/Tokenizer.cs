using LealLang.Core.Syntax;

namespace LealLang.Core.Analyzer;

public sealed class Tokenizer
{
	private readonly string _text;
	private int _position;

	private Tokenizer(string text)
	{
		_text = text;
		_position = 0;
	}

	private char Current => _position < _text.Length ? _text[_position] : '\0';
	private char Next => _position + 1 < _text.Length ? _text[_position + 1] : '\0';

	private List<SyntaxToken> Tokenize()
	{
		var tokens = new List<SyntaxToken>();

		SyntaxToken token;

		do
		{
			token = NextToken();

			if (token.Kind != SyntaxKind.WhiteSpaceToken && 
				token.Kind != SyntaxKind.BadToken && 
				token.Kind != SyntaxKind.EndOfFileToken)
				tokens.Add(token);
		}
		while (token.Kind != SyntaxKind.EndOfFileToken);

		return tokens;
	}

	private SyntaxToken NextToken()
	{
		var start = _position;

		if (char.IsWhiteSpace(Current))
		{
			while (char.IsWhiteSpace(Current))
				_position++;

			return new SyntaxToken(SyntaxKind.WhiteSpaceToken, _position, _text.Substring(start, _position - start), null);
		}

		if (char.IsDigit(Current))
		{
			while (char.IsDigit(Current))
				_position++;

			var text = _text.Substring(start, _position - start);

			if (!int.TryParse(text, out var value))
				throw new Exception($"Invalid number: {text}");

			return new SyntaxToken(SyntaxKind.NumberToken, _position, text, value);
		}

        return Current switch
        {
            '+' => new SyntaxToken(SyntaxKind.PlusToken, _position++, "+"),
            '-' => new SyntaxToken(SyntaxKind.MinusToken, _position++, "-"),
            '*' => new SyntaxToken(SyntaxKind.StarToken, _position++, "*"),
            '/' => new SyntaxToken(SyntaxKind.SlashToken, _position++, "/"),
            '\0' => new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0"),
            _ => new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(start, 1)),// TODO: Error handling
        };
    }

	public static List<SyntaxToken> Tokenize(string text)
	{
		var tokenizer = new Tokenizer(text);
		return tokenizer.Tokenize();
	}
}