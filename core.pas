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

  TahaComposite = TInterfacedObject;

  TahaOpaque = TInterfacedObject;

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

function SortIntArray(const param: IahaArray; const rel: IahaBinaryRelation; out value: IahaSequence): Boolean;

implementation


type

{ TahaObject }
  TahaObject = class(TInterfacedObject, IahaObject)
  protected
    function state(out value): Boolean; virtual;
    function get(out value): Boolean;
    function copy(out value): Boolean; virtual;
  end;

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
  //IahaObject(value) := TahaObject.Create;
  Result := True;
end;

{ TahaSegmentWrapper }

function TahaSegmentWrapper.size(out value: TahaInteger): Boolean;
begin
  Result := True;
  value := FHigh - FLow;
end;

function TahaSegmentWrapper.at(const index: TahaInteger; out value): Boolean;
begin
  Result := (index >= 0) and (index < FHigh - FLow);
  if Result then
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
  Result := True;
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

type

  { TArraySlice }

  TArraySlice = class(TahaObject, IahaSequence)
  private
    FArray: IahaArray;
    FIndex: TahaInteger;
    FCount: TahaInteger;
  protected
    function state(out value): Boolean; override;
    function skip(out new: IahaSequence): Boolean;
  end;

  { TMergeSeq }

  generic TMergeSeq<TItem> = class(TahaObject, IahaSequence)
  private
    FSeq1: IahaSequence;
    FSeq2: IahaSequence;
    FRel: IahaBinaryRelation;
  protected
    function state(out value): Boolean; override;
    function skip(out new: IahaSequence): Boolean;
  public
    constructor Create(const Seq1, Seq2: IahaSequence; const rel: IahaBinaryRelation);
  end;

  TMergeIntSeq = specialize TMergeSeq<Integer>;


function SortIntArray(const param: IahaArray; const rel: IahaBinaryRelation; out value: IahaSequence): Boolean;
var
  L, I, J: TahaInteger;
  S: TArraySlice;
  SS: array of IahaSequence;
  A, B: TahaInteger;
begin
  if param.size(L) then
    begin
      Result := True;
      if L > 0 then
        begin
  //split array into ordered slices
          SetLength(SS, L);
          S := TArraySlice.Create;
          S.FArray := param;
          S.FIndex := 0;
          I := 0;
          J := 0;
          while I < L do
          begin
            if I < L - 1 then
              begin
                repeat
                  param.at(I, A);
                  Inc(I);
                  if I = L then Break;
                  param.at(I, B);
                until not rel.Check(A, B);
              end;
            S.FCount := I - S.FIndex;
            SS[J] := S;
            Inc(J);
            Inc(I);
            if I < L then
              begin
                S := TArraySlice.Create;
                S.FArray := param;
                S.FIndex := I;
              end;
          end;
          //SetLength(SS, J);
  //merge slices
          value := SS[0];
          Dec(J);
          while J > 0 do
          begin
            value := TMergeIntSeq.Create(value, SS[J], rel);
            Dec(J);
          end;
        end
      else
        value := TArraySlice.Create;
    end
  else
    Result := False;
end;

{ TArraySlice }

function TArraySlice.state(out value): Boolean;
begin
  Result := FCount > 0;
  if Result then
    Result := FArray.at(FIndex, value);
end;

function TArraySlice.skip(out new: IahaSequence): Boolean;
begin
  if FCount > 0 then
    begin
      Inc(FIndex);
      Dec(FCount);
      new := Self;
    end
  else
    Result := False;
end;

{ TMergeSeq }

function TMergeSeq.state(out value): Boolean;
var
  S1, S2: TItem;
begin
  if (FSeq1.state(S1) and not FSeq2.state(S2)) or FRel.Check(S1, S2) then
    begin
      TItem(value) := S1;
      Result := True;
    end
  else
  if (not FSeq1.state(S1) and FSeq2.state(S2)) or FRel.Check(S2, S1) then
    begin
      TItem(value) := S2;
      Result := True;
    end
  else
    Result := False;
end;

function TMergeSeq.skip(out new: IahaSequence): Boolean;
var
  S1, S2: TItem;
  next: IahaSequence;
begin
  new := Self;
  if (FSeq1.state(S1) and not FSeq2.state(S2)) or FRel.Check(S1, S2) then
    begin
      Result := FSeq1.skip(next);
      if Result then
        FSeq1 := next;
    end
  else
  if (not FSeq1.state(S1) and FSeq2.state(S2)) or FRel.Check(S2, S1) then
    begin
      Result := FSeq2.skip(next);
      if Result then
        FSeq2 := next;
    end
  else
    Result := False;
end;

constructor TMergeSeq.Create(const Seq1, Seq2: IahaSequence; const rel: IahaBinaryRelation);
begin
  inherited Create;
  FSeq1 := Seq1;
  FSeq2 := Seq2;
  FRel := rel;
end;

end.

