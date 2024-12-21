using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Core.Analyzer.Binding;

internal sealed class BoundBinaryOperator
{
	private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type)
		: this(syntaxKind, kind, type, type, type) { }
	
	private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType, Type resultType)
		: this(syntaxKind, kind, operandType, operandType, resultType) { }
	
	private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
	{
		SyntaxKind = syntaxKind;
		Kind = kind;
		LeftType = leftType;
		RightType = rightType;
		ResultType = resultType;
	}

	public SyntaxKind SyntaxKind { get; }
	public BoundBinaryOperatorKind Kind { get; }
	public Type LeftType { get; }
	public Type RightType { get; }
	public Type ResultType { get; }

	private static readonly BoundBinaryOperator[] Operators =
	[
		new(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
		new(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
		new(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
		new(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),
		
		new(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
		new(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
		
		new(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.LogicalEquality, typeof(int), typeof(bool)),
		new(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.LogicalEquality, typeof(bool), typeof(bool)),
		new(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.LogicalEquality, typeof(bool), typeof(int), typeof(bool)),
		new(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.LogicalEquality, typeof(int), typeof(bool), typeof(bool)),
	];

	public static BoundBinaryOperator? Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
		=> Operators.FirstOrDefault(op => op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType);
}