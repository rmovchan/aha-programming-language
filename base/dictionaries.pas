unit Dictionaries;

{$mode objfpc}{$H+}

interface

uses
  AhaCore, BDict;

function Module(key, value: TTypeParameterType; out output: IDictionaries): Boolean;

implementation

uses
  CCDict, ICDict, UCDict, CIDict, IIDict, UIDict, CUDict, IUDict, UUDict;

function Module(key, value: TTypeParameterType; out output: IDictionaries): Boolean;
begin
  case key of
  tptCharacter:
    case value of
    tptCharacter:
      output := CCDict.TDictionaries.Create;
    tptInteger:
      output := ICDict.TDictionaries.Create;
    else
      output := UCDict.TDictionaries.Create;
    end;
  tptInteger:
  else
  end;
end;

end.

