using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Core.Analyzer.Binding;

internal sealed class BoundUnaryOperator
{
	private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType)
		: this(syntaxKind, kind, operandType, operandType) { }

	private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
	{
		SyntaxKind = syntaxKind;
		Kind = kind;
		OperandType = operandType;
		ResultType = resultType;
	}

	public SyntaxKind SyntaxKind { get; }
	public BoundUnaryOperatorKind Kind { get; }
	public Type OperandType { get; }
	public Type ResultType { get; }

	public static BoundUnaryOperator[] Operators =
	[
		new(SyntaxKind.ExclamationToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
		new(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
		new(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
	];
	
	public static BoundUnaryOperator? Bind(SyntaxKind syntaxKind, Type operandType) 
		=> Operators.FirstOrDefault(op => op.SyntaxKind == syntaxKind && op.OperandType == operandType);
}