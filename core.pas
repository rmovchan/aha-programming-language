unit core;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils;

type
  TahaFoo = record end;

  PahaInteger =^TahaInteger;
  TahaInteger = Int64;

  PahaCharacter = ^TahaCharacter;
  TahaCharacter = WideChar;

  IahaArray = interface
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
    function write(out value): Boolean;
  end;

  IahaObject = interface
    function state(out value): Boolean;
  end;

  IahaSequence = interface(IahaObject)
    function skip(out new: IahaSequence): Boolean;
  end;

  { TahaObject }

  TahaObject = class(TInterfacedObject, IahaObject)
  protected
    function state(out value): Boolean; virtual;
    function get(out value): Boolean;
    function copy(out value): Boolean; virtual;
  end;

  TahaComposite = TInterfacedObject;

  TahaOpaque = TInterfacedObject;

  { TahaFooArrayWrapper }

  TahaFooArrayWrapper = class(TInterfacedObject, IahaArray)
  private
    FSize: TahaInteger;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
    function write(out value): Boolean;
  public
    constructor Create(const content: TahaInteger);
  end;

  TahaCharArray = UnicodeString;

  { TahaCharArrayWrapper }

  TahaCharArrayWrapper = class(TInterfacedObject, IahaArray)
  private
    FSize: TahaInteger;
    FItems: PahaCharacter;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
    function write(out value): Boolean;
  public
    constructor Create(const content: TahaCharArray);
    destructor Destroy; override;
  end;

  TahaIntArray = array of TahaInteger;

  { TahaIntArrayWrapper }

  TahaIntArrayWrapper = class(TInterfacedObject, IahaArray)
  private
    FSize: TahaInteger;
    FItems: PahaInteger;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
    function write(out value): Boolean;
  public
    constructor Create(const content: TahaIntArray);
    destructor Destroy; override;
  end;

  { TahaSegmentWrapper }

  TahaSegmentWrapper = class(TInterfacedObject, IahaArray)
  private
    FLow: TahaInteger;
    FHigh: TahaInteger;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
    function write(out value): Boolean;
  public
    constructor Create(const alow, ahigh: TahaInteger);
  end;

  TahaOtherArray = array of IUnknown;

  { TahaOherArrayWrapper }

  TahaOherArrayWrapper = class(TInterfacedObject, IahaArray)
  private
    FArray: TahaOtherArray;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
    function write(out value): Boolean;
  public
    constructor Create(const content: TahaOtherArray);
  end;

  IahaFooRelation = interface
    function Check: Boolean;
  end;

  IahaUnaryRelation = interface
    function Check(const param): Boolean;
  end;

  IahaBinaryRelation = interface
    function Check(const param1, param2): Boolean;
  end;

  IahaUnaryFunction = interface
    function Get(const param; out value): Boolean;
  end;

  IahaBinaryFunction = interface
    function Get(const param1, param2; out value): Boolean;
  end;

  TahaFunction = TInterfacedObject;

function CharArrayToString(const a: IahaArray; out s: UnicodeString): Boolean; inline;

function IntPlus(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntMinus(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntMult(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntDiv(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntMod(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntLess(const a, b: TahaInteger): Boolean; inline;
function IntLessEqual(const a, b: TahaInteger): Boolean; inline;
function IntEqual(const a, b: TahaInteger): Boolean; inline;
function IntNotEqual(const a, b: TahaInteger): Boolean; inline;
function IntGreaterEqual(const a, b: TahaInteger): Boolean; inline;
function IntGreater(const a, b: TahaInteger): Boolean; inline;
function CharEqual(const a, b: TahaCharacter): Boolean; inline;
function CharNotEqual(const a, b: TahaCharacter): Boolean; inline;

function IntSortArray(const param: IahaArray; const rel: IahaBinaryRelation; out value: IahaSequence): Boolean;

implementation

{ TahaObject }

function TahaObject.state(out value): Boolean;
begin
  Result := True;
end;

function TahaObject.get(out value): Boolean;
begin
  if RefCount > 1 then
    Result := copy(value)
  else
    begin
      IahaObject(value) := Self;
      Result := True;
    end;
end;

function TahaObject.copy(out value): Boolean;
begin
  Result := True;
end;

{ TahaSegmentWrapper }

function TahaSegmentWrapper.size(out value: TahaInteger): Boolean;
begin
  value := FHigh - FLow;
  Result := True;
end;

function TahaSegmentWrapper.at(const index: TahaInteger; out value): Boolean;
begin
  TahaInteger(value) := FLow + index;
end;

function TahaSegmentWrapper.write(out value): Boolean;
var
  i: TahaInteger;
  p: PahaInteger;
begin
  i := FLow;
  p := @value;
  while i < FHigh do
  begin
    p^ := i;
    Inc(i);
    Inc(p);
  end;
end;

constructor TahaSegmentWrapper.Create(const alow, ahigh: TahaInteger);
begin
  Inherited Create;
  FLow := alow;
  FHigh := ahigh;
end;

{ TahaOherArrayWrapper }

function TahaOherArrayWrapper.size(out value: TahaInteger): Boolean;
begin
  value := Length(FArray);
  Result := True;
end;

function TahaOherArrayWrapper.at(const index: TahaInteger; out value): Boolean;
begin
  Result := (index >= 0) and (index < Length(FArray));
  if Result then
    IUnknown(value) := FArray[index];
end;

function TahaOherArrayWrapper.write(out value): Boolean;
var
  i: TahaInteger;
  p: ^IUnknown;
begin
  i := 0;
  p := @value;
  while i < Length(FArray) do
  begin
    p^ := FArray[i];
    Inc(i);
    Inc(p);
  end;
  Result := True;
end;

constructor TahaOherArrayWrapper.Create(const content: TahaOtherArray);
begin
  inherited Create;
  FArray := content;
end;

{ TahaIntArrayWrapper }

function TahaIntArrayWrapper.size(out value: TahaInteger): Boolean;
begin
  value := FSize;
  Result := True;
end;

function TahaIntArrayWrapper.at(const index: TahaInteger; out value): Boolean;
begin
  Result := (index >= 0) and (index < FSize);
  if Result then
    TahaInteger(value) := FItems[index];
end;

function TahaIntArrayWrapper.write(out value): Boolean;
begin
  Move(FItems^, value, FSize * SizeOf(TahaInteger));
  Result := True;
end;

constructor TahaIntArrayWrapper.Create(const content: TahaIntArray);
begin
  inherited Create;
  FSize := Length(content);
  GetMem(FItems, FSize * SizeOf(TahaCharacter));
  Move(content[1], FItems^, FSize * SizeOf(TahaCharacter));
end;

destructor TahaIntArrayWrapper.Destroy;
begin
  FreeMem(FItems);
  inherited Destroy;
end;

{ TahaCharArrayWrapper }

function TahaCharArrayWrapper.size(out value: TahaInteger): Boolean;
begin
  value := FSize;
  Result := True;
end;

function TahaCharArrayWrapper.at(const index: TahaInteger; out value): Boolean;
begin
  Result := (index >= 0) and (index < FSize);
  if Result then
    TahaCharacter(value) := FItems[index];
end;

function TahaCharArrayWrapper.write(out value): Boolean;
begin
  Move(FItems^, value, FSize * SizeOf(TahaCharacter));
  Result := True;
end;

constructor TahaCharArrayWrapper.Create(const content: TahaCharArray);
begin
  inherited Create;
  FSize := Length(content);
  GetMem(FItems, FSize * SizeOf(TahaCharacter));
  Move(content[1], FItems^, FSize * SizeOf(TahaCharacter));
end;

destructor TahaCharArrayWrapper.Destroy;
begin
  FreeMem(FItems);
  inherited Destroy;
end;

{ TahaFooArrayWrapper }

function TahaFooArrayWrapper.size(out value: TahaInteger): Boolean;
begin
  value := FSize;
  Result := True;
end;

function TahaFooArrayWrapper.at(const index: TahaInteger; out value): Boolean;
begin
  Result := (index >= 0) and (index < FSize);
end;

function TahaFooArrayWrapper.write(out value): Boolean;
begin
  Result := True;
end;

constructor TahaFooArrayWrapper.Create(const content: TahaInteger);
begin
  FSize := content;
end;

{$O+}

function CharArrayToString(const a: IahaArray; out s: UnicodeString): Boolean; inline;
var
  l: TahaInteger;
begin
  if a.size(l) then
    begin
      try
        SetLength(s, l);
        Result := a.write(s[1]);
      except
        Result := False;
      end;
    end
  else
    Result := False;
end;

function IntPlus(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
begin
  Result := True;
  try
    c := a + b;
  except
    Result := False;
  end;
end;

function IntMinus(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
begin
  Result := True;
  try
    c := a - b;
  except
    Result := False;
  end;
end;

function IntMult(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
begin
  Result := True;
  try
    c := a * b;
  except
    Result := False;
  end;
end;

function IntDiv(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
begin
  Result := True;
  try
    c := a div b;
  except
    Result := False;
  end;
end;

function IntMod(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
begin
  Result := True;
  try
    c := a mod b;
  except
    Result := False;
  end;
end;

function IntLess(const a, b: TahaInteger): Boolean; inline;
begin
  Result := a < b;
end;

function IntLessEqual(const a, b: TahaInteger): Boolean; inline;
begin
  Result := a <= b;
end;

function IntEqual(const a, b: TahaInteger): Boolean; inline;
begin
  Result := a = b;
end;

function IntNotEqual(const a, b: TahaInteger): Boolean; inline;
begin
  Result := a <> b;
end;

function IntGreaterEqual(const a, b: TahaInteger): Boolean; inline;
begin
  Result := a >= b;
end;

function IntGreater(const a, b: TahaInteger): Boolean; inline;
begin
  Result := a > b;
end;

function CharEqual(const a, b: TahaCharacter): Boolean; inline;
begin
  Result := a = b;
end;

function CharNotEqual(const a, b: TahaCharacter): Boolean; inline;
begin
  Result := a <> b;
end;

function IntSortArray(const param: IahaArray; const rel: IahaBinaryRelation; out value: IahaSequence): Boolean;
begin

end;

end.

