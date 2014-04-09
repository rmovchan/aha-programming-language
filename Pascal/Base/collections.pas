unit collections;
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
        obj Item
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
{$mode objfpc}{$H+}

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
  IReplaceParam = interface
    function at(out index: TahaInteger): Boolean;
    function item(out value): Boolean;
  end;

  IExchangeParam = interface
    function first(out index: TahaInteger): Boolean;
    function second(out index: TahaInteger): Boolean;
  end;

  IDynamicArray = interface(IahaObject)
    function add(out new: IDynamicArray; const item): Boolean;
    function replace(out new: IDynamicArray; const param: IReplaceParam): Boolean;
    function exchange(out new: IDynamicArray; const param: IExchangeParam): Boolean;
    function move(out new: IDynamicArray; const param: IExchangeParam): Boolean;
    function insert(out new: IDynamicArray; const param: IReplaceParam): Boolean;
    function delete(out new: IDynamicArray; const index: TahaInteger): Boolean;
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
    function state(out value): Boolean;
    function add(out new: IDynamicArray; const item): Boolean;
    function replace(out new: IDynamicArray; const param: IReplaceParam): Boolean;
    function exchange(out new: IDynamicArray; const param: IExchangeParam): Boolean;
    function move(out new: IDynamicArray; const param: IExchangeParam): Boolean;
    function insert(out new: IDynamicArray; const param: IReplaceParam): Boolean;
    function delete(out new: IDynamicArray; const index: TahaInteger): Boolean;
  protected
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
    function state(out value): Boolean;
    function add(out new: IDynamicArray; const item): Boolean;
    function replace(out new: IDynamicArray; const param: IReplaceParam): Boolean;
    function exchange(out new: IDynamicArray; const param: IExchangeParam): Boolean;
    function move(out new: IDynamicArray; const param: IExchangeParam): Boolean;
    function insert(out new: IDynamicArray; const param: IReplaceParam): Boolean;
    function delete(out new: IDynamicArray; const index: TahaInteger): Boolean;
  protected
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
    function state(out value): Boolean;
    function add(out new: IDynamicArray; const item): Boolean;
    function replace(out new: IDynamicArray; const param: IReplaceParam): Boolean;
    function exchange(out new: IDynamicArray; const param: IExchangeParam): Boolean;
    function move(out new: IDynamicArray; const param: IExchangeParam): Boolean;
    function insert(out new: IDynamicArray; const param: IReplaceParam): Boolean;
    function delete(out new: IDynamicArray; const index: TahaInteger): Boolean;
  protected
    function copy(out value): Boolean; override;
  end;

  IDynamicSequence = interface(IahaObject)
    function push(out new: IDynamicSequence; const item): Boolean;
    function pop(out new: IDynamicSequence): Boolean;
  end;

  { TStack }

  generic TStack<TItem> = class(TahaObject, IDynamicSequence)
  private type TNode = record next: ^TNode; item: TItem; end;
  private
    head: Pointer;
    function state(out value): Boolean;
    function push(out new: IDynamicSequence; const item): Boolean;
    function pop(out new: IDynamicSequence): Boolean;
  protected
    function copy(out value): Boolean; override;
  public
    destructor Destroy; override;
  end;

  TCharStack = specialize TStack<TahaCharacter>;
  TIntStack = specialize TStack<TahaInteger>;
  TOtherStack = specialize TStack<IUnknown>;

  { TQueue }

  generic TQueue<TItem> = class(TahaObject, IDynamicSequence)
  private type TNode = record next, prev: ^TNode; item: TItem; end;
  private
    head: Pointer;
    tail: Pointer;
    function state(out value): Boolean;
    function push(out new: IDynamicSequence; const item): Boolean;
    function pop(out new: IDynamicSequence): Boolean;
  protected
    function copy(out value): Boolean; override;
  public
    destructor Destroy; override;
  end;

  TCharQueue = specialize TQueue<TahaCharacter>;
  TIntQueue = specialize TQueue<TahaInteger>;
  TOtherQueue = specialize TQueue<IUnknown>;

(*
  { TStorage }

  generic TStorage<TItem> = class(TahaObject, IStorage, IahaUnaryFunction)
  private type TNode = record storage: TObject; next, prev: ^TNode; item: TItem; end;
  private
    head: Pointer;
    count: TahaInteger;
    function add(const item): Boolean;
    function replace(const param): Boolean;
    function delete(const addr): Boolean;
    function Get(const param; out value): Boolean;
  protected
    function state(out value): Boolean; override;
    function copy(out value): Boolean; override;
  end;

  { TAddress }

  TAddress = class(TahaOpaque, IahaOpaque)
  private
    FValue: Pointer;
    function get(out value): Boolean;
  end;

  IStorageState = interface
    function next(out value: IahaOpaque): Boolean;
    function get(out value: IahaUnaryFunction): Boolean;
    function occupied(out value: IahaArray): Boolean;
  end;

  { TStorageState }

  TStorageState = class(TahaComposite, IStorageState)
  private
    FNext: IahaOpaque;
    FGet: IahaUnaryFunction;
    FOcc: IahaArray;
    function next(out value: IahaOpaque): Boolean;
    function get(out value: IahaUnaryFunction): Boolean;
    function occupied(out value: IahaArray): Boolean;
 end;

  TCharStorage = specialize TStorage<TahaCharacter>;
  TIntStorage = specialize TStorage<TahaInteger>;
  TOtherStorage = specialize TStorage<IUnknown>;

*)
function GetModuleData(out value: IModuleData; const Item: ITypeInfoEx): Boolean;
begin
  Result := True;
  try
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
(*
{ TStorageState }

function TStorageState.next(out value: IahaOpaque): Boolean;
begin
  value := FNext;
  Result := True;
end;

function TStorageState.get(out value: IahaUnaryFunction): Boolean;
begin
  value := FGet;
  Result := True;
end;

function TStorageState.occupied(out value: IahaArray): Boolean;
begin
  value := FOcc;
  Result := True;
end;

{ TGetFunction }

{ TAddress }

function TAddress.get(out value): Boolean;
begin
  Pointer(value) := FValue;
end;

{ TStorage }

function TStorage.add(const item): Boolean;
var
  newhead: ^TNode;
begin
  try
    TNode(head^).item := TItem(item);
    New(newhead);
    TNode(head^).prev := newhead;
    newhead^.storage := Self;
    newhead^.next := head;
    newhead^.prev := nil;
    head := newhead;
    Inc(count);
    Result := True;
  except
    Result := False;
  end;
end;

function TStorage.replace(const param): Boolean;
var
  addr: IahaOpaque;
  node: ^TNode;
  item: TItem;
begin
  try
    if IReplaceStorageParam(param).at(addr) and addr.get(node) and IReplaceStorageParam(param).item(item) then
      begin
        node^.item := item;
        Result := True;
      end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function TStorage.delete(const addr): Boolean;
begin

end;

function TStorage.Get(const param; out value): Boolean;
var
  addr: IahaOpaque;
  node: ^TNode;
begin
  if IahaOpaque(param).get(node) and (TNode(node^).storage = Self) then
    begin
      TItem(value) := node^.item;
      Result := True;
    end
  else
    Result := False;
end;

function TStorage.state(out value): Boolean;
var
  addr: TAddress;
  sstate: TStorageState;
  items: array of TItem;
  i: TahaInteger;
  p: ^TNode;
begin
  try
    sstate := TStorageState.Create;
    addr := TAddress.Create;
    addr.FValue := head;
    sstate.FNext := addr;
    sstate.FGet := Self;
    SetLength(items, count);
    i := 0;
    p := TNode(head^).next;
    while Assigned(p) do
    begin
      items[i] := p^.item;
      p := p^.next;
      Inc(i);
    end;
    //state.FOcc;
    IStorageState(value) := sstate;
    Result := True;
  except
    Result := False;
  end;
end;

function TStorage.copy(out value): Boolean;
begin
  //Result:=inherited copy(value);
end;
*)
{ TQueue }

function TQueue.push(out new: IDynamicSequence; const item): Boolean;
var
  newhead: ^TNode;
  obj: TQueue;
begin
  try
    Result := getnew(obj);
    if Result then
      begin
        System.New(newhead);
        TNode(obj.head^).prev := newhead;
        newhead^.item := TItem(item);
        newhead^.next := head;
        obj.head := newhead;
        new := obj;
      end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function TQueue.pop(out new: IDynamicSequence): Boolean;
var
  tmp: ^TNode;
  obj: TQueue;
begin
  if Assigned(tail) and getnew(obj) then
    begin
      tmp := obj.tail;
      obj.tail := TNode(obj.tail^).prev;
      Dispose(tmp);
      new := obj;
      Result := True;
    end
  else
    Result := False;
end;

function TQueue.state(out value): Boolean;
begin
  Result := Assigned(tail);
  if Result then
    TItem(value) := TNode(tail^).item;
end;

function TQueue.copy(out value): Boolean;
var
  queue: TQueue;
  p, q, r: ^TNode;
begin
  try
    queue := TQueue.Create;
    p := head;
    q := nil;
    while Assigned(p) do
    begin
      New(r);
      if Assigned(q) then
        q^.next := r
      else
        queue.head := r;
      q := r;
      q^.item := p^.item;
      p := p^.next;
    end;
    TObject(value) := queue;
    Result := True;
  except
    Result := False;
  end;
end;

destructor TQueue.Destroy;
var
  p, q: ^TNode;
begin
  p := head;
  while Assigned(p) do
  begin
    q := p^.next;
    Dispose(p);
    p := q;
  end;
  inherited Destroy;
end;

{ TStack }

function TStack.push(out new: IDynamicSequence; const item): Boolean;
var
  newhead: ^TNode;
  obj: TStack;
begin
  try
    Result := getnew(obj);
    if Result then
      begin
        System.New(newhead);
        newhead^.item := TItem(item);
        newhead^.next := obj.head;
        obj.head := newhead;
        new := obj;
      end;
  except
    Result := False;
  end;
end;

function TStack.pop(out new: IDynamicSequence): Boolean;
var
  tmp: ^TNode;
  obj: TStack;
begin
  if Assigned(head) and getnew(obj) then
    begin
      tmp := head;
      obj.head := TNode(obj.head^).next;
      Dispose(tmp);
      new := obj;
      Result := True;
    end
  else
    Result := False;
end;

function TStack.state(out value): Boolean;
begin
  Result := Assigned(head);
  if Result then
    TItem(value) := TNode(head^).item;
end;

function TStack.copy(out value): Boolean;
var
  stack: TStack;
  p, q, r: ^TNode;
begin
  try
    stack := TStack.Create;
    stack.head := head;
    TObject(value) := stack;
    p := head;
    q := nil;
    while Assigned(p) do
    begin
      New(r);
      if Assigned(q) then
        q^.next := r;
      q := r;
      q^.item := p^.item;
      p := p^.next;
    end;
    Result := True;
  except
    Result := False;
  end;
end;

destructor TStack.Destroy;
var
  p, q: ^TNode;
begin
  p := head;
  while Assigned(p) do
  begin
    q := p^.next;
    Dispose(p);
    p := q;
  end;
  inherited Destroy;
end;

function TModuleDataOther.DynamicArray(out value: IUnknown): Boolean;
begin
  try
    value := TDynamicOtherArray.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataOther.Stack(out value: IUnknown): Boolean;
begin
  try
    value := TOtherStack.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataOther.Queue(out value: IUnknown): Boolean;
begin
  try
    value := TOtherQueue.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataOther.Storage(out value: IUnknown): Boolean;
begin
  Result := False;
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
  try
    value := TIntStack.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataInt.Queue(out value: IUnknown): Boolean;
begin
  try
    value := TIntQueue.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataInt.Storage(out value: IUnknown): Boolean;
begin
  Result := False;
end;

function TDynamicIntArray.add(out new: IDynamicArray; const item): Boolean;
var
  obj: TDynamicIntArray;
begin
  Result := getnew(obj);
  if Result then
    begin
      SetLength(obj.FItems, Length(obj.FItems) + 1);
      FItems[High(obj.FItems)] := TahaInteger(item);
      new := obj;
    end;
end;

function TDynamicIntArray.replace(out new: IDynamicArray; const param: IReplaceParam): Boolean;
var
  index: TahaInteger;
  item: TahaInteger;
  obj: TDynamicIntArray;
begin
  if param.at(index) and (index >= 0) and (index < Length(FItems)) and param.item(item) then
    begin
      Result := getnew(obj);
      if Result then
      begin
        obj.FItems[index] := item;
        new := obj;
      end;
    end
  else
    Result := False;
end;

function TDynamicIntArray.exchange(out new: IDynamicArray; const param: IExchangeParam): Boolean;
var
  first, second: TahaInteger;
  item: TahaInteger;
  obj: TDynamicIntArray;
begin
  Result :=
    param.first(first) and (first >= 0) and (first < Length(FItems)) and
    param.second(second) and (second >= 0) and (second < Length(FItems));
  if Result then
    Result := getnew(obj);
  if Result then
    begin
      item := obj.FItems[first];
      obj.FItems[first] := obj.FItems[second];
      obj.FItems[second] := item;
      new := obj;
    end;
end;

function TDynamicIntArray.move(out new: IDynamicArray; const param: IExchangeParam): Boolean;
var
  first, second: TahaInteger;
  item: TahaInteger;
  obj: TDynamicIntArray;
begin
  Result :=
    param.first(first) and (first >= 0) and (first < Length(FItems)) and
    param.second(second) and (second >= 0) and (second < Length(FItems));
  if Result then
    Result := getnew(obj);
  if Result then
    begin
      item := obj.FItems[first];
      if first < second then
        begin
          System.Move(obj.FItems[first + 1], obj.FItems[first], (second - first) * SizeOf(TahaInteger));
        end
      else
      if first > second then
        begin
          System.Move(obj.FItems[second], obj.FItems[second + 1], (first - second) * SizeOf(TahaInteger));
        end;
      obj.FItems[second] := item;
      new := obj;
    end;
end;

function TDynamicIntArray.insert(out new: IDynamicArray; const param: IReplaceParam): Boolean;
var
  index: TahaInteger;
  item: TahaInteger;
  obj: TDynamicIntArray;
begin
  try
    if param.at(index) and (index >= 0) and (index < Length(FItems)) and param.item(item) then
      begin
        Result := getnew(obj);
        if Result then
          begin
            SetLength(obj.FItems, Length(FItems) + 1);
            System.Move(obj.FItems[index], obj.FItems[index + 1], (Length(FItems) - index - 1) * SizeOf(TahaInteger));
            obj.FItems[index] := item;
            new := obj;
          end;
      end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function TDynamicIntArray.delete(out new: IDynamicArray; const index: TahaInteger): Boolean;
var
  obj: TDynamicIntArray;
begin
  if (index >= 0) and (index < Length(FItems)) then
    begin
      Result := getnew(obj);
      if Result then
        begin
          System.Move(obj.FItems[index + 1], obj.FItems[index], (Length(FItems) - index - 1) * SizeOf(TahaInteger));
          SetLength(obj.FItems, Length(FItems) - 1);
          new := obj;
        end;
    end
  else
    Result := False;
end;

function TDynamicIntArray.state(out value): Boolean;
begin
  Result := True;
  try
    IahaArray(value) := TahaIntArrayWrapper.Create(System.Copy(FItems, 0, Length(FItems)));
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
    TDynamicIntArray(value) := clone;
    Result := True;
  except
    Result := False;
  end;
end;

function TDynamicCharArray.add(out new: IDynamicArray; const item): Boolean;
var
  obj: TDynamicCharArray;
begin
  try
    Result := getnew(obj);
    if Result then
      begin
        obj.FItems := obj.FItems + TahaCharacter(item);
        new := obj;
      end;
  except
    Result := False;
  end;
end;

function TDynamicCharArray.replace(out new: IDynamicArray; const param: IReplaceParam): Boolean;
var
  index: TahaInteger;
  item: TahaCharacter;
  obj: TDynamicCharArray;
begin
  if param.at(index) and (index >= 0) and (index < Length(FItems)) and param.item(item) then
    begin
      Result := getnew(obj);
      if Result then
        begin
          obj.FItems[index + 1] := item;
          new := obj;
        end;
    end
  else
    Result := False;
end;

function TDynamicCharArray.exchange(out new: IDynamicArray; const param: IExchangeParam): Boolean;
var
  first, second: TahaInteger;
  ch: TahaCharacter;
  obj: TDynamicCharArray;
begin
  Result :=
    param.first(first) and (first >= 0) and (first < Length(FItems)) and
    param.second(second) and (second >= 0) and (second < Length(FItems)) and
    getnew(obj);
  if Result then
    begin
      ch := obj.FItems[first + 1];
      obj.FItems[first + 1] := obj.FItems[second + 1];
      obj.FItems[second + 1] := ch;
      new := obj;
    end;
end;

function TDynamicCharArray.move(out new: IDynamicArray; const param: IExchangeParam): Boolean;
var
  first, second: TahaInteger;
  ch: TahaCharacter;
  obj: TDynamicCharArray;
begin
  Result :=
    param.first(first) and (first >= 0) and (first < Length(FItems)) and
    param.second(second) and (second >= 0) and (second < Length(FItems)) and
    getnew(obj);
  if Result then
    begin
      ch := obj.FItems[first + 1];
      if first < second then
        begin
          System.Move(obj.FItems[first + 2], obj.FItems[first + 1], (second - first) * SizeOf(TahaCharacter));
        end
      else
      if first > second then
        begin
          System.Move(obj.FItems[second + 1], obj.FItems[second + 2], (first - second) * SizeOf(TahaCharacter));
        end;
      obj.FItems[second + 1] := ch;
      new := obj;
    end;
end;

function TDynamicCharArray.insert(out new: IDynamicArray; const param: IReplaceParam): Boolean;
var
  index: TahaInteger;
  item: TahaCharacter;
  obj: TDynamicCharArray;
begin
  try
    if param.at(index) and (index >= 0) and (index < Length(FItems)) and param.item(item) and getnew(obj) then
      begin
        System.Insert(item, obj.FItems, index + 1);
        new := obj;
        Result := True;
      end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function TDynamicCharArray.delete(out new: IDynamicArray; const index: TahaInteger): Boolean;
var
  obj: TDynamicCharArray;
begin
  if (index >= 0) and (index < Length(FItems)) and getnew(obj) then
    begin
      System.Delete(obj.FItems, index + 1, 1);
      new := obj;
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
    TDynamicCharArray(value) := clone;
  except
    Result := False;
  end;
end;

{ TDynamicArray }

function TDynamicOtherArray.add(out new: IDynamicArray; const item): Boolean;
var
  obj: TDynamicOtherArray;
begin
  Result := getnew(obj);
  if Result then
    begin
      SetLength(obj.FItems, Length(FItems) + 1);
      obj.FItems[High(FItems)] := IUnknown(item);
    end;
end;

function TDynamicOtherArray.replace(out new: IDynamicArray; const param: IReplaceParam): Boolean;
var
  index: TahaInteger;
  item: IUnknown;
  obj: TDynamicOtherArray;
begin
  if param.at(index) and (index >= 0) and (index < Length(FItems)) and param.item(item) and getnew(obj) then
    begin
      obj.FItems[index] := item;
      Result := True;
    end
  else
    Result := False;
end;

function TDynamicOtherArray.exchange(out new: IDynamicArray; const param: IExchangeParam): Boolean;
var
  first, second: TahaInteger;
  item: IUnknown;
  obj: TDynamicOtherArray;
begin
  Result :=
    param.first(first) and (first >= 0) and (first < Length(FItems)) and
    param.second(second) and (second >= 0) and (second < Length(FItems)) and
    getnew(obj);
  if Result then
    begin
      item := obj.FItems[first];
      obj.FItems[first] := obj.FItems[second];
      obj.FItems[second] := item;
    end;
end;

function TDynamicOtherArray.move(out new: IDynamicArray; const param: IExchangeParam): Boolean;
var
  first, second: TahaInteger;
  item: IUnknown;
  obj: TDynamicOtherArray;
begin
  Result :=
    param.first(first) and (first >= 0) and (first < Length(FItems)) and
    param.second(second) and (second >= 0) and (second < Length(FItems)) and
    getnew(obj);
  if Result then
    begin
      item := obj.FItems[first];
      obj.FItems[first] := nil;
      if first < second then
        begin
          System.Move(obj.FItems[first + 1], obj.FItems[first], (second - first) * SizeOf(IUnknown));
        end
      else
      if first > second then
        begin
          System.Move(obj.FItems[second], obj.FItems[second + 1], (first - second) * SizeOf(IUnknown));
        end;
      obj.FItems[second] := item;
    end;
end;

function TDynamicOtherArray.insert(out new: IDynamicArray; const param: IReplaceParam): Boolean;
var
  index: TahaInteger;
  item: IUnknown;
  obj: TDynamicOtherArray;
begin
  try
    if param.at(index) and (index >= 0) and (index < Length(FItems)) and param.item(item) and getnew(obj) then
      begin
        SetLength(obj.FItems, Length(FItems) + 1);
        System.Move(obj.FItems[index], obj.FItems[index + 1], (Length(FItems) - index - 1) * SizeOf(IUnknown));
        obj.FItems[index]._AddRef; //?
        obj.FItems[index] := item;
        Result := True;
      end
    else
      Result := False;
  except
    Result := False;
  end;
end;

function TDynamicOtherArray.delete(out new: IDynamicArray; const index: TahaInteger): Boolean;
var
  obj: TDynamicOtherArray;
begin
  if (index >= 0) and (index < Length(FItems)) and getnew(obj) then
    begin
      obj.FItems[index] := nil;
      System.Move(obj.FItems[index + 1], obj.FItems[index], (Length(FItems) - index - 1) * SizeOf(IUnknown));
      //FItems[High(FItems)] := nil;
      SetLength(obj.FItems, Length(FItems) - 1);
      Result := True;
    end
  else
    Result := False;
end;

function TDynamicOtherArray.state(out value): Boolean;
begin
  Result := True;
  try
    IahaArray(value) := TahaOherArrayWrapper.Create(System.Copy(FItems, 0, Length(FItems)));
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
    TDynamicOtherArray(value) := clone;
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
  try
    value := TCharStack.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataChar.Queue(out value: IUnknown): Boolean;
begin
  try
    value := TCharQueue.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleDataChar.Storage(out value: IUnknown): Boolean;
begin
  Result := False;
end;

end.

