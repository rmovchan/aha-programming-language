unit dictionaries;
(*
doc
    Title:   "Dictionaries"
    Package: "Aha! Base Library"
    Purpose: "Associative arrays"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2011-08-12"
end

type Key: arbitrary "lookup key"
type Value: arbitrary "lookup value"

export Types:
    type Dictionary:
        [
            at: { Key -> Value } "returns Value for given Key"
            keys: [Key] "all keys in the dictionary"
            values: [Value] "all values in the dictionary"
        ] "a dictionary"
    type DictionaryBuilder:
        obj Dictionary
            add([ key: Key value: Value ]) "add a new Key-Value pair"
            replace([ key: Key value: Value ]) "replace Value for existing Key"
            remove(Key) "remove Value for the Key"
        end "a dictionary builder"
    type HashFunc: { Key -> integer } "a hash function"
    type Equality: { Key, Key } "a key comparison function"
    type HashTableParams:
        [
            hash: HashFunc "hash function"
            equal: Equality "key comparison function"
            size: integer "table size"
        ] "hash table parameters"
end

export Constructors:
    the HashTable: { HashTableParams -> DictionaryBuilder } "hash table constructor"
end
*)
{$mode objfpc}{$H+}

interface

uses
  core, metatypes;

type
  IModuleData = interface
    function HashTable(out value: IUnknown): Boolean;
  end;

  IAddParam = interface
    function key(out value): Boolean;
    function val(out value): Boolean;
  end;

  IDictionary = interface
    function at(out value: IahaUnaryFunction): Boolean;
    function keys(out value: IahaArray): Boolean;
    function values(out value: IahaArray): Boolean;
  end;

  IHashTable = interface(IahaObject)
    function add(out new: IHashTable; const param: IAddParam): Boolean;
    function replace(out new: IHashTable; const param: IAddParam): Boolean;
    function remove(out new: IHashTable; const key): Boolean;
  end;

  IHashTableParam = interface
    function hash(out value: IahaUnaryFunction): Boolean;
    function equal(out value: IahaBinaryRelation): Boolean;
    function size(out value: TahaInteger): Boolean;
  end;

type

  { TArrayWrapper }

  generic TArrayWrapper<TItem> = class(TInterfacedObject, IahaArray)
  private
    type PItem = ^TItem;
  private
    FArray: array of TItem;
    procedure resize(count: TahaInteger); inline;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
    function write(out value): Boolean;
  end;


  { THashTable }

  generic THashTable<TKey, TValue> = class(TahaObject, IHashTable, IDictionary, IahaUnaryFunction)
  private
    type TNode = record key: TKey; val: TValue; next, prev: ^TNode; end;
    type PNode = ^TNode;
    type TKeyArray = specialize TArrayWrapper<TKey>;
    type TValueArray = specialize TArrayWrapper<TValue>;
  private
    index: array of PNode;
    hash: IahaUnaryFunction;
    equal: IahaBinaryRelation;
    count: TahaInteger;
    function find(idx: array of PNode; const key: TKey; out value: PNode): Boolean;
    function add(out new: IHashTable; const param: IAddParam): Boolean;
    function replace(out new: IHashTable; const param: IAddParam): Boolean;
    function remove(out new: IHashTable; const key): Boolean;
    function state(out value): Boolean;
    function at(out value: IahaUnaryFunction): Boolean;
    function keys(out value: IahaArray): Boolean;
    function values(out value: IahaArray): Boolean;
    function Get(const param; out value): Boolean;
    constructor CreateCopy(const aindex: array of PNode; const ahash: IahaUnaryFunction; const aequal: IahaBinaryRelation);
  protected
    function copy(out value): Boolean; override;
  public
    constructor Create(const param: IHashTableParam);
  end;

function GetModuleData(out data: IModuleData; const Key, Value: ITypeInfoEx): Boolean;

implementation

uses
  SysUtils;

type
{ THashTableParam }

  THashTableParam = class(TahaComposite, IHashTableParam)
  private
    fhash: IahaUnaryFunction;
    fequal: IahaBinaryRelation;
    fsize: TahaInteger;
    function hash(out value: IahaUnaryFunction): Boolean;
    function equal(out value: IahaBinaryRelation): Boolean;
    function size(out value: TahaInteger): Boolean;
  end;

  { THashTableConstructor }

  generic THashTableConstructor<TKey, TValue> = class(TahaFunction, IahaUnaryFunction)
  private
    type THT = specialize THashTable<TKey, TValue>;
  private
    function Get(const param; out value): Boolean;
  end;

  { TModuleData }

  generic TModuleData<TKey, TValue> = class(TahaComposite, IModuleData)
  private
    type THashTableSpec = specialize THashTableConstructor<TKey, TValue>;
  private
    function HashTable(out value: IUnknown): Boolean;
  end;

   TModuleDataII = specialize TModuleData<TahaInteger, TahaInteger>;
   TModuleDataIC = specialize TModuleData<TahaInteger, TahaCharacter>;
   TModuleDataIO = specialize TModuleData<TahaInteger, IUnknown>;
   TModuleDataCI = specialize TModuleData<TahaCharacter, TahaInteger>;
   TModuleDataCC = specialize TModuleData<TahaCharacter, TahaCharacter>;
   TModuleDataCO = specialize TModuleData<TahaCharacter, IUnknown>;
   TModuleDataOI = specialize TModuleData<IUnknown, TahaInteger>;
   TModuleDataOC = specialize TModuleData<IUnknown, TahaCharacter>;
   TModuleDataOO = specialize TModuleData<IUnknown, IUnknown>;

function GetModuleData(out data: IModuleData; const Key, Value: ITypeInfoEx
  ): Boolean;
begin
  try
    if Key.integerType then
      begin
        if Value.integerType then
          begin
            data := TModuleDataII.Create;
          end
        else
        if Value.characterType then
          begin
            data := TModuleDataIC.Create;
          end
        else
          begin
            data := TModuleDataIO.Create;
          end;
      end
    else
    if Key.characterType then
      begin
        if Value.integerType then
          begin
            data := TModuleDataCI.Create;
          end
        else
        if Value.characterType then
          begin
            data := TModuleDataCC.Create;
          end
        else
          begin
            data := TModuleDataCO.Create;
          end;
      end
    else
      begin
        if Value.integerType then
          begin
            data := TModuleDataOI.Create;
          end
        else
        if Value.characterType then
          begin
            data := TModuleDataOC.Create;
          end
        else
          begin
            data := TModuleDataOO.Create;
          end;
      end;
    Result := True;
  except
    Result := False;
  end;
end;

{ TArrayWrapper }

procedure TArrayWrapper.resize(count: TahaInteger);
begin
  SetLength(FArray, count);
end;

function TArrayWrapper.size(out value: TahaInteger): Boolean;
begin
  value := Length(FArray);
  Result := True;
end;

function TArrayWrapper.at(const index: TahaInteger; out value): Boolean;
begin
  Result := (index >= 0) and (index < Length(FArray));
  TItem(value) := FArray[index];
end;

function TArrayWrapper.write(out value): Boolean;
var
  i: TahaInteger;
  p: PItem;
begin
  i := 0;
  p := @value;
  while i < Length(FArray) do
  begin
    p^ := FArray[i];
    Inc(p);
    Inc(i);
  end;
  Result := True;
end;

{ THashTableParam }

function THashTableParam.hash(out value: IahaUnaryFunction): Boolean;
begin
  value := fhash;
  Result := True;
end;

function THashTableParam.equal(out value: IahaBinaryRelation): Boolean;
begin
  value := fequal;
  Result := True;
end;

function THashTableParam.size(out value: TahaInteger): Boolean;
begin
  value := fsize;
  Result := True;
end;

{ THashTableConstructor }

function THashTableConstructor.Get(const param; out value): Boolean;
begin
  try
    THT(value) := THT.Create(IHashTableParam(param));
    Result := True;
  except
    Result := False;
  end;
end;

{ THashTable }

function THashTable.find(idx: array of PNode; const key: TKey; out value: PNode): Boolean;
var
  h: TahaInteger;
begin
  if hash.Get(key, h) then
    begin
      value := idx[h mod Length(index)];
      while Assigned(value) do
      begin
        if equal.Check(key, value^.key) then
          Break;
        value := value^.next;
      end;
      Result := Assigned(value);
    end
  else
    Result := False;
end;

function THashTable.add(out new: IHashTable; const param: IAddParam): Boolean;
var
  key: TKey;
  val: TValue;
  p, q: PNode;
  obj: THashTable;
  h: TahaInteger;
begin
  Result := param.key(key) and param.val(val) and (not find(index, key, p)) and getnew(obj);
  if Result then
  begin
    if hash.Get(key, h) then
      begin
        try
          System.New(p);
          q := obj.index[h mod Length(index)];
          p^.next := q;
          q^.prev := p;
          p^.prev := nil;
          p^.key := key;
          p^.val := val;
          obj.index[h mod Length(index)] := p;
          Inc(obj.count);
          new := obj;
        except
          if obj <> Self then
            obj.Free;
          Result := False;
        end;
      end
    else
      Result := False;
  end;
end;

function THashTable.replace(out new: IHashTable; const param: IAddParam
  ): Boolean;
var
  key: TKey;
  val: TValue;
  p: PNode;
  obj: THashTable;
begin
  Result := param.key(key) and param.val(val) and getnew(obj) ;
  if Result then
  begin
    if find(obj.index, key, p) then
      begin
        p^.val := val;
        new := obj;
      end
    else
      begin
        if obj <> Self then
          obj.Free;
        Result := False;
      end;
  end;
end;

function THashTable.remove(out new: IHashTable; const key): Boolean;
var
  p, q: PNode;
  obj: THashTable;
  h: TahaInteger;
begin
  Result := getnew(obj);
  if Result then
  begin
    if find(obj.index, TKey(key), p) then
      begin
        q := p^.prev;
        if Assigned(q) then
          begin
            q^.next := p^.next;
            Dispose(p);
            Dec(obj.count);
            new := obj;
          end
        else
          begin
            if hash.Get(key, h) then
              begin
                obj.index[h mod Length(index)] := p^.next;
                Dispose(p);
                Dec(obj.count);
                new := obj;
              end
            else
              begin
                if obj <> Self then
                  obj.Free;
                Result := False;
              end;
          end;
      end
    else
      begin
        if obj <> Self then
          obj.Free;
        Result := False;
      end;
  end;
end;

function THashTable.state(out value): Boolean;
begin
  IDictionary(value) := Self;
  Result := True;
end;

function THashTable.at(out value: IahaUnaryFunction): Boolean;
begin
  value := Self;
  Result := True;
end;

function THashTable.keys(out value: IahaArray): Boolean;
var
  i, j: TahaInteger;
  p: PNode;
  arr: TKeyArray;
begin
  try
    arr := TKeyArray.Create;
    arr.resize(count);
    i := 0;
    j := 0;
    while i < Length(index) do
    begin
      p := index[i];
      while Assigned(p) do
      begin
        arr.FArray[j] := p^.key;
        Inc(j);
        p := p^.next;
      end;
      Inc(i);
    end;
    value := arr;
    Result := True;
  except
    Result := False;
  end;
end;

function THashTable.values(out value: IahaArray): Boolean;
var
  i, j: TahaInteger;
  p: PNode;
  arr: TValueArray;
begin
  try
    arr := TValueArray.Create;
    arr.resize(count);
    i := 0;
    j := 0;
    while i < Length(index) do
    begin
      p := index[i];
      while Assigned(p) do
      begin
        arr.FArray[j] := p^.val;
        Inc(j);
        p := p^.next;
      end;
      Inc(i);
    end;
    value := arr;
    Result := True;
  except
    Result := False;
  end;
end;

function THashTable.Get(const param; out value): Boolean;
var
  p: PNode;
begin
  Result := find(index, TKey(param), p);
  if Result then
    TValue(value) := p^.val;
end;

function THashTable.copy(out value): Boolean;
var
  clone: THashTable;
begin
  try
    clone := THashTable.CreateCopy(index, hash, equal);
    TahaObject(value) := clone;
    Result := True;
  except
    Result := False;
  end;
end;

constructor THashTable.Create(const param: IHashTableParam);
var
  size, i: TahaInteger;
begin
  if param.hash(hash) and param.equal(equal) and param.size(size) then
    begin
      inherited Create;
      SetLength(index, size);
      i := 0;
      while i < size do
      begin
        index[i] := nil;
        Inc(i);
      end;
    end
  else
    Abort;
end;

constructor THashTable.CreateCopy(const aindex: array of PNode;
  const ahash: IahaUnaryFunction; const aequal: IahaBinaryRelation);
var
  i: TahaInteger;
  p, q, r: PNode;
begin
  inherited Create;
  hash := ahash;
  equal := aequal;
  SetLength(index, Length(aindex));
  i := 0;
  while i < Length(aindex) do
  begin
    p := aindex[i];
    r := nil;
    while Assigned(p) do
    begin
      New(q);
      q^.key := p^.key;
      q^.val := p^.val;
      q^.next := nil;
      if Assigned(r) then
        begin
          r^.next := q;
          q^.prev := r;
        end
      else
        begin
          q^.prev := nil;
          index[i] := q;
        end;
      r := q;
      p := p^.next;
    end;
    Inc(i);
  end;
end;

{ TModuleData }

function TModuleData.HashTable(out value: IUnknown): Boolean;
begin
  try
    value := THashTableSpec.Create;
    Result := True;
  except
    Result := False;
  end;
end;

end.

