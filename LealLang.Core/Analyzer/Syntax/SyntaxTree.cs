using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class SyntaxTree
{
	public SyntaxTree(DiagnosticManager diagnostics, ExpressionSyntax rootExpression, SyntaxToken endOfFileToken)
	{
		Diagnostics = diagnostics;
		RootExpression = rootExpression;
		EndOfFileToken = endOfFileToken;
	}

	public DiagnosticManager Diagnostics { get; }
	public ExpressionSyntax RootExpression { get; }
	public SyntaxToken EndOfFileToken { get; }
	
	public static SyntaxTree Parse(string text) 
	{
		var parser = new Parser(text);
		return parser.Parse();
	}
	
	public static IEnumerable<SyntaxToken> ParseTokens(string text) 
	{
		var lexer = new Lexer(text);
		
		while (true) 
		{
			var token = lexer.Lex();
			
			if (token.Kind == SyntaxKind.EndOfFileToken)
				break;
			
			yield return token;
		}
	}
}