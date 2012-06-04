unit BDict;

{$mode objfpc}{$H+}

interface

uses
  AhaCore;

type
  IDictionary = interface
    function get(const key; out value): Boolean;
    function has(const key): Boolean;
    function keys(out output: IahaArray): Boolean;
    function values(out output: IahaArray): Boolean;
  end;

  IDictionaryBuilder = interface
    function getState(out output: IDictionary): Boolean;
    function add(new: Boolean; const key, value; out output: IDictionaryBuilder): Boolean;
    function remove(new: Boolean; const key; out output: IDictionaryBuilder): Boolean;
    function replace(new: Boolean; const key, value; out output: IDictionaryBuilder): Boolean;
  end;

  IHashFunction = interface
    function get(const key; out output: Int64): Boolean;
  end;

  IEqual = interface
    function check(const x, y): Boolean;
  end;

  IHashTable = interface
    function get(const hash: IHashFunction; const equal: IEqual; const size: Int64;
             out output: IDictionaryBuilder): Boolean;
  end;

  IDictionaries = interface
    function HashTable(out output: IHashTable): Boolean;
  end;

implementation

end.

