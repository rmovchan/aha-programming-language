unit collections;

{$mode objfpc}{$H+}
(*
doc
    Title:   "Collections"
    Purpose: "Generic collections: dynamic arrays, stacks, queues"
    Package: "Aha! Base Library"
    Author:  "Roman Movchan"
    Created: "2010-10-14"
end

type Item: arbitrary "collection item"

export Types:
    type DynamicArray:
        obj [Item]
            add(Item) "add new item"
            replace([ at: integer item: Item ]) "replace item at index"
            exchange([ first: integer second: integer ]) "swap two items"
            move([ from: integer to: integer ]) "move item to new position"
            insert([ at: integer item: Item ]) "insert item at index"
            delete(integer) "delete item at index"
        end "a dynamic array"

    type DynamicSequence:
        obj Item*
            push(Item) "add an item"
            pop "remove an item"
        end "a dynamic sequence"

    type Address: opaque "address for random access"

    type RandomStorage:
        obj [ next: Address "next available address" get: { Address -> Item } "get item at address" occupied: [Address] "all occupied addresses" ]
            add(Item) "add an item to storage at the next available address"
            replace([ at: Address item: Item ]) "replace item at address"
            delete(Address) "delete item at address"
        end "random storage"
end

export Constructors:
    the DynamicArray: DynamicArray "zero-length dynamic array"
    the Stack: DynamicSequence "empty stack"
    the Queue: DynamicSequence "empty queue"
    the Storage: RandomStorage "empty random storage"
end
*)
interface

uses
  core, metatypes;

type
  IModuleData = interface
    function DynamicArray(out value: IUnknown): Boolean;
    function Stack(out value: IUnknown): Boolean;
    function Queue(out value: IUnknown): Boolean;
    function Storage(out value: IUnknown): Boolean;
  end;

function GetModuleData(out value: IModuleData; const Item: ITypeInfoEx): Boolean;


implementation

type
  IDynamicArray = interface(IahaObject)
    function add(const item): Boolean;
    function replace(const param): Boolean;
    function exchange(const param): Boolean;
    function move(const param): Boolean;
    function insert(const param): Boolean;
    function delete(const index): Boolean;
  end;

  IReplaceParam = interface
    function at(out index: TahaInteger): Boolean;
    function item(out value): Boolean;
  end;

  IExchangeParam = interface
    function first(out index: TahaInteger): Boolean;
    function second(out index: TahaInteger): Boolean;
  end;

  { TModuleDataFoo }

  TModuleDataFoo = class(TahaComposite, IModuleData)
  private
    function DynamicArray(out value: IUnknown): Boolean;
    function Stack(out value: IUnknown): Boolean;
    function Queue(out value: IUnknown): Boolean;
    function Storage(out value: IUnknown): Boolean;
  end;

  { TDynamicFooArray }

  TDynamicFooArray = class(TahaObject, IDynamicArray)
  private
    FSize: TahaInteger;
    function add(const item): Boolean;
    function replace(const param): Boolean;
    function exchange(const param): Boolean;
    function move(const param): Boolean;
    function insert(const param): Boolean;
    function delete(const index): Boolean;
  protected
    function state(out value): Boolean; override;
    function copy(out value): Boolean; override;
  end;

  { TModuleDataChar }

  TModuleDataChar = class(TahaComposite, IModuleData)
  private
    function DynamicArray(out value: IUnknown): Boolean;
    function Stack(out value: IUnknown): Boolean;
    function Queue(out value: IUnknown): Boolean;
    function Storage(out value: IUnknown): Boolean;
  end;

  { TDynamicCharArray }

  TDynamicCharArray = class(TahaObject, IDynamicArray)
  private
    FItems: UnicodeString;
    function add(const item): Boolean;
    function replace(const param): Boolean;
    function exchange(const param): Boolean;
    function move(const param): Boolean;
    function insert(const param): Boolean;
    function delete(const index): Boolean;
  protected
    function state(out value): Boolean; override;
    function copy(out value): Boolean; override;
  end;

  { TModuleDataInt }

  TModuleDataInt = class(TahaComposite, IModuleData)
  private
    function DynamicArray(out value: IUnknown): Boolean;
    function Stack(out value: IUnknown): Boolean;
    function Queue(out value: IUnknown): Boolean;
    function Storage(out value: IUnknown): Boolean;
  end;

  { TDynamicIntArray }

  TDynamicIntArray = class(TahaObject, IDynamicArray)
  private
    FItems: array of TahaInteger;
    function add(const item): Boolean;
    function replace(const param): Boolean;
    function exchange(const param): Boolean;
    function move(const param): Boolean;
    function insert(const param): Boolean;
    function delete(const index): Boolean;
  protected
    function state(out value): Boolean; override;
    function copy(out value): Boolean; override;
  end;

  { TModuleDataOther }

  TModuleDataOther = class(TahaComposite, IModuleData)
  private
    function DynamicArray(out value: IUnknown): Boolean;
    function Stack(out value: IUnknown): Boolean;
    function Queue(out value: IUnknown): Boolean;
    function Storage(out value: IUnknown): Boolean;
  end;

  { TDynamicOtherArray }

  TDynamicOtherArray = class(TahaObject, IDynamicArray)
  private
    FItems: array of IUnknown;
    function add(const item): Boolean;
    function replace(const param): Boolean;
    function exchange(const param): Boolean;
    function move(const param): Boolean;
    function insert(const param): Boolean;
    function delete(const index): Boolean;
  protected
    function state(out value): Boolean; override;
    function copy(out value): Boolean; override;
  end;

function GetModuleData(out value: IModuleData; const Item: ITypeInfoEx): Boolean;
begin
  Result := True;
  try
    if Item.fooType then
      value := TModuleDataFoo.Create
    else
    if Item.characterType then
      value := TModuleDataChar.Create
    else
    if Item.integerType then
      value := TModuleDataInt.Create
    else
      value := TModuleDataOther.Create;
  except
    Result := False;
  end;
end;

function TDynamicFooArray.Add(const item): Boolean;
begin
  try
    Inc(FSize);
    Result := True;
  except
    Result := False;
  end;
end;

function TDynamicFooArray.replace(const param): Boolean;
var
  index: TahaInteger;
begin
  Result := IReplaceParam(param).at(index) and (index >= 0) and (index < FSize);
end;

function TDynamicFooArray.exchange(const param): Boolean;
var
  i1, i2: TahaInteger;
begin
  Result :=
    IExchangeParam(param).first(i1) and (i1 >= 0) and (i1 < FSize) and
    IExchangeParam(param).second(i2) and (i2 >= 0) and (i2 < FSize);
end;

function TDynamicFooArray.move(const param): Boolean;
var
  i1, i2: TahaInteger;
begin
  Result :=
    IExchangeParam(param).first(i1) and (i1 >= 0) and (i1 < FSize) and
    IExchangeParam(param).second(i2) and (i2 >= 0) and (i2 < FSize);
end;

function TDynamicFooArray.insert(const param): Boolean;
var
  index: TahaInteger;
begin
  Result := IReplaceParam(param).at(index) and (index >= 0) and (index < FSize);
  if Result then
    try
      Inc(FSize);
    except
      Result := False;
    end;
end;

function TDynamicFooArray.delete(const index): Boolean;
begin
  Result := (TahaInteger(index) >= 0) and (TahaInteger(index) < FSize);
  if Result then
    Dec(FSize);
end;

function TDynamicFooArray.state(out value): Boolean;
begin
  try
    IahaArray(value) := TahaFooArrayWrapper.Create(FSize);
    Result := True;
  except
    Result := False;
  end;
end;

function TDynamicFooArray.copy(out value): Boolean;
var
  clone: TDynamicFooArray;
begin
  try
    clone := TDynamicFooArray.Create;
    clone.FSize := FSize;
    IDynamicArray(value) := clone;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataFoo.DynamicArray(out value: IUnknown): Boolean;
begin
  try
    value := TDynamicFooArray.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataFoo.Stack(out value: IUnknown): Boolean;
begin

end;

function TModuleDataFoo.Queue(out value: IUnknown): Boolean;
begin

end;

function TModuleDataFoo.Storage(out value: IUnknown): Boolean;
begin

end;

function TModuleDataOther.DynamicArray(out value: IUnknown): Boolean;
begin

end;

function TModuleDataOther.Stack(out value: IUnknown): Boolean;
begin

end;

function TModuleDataOther.Queue(out value: IUnknown): Boolean;
begin

end;

function TModuleDataOther.Storage(out value: IUnknown): Boolean;
begin

end;

function TModuleDataInt.DynamicArray(out value: IUnknown): Boolean;
begin
  try
    value := TDynamicIntArray.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataInt.Stack(out value: IUnknown): Boolean;
begin

end;

function TModuleDataInt.Queue(out value: IUnknown): Boolean;
begin

end;

function TModuleDataInt.Storage(out value: IUnknown): Boolean;
begin

end;

function TDynamicIntArray.Add(const item): Boolean;
begin
  SetLength(FItems, Length(FItems) + 1);
  FItems[High(FItems)] := TahaInteger(item);
end;

function TDynamicIntArray.replace(const param): Boolean;
begin

end;

function TDynamicIntArray.exchange(const param): Boolean;
begin

end;

function TDynamicIntArray.move(const param): Boolean;
begin

end;

function TDynamicIntArray.insert(const param): Boolean;
begin

end;

function TDynamicIntArray.delete(const index): Boolean;
begin

end;

function TDynamicIntArray.state(out value): Boolean;
begin
  Result := True;
  try
    IahaArray(value) := TahaIntArrayWrapper.Create(FItems);
  except
    Result := False;
  end;
end;

function TDynamicIntArray.copy(out value): Boolean;
var
  clone: TDynamicIntArray;
begin
  try
    clone := TDynamicIntArray.Create;
    clone.FItems := System.Copy(FItems, 0, Length(FItems));
    IDynamicArray(value) := clone;
    Result := True;
  except
    Result := False;
  end;
end;

function TDynamicCharArray.Add(const item): Boolean;
begin
  try
    FItems := FItems + TahaCharacter(item);
    Result := True;
  except
    Result := False;
  end;
end;

function TDynamicCharArray.replace(const param): Boolean;
var
  index: TahaInteger;
  item: TahaCharacter;
begin
  try
    if IReplaceParam(param).at(index) and (index >= 0) and (index < Length(FItems)) and IReplaceParam(param).item(item) then
      begin
        FItems[index + 1] := item;
        Result := True;
      end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function TDynamicCharArray.exchange(const param): Boolean;
var
  first, second: TahaInteger;
  ch: TahaCharacter;
begin
  Result :=
    IExchangeParam(param).first(first) and (first >= 0) and (first < Length(FItems)) and
    IExchangeParam(param).second(second) and (second >= 0) and (second < Length(FItems));
  if Result then
    begin
      ch := FItems[first + 1];
      FItems[first + 1] := FItems[second + 1];
      FItems[second + 1] := ch;
    end;
end;

function TDynamicCharArray.move(const param): Boolean;
var
  first, second: TahaInteger;
  ch: TahaCharacter;
begin
  Result :=
    IExchangeParam(param).first(first) and (first >= 0) and (first < Length(FItems)) and
    IExchangeParam(param).second(second) and (second >= 0) and (second < Length(FItems));
  if Result then
    begin
      ch := FItems[first + 1];
      if first < second then
        begin
          System.Move(FItems[first + 2], FItems[first + 1], (second - first) * SizeOf(TahaCharacter));
        end
      else
      if first > second then
        begin
          System.Move(FItems[second + 1], FItems[second + 2], (first - second) * SizeOf(TahaCharacter));
        end;
      FItems[second + 1] := ch;
    end;
end;

function TDynamicCharArray.insert(const param): Boolean;
var
  index: TahaInteger;
  item: TahaCharacter;
begin
  try
    if IReplaceParam(param).at(index) and (index >= 0) and (index < Length(FItems)) and IReplaceParam(param).item(item) then
      begin
        System.Insert(item, FItems, index + 1);
        Result := True;
      end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function TDynamicCharArray.delete(const index): Boolean;
begin
  if (TahaInteger(index) >= 0) and (TahaInteger(index) < Length(FItems)) then
    begin
      System.Delete(FItems, TahaInteger(index) + 1, 1);
      Result := True;
    end
  else
    Result := False;
end;

function TDynamicCharArray.state(out value): Boolean;
begin
  try
    IahaArray(value) := TahaCharArrayWrapper.Create(FItems);
    Result := True;
  except
    Result := False;
  end;
end;

function TDynamicCharArray.copy(out value): Boolean;
var
  clone: TDynamicCharArray;
begin
  Result := True;
  try
    clone := TDynamicCharArray.Create;
    clone.FItems := FItems;
    IDynamicArray(value) := clone;
  except
    Result := False;
  end;
end;

{ TDynamicArray }

function TDynamicOtherArray.Add(const item): Boolean;
begin
  SetLength(FItems, Length(FItems) + 1);
  FItems[High(FItems)] := IUnknown(item);
end;

function TDynamicOtherArray.replace(const param): Boolean;
begin

end;

function TDynamicOtherArray.exchange(const param): Boolean;
begin

end;

function TDynamicOtherArray.move(const param): Boolean;
var
  first, second, i: TahaInteger;
  item: IUnknown;
begin
  Result :=
    IExchangeParam(param).first(first) and (first >= 0) and (first < Length(FItems)) and
    IExchangeParam(param).second(second) and (second >= 0) and (second < Length(FItems));
  if Result then
    begin
      item := FItems[first + 1];
      if first < second then
        begin
          i := first + 1;
          while i < second do
          begin
            FItems[i] := FItems[i + 1];
            Inc(i);
          end;
          FItems[second + 1] := item;
        end
      else
      if first > second then
        begin
          i := first + 1;
          while i > second do
          begin
            FItems[i] := FItems[i - 1];
            Dec(i);
          end;
          FItems[second + 1] := item;
        end;
    end;
end;

function TDynamicOtherArray.insert(const param): Boolean;
begin

end;

function TDynamicOtherArray.delete(const index): Boolean;
begin

end;

function TDynamicOtherArray.state(out value): Boolean;
begin
  Result := True;
  try
    IahaArray(value) := TahaOherArrayWrapper.Create(FItems);
  except
    Result := False;
  end;
end;

function TDynamicOtherArray.copy(out value): Boolean;
var
  clone: TDynamicOtherArray;
begin
  Result := True;
  try
    clone := TDynamicOtherArray.Create;
    clone.FItems := System.Copy(FItems, 0, Length(FItems));
    IDynamicArray(value) := clone;
  except
    Result := False;
  end;
end;

{ TModuleData }

function TModuleDataChar.DynamicArray(out value: IUnknown): Boolean;
begin
  try
    value := TDynamicCharArray.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataChar.Stack(out value: IUnknown): Boolean;
begin

end;

function TModuleDataChar.Queue(out value: IUnknown): Boolean;
begin

end;

function TModuleDataChar.Storage(out value: IUnknown): Boolean;
begin

end;

end.

