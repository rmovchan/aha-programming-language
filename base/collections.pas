unit Collections;

{$mode objfpc}{$H+}

interface

uses
  AhaCore, BColl;

function Module(item: TTypeParameterType; out output: ICollections): Boolean;

implementation

uses
  CColl, IColl, UColl;

function Module(item: TTypeParameterType; out output: ICollections): Boolean;
begin
  Result := True;
  case item of
  tptNull:
    output := NColl.TCollections.Create;
  tptCharacter:
    output := CColl.TCollections.Create;
  tptInteger:
    output := IColl.TCollections.Create;
  else
    output := UColl.TCollections.Create;
  end;
end;

end.

