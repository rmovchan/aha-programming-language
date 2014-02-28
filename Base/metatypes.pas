unit metatypes;

{$mode objfpc}{$H+}
(*
doc
    Title: "Meta Types"
    Purpose: "Values describing Aha! types"
    Package: "Aha! Base Library"
    Author: "Roman Movchan, Melbourne, Australia"
    Created: "2013-08-10"
end

type String: [character] "alias for character array type"

export Types:
    type DataType: opaque "meta-type"
    type ComField: [ id: [ name: String | op: String ] "field name or operator" datatype: DataType "field type" ] "composite field descriptor"
    type ComVariant: [ComField] "variant of a composite"
    type TypeInfo:
        [
            nilType: "nil" |
            integerType: "integer" |
            characterType: "character" |
            arrayType: DataType "array" |
            compositeType: [ComVariant] "composite type" |
            functionType: [ param: [ single: DataType | first: DataType second: DataType ] result: DataType ] "function" |
            objectType: [ state: DataType actions: [[ name: String param: DataType ]] ] "object or sequence" |
            opaqueType: "opaque"
        ] "type information"
end

export Utils:
    the TypeInfo: { DataType -> TypeInfo } "type info for given type"
    the Nesting: { DataType -> integer } "nesting level for given type; 0 for a simple type"
    the Foo: DataType "foo meta-type"
    the Integer: DataType "integer meta-type"
    the Character: DataType "character meta-type"
    the Array: { DataType -> DataType } "array of given type meta-type"
    the Seq: { DataType -> DataType } "sequence of given type meta-type"
    the Com: { [ComVariant] -> DataType } "composite meta-type"
    the Function: { [ param: [ single: DataType | first: DataType second: DataType ] result: DataType ] -> DataType } "function meta-type"
    the Object: { [ state: DataType actions: [[ name: String param: DataType ]] ] -> DataType } "object meta-type"
    (DataType <= DataType): { DataType, DataType } "covariance: does every value of first type belong to second?"
    (DataType >= DataType): { DataType, DataType } "contravariance: reverse to <="
    (DataType = DataType): { DataType, DataType } "are types equivalent (i.e.both <= and >=)?"
end
*)
interface

uses
  core;

type
  IahaModuleData = interface
    function TypeInfo(out value: IahaUnaryFunction): Boolean;
    function Nesting(out value: IahaUnaryFunction): Boolean;
    function FooType(out value: IUnknown): Boolean;
    function IntType(out value: IUnknown): Boolean;
    function CharType(out value: IUnknown): Boolean;
    function ArrayType(out value: IahaUnaryFunction): Boolean;
    function SeqType(out value: IahaUnaryFunction): Boolean;
    function ComType(out value: IahaUnaryFunction): Boolean;
    function FuncType(out value: IahaUnaryFunction): Boolean;
    function ObjType(out value: IahaUnaryFunction): Boolean;
    function Covariant(out value: IahaBinaryRelation): Boolean;
    function Contravariant(out value: IahaBinaryRelation): Boolean;
    function Equivalent(out value: IahaBinaryRelation): Boolean;
  end;

function GetModuleData(out value: IahaModuleData): Boolean;

implementation

type

  { TModuleData }

  TModuleData = class(TahaComposite, IahaModuleData)
  private
    function TypeInfo(out value: IahaUnaryFunction): Boolean;
    function Nesting(out value: IahaUnaryFunction): Boolean;
    function FooType(out value: IUnknown): Boolean;
    function IntType(out value: IUnknown): Boolean;
    function CharType(out value: IUnknown): Boolean;
    function ArrayType(out value: IahaUnaryFunction): Boolean;
    function SeqType(out value: IahaUnaryFunction): Boolean;
    function ComType(out value: IahaUnaryFunction): Boolean;
    function FuncType(out value: IahaUnaryFunction): Boolean;
    function ObjType(out value: IahaUnaryFunction): Boolean;
    function Covariant(out value: IahaBinaryRelation): Boolean;
    function Contravariant(out value: IahaBinaryRelation): Boolean;
    function Equivalent(out value: IahaBinaryRelation): Boolean;
  public
    constructor Create;
  end;

function GetModuleData(out value: IahaModuleData): Boolean;
begin
  Result := True;
  try
    value := TModuleData.Create;
  except
    Result := False;
  end;
end;

type
  ITypeInfo = interface
    function fooType: Boolean;
    function characterType: Boolean;
    function integerType: Boolean;
    function arrayType(out datatype: ITypeInfo): Boolean;
    function compositeType(out comvars: IahaArray): Boolean;
    function functionType(out datatype: IUnknown): Boolean;
    function objectType(out datatype: IUnknown): Boolean;
    function opaqueType: Boolean;
  end;

  { TBaseTypeInfo }

  TBaseTypeInfo = class(TahaComposite, ITypeInfo)
  private
    function fooType: Boolean; virtual;
    function characterType: Boolean; virtual;
    function integerType: Boolean; virtual;
    function arrayType(out datatype: ITypeInfo): Boolean; virtual;
    function compositeType(out comvars: IahaArray): Boolean; virtual;
    function functionType(out datatype: IUnknown): Boolean; virtual;
    function objectType(out datatype: IUnknown): Boolean; virtual;
    function opaqueType: Boolean; virtual;
  end;

  { TFooType }

  TFooType = class(TBaseTypeInfo)
  private
    function fooType: Boolean; override;
  end;

  { TCharacterType }

  TCharacterType = class(TBaseTypeInfo)
  private
    function characterType: Boolean; override;
  end;

  { TIntegerType }

  TIntegerType = class(TBaseTypeInfo)
  private
    function integerType: Boolean; override;
  end;

  { TArrayType }

  TArrayType = class(TBaseTypeInfo)
  private
    FItemType: ITypeInfo;
    function arrayType(out datatype: ITypeInfo): Boolean; override;
  public
    constructor Create(const itemtype: ITypeInfo);
  end;

  { TArrayTypeConstructor }

  TArrayTypeConstructor = class(TahaFunction, IahaUnaryFunction)
  private
    function Get(const param; out value): Boolean;
  end;

  { Class implementation }

function TArrayType.arrayType(out datatype: ITypeInfo): Boolean;
begin
  datatype := FItemType;
  Result := True;
end;

constructor TArrayType.Create(const itemtype: ITypeInfo);
begin
  inherited Create;
  FItemType := itemtype;
end;

{ TArrayTypeConstructor }

function TArrayTypeConstructor.Get(const param; out value): Boolean;
begin
  ITypeInfo(value) := TArrayType.Create(ITypeInfo(param));
end;

{ TFooType }

function TFooType.fooType: Boolean;
begin
  Result := True;
end;

{ TCharacterType }

function TCharacterType.characterType: Boolean;
begin
  Result := True;
end;

{ TIntegerType }

function TIntegerType.integerType: Boolean;
begin
  Result := True;
end;

{ TBaseTypeInfo }

function TBaseTypeInfo.fooType: Boolean;
begin
  Result := False;
end;

function TBaseTypeInfo.characterType: Boolean;
begin
  Result := False;
end;

function TBaseTypeInfo.integerType: Boolean;
begin
  Result := False;
end;

function TBaseTypeInfo.arrayType(out datatype: ITypeInfo): Boolean;
begin
  Result := False;
end;

function TBaseTypeInfo.compositeType(out comvars: IahaArray): Boolean;
begin
  Result := False;
end;

function TBaseTypeInfo.functionType(out datatype: IUnknown): Boolean;
begin
  Result := False;
end;

function TBaseTypeInfo.objectType(out datatype: IUnknown): Boolean;
begin
  Result := False;
end;

function TBaseTypeInfo.opaqueType: Boolean;
begin
  Result := False;
end;

{ TModuleData }

function TModuleData.TypeInfo(out value: IahaUnaryFunction): Boolean;
begin

end;

function TModuleData.Nesting(out value: IahaUnaryFunction): Boolean;
begin

end;

function TModuleData.FooType(out value: IUnknown): Boolean;
begin
  value := TFooType.Create;
  Result := True;
end;

function TModuleData.IntType(out value: IUnknown): Boolean;
begin

end;

function TModuleData.CharType(out value: IUnknown): Boolean;
begin

end;

function TModuleData.ArrayType(out value: IahaUnaryFunction): Boolean;
begin
  value := TArrayTypeConstructor.Create;
end;

function TModuleData.SeqType(out value: IahaUnaryFunction): Boolean;
begin

end;

function TModuleData.ComType(out value: IahaUnaryFunction): Boolean;
begin

end;

function TModuleData.FuncType(out value: IahaUnaryFunction): Boolean;
begin

end;

function TModuleData.ObjType(out value: IahaUnaryFunction): Boolean;
begin

end;

function TModuleData.Covariant(out value: IahaBinaryRelation): Boolean;
begin

end;

function TModuleData.Contravariant(out value: IahaBinaryRelation): Boolean;
begin

end;

function TModuleData.Equivalent(out value: IahaBinaryRelation): Boolean;
begin

end;

constructor TModuleData.Create;
begin

end;

end.

