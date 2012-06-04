type

  { TDictionaries }

  TDictionaries = class(TInterfacedObject, IDictionaries)
  protected
    function HashTable(out output: IHashTable): Boolean;
  end;

implementation

type
PNode = ^TNode;
TNode = record
  next: PNode;
  key: TKey;
  value: TValue;
end;

{ THashTable }

THashTable = class(TInterfacedObject, IHashTable)
protected
  function get(const hash: IHashFunction; const equal: IEqual; const size: Int64;
           out output: IDictionaryBuilder): Boolean;
end;

{ THashDictionary }

THashDictionary = class(TInterfacedObject, IDictionary, IDictionaryBuilder)
private
  FList: array of PNode;
  FHash: IHashFunction;
  FEqual: IEqual;
  function Copy: THashDictionary;
protected
  function has(const key): Boolean;
  function get(const key; out value): Boolean;
  function keys(out output: IahaArray): Boolean;
  function values(out output: IahaArray): Boolean;
  function getState(out output: IDictionary): Boolean;
  function add(new: Boolean; const key, value; out output: IDictionaryBuilder): Boolean;
  function replace(new: Boolean; const key, value; out output: IDictionaryBuilder): Boolean;
  function remove(new: Boolean; const key; out output: IDictionaryBuilder): Boolean;
public
  constructor Create(const hash: IHashFunction; const equal: IEqual; const size: Int64);
end;

{ THashTable }

function THashTable.get(const hash: IHashFunction; const equal: IEqual;
  const size: Int64; out output: IDictionaryBuilder): Boolean;
begin
  Result := True;
  output := THashDictionary.Create(hash, equal, size);
end;

{ TDictionaries }

function TDictionaries.HashTable(out output: IHashTable): Boolean;
begin
  Result := True;
  output := THashTable.Create;
end;

{ THashDictionary }

function THashDictionary.Copy: THashDictionary;
var
i: Cardinal;
node, oldnode, prevnode: PNode;
begin
Result := THashDictionary.Create(FHash, FEqual, Length(FList));
for i := 0 to High(FList) do begin
  oldnode := FList[i];
  prevnode := nil;
  while Assigned(oldnode) do begin
    New(node);
    node^.key := oldnode^.key;
    node^.value := oldnode^.value;
    if Assigned(prevnode) then
      prevnode^.next := node
    else
      Result.FList[i] := node;
    oldnode := oldnode^.next;
  end;
end;
end;

function THashDictionary.has(const key): Boolean;
var
index: Int64;
node: PNode;
begin
Result := False;
if FHash.get(key, index) then begin
  node := FList[index mod Length(FList)];
  while Assigned(node) do begin
    if FEqual.check(node^.key, key) then begin
      Result := True;
      Exit;
    end;
    node := node^.next;
  end;
end;
end;

function THashDictionary.get(const key; out value): Boolean;
var
index: Int64;
node: PNode;
begin
Result := False;
if FHash.get(key, index) then begin
  node := FList[index mod Length(FList)];
  while Assigned(node) do begin
    if FEqual.check(node^.key, key) then begin
      Result := True;
      TValue(value) := node^.value;
      Exit;
    end;
    node := node^.next;
  end;
end;
end;

function THashDictionary.keys(out output: IahaArray): Boolean;
begin

end;

function THashDictionary.values(out output: IahaArray): Boolean;
begin

end;

function THashDictionary.getState(out output: IDictionary): Boolean;
begin
Result := True;
output := Self;
end;

function THashDictionary.add(new: Boolean; const key, value; out output: IDictionaryBuilder): Boolean;
var
index: Int64;
node, prevnode: PNode;
list: array of PNode;
dict: THashDictionary;
begin
if FHash.get(key, index) then begin
  Result := True;
  if not new then begin
    node := FList[index mod Length(FList)];
    prevnode := nil;
    while Assigned(node) do begin
      if FEqual.check(node^.key, key) then begin
        Result := False;
        Exit;
      end;
      prevnode := node;
      node := node^.next;
    end;
    System.New(node);
    node^.key := TKey(key);
    node^.value := TValue(value);
    node^.next := nil;
    if Assigned(prevnode) then
      prevnode^.next := node
    else
      FList[index mod Length(FList)] := node;
    output := Self;
  end
  else begin
    dict := Copy;
    list := dict.FList;
    node := list[index mod Length(list)];
    prevnode := nil;
    while Assigned(node) do begin
      if FEqual.check(node^.key, key) then begin
        Result := False;
        dict.Free;
        Exit;
      end;
      prevnode := node;
      node := node^.next;
    end;
    System.New(node);
    node^.key := TKey(key);
    node^.value := TValue(value);
    node^.next := nil;
    if Assigned(prevnode) then
      prevnode^.next := node
    else
      list[index mod Length(list)] := node;
    output := dict;
  end;
end
else
  Result := False;
end;

function THashDictionary.replace(new: Boolean; const key, value; out output: IDictionaryBuilder): Boolean;
var
index: Int64;
node: PNode;
dict: THashDictionary;
list: array of PNode;
begin
Result := False;
if FHash.get(key, index) then begin
  if not new then begin
    dict := Self;
    list := FList;
  end
  else begin
    dict := Copy;
    list := dict.FList;
  end;
  node := list[index mod Length(list)];
  while Assigned(node) do begin
    if FEqual.check(node^.key, key) then begin
      Result := True;
      node^.value := TValue(value);
      output := dict;
      Exit;
    end;
    node := node^.next;
  end;
  if dict <> Self then
    dict.Free;
end;
end;

function THashDictionary.remove(new: Boolean; const key; out output: IDictionaryBuilder): Boolean;
var
index: Int64;
node, prevnode: PNode;
dict: THashDictionary;
list: array of PNode;
begin
Result := False;
if FHash.get(key, index) then begin
  if not new then begin
    dict := Self;
    list := FList;
  end
  else begin
    dict := Copy;
    list := dict.FList;
  end;
  prevnode := nil;
  node := list[index mod Length(list)];
  while Assigned(node) do begin
    if FEqual.check(node^.key, key) then begin
      Result := True;
      if Assigned(prevnode) then
        prevnode^.next := node^.next
      else
        list[index mod Length(list)] := node^.next;
      Dispose(node);
      output := dict;
      Exit;
    end;
    prevnode := node;
    node := node^.next;
  end;
  if not new then
    dict.Free;
end;
end;

constructor THashDictionary.Create(const hash: IHashFunction;
const equal: IEqual; const size: Int64);
begin
FHash := hash;
FEqual := equal;
SetLength(FList, size);
end;

