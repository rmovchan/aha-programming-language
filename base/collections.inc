type
  TArray = array of TItem;
  PItem = ^TItem;

  { TDynamicArray }

  TDynamicArray = class(TInterfacedObject, IDynamicArray)
  private
    FList: array of PItem;
    FLength: Int64;
  protected
    function getState(out output): Boolean;
    function add(new: Boolean; const item; out output: IDynamicArray): Boolean;
  public
    constructor Create;
    destructor Destroy; override;
  end;

  { TStack }

  TStack = class(TInterfacedObject, IInOutStorage)
  private
    next: IInOutStorage;
    value: TItem;
  protected
    function getState(out output): Boolean;
    function pop(new: Boolean; out output: IInOutStorage): Boolean;
    function push(new: Boolean; const item; out output: IInOutStorage): Boolean;
  public
    constructor Create(const head: TItem; const tail: IInOutStorage);
  end;

  { TEmptyStack }

  TEmptyStack = class(TInterfacedObject, IInOutStorage)
  protected
    function getState(out output): Boolean;
    function pop(new: Boolean; out output: IInOutStorage): Boolean;
    function push(new: Boolean; const item; out output: IInOutStorage): Boolean;
  public
    constructor Create;
  end;

  { TQueue }

  PNode = ^TNode;
  TNode = record
    next: PNode;
    value: TItem;
    refcount: Cardinal;
  end;

  TahaQueue = class(TInterfacedObject, IInOutStorage)
  private
    head: PNode;
    tail: PNode;
  protected
    function getState(out output): Boolean;
    function pop(new: Boolean; out output: IInOutStorage): Boolean;
    function push(new: Boolean; const item; out output: IInOutStorage): Boolean;
  public
    constructor Create(ahead, atail: PNode);
    destructor Destroy; override;
  end;


{ TQueue }

function TahaQueue.getState(out output): Boolean;
begin
  Result := Assigned(head);
  if Result then
    TItem(output) := head^.value;
end;

function TahaQueue.pop(new: Boolean; out output: IInOutStorage): Boolean;
var
  newhead: PNode;
begin
  Result := Assigned(head);
  if Result then begin
    IInOutStorage(output) := TahaQueue.Create(head^.next, tail);
    Dec(head^.refcount);
    if head^.refcount = 0 then
      Dispose(head);
  end;
end;

function TahaQueue.push(new: Boolean; const item; out output: IInOutStorage): Boolean;
var
  newtail, node: PNode;
begin
  Result := True;
  node := head;
  while Assigned(node) do begin
    Inc(node^.refcount);
    if node = tail then
      Break;
    node := node^.next;
  end;
  System.New(newtail);
  newtail^.next := head;
  newtail^.value := TItem(item);
  newtail^.refcount := 1;
  if Assigned(tail) then
    tail^.next := newtail;
  IInOutStorage(output) := TahaQueue.Create(head, newtail);
end;

constructor TahaQueue.Create(ahead, atail: PNode);
begin
  head := ahead;
  tail := atail;
end;

destructor TahaQueue.Destroy;
var
  node: PNode;
begin
  node := head;
  while Assigned(node) do begin
    Dec(node^.refcount);
    if node^.refcount = 0 then
      Dispose(node);
    if node = tail then
      Break;
    node := node^.next;
  end;
  inherited Destroy;
end;

{ TEmptyStack }

function TEmptyStack.getState(out output): Boolean;
begin
  Result := False;
end;

function TEmptyStack.pop(new: Boolean; out output: IInOutStorage): Boolean;
begin
  Result := False;
end;

function TEmptyStack.push(new: Boolean; const item; out output: IInOutStorage): Boolean;
begin
  output := TStack.Create(TItem(item), Self);
  Result := True;
end;

constructor TEmptyStack.Create;
begin
end;

{ TStack }

constructor TStack.Create(const head: TItem; const tail: IInOutStorage);
begin
  value := head;
  next := tail;
end;

function TStack.getState(out output): Boolean;
begin
  Result := True;
  if Result then
    TItem(output) := value;
end;

function TStack.pop(new: Boolean; out output: IInOutStorage): Boolean;
begin
  Result := True;
  output := next;
end;

function TStack.push(new: Boolean; const item; out output: IInOutStorage): Boolean;
begin
  output := TStack.Create(TItem(item), Self);
  Result := True;
end;

{ TCollections }

function TCollections.DynamicArray(out output: IDynamicArray): Boolean;
begin
  try
    output := TDynamicArray.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TCollections.Queue(out Output: IInOutStorage): Boolean;
begin
  Result := True;
  output := TahaQueue.Create(nil, nil);
end;

function TCollections.Stack(out Output: IInOutStorage): Boolean;
begin
  Result := True;
  output := TEmptyStack.Create;
end;

constructor TCollections.Create;
begin

end;

{ TDynamicArray }

function TDynamicArray.getState(out output): Boolean;
begin
  Result := AhaArrayOf([], IahaArray(output));
end;

function TDynamicArray.add(new: Boolean; const item; out output: IDynamicArray): Boolean;
var
  Arr: TDynamicArray;
  i: Integer;
begin
  try
    if not new then begin
      Arr := Self;
    end
    else begin
      Arr := TDynamicArray.Create;
    end;
    Result := True;
    output := Arr;
  except
    Result := False;
  end;
end;

constructor TDynamicArray.Create;
begin
end;

destructor TDynamicArray.Destroy;
begin
  inherited Destroy;
end;


