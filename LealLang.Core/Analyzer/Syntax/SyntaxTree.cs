using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class SyntaxTree
{
	private SyntaxTree(SourceText sourceText)
	{
		var parser = new Parser(sourceText);
		Root = parser.ParseCompilationUnit();
		
		SourceText = sourceText;
		Diagnostics = parser.Diagnostics;
	}

	public SourceText SourceText { get; }
	public DiagnosticManager Diagnostics { get; }
	public CompilationUnitSyntax Root { get; }
	
	public static SyntaxTree Parse(string text) => new(SourceText.From(text));
	
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