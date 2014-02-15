use Tokenizer: Aha/Tokenizer

type Expression: opaque
type Statement: opaque
type TypeRef: opaque

type ExpressionInfo:
    [
        charliteral: character |
        strliteral: String |
        intliteral: integer |
        varref: [ local: String | module: String public: String ] |
        unioper: String |
        binoper: String |
        arrayliteral: |
        emptyarray: |
        closure: |
        inclusivesegment: |
        exclusivesegment: |
        state: |
        length: |
        whereex: |
		whenex:
    ]
type Statement:
    [
        definition: |
        assertion: |
        letrecst: |
        allst: |
        anyst: |
        unlesst: |
        notst: |
        truest: |
        falsest: |
        foreachst: |
        forsomest: 
    ] 
type TypeRef =
    [
        standard: [ isnil: | isinteger: | ischaracter: ] |
        identifier: String |
        arraytype: |
        sequencetype: |
        compositetype: |
        functiontype: |
        objecttype: 
    ]
type Element:
    [
        specification: |
        implementation: |
        expression: Expression |
        statement: Statement |
        substitution: |
        condition: |
        identifier: String |
        description: String |
        typedef: [ typename: String kind: [ isopaque: | isnormal: ] description: String ] |
        typeref: TypeRef |
        typerefext: TypeRefExt |
        docgroup: |
        docentry: [ key: String value: String ] |
        modulepath: |
        grouplist: |
        closure:  
    ]
type Error: [ message: String position: [ line: integer col: integer | unknown: ]]
    
use SyntaxTrees: Base/Trees(Node: Element)
import SyntaxTrees(Types)

        