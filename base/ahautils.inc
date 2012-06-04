type
  PItem = ^TItem;

  { TahaArray }

  TahaArray = class(TInterfacedObject, IahaArray)
  private
    FArray: TArray;
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
    constructor Create(arr: TArray);
  end;

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

  { TahaArraySeq }

  TahaArraySeq = class(TInterfacedObject, IahaSequence)
  private
    FArray: TArray;
    FNext: Int64;
  protected
    function getState(out output): Boolean;
    function next(new: Boolean; out output: IahaSequence): Boolean;
  public
    constructor Create(const arr: TArray; const first: Int64);
  end;

  { TahaSingleItemSeq }

  TahaSingleItemSeq = class(TInterfacedObject, IahaSequence)
  private
    FItem: TItem;
  protected
    function getState(out output): Boolean;
    function next(new: Boolean; out output: IahaSequence): Boolean;
  public
    constructor Create(const item: TItem);
  end;

function TahaSingleItemSeq.getState(out output): Boolean;
begin
  Result := True;
  TItem(output) := FItem;
end;

function TahaSingleItemSeq.next(new: Boolean; out output: IahaSequence
  ): Boolean;
begin
  Result := EmptySeq(output);
end;

constructor TahaSingleItemSeq.Create(const item: TItem);
begin
  FItem := item;
end;

{ TahaArraySeq }

function TahaArraySeq.getState(out output): Boolean;
var
  count: Int64;
begin
  Result := FNext < Length(FArray);
  if Result then
    TItem(output) := FArray[FNext];
end;

function TahaArraySeq.next(new: Boolean; out output: IahaSequence): Boolean;
var
  list: TahaArraySeq;
  count: Int64;
  item: TItem;
  dummy: Int64;
begin
  Result := FNext < Length(FArray);
  if Result then begin
    if new or (RefCount > 1) then begin
      try
        list := TahaArraySeq.Create(FArray, FNext + 1);
        Result := list.getState(dummy);
        if Result then
          output := list;
      except
        Result := False;
      end;
    end
    else begin
      Inc(FNext);
      output := Self;
    end;
  end;
end;

constructor TahaArraySeq.Create(const arr: TArray; const first: Int64);
var
  item: TItem;
begin
  FArray := arr;
  FNext := first;
end;

function TahaArray.at(const index: Int64; out output): Boolean;
begin
  Result := (index >= 0) and (index < Length(FArray));
  if Result then
    TItem(output) := FArray[index];
end;

function TahaArray.size(out output: Int64): Boolean;
begin
  Result := True;
  output := Length(FArray);
end;

constructor TahaArray.Create(arr: TArray);
begin
  FArray := arr;
end;

function TahaArrayList.getState(out output): Boolean;
var
  count: Int64;
begin
  Result := FArray.size(count) and (FNext < count);
  if Result then
    Result := FArray.at(FNext, output);
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
    if new or (RefCount > 1) then begin
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

constructor TahaArrayList.Create(const arr: IahaArray; const rel: IahaUnaryRelation; const first: Int64);
var
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

function TahaArray.apply(const func: IahaBinaryFunction; out output): Boolean;
var
  i: Cardinal;
  l: Cardinal;
  acc, res: TItem;
begin
  l := Length(FArray);
  if l > 0 then
  begin
    Result := True;
    acc := FArray[0];
    i := 1;
    while i < l do begin
      if func.get(acc, FArray[i], res) then
        acc := res
      else begin
        Result := False;
        Exit;
      end;
      Inc(i);
    end;
    TItem(output) := acc;
  end
  else
    Result := False;
end;

function TahaArray.splitmerge(const func: IahaBinaryFunction; out output: IahaArray): Boolean;
var
  i: Cardinal;
  l: Cardinal;
  arr: array of IahaArray;
  temp: TArray;
  res: IahaArray;
begin
  try
    l := Length(FArray);
    if l > 0 then
    begin
      SetLength(arr, l);
      SetLength(temp, 1);
      for i := 0 to l - 1 do begin
        temp[0] := FArray[i];
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

type

  { TSortedInit }

  TSortedInit = class(TInterfacedObject, IahaUnaryFunction)
  private
    FArray: TArray;
    FFunc: IahaBinaryRelation;
  protected
    function get(const x; out output): Boolean;
  public
    constructor Create(arr: TArray);
  end;

  { TSortedMerge }

  TSortedMerge = class(TInterfacedObject, IahaUnaryFunction)
  private
    FSorted: IahaUnaryFunction;
    FFunc: IahaBinaryRelation;
  protected
    function get(const x; out output): Boolean;
  public
    constructor Create(const func: IahaBinaryRelation; const sorted: IahaUnaryFunction);
  end;

  { TSortedSeqMerge }

  TSortedSeqMerge = class(TInterfacedObject, IahaSequence)
  private
    FSeq1: IahaSequence;
    FSeq2: IahaSequence;
    FFunc: IahaBinaryRelation;
  protected
    function getState(out output): Boolean;
    function next(new: Boolean; out output: IahaSequence): Boolean;
  public
    constructor Create(const func: IahaBinaryRelation; const seq1, seq2: IahaSequence);
  end;

{ TSortedSeqMerge }

function TSortedSeqMerge.getState(out output): Boolean;
var
  item1: TItem;
  item2: TItem;
begin
  if FSeq1.getState(item1) then begin
    if FSeq2.getState(item2) then begin
      if FFunc.check(item1, item2) then
        TItem(output) := item1
      else
        TItem(output) := item2;
      Result := True;
    end else
      Result := False;
  end else begin
    if FSeq2.getState(item2) then begin
      TItem(output) := item2;
      Result := True;
    end else
      Result := False;
  end;
end;

function TSortedSeqMerge.next(new: Boolean; out output: IahaSequence): Boolean;
var
  item1: TItem;
  item2: TItem;
begin
  if FSeq1.getState(item1) then begin
    if FSeq2.getState(item2) then begin
      if FFunc.check(item1, item2) then
        Result := FSeq1.next(new, output)
      else
        Result := FSeq2.next(new, output);
    end else
      Result := False;
  end else begin
    if FSeq2.getState(item2) then begin
      Result := FSeq2.next(new, output);
    end else
      Result := False;
  end;
end;

constructor TSortedSeqMerge.Create(const func: IahaBinaryRelation; const seq1, seq2: IahaSequence);
begin
  FSeq1 := seq1;
  FSeq2 := seq2;
  FFunc := func;
end;

{ TSortedMerge }

function TSortedMerge.get(const x; out output): Boolean;
var
  i, i1, i2: Int64;
  seq1, seq2: IahaSequence;
begin
  try
    i := Int64(x);
    i1 := i + i;
    i2 := i1 + 1;
    Result := FSorted.get(i1, seq1) and FSorted.get(i2, seq2);
    if Result then begin
      IahaSequence(output) := TSortedSeqMerge.Create(FFunc, seq1, seq2);
    end;
  except
    Result := False;
  end;
end;

constructor TSortedMerge.Create(const func: IahaBinaryRelation; const sorted: IahaUnaryFunction);
begin
  FSorted := sorted;
  FFunc := func;
end;

{ TSortedInit }

function TSortedInit.get(const x; out output): Boolean;
var
  i: Int64;
begin
  try
    i := Int64(x);
    if i > High(FArray) then
      Result := EmptySeq(IahaSequence(output))
    else begin
      IahaSequence(output) := TahaSingleItemSeq.Create(FArray[i]);
      Result := True;
    end;
  except
    Result := False;
  end;
end;

constructor TSortedInit.Create(arr: TArray);
begin
  FArray := arr;
end;

function TahaArray.sort(const func: IahaBinaryRelation; out output: IahaSequence): Boolean;
var
  cnt: Int64;
  sorted: IahaUnaryFunction;
  zero: Int64;
begin
  try
    if Length(FArray) = 0 then begin
      Result := EmptySeq(output);
    end else begin
      cnt := Length(FArray);
      sorted := TSortedInit.Create(FArray);
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

function TahaArray.forEach(const func: IahaUnaryRelation): Boolean;
var
  i: Cardinal;
  l: Cardinal;
  item: PItem;
begin
  l := Length(FArray);
  i := 0;
  item := @FArray[0];
  while i < l do begin
    if not func.check(item^) then
    begin
      Result := False;
      Exit;
    end;
    Inc(i);
    Inc(item);
  end;
  Result := True;
end;

function TahaArray.forSome(const func: IahaUnaryRelation): Boolean;
var
  i: Cardinal;
  l: Cardinal;
  item: PItem;
begin
  l := Length(FArray);
  i := 0;
  item := @FArray[0];
  while i < l do begin
    if func.check(item^) then
    begin
      Result := True;
      Exit;
    end;
    Inc(i);
    Inc(item);
  end;
  Result := False;
end;

function AhaArrayOf(arr: array of TItem; out output: IahaArray): Boolean; inline;
var
  temp: TArray;
  i: Cardinal;
begin
  try
    SetLength(temp, Length(arr));
    for i := 0 to High(arr) do
      temp[i] := arr[i];
    output := TahaArray.Create(temp);
    Result := True;
  except
    Result := False;
  end;
end;

function AhaArrayFromArray(arr: TArray; out output: IahaArray): Boolean; inline;
begin
  try
    output := TahaArray.Create(arr);
    Result := True;
  except
    Result := False;
  end;
end;

function AhaArrayFromFunc(const func: IahaUnaryFunction; const count: Int64;
  out output: IahaArray): Boolean;
var
  i: Int64;
  item: TItem;
  temp: TArray;
begin
  try
    SetLength(temp, count);
    i := 0;
    while i < count do begin
      if not func.get(i, temp[i]) then
      begin
        Result := False;
        Exit;
      end;
      Inc(i);
    end;
    output := TahaArray.Create(temp);
    Result := True;
  except
    Result := False;
  end;
end;

function AhaArrayFromSeq(const seq: IahaSequence; const count: Int64;
  out output: IahaArray): Boolean;
var
  curr: IahaSequence;
  next: IahaSequence;
  i: Int64;
  temp: TArray;
begin
  try
    SetLength(temp, count);
    i := 0;
    curr := seq;
    while i < count do begin
      if not curr.getState(temp[i]) then begin
        Result := False;
        Exit;
      end;
      Inc(i);
      if i < count then begin
        if curr.next(False, next) then
          curr := next
        else begin
          Result := False;
          Exit;
        end;
      end;
    end;
    output := TahaArray.Create(temp);
    Result := True;
  except
    Result := False;
  end;
end;

function TahaArray.select(const func: IahaUnaryRelation;
  out output: IahaArray): Boolean;
var
  temp: TArray;
  l: Cardinal;
  i: Cardinal;
  j: Cardinal;
  item, copy: PItem;
begin
  try
    l := Length(FArray);
    SetLength(temp, l);
    i := 0;
    j := 0;
    item := @FArray[0];
    copy := @temp[0];
    while i < l do begin
      if func.check(item^) then begin
        copy^ := item^;
        Inc(j);
        Inc(copy);
      end;
      Inc(i);
      Inc(item);
    end;
    SetLength(temp, j);
    output := TahaArray.Create(temp);
    Result := True;
  except
    Result := False;
  end;
end;

function TahaArray.count(const func: IahaUnaryRelation;
  out output: Int64): Boolean;
var
  i: Cardinal;
  l: Cardinal;
  item: PItem;
begin
  l := Length(FArray);
  i := 0;
  item := @FArray[0];
  output := 0;
  while i < l do begin
    if func.check(item^) then
      Inc(output);
    Inc(i);
    Inc(item);
  end;
  Result := True;
end;

function TahaArray.such(const func: IahaUnaryRelation; out output): Boolean;
var
  l: Cardinal;
  i: Cardinal;
  item: PItem;
begin
  l := Length(FArray);
  i := 0;
  item := @FArray[0];
  while i < l do begin
    if func.check(item^) then begin
      TItem(output) := item^;
      Result := True;
      Exit;
    end;
    Inc(i);
    Inc(item);
  end;
  Result := False;
end;

function AhaSeqSelect(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: IahaArray): Boolean;
var
  i: Int64;
  item: TItem;
  curr, next: IahaSequence;
  temp: TArray;
  j: Int64;
begin
  try
    if count >= 0 then begin
      SetLength(temp, count);
      i := 0;
      j := 0;
      curr := seq;
      while i < count do begin
        if curr.getState(item) and func.check(item) then begin
          temp[j] := item;
          Inc(j);
        end else begin
          Result := False;
          Exit;
        end;
        if not curr.next(False, next) then
          Break;
        curr := next;
        Inc(i);
      end;
      if j <> count then
        SetLength(temp, j);
      Result := True;
      output := TahaArray.Create(temp);
    end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function AhaSeqSuch(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: TItem): Boolean;
var
  i: Int64;
  item: TItem;
  curr, next: IahaSequence;
begin
  i := 0;
  curr := seq;
  while i < count do begin
    if curr.getState(item) and func.check(item) then begin
      output := item;
      Result := True;
      Exit;
    end;
    if not curr.next(False, next) then
      Break;
    curr := next;
    Inc(i);
  end;
  Result := False;
end;

function AhaArrayAppend(arr: array of IahaArray; out output: IahaArray): Boolean;
var
  i: Cardinal;
  k: Cardinal;
  l: Cardinal;
  count: Int64;
  temp: TArray;
begin
  try
    l := 0;
    for i := 0 to High(arr) do begin
      if not arr[i].size(count) then begin
        Result := False;
        Exit;
      end;
      Inc(l, count);
    end;
    SetLength(temp, l);
    k := 0;
    for i := 0 to High(arr) do begin
      if arr[i].size(count) then begin
        if not arr[i].write(temp[k]) then begin
          Result := False;
          Exit;
        end;
        Inc(k, count);
      end
      else begin
        Result := False;
        Exit;
      end;
    end;
    output := TahaArray.Create(temp);
    Result := True;
  except
    Result := False;
  end;
end;

function TahaArray.list(const func: IahaUnaryRelation; out output: IahaSequence): Boolean;
begin
  try
    output := TahaArrayList.Create(Self, func, 0);
    Result := True;
  except
    Result := False;
  end;
end;

function TahaArray.write(out output): Boolean;
{$IFDEF Streamline}
begin
  if Length(FArray) > 0 then
    System.Move(FArray[0], output, Length(FArray) * SizeOf(TItem));
  Result := True;
end;
{$ELSE}
var
  i: Cardinal;
  l: Cardinal;
  item, copy: PItem;
begin
  l := Length(FArray);
  i := 0;
  item := @FArray[0];
  copy := @output;
  while i < l do begin
    copy^ := item^;
    Inc(i);
    Inc(item);
    Inc(copy);
  end;
  Result := True;
end;
{$ENDIF}

type

  { TahaGroupSeq }

  TahaGroupSeq = class(TInterfacedObject, IahaSequence)
  private
    FSeq: IahaSequence;
    FFunc: IahaBinaryRelation;
    FCount: Int64;
  protected
    function getState(out output): Boolean;
    function next(new: Boolean; out output: IahaSequence): Boolean;
  public
    constructor Create(const seq: IahaSequence; const func: IahaBinaryRelation; const count: Int64);
  end;

{ TahaGroupSeq }

function TahaGroupSeq.getState(out output): Boolean;
var
  temp: TArray;
  first, curr: TItem;
  prev, nxt: IahaSequence;
  i: Cardinal;
begin
  if FSeq.getState(first) then begin
    try
      SetLength(temp, FCount);
      temp[0] := first;
      i := 1;
      prev := FSeq;
      Result := True;
      while i < FCount do begin
        if prev.next(False, nxt) then begin
          if nxt.getState(curr) then begin
            if FFunc.check(first, curr) then
              temp[i] := curr
            else
              Break;
          end else
            Break;
        end else
          Break;
        prev := nxt;
        Inc(i);
      end;
      SetLength(temp, i);
      IahaArray(output) := TahaArray.Create(temp);
    except
      Result := False;
    end;
  end else
    Result := False;
end;

function TahaGroupSeq.next(new: Boolean; out output: IahaSequence): Boolean;
var
  first, curr: TItem;
  prev, nxt: IahaSequence;
  i: Cardinal;
begin
  if FSeq.getState(first) then begin
    try
      i := 1;
      prev := FSeq;
      Result := False;
      while i < FCount do begin
        if prev.next(False, nxt) then begin
          if nxt.getState(curr) then begin
            if not FFunc.check(first, curr) then
            begin
              Result := True;
              IahaSequence(output) := nxt;
              Exit;
            end;
          end else
            Exit;
        end else begin
          Exit;
        end;
        prev := nxt;
        Inc(i);
      end;
      Result := True;
      IahaSequence(output) := prev;
    except
      Result := False;
    end;
  end else
    Result := False;
end;

constructor TahaGroupSeq.Create(const seq: IahaSequence; const func: IahaBinaryRelation;
  const count: Int64);
begin
  FSeq := seq;
  FFunc := func;
  FCount := count;
end;

function AhaGroup(const seq: IahaSequence; const func: IahaBinaryRelation; const count: Int64;
  out output: IahaSequence): Boolean;
begin
  try
    IahaSequence(output) := TahaGroupSeq.Create(seq, func, count);
    Result := True
  except
    Result := False;
  end;
end;
