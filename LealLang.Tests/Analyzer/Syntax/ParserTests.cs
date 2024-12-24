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

	[Theory]
	[MemberData(nameof(GetUnaryOperatorPairsData))]
	public void Parser_UnaryExpression_PrecedenceTest(SyntaxKind unaryOperator, SyntaxKind binaryOperator)
	{
		var unaryPrecedence = unaryOperator.GetUnaryPrecedence();
		var binaryPrecedence = binaryOperator.GetUnaryPrecedence();
		var unaryText = unaryOperator.GetText() ?? "";
		var binaryText = binaryOperator.GetText() ?? "";
		var text = $"{unaryText} a {binaryText} b";
		var expression = SyntaxTree.Parse(text).RootExpression;

		using var e = new AssertingExpression(expression);

		if (unaryPrecedence >= binaryPrecedence)
		{
			e.AssertNode(SyntaxKind.BinaryExpression);
			e.AssertNode(SyntaxKind.UnaryExpression);
			e.AssertToken(unaryOperator, unaryText);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "a");
			e.AssertToken(binaryOperator, binaryText);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "b");
		}
		else
		{
			e.AssertNode(SyntaxKind.UnaryExpression);
			e.AssertToken(unaryOperator, unaryText);
			e.AssertNode(SyntaxKind.BinaryExpression);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "a");
			e.AssertToken(binaryOperator, binaryText);
			e.AssertNode(SyntaxKind.NameExpression);
			e.AssertToken(SyntaxKind.IdentifierToken, "b");
		}
	}

	public static TheoryData<SyntaxKind, SyntaxKind> GetUnaryOperatorPairsData()
	{
		var data = new TheoryData<SyntaxKind, SyntaxKind>();

		foreach (var operator1 in SyntaxRules.GetUnaryOperatorKinds())
			foreach (var operator2 in SyntaxRules.GetUnaryOperatorKinds())
				data.Add(operator1, operator2);

		return data;
	}

	public static TheoryData<SyntaxKind, SyntaxKind> GetBinaryOperatorPairsData()
	{
		var data = new TheoryData<SyntaxKind, SyntaxKind>();

		foreach (var unary in SyntaxRules.GetBinaryOperatorKinds())
			foreach (var binary in SyntaxRules.GetBinaryOperatorKinds())
				data.Add(unary, binary);

		return data;
	}
}