
namespace LealLang.Core.Analyzer.Syntax;

public sealed class Lexer
{
	private readonly string _text;
	private int _position;
	
	public Lexer(string text)
	{
		_text = text;
		_position = 0;
	}
	
	private char Current => _position >= _text.Length ? '\0' : _text[_position];
	
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
				
			var text =  GetText(start);
			
			if (!int.TryParse(text, out var value))
				throw new($"The number {_text} is not a valid Int32.");
				
			return new(SyntaxKind.NumberToken, start, text, value);
		}
		
		if (char.IsWhiteSpace(Current))
		{
			while (char.IsWhiteSpace(Current))
				Advance();
			
			return new(SyntaxKind.WhitespaceToken, start, GetText(start));
		}
		
		var kind = Current switch 
		{
			'+' => SyntaxKind.PlusToken,
			'-' => SyntaxKind.MinusToken,
			'*' => SyntaxKind.StarToken,
			'/' => SyntaxKind.SlashToken,
			'(' => SyntaxKind.OpenParenthesisToken,
			')' => SyntaxKind.CloseParenthesisToken,
			_ => SyntaxKind.BadToken	
		};
		
		Advance();
		return new(kind, start, GetText(start));
	}
}