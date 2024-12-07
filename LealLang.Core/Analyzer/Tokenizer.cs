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

	private List<Token> Tokenize()
	{
		var tokens = new List<Token>();
		
		Token token;
		
		do 
		{
			token = NextToken();
			tokens.Add(token);
		}
		while (token.Type != TokenType.EndOfFileToken);
		
		return tokens;
	}

	private Token NextToken()
	{
		var start = _position;

		if (char.IsWhiteSpace(Current))
		{
			while (char.IsWhiteSpace(Current))
				_position++;

			return new Token(TokenType.WhiteSpaceToken, _text.Substring(start, _position - start), start, _position - start, null);
		}

		if (char.IsDigit(Current))
		{
			while (char.IsDigit(Current))
				_position++;

			var text = _text.Substring(start, _position - start);

			if (!int.TryParse(text, out var value))
				throw new Exception($"Invalid number: {text}");

			return new Token(TokenType.NumberToken, text, start, _position - start, value);
		}

		switch (Current)
		{
			case '+':
				_position++;
				return new Token(TokenType.PlusToken, "+", start, 1, null);
			case '-':
				_position++;
				return new Token(TokenType.MinusToken, "-", start, 1, null);
			case '*':
				_position++;
				return new Token(TokenType.AsteriskToken, "*", start, 1, null);
			case '/':
				_position++;
				return new Token(TokenType.SlashToken, "/", start, 1, null);
			case '\0':
				return new Token(TokenType.EndOfFileToken, "\0", start, 0, null);
			default:
				throw new Exception($"Invalid character: {Current}");
		}
	}

	public static List<Token> Tokenize(string text)
	{
		var tokenizer = new Tokenizer(text);
		return tokenizer.Tokenize();
	}
}