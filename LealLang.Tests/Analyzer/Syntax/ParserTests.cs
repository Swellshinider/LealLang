using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Tests.Analyzer.Syntax;

public class ParserTests
{
	[Theory]
	[MemberData(nameof(GetBinaryOperatorPairsData))]
	public void Parser_BinaryExpression_PrecedenceTest(SyntaxKind operator1, SyntaxKind operator2)
	{
		var op1Precedence = operator1.GetBinaryPrecedence();
		var op2Precedence = operator2.GetBinaryPrecedence();
		var op1Text = operator1.GetText() ?? "";
		var op2Text = operator2.GetText() ?? "";
		var text = $"a {op1Text} b {op2Text} c";
		var expression = SyntaxTree.Parse(text).RootExpression;

		using var e = new AssertingExpression(expression);

		if (op1Precedence >= op2Precedence)
		{
			e.AssertNode(SyntaxKind.BinaryExpression);
			e.AssertNode(SyntaxKind.BinaryExpression);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "a");
			e.AssertToken(operator1, op1Text);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "b");
			e.AssertToken(operator2, op2Text);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "c");
		}
		else
		{
			e.AssertNode(SyntaxKind.BinaryExpression);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "a");
			e.AssertToken(operator1, op1Text);
			e.AssertNode(SyntaxKind.BinaryExpression);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "b");
			e.AssertToken(operator2, op2Text);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "c");
		}
	}

	public static TheoryData<SyntaxKind, SyntaxKind> GetBinaryOperatorPairsData()
	{
		var data = new TheoryData<SyntaxKind, SyntaxKind>();

		foreach (var operator1 in SyntaxRules.GetBinaryOperatorKinds())
			foreach (var operator2 in SyntaxRules.GetBinaryOperatorKinds())
				data.Add(operator1, operator2);

		return data;
	}
}