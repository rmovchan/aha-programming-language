unit AhaCore;

{$mode objfpc}{$H+}

interface

type
  TTypeParameterType = (tptNull, tptCharacter, tptInteger, tptUserDefined);

  TahaNil = packed record
    dummy: Byte;
  end;

  IahaUnaryRelation = interface
    function check(const x): Boolean;
  end;

  IahaBinaryRelation = interface
    function check(const x, y): Boolean;
  end;

  IahaUnaryFunction = interface
    function get(const x; out output): Boolean;
  end;

  IahaBinaryFunction = interface
    function get(const x, y; out output): Boolean;
  end;

  IahaArrayFunction = interface
    function get(const index: Int64; out output): Boolean;
  end;

  IahaSequence = interface
    function getState(out output): Boolean;
    function next(new: Boolean; out output: IahaSequence): Boolean;
  end;

  IahaArray = interface
    function at(const index: Int64; out output): Boolean;
    function size(out output: Int64): Boolean;
    function apply(const func: IahaBinaryFunction; out output): Boolean;
    function sort(const func: IahaBinaryRelation; out output: IahaSequence): Boolean;
    function forEach(const func: IahaUnaryRelation): Boolean;
    function forSome(const func: IahaUnaryRelation): Boolean;
    function select(const func: IahaUnaryRelation; out output: IahaArray): Boolean;
    function count(const func: IahaUnaryRelation; out output: Int64): Boolean;
    function list(const func: IahaUnaryRelation; out output: IahaSequence): Boolean;
    function such(const func: IahaUnaryRelation; out output): Boolean;
    function write(out output): Boolean;
  end;

function EmptySeq(out output: IahaSequence): Boolean; inline;
function IntegerSum(const x, y: Int64; out output: Int64): Boolean; inline;
function IntegerLess(const x, y: Int64): Boolean; inline;
function CharacterLess(x, y: WideChar): Boolean; inline;
function AhaStringToString(const str: IahaArray; out output: UnicodeString): Boolean; inline;

implementation

type

  { TahaEmptySeq }

  TahaEmptySeq = class(TInterfacedObject, IahaSequence)
  protected
    function getState(out output): Boolean;
    function next(new: Boolean; out output: IahaSequence): Boolean;
  end;

function EmptySeq(out output: IahaSequence): Boolean; inline;
begin
  try
    output := TahaEmptySeq.Create;
    Result := True;
  except
    Result := False;
  end;
end;

{$Q+}
function IntegerSum(const x, y: Int64; out output: Int64): Boolean; inline;
begin
  try
    output := x + y;
    Result := True;
  except
    Result := False;
  end;
end;
{$Q-}

function IntegerLess(const x, y: Int64): Boolean; inline;
begin
  Result := x < y;
end;

function CharacterLess(x, y: WideChar): Boolean; inline;
begin
  Result := x < y;
end;

function AhaStringToString(const str: IahaArray; out output: UnicodeString): Boolean;
var
  l: Int64;
begin
  try
    if str.size(l) then begin
      SetLength(output, l);
      str.write(output[1]);
      Result := True;
    end
    else
      Result := False;
  except
    Result := False;
  end;
end;

{ TahaEmptySeq }

function TahaEmptySeq.getState(out output): Boolean;
begin
  Result := False;
end;

function TahaEmptySeq.next(new: Boolean; out output: IahaSequence): Boolean;
begin
  Result := False;
end;

end.

