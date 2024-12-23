namespace LealLang.Core.Analyzer.Syntax.Expressions;

public sealed class NameExpressionSyntax : ExpressionSyntax 
{
	public NameExpressionSyntax(SyntaxToken identifierToken) 
	{
		IdentifierToken = identifierToken;
	}

	public SyntaxToken IdentifierToken { get; }

	public override SyntaxKind Kind => SyntaxKind.NameExpression;
}