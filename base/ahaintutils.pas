unit AhaIntUtils;

{$mode objfpc}{$H+}

interface

uses
  AhaCore;

type
  TItem = Int64;
  TArray = array of TItem;

function AhaArrayOf(arr: array of TItem; out output: IahaArray): Boolean; inline;

function AhaArrayFromArray(arr: TArray; out output: IahaArray): Boolean; inline;

function AhaArrayAppend(arr: array of IahaArray; out output: IahaArray): Boolean;

function AhaArrayFromFunc(const func: IahaUnaryFunction; const count: Int64;
  out output: IahaArray): Boolean; inline;

function AhaArrayFromSeq(const seq: IahaSequence; const count: Int64;
  out output: IahaArray): Boolean; inline;

function AhaSeqSelect(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: IahaArray): Boolean;

function AhaSeqSuch(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: TItem): Boolean;

function AhaSegment(const x, y: Int64; out output: IahaArray): Boolean; inline;

function AhaGroup(const seq: IahaSequence; const func: IahaBinaryRelation; const count: Int64;
  out output: IahaSequence): Boolean;

implementation

{$DEFINE Streamline}
{$I ahautils.inc}

type
  { TSegment }

  TSegment = class(TInterfacedObject, IahaArray)
  private
    FLow: Int64;
    FHigh: Int64;
  protected
    function at(const index: Int64; out output): Boolean;
    function size(out output: Int64): Boolean;
    function apply(const func: IahaBinaryFunction; out output): Boolean;
    function splitmerge(const func: IahaBinaryFunction; out output: IahaArray): Boolean;
    function sort(const func: IahaBinaryRelation; out output: IahaSequence): Boolean;
    function forEach(const func: IahaUnaryRelation): Boolean;
    function forSome(const func: IahaUnaryRelation): Boolean;
    function select(const func: IahaUnaryRelation; out output: IahaArray): Boolean;
    function count(const func: IahaUnaryRelation; out output: Int64): Boolean;
    function list(const func: IahaUnaryRelation; out output: IahaSequence): Boolean;
    function such(const func: IahaUnaryRelation; out output): Boolean;
    function write(out output): Boolean;
  public
    constructor Create(const low, high: Int64);
  end;

  function AhaSegment(const x, y: Int64; out output: IahaArray): Boolean;
  begin
    Result := x <= y;
    if Result then
      try
        output := TSegment.Create(x, y);
        Result := True;
      except
        Result := False;
      end;
  end;

{ TSegment }

function TSegment.at(const index: Int64; out output): Boolean;
begin
  Result := (index >= 0) and (FLow + index < FHigh);
  if Result then
    try
      Int64(output) := index + FLow;
    except
      Result := False;
    end;
end;

function TSegment.size(out output: Int64): Boolean;
begin
  Result := True;
  output := FHigh - FLow;
end;

function TSegment.apply(const func: IahaBinaryFunction; out output): Boolean;
var
  i: Int64;
  acc, res: TItem;
begin
  Result := True;
  acc := FLow;
  i := FLow + 1;
  while i < FHigh do begin
    if func.get(acc, i, res) then
      acc := res
    else begin
      Result := False;
      Exit;
    end;
    Inc(i);
  end;
  TItem(output) := acc;
end;

function TSegment.splitmerge(const func: IahaBinaryFunction; out output: IahaArray): Boolean;
var
  i: Cardinal;
  l: Cardinal;
  arr: array of IahaArray;
  temp: TArray;
  res: IahaArray;
begin
  try
    l := FHigh - FLow;
    if l > 0 then
    begin
      SetLength(arr, l);
      SetLength(temp, 1);
      for i := 0 to l - 1 do begin
        temp[0] := FLow + i;
        arr[i] := TahaArray.Create(Copy(temp, 0, 1));
      end;
      while l <> 1 do begin
        for i := 0 to l div 2 - 1 do begin
          if func.get(arr[i + i], arr[i + i + 1], res) then
            arr[i] := res
          else begin
            Result := False;
            Exit;
          end;
        end;
        for i := l div 2 to (l + 1) div 2 - 1 do
          arr[i] := arr[i + 1];
        l := (l + 1) div 2;
        SetLength(arr, l);
      end;
      Result := True;
      output := arr[0];
    end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function TSegment.sort(const func: IahaBinaryRelation; out output: IahaSequence
  ): Boolean;
var
  cnt: Int64;
  sorted: IahaUnaryFunction;
  temp: TArray;
  zero: Int64;
begin
  try
    if FHigh = FLow then begin
      Result := EmptySeq(output);
    end else begin
      cnt := FHigh - FLow;
      SetLength(temp, cnt);
      write(temp);
      sorted := TSortedInit.Create(temp);
      while cnt <> 1 do begin
        sorted := TSortedMerge.Create(func, sorted);
        cnt := (cnt + 1) div 2;
      end;
      zero := 0;
      Result := sorted.get(zero, output);
    end;
  except
    Result := False;
  end;
end;

function TSegment.forEach(const func: IahaUnaryRelation): Boolean;
var
  item: Int64;
begin
  item := FLow;
  while item < FHigh do begin
    if not func.check(item) then
    begin
      Result := False;
      Exit;
    end;
    Inc(item);
  end;
  Result := True;
end;

function TSegment.forSome(const func: IahaUnaryRelation): Boolean;
var
  item: Int64;
begin
  item := FLow;
  while item < FHigh do begin
    if func.check(item) then
    begin
      Result := True;
      Exit;
    end;
    Inc(item);
  end;
  Result := False;
end;

function TSegment.select(const func: IahaUnaryRelation; out output: IahaArray): Boolean;
var
  temp: TArray;
  j: Cardinal;
  item: TItem;
  copy: PItem;
begin
  try
    SetLength(temp, FHigh - FLow);
    j := 0;
    item := FLow;
    copy := @temp[0];
    while item < FHigh do begin
      if func.check(item) then begin
        copy^ := item;
        Inc(j);
        Inc(copy);
      end;
      Inc(item);
    end;
    SetLength(temp, j);
    output := TahaArray.Create(temp);
    Result := True;
  except
    Result := False;
  end;
end;

function TSegment.count(const func: IahaUnaryRelation; out output: Int64): Boolean;
var
  item: TItem;
begin
  item := FLow;
  output := 0;
  while item < FHigh do begin
    if func.check(item) then
      Inc(output);
    Inc(item);
  end;
  Result := True;
end;

function TSegment.list(const func: IahaUnaryRelation; out output: IahaSequence): Boolean;
begin
  try
    output := TahaArrayList.Create(Self, func, 0);
    Result := True;
  except
    Result := False;
  end;
end;

function TSegment.such(const func: IahaUnaryRelation; out output): Boolean;
var
  item: TItem;
begin
  item := FLow;
  while item < FHigh do begin
    if func.check(item) then begin
      TItem(output) := item;
      Result := True;
      Exit;
    end;
    Inc(item);
  end;
  Result := False;
end;

function TSegment.write(out output): Boolean;
var
  i: Int64;
  copy: PInt64;
begin
  i := FLow;
  copy := @output;
  while i < FHigh do begin
    copy^ := i;
    Inc(i);
    Inc(copy);
  end;
end;

constructor TSegment.Create(const low, high: Int64);
begin
  FLow := low;
  FHigh := high;
end;

end.

