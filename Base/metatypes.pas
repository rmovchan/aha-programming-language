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
  IModuleData = interface
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

  ITypeInfoEx = interface;

  ITypeInfo = interface
    function fooType: Boolean;
    function characterType: Boolean;
    function integerType: Boolean;
    function arrayType(out datatype: ITypeInfoEx): Boolean;
    function compositeType(out comvars: IahaArray): Boolean;
    function functionType(out datatype: IUnknown): Boolean;
    function objectType(out datatype: IUnknown): Boolean;
    function opaqueType: Boolean;
  end;

  ITypeInfoEx = interface(ITypeInfo)
    function nesting(out value: TahaInteger): Boolean;
  end;

  IAttribute = interface
    function name(out value: IahaArray): Boolean;
    function datatype(out value: ITypeInfoEx): Boolean;
  end;

function GetModuleData(out value: IModuleData): Boolean;

implementation

uses
  SysUtils;

type

  { TModuleData }

  TModuleData = class(TahaComposite, IModuleData)
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

function GetModuleData(out value: IModuleData): Boolean;
begin
  Result := True;
  try
    value := TModuleData.Create;
  except
    Result := False;
  end;
end;

type
  { TBaseTypeInfo }

  TBaseTypeInfo = class(TahaComposite, ITypeInfoEx)
  private
    FNesting: TahaInteger;
    function fooType: Boolean; virtual;
    function characterType: Boolean; virtual;
    function integerType: Boolean; virtual;
    function arrayType(out datatype: ITypeInfoEx): Boolean; virtual;
    function compositeType(out comvars: IahaArray): Boolean; virtual;
    function functionType(out datatype: IUnknown): Boolean; virtual;
    function objectType(out datatype: IUnknown): Boolean; virtual;
    function opaqueType: Boolean; virtual;
    function nesting(out value: TahaInteger): Boolean; virtual;
  public
    constructor Create(const N: TahaInteger);
  end;

  { TTypeInfo }

  TTypeInfo = class(TahaFunction, IahaUnaryFunction)
  private
    function Get(const param; out value): Boolean;
  end;

  { TNesting }

  TNesting = class(TahaFunction, IahaUnaryFunction)
  private
    function Get(const param; out value): Boolean;
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
    FItemType: ITypeInfoEx;
    function arrayType(out datatype: ITypeInfoEx): Boolean; override;
  public
    constructor Create(const itemtype: ITypeInfoEx);
  end;

  { TArrayTypeConstructor }

  TArrayTypeConstructor = class(TahaFunction, IahaUnaryFunction)
  private
    function Get(const param; out value): Boolean;
  end;

  { TCompositeType }

  TCompositeType = class(TBaseTypeInfo)
  private
    FAttributes: IahaArray;
    function compositeType(out comvars: IahaArray): Boolean; override;
  public
    constructor Create(const comvars: IahaArray);
end;

  { TCompositeTypeConstructor }

  TCompositeTypeConstructor = class(TahaFunction, IahaUnaryFunction)
  private
    function Get(const param; out value): Boolean;
  end;

  { Class implementation }

{ TTypeInfo }

function TTypeInfo.Get(const param; out value): Boolean;
begin
  Result := True;
  ITypeInfo(value) := ITypeInfoEx(param);
end;

{ TNesting }

function TNesting.Get(const param; out value): Boolean;
begin
  Result := ITypeInfoEx(param).nesting(TahaInteger(value));
end;

{ TCompositeTypeConstructor }

function TCompositeTypeConstructor.Get(const param; out value): Boolean;
begin
  try
    ITypeInfoEx(value) := TCompositeType.Create(IahaArray(param));
    Result := True;
  except
    Result := False;
  end;
end;

{ TCompositeType }

function TCompositeType.compositeType(out comvars: IahaArray): Boolean;
begin
  comvars := FAttributes;
  Result := True;
end;

constructor TCompositeType.Create(const comvars: IahaArray);
var
  I, J, K, L, N, M: TahaInteger;
  V: IahaArray;
  A: IAttribute;
  D: ITypeInfoEx;
begin
  if comvars.size(N) then
    begin
      M := 0;
      I := 0;
      while I < N do //scan variants
      begin
        if comvars.at(I, V) and V.size(L) then
          begin
            J := 0;
            while J < L do //scan attributes
            begin
              if V.at(J, A) then
                begin
                  if A.datatype(D) and D.nesting(K) and (K > M) then
                    M := K; //find maximum nesting level
                end
              else
                Abort;
              Inc(J);
            end;
          end
        else
          Abort;
        Inc(I);
      end;
      inherited Create(M + 1);
      FAttributes := comvars;
    end
  else
    Abort;
end;

{ TArrayType }

function TArrayType.arrayType(out datatype: ITypeInfoEx): Boolean;
begin
  datatype := FItemType;
  Result := True;
end;

constructor TArrayType.Create(const itemtype: ITypeInfoEx);
var
  N: TahaInteger;
begin
  if itemtype.nesting(N) then
    begin
      inherited Create(N + 1);
      FItemType := itemtype;
    end
  else
    Abort;
end;

{ TArrayTypeConstructor }

function TArrayTypeConstructor.Get(const param; out value): Boolean;
begin
  try
    ITypeInfoEx(value) := TArrayType.Create(ITypeInfoEx(param));
    Result := True;
  except
    Result := False;
  end;
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

function TBaseTypeInfo.arrayType(out datatype: ITypeInfoEx): Boolean;
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

function TBaseTypeInfo.nesting(out value: TahaInteger): Boolean;
begin
  value := FNesting;
  Result := True;
end;

constructor TBaseTypeInfo.Create(const N: TahaInteger);
begin
  inherited Create;
  FNesting := N;
end;

{ TModuleData }

function TModuleData.TypeInfo(out value: IahaUnaryFunction): Boolean;
begin
  try
    value := TTypeInfo.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.Nesting(out value: IahaUnaryFunction): Boolean;
begin
  try
    value := TNesting.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.FooType(out value: IUnknown): Boolean;
begin
  try
    value := TFooType.Create(0);
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.IntType(out value: IUnknown): Boolean;
begin
  try
    value := TIntegerType.Create(0);
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.CharType(out value: IUnknown): Boolean;
begin
  try
    value := TCharacterType.Create(0);
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.ArrayType(out value: IahaUnaryFunction): Boolean;
begin
  try
    value := TArrayTypeConstructor.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.SeqType(out value: IahaUnaryFunction): Boolean;
begin

  Result := True;
end;

function TModuleData.ComType(out value: IahaUnaryFunction): Boolean;
begin

  Result := True;
end;

function TModuleData.FuncType(out value: IahaUnaryFunction): Boolean;
begin

  Result := True;
end;

function TModuleData.ObjType(out value: IahaUnaryFunction): Boolean;
begin

  Result := True;
end;

function TModuleData.Covariant(out value: IahaBinaryRelation): Boolean;
begin

  Result := True;
end;

function TModuleData.Contravariant(out value: IahaBinaryRelation): Boolean;
begin

  Result := True;
end;

function TModuleData.Equivalent(out value: IahaBinaryRelation): Boolean;
begin

  Result := True;
end;

constructor TModuleData.Create;
begin

end;

end.

