use Tokenizer: Aha/Tokenizer
use Meta: Base/MetaTypes

type Variable: opaque
type Context: opaque
type FinalExpression: opaque
type Expression: { Context -> FinalExpression }
type FinalStatement: opaque
type Statement: { Context -> FinalStatement }
type TypeRef: opaque
type Specification: opaque
type Implementation: opaque

the Variable: { [ name: String datatype: TypeRef ] -> Variable }
the UnaryOp: { [ operator: String operand: TypeRef datatype: TypeRef ] -> Variable }
the BinaryOp: { [ operator: String left: TypeRef right: TypeRef datatype: TypeRef ] -> Variable }

the FooType: TypeRef
the CharacterType: TypeRef
the IntegerType: TypeRef
the ArrayType: { TypeRef -> TypeRef }
the SeqType: { TypeRef -> TypeRef }
the CompositeType:	{ [[Variable]] -> TypeRef }
the OpaqueType: { String -> TypeRef }
the OpaqueTypeExt: { Module, String -> TypeRef }

the Foo: Expression
the Integer: { integer -> Expression }
the Character: { character -> Expression }
the String: { String -> Expression }
the Array: { [Expression] -> Expression }
the EmptyArray: { TypeRef -> Expression }
the VarValue: { Variable -> Expression }

the ExpressionDataType: { Expression -> DataType }
the ExpressionInput: { Expression -> [ vars: [String] unary: [String] binary: [String] ] }

the VarDef: { String, Expression -> Statement }
the Assertion: { Expression -> Statement }
the All: { [Statement] -> Statement }
the Any: { [Statement] -> Statement }
the Unless: { Statement, Statement -> Statement }
the Not: { Statement -> Statement }
the ForEach: { [ var: String in: Expression that: Statement ] -> Statement }
the ForSome: { [ var: String in: Expression that: Statement ] -> Statement }
the True: Statement
the False: Statement

the StatementInfo:
	{ Statement ->
		[
			output: [Variable]
			input: [Variable]
		]
	}
	
the VariableInfo:
	{ Variable -> DataType }

type Text: [ lines: String* count: integer ]
the PascalCode: { Implementation -> Text } 

type Error: [ message: String position: [ line: integer col: integer | unknown: ]]
            