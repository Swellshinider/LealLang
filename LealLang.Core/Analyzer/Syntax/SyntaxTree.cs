using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Syntax.Expressions;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class SyntaxTree
{
	public SyntaxTree(SourceText sourceText, DiagnosticManager diagnostics, ExpressionSyntax rootExpression, SyntaxToken endOfFileToken)
	{
        SourceText = sourceText;
        Diagnostics = diagnostics;
		RootExpression = rootExpression;
		EndOfFileToken = endOfFileToken;
	}

    public SourceText SourceText { get; }
    public DiagnosticManager Diagnostics { get; }
	public ExpressionSyntax RootExpression { get; }
	public SyntaxToken EndOfFileToken { get; }
	
	public static SyntaxTree Parse(string text) => Parse(SourceText.From(text));
	
	public static SyntaxTree Parse(SourceText sourceText) 
	{
		var parser = new Parser(sourceText);
		return parser.Parse();
	}
	
	public static IEnumerable<SyntaxToken> ParseTokens(string text) => ParseTokens(SourceText.From(text));
	
	public static IEnumerable<SyntaxToken> ParseTokens(SourceText sourceText) 
	{
		var lexer = new Lexer(sourceText);
		
		while (true) 
		{
			var token = lexer.Lex();
			
			if (token.Kind == SyntaxKind.EndOfFileToken)
				break;
			
			yield return token;
		}
	}
}