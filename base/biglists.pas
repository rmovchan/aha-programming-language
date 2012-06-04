unit BigLists;
(****************************************************************************
 * Description: efficient implementation of very large lists                *
 * Author: Roman Movchan                                                    *
 ****************************************************************************)
{$O+}
{$R-}

interface

const
  BlockSize = 1024;

type

  { TBigList }

  generic TBigList<T> = class(TObject)
  private
    FList: array of array [0 .. BlockSize - 1] of T;
    FCount: Integer;
    function GetCapacity: Integer; inline;
    procedure QuickSort(L, R: Integer);
  protected
    function Get(Index: Integer): T; inline;
    procedure Grow; inline;
    procedure Put(Index: Integer; const Item: T); inline;
    procedure SetCapacity(NewCapacity: Integer); inline;
    function CompareItems(i, j: Integer): Integer; virtual;
  public
    constructor Create;
    destructor Destroy; override;
    function Add(const Item: T): Integer; inline;
    procedure Clear; virtual;
    procedure Delete(Index: Integer); virtual;
    procedure Insert(Index: Integer; const Item: T);
    procedure Exchange(i, j: Integer); inline;
    procedure Sort;
    property Capacity: Integer read GetCapacity write SetCapacity;
    property Count: Integer read FCount;
    property Items[Index: Integer]: T read Get write Put; default;
  end;

implementation

uses
  SysUtils;

{ TBigList }

function TBigList.Add(const Item: T): Integer;
begin
  if Count = BlockSize * Length(FList) then
    Grow;
  Result := Count;
  FList[Result div BlockSize][Result mod BlockSize] := Item;
  Inc(FCount);
end;

procedure TBigList.Clear;
begin
  FCount := 0;
  SetLength(FList, 0);
end;

function TBigList.CompareItems(i, j: Integer): Integer;
begin
  Result := 0;
end;

constructor TBigList.Create;
begin
end;

procedure TBigList.Delete(Index: Integer);
var
  iBlock: Integer;
  i: Integer;
  iLastIndex: Integer;
begin
  Assert((Index >= 0) and (Index < Count), 'Index out of range');
  iBlock := Index div BlockSize;
  i := Index mod BlockSize;
  iLastIndex := BlockSize - 1;
  //Finalize(FList[iBlock][i]); // in case it contains a string or interface
  if i <> iLastIndex then
    Move(FList[iBlock][i + 1], FList[iBlock][i], (iLastIndex - i) * SizeOf(T));
  FillChar(FList[iBlock][iLastIndex - i], SizeOf(T), 0); // to prevent ref count decrement
  while iBlock < High(FList) - 1 do // shift data in remaining blocks
  begin
    FList[iBlock][iLastIndex] := FList[iBlock + 1][0];
    Inc(iBlock);
    if iBlock = High(FList) then
    begin
      i := Count mod BlockSize - 1; // number of items in the last block - 1
      if i <> 0 then
        begin
          Move(FList[iBlock][1], FList[iBlock][0], i * SizeOf(T));
          //FillChar(FList[iBlock][i], SizeOf(T), 0); // to prevent ref count decrement
        end
      else
        begin
          SetLength(FList, Length(FList) - 1);
        end;
    end
    else
      Move(FList[iBlock][1], FList[iBlock][0], iLastIndex * SizeOf(T));
  end;
  Dec(FCount);
end;

destructor TBigList.Destroy;
begin
  Clear;
  inherited;
end;

procedure TBigList.Exchange(i, j: Integer);
var
  Temp: T;
begin
  Assert((i >= 0) and (i < Count), 'Index out of range');
  Assert((j >= 0) and (j < Count), 'Index out of range');
  Temp := FList[i div BlockSize][i mod BlockSize];
  FList[i div BlockSize][i mod BlockSize] := FList[j div BlockSize][j mod BlockSize];
  FList[j div BlockSize][j mod BlockSize] := Temp;
end;

function TBigList.Get(Index: Integer): T;
begin
  Assert((Index >= 0) and (Index < Count), 'Index out of range');
  Result := FList[Index div BlockSize][Index mod BlockSize];
end;

function TBigList.GetCapacity: Integer;
begin
  Result := BlockSize * Length(FList);
end;

procedure TBigList.Grow;
begin
  SetLength(FList, Length(FList) + 1);
end;

procedure TBigList.Insert(Index: Integer; const Item: T);
var
  iBlock: Integer;
  i: Integer;
  iLastIndex: Integer;
  LastItem: T;
  NewLastItem: T;
begin
  Assert((Index >= 0) and (Index <= Count), 'Index out of range');
  if Index = Count then begin
    Add(Item);
    Exit;
  end;
  if Length(FList) = 1 then begin
    if Count = BlockSize then
      Grow;
    if Length(FList) > 1 then begin
      FList[1][0] := FList[0][BlockSize - 1];
      Move(FList[0][Index], FList[0][Index + 1], (BlockSize - Index - 1) * SizeOf(T));
    end else
      Move(FList[0][Index], FList[0][Index + 1], (Count - Index - 1) * SizeOf(T));
    //FillChar(FList[0][Index], SizeOf(T), 0); // to prevent ref count decrement
    FList[0][Index] := Item;
    Inc(FCount);
    Exit;
  end;
  iBlock := Index div BlockSize;
  i := Index mod BlockSize;
  iLastIndex := BlockSize - 1;
  LastItem := FList[iBlock][iLastIndex];
  if i <> iLastIndex then
    Move(FList[iBlock][i], FList[iBlock][i + 1], (iLastIndex - i) * SizeOf(T));
  //FillChar(FList[iBlock][i], SizeOf(T), 0); // to prevent ref count decrement
  FList[iBlock][i] := Item;
  while iBlock < High(FList) do // shift data in remaining blocks
  begin
    Inc(iBlock);
    if iBlock = High(FList) then  // last block?
    begin
      i := (Count - 1) mod BlockSize; // number of items in the last block - 1
      Move(FList[iBlock][1], FList[iBlock][0], i * SizeOf(T));
      if i = iLastIndex then
      begin
        Grow;
        FList[High(FList)][0] := LastItem;
      end;
    end
    else
    begin
      NewLastItem := FList[iBlock][iLastIndex];
      Move(FList[iBlock][0], FList[iBlock][1], iLastIndex * SizeOf(T)); // move entire block
      FList[iBlock][0] := LastItem;
      LastItem := NewLastItem;
    end;
  end;
  Inc(FCount);
end;

procedure TBigList.Put(Index: Integer; const Item: T);
begin
  Assert((Index >= 0) and (Index < Count), 'Index out of range');
  FList[Index div BlockSize][Index mod BlockSize] := Item;
end;

procedure TBigList.QuickSort(L, R: Integer);
var
  i: Integer;
  j: Integer;
  m: Integer;
begin
  repeat
    i := L;
    j := R;
    m := (L + R) shr 1;
    repeat
      while CompareItems(i, m) < 0 do
        Inc(i);
      while CompareItems(j, m) > 0 do
        Dec(j);
      if i <= j then
      begin
        Exchange(i, j);
        if m = i then
          m := j
        else if m = j then
          m := i;
        Inc(i);
        Dec(j);
      end;
    until i > j;
    if L < j then
      QuickSort(L, j);
    L := i;
  until i >= R;
end;

procedure TBigList.SetCapacity(NewCapacity: Integer);
begin
  while Capacity < NewCapacity do
    Grow;
end;

procedure TBigList.Sort;
begin
  if Count > 1 then
    QuickSort(0, Count - 1);
end;

end.
