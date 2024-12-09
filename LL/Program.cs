using LealLang.Core.Analyzer.Syntax;

namespace LL;

public static class Program
{
	public static void Main(string[] args)
	{
		Console.Title = "LealLang: Interactive Console";
		while (true)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("> ");
			var input = Console.ReadLine() ?? string.Empty;
			Console.ResetColor();

			if (input == "#exit")
				break;
				
			if (input == "#cls")
			{
				Console.Clear();
				continue;
			}

			var lexer = new Lexer(input);
			SyntaxToken token;
			
			do 
			{
				token = lexer.Lex();
				
				if (token.Kind != SyntaxKind.WhitespaceToken && 
					token.Kind != SyntaxKind.BadToken && 
					token.Kind != SyntaxKind.EndOfFileToken)
					Console.WriteLine($"{token.Kind}: '{token.Text}'");
			} while (token.Kind != SyntaxKind.EndOfFileToken);
		}
	}
}