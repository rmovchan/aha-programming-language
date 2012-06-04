unit AhaNilUtils;

{$mode objfpc}{$H+}

interface

uses
  AhaCore;

type
  TItem = TahaNil;

function AhaArrayOf(nils: Cardinal; out output: IahaArray): Boolean; inline;

function AhaArrayAppend(arr: array of IahaArray; out output: IahaArray): Boolean;

function AhaArrayFromFunc(const func: IahaUnaryFunction; const count: Int64;
  out output: IahaArray): Boolean; inline;

function AhaArrayFromSeq(const seq: IahaSequence; const count: Int64;
  out output: IahaArray): Boolean; inline;

function AhaSeqSelect(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: IahaArray): Boolean;

function AhaSeqSuch(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: TItem): Boolean;

implementation

type

  { TahaArray }

  TahaArray = class(TInterfacedObject, IahaArray)
  private
    FCount: Int64;
  protected
    function at(const index: Int64; out output): Boolean;
    function size(out output: Int64): Boolean;
    function apply(const func: IahaBinaryFunction; out output): Boolean;
    function splitmerge(const func: IahaBinaryFunction; out output: IahaArray): Boolean;
    function forEach(const func: IahaUnaryRelation): Boolean;
    function forSome(const func: IahaUnaryRelation): Boolean;
    function select(const func: IahaUnaryRelation; out output: IahaArray): Boolean;
    function count(const func: IahaUnaryRelation; out output: Int64): Boolean;
    function list(const func: IahaUnaryRelation; out output: IahaSequence): Boolean;
    function such(const func: IahaUnaryRelation; out output): Boolean;
    function write(out output): Boolean;
  public
    constructor Create(const acount: Int64);
  end;

  { TahaNilArrayFromSeq }

  TahaNilArrayFromSeq = class(TInterfacedObject, IahaArray)
  private
    FSeq: IahaSequence;
    FLength: Int64;
    FCount: Int64;
  protected
    function at(const index: Int64; out output): Boolean;
    function count(out output: Int64): Boolean;
  public
    constructor Create(const seq: IahaSequence; const acount: Int64);
  end;

  { TahaArrayList }

  TahaArrayList = class(TInterfacedObject, IahaSequence)
  private
    FArray: IahaArray;
    FRel: IahaUnaryRelation;
    FNext: Int64;
  protected
    function getState(out output): Boolean;
    function next(new: Boolean; out output: IahaSequence): Boolean;
  public
    constructor Create(const arr: IahaArray; const rel: IahaUnaryRelation; const first: Int64);
  end;

function AhaArrayOf(nils: Cardinal; out output: IahaArray): Boolean;
begin
  try
    output := TahaArray.Create(nils);
    Result := True;
  except
    Result := False;
  end;
end;

function AhaArrayAppend(arr: array of IahaArray; out output: IahaArray): Boolean;
var
  i: Cardinal;
  count: Int64;
  total: Int64;
begin
  total := 0;
  for i := 0 to High(arr) do begin
    if arr[i].size(count) then
      Inc(total, count)
    else begin
      Result := False;
      Exit;
    end;
  end;
  Result := True;
  output := TahaArray.Create(total);
end;

function TahaArray.apply(const func: IahaBinaryFunction;
  out output: TItem): Boolean;
begin
  if FCount > 0 then begin
    Result := True;
  end
  else
    Result := False;
end;

function TahaArray.forEach(const func: IahaUnaryRelation): Boolean;
begin
  Result := True;
end;

function TahaArray.forSome(const func: IahaUnaryRelation): Boolean;
begin
  Result := False;
end;

function AhaArrayFromFunc(const func: IahaArrayFunction; const count: Int64;
  out output: IahaArray): Boolean;
begin

end;

function AhaArrayFromSeq(const seq: IahaSequence; const count: Int64;
  out output: IahaArray): Boolean;
begin

end;

function TahaArray.select(const func: IahaUnaryRelation;
  out output: IahaArray): Boolean;
begin
  output := TahaArray.Create(FCount);
  Result := True;
end;

function TahaArray.count(const func: IahaUnaryRelation;
  out output: Int64): Boolean;
begin
  Result := True;
  output := FCount;
end;

function TahaArray.list(const func: IahaUnaryRelation;
  out output: IahaSequence): Boolean;
begin
  try
    output := TahaArrayList.Create(arr, func, 0);
    Result := True;
  except
    Result := False;
  end;
end;

function TahaArray.such(const func: IahaUnaryRelationIahaUnaryRelation;
  out output: TItem): Boolean;
begin
  Result := False;
end;

function AhaSeqSelect(const arr: IahaArray; const func: IahaUnaryRelation;
  const count: Int64; out output: IahaArray): Boolean;
begin

end;

function AhaSeqSuch(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: TItem): Boolean;
begin

end;

{ TahaArrayList }

function TahaArrayList.getState(out output): Boolean;
var
  count: Int64;
begin
  Result := FArray.size(count) and (FNext < count);
  if Result then
    Int64(output) := FNext;
end;

function TahaArrayList.next(new: Boolean; out output: IahaSequence): Boolean;
var
  list: TahaArrayList;
  count: Int64;
  item: TItem;
  dummy: Int64;
begin
  Result := FArray.size(count) and (FNext < count);
  if Result then begin
    if new then begin
      try
        list := TahaArrayList.Create(FArray, FRel, FNext + 1);
        Result := list.getState(dummy);
        if Result then
          output := list;
      except
        Result := False;
      end;
    end
    else begin
      Inc(FNext);
      while FNext < count do begin
        if FArray.at(FNext, item) then begin
          if FRel.check(item) then begin
            output := Self;
            Exit;
          end
          else
            Inc(FNext);
        end
        else
          Break;
      end;
      Result := False;
    end;
  end;
end;

constructor TahaArrayList.Create(const arr: IahaArray; const rel: IahaUnaryRelation;
  const first: Int64);
var
  list: TahaArrayList;
  count: Int64;
  item: TItem;
begin
  FArray := arr;
  FRel := rel;
  FArray.size(count);
  FNext := first;
  while FNext < count do begin
    if FArray.at(FNext, item) then begin
      if FRel.check(item) then begin
        Break;
      end
      else
        Inc(FNext);
    end
    else begin
      FNext := count;
      Break;
    end;
  end;
end;

{ TahaNilArrayFromSeq }

function TahaNilArrayFromSeq.at(const index: Int64; out output): Boolean;
var
  seq: IahaSequence;
  temp: TItem;
begin
  Result := (index >= 0) and (index < FLength);
  if Result then begin
    if index < FCount then
      FCount := 0;
    while FCount <> index do begin
      if not FSeq.getState(temp) then begin
        Result := False;
        Exit;
      end;
      Inc(FCount);
      FSeq.next(False, seq);
      FSeq := seq;
    end;
    Result := FSeq.getState(output);
    Inc(FCount);
    FSeq.next(False, seq);
    FSeq := seq;
  end;
end;

function TahaNilArrayFromSeq.count(out output: Int64): Boolean;
begin
  Result := True;
  output := FLength;
end;

constructor TahaNilArrayFromSeq.Create(const seq: IahaSequence;
  const acount: Int64);
begin
  FSeq := seq;
  FLength := acount;
end;

{ TahaArray }

function TahaArray.at(const index: Int64; out output): Boolean;
begin
  Result := (index >= 0) and (index < FCount);
end;

function TahaArray.size(out output: Int64): Boolean;
begin
  Result := True;
  output := FCount;
end;

function TahaArray.splitmerge(const func: IahaBinaryFunction; out output: IahaArray
  ): Boolean;
begin

end;

function TahaArray.write(out output): Boolean;
begin

end;

constructor TahaArray.Create(const acount: Int64);
begin
  FCount := acount;
end;

end.

