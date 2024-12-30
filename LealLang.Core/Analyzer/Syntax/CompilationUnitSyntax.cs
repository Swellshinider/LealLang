using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class CompilationUnitSyntax : SyntaxNode
{
	public CompilationUnitSyntax(ExpressionSyntax expression, SyntaxToken endOfFileToken)
	{
        Expression = expression;
        EndOfFileToken = endOfFileToken;
    }
	
    public ExpressionSyntax Expression { get; }
    public SyntaxToken EndOfFileToken { get; }

	public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
}