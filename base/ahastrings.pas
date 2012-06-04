unit AhaStrings;

{$mode objfpc}{$H+}{$Q+}

interface

uses
  AhaCore;

type
  IStringBuilder = interface
    function getState(out output: IahaArray): Boolean;
    function add(new: Boolean; ch: WideChar; out output: IStringBuilder): Boolean;
    function append(new: Boolean; const str: IahaArray; out output: IStringBuilder): Boolean;
    function delete(new: Boolean; const index, count: Int64; out output: IStringBuilder): Boolean;
    function extract(new: Boolean; const index, count: Int64; out output: IStringBuilder): Boolean;
    function insert(new: Boolean; const index: Int64; const str: IahaArray; out output: IStringBuilder): Boolean;
    function put(new: Boolean; const index: Int64; ch: WideChar; out output: IStringBuilder): Boolean;
    function replace(new: Boolean; const index, count: Int64; const str: IahaArray; out output: IStringBuilder): Boolean;
  end;

  IStrings = interface
    function StringBuilder(out output: IStringBuilder): Boolean;
    function EqualString(out output: IahaBinaryRelation): Boolean;
  end;

function Module(out output: IStrings): Boolean;

implementation

uses
  SysUtils, AhaCharUtils;

const
  BlockSize = 1024;

type

  { TStringBuilder }

  TStringBuilder = class(TInterfacedObject, IStringBuilder)
  private
    FString: array of PWideChar;
    FLength: Int64;
    procedure Grow;
    function Copy: TStringBuilder;
    procedure Shift(index, count: Int64);
  protected
    function getState(out output: IahaArray): Boolean;
    function add(new: Boolean; ch: WideChar; out output: IStringBuilder): Boolean;
    function put(new: Boolean; const index: Int64; ch: WideChar; out output: IStringBuilder): Boolean;
    function append(new: Boolean; const str: IahaArray; out output: IStringBuilder): Boolean;
    function insert(new: Boolean; const index: Int64; const str: IahaArray; out output: IStringBuilder): Boolean;
    function replace(new: Boolean; const index, count: Int64; const str: IahaArray; out output: IStringBuilder): Boolean;
    function delete(new: Boolean; const index, count: Int64; out output: IStringBuilder): Boolean;
    function extract(new: Boolean; const index, count: Int64; out output: IStringBuilder): Boolean;
  end;

  { TEqualString }

  TEqualString = class(TInterfacedObject, IahaBinaryRelation)
  protected
    function check(const x, y): Boolean;
  end;

  { TStrings }

  TStrings = class(TInterfacedObject, IStrings)
  protected
    function StringBuilder(out output: IStringBuilder): Boolean;
    function EqualString(out output: IahaBinaryRelation): Boolean;
  end;

{ TEqualString }

function TEqualString.check(const x, y): Boolean;
var
  str1, str2: IahaArray;
  count1, count2: Int64;
  i: Cardinal;
  ch1, ch2: WideChar;
begin
  str1 := IahaArray(x);
  str2 := IahaArray(y);
  if str1.size(count1) and str2.size(count2) and (count1 = count2) then begin
    Result := True;
    for i := 0 to count1 - 1 do
      if str1.at(i, ch1) and str2.at(i, ch2) then begin
        if ch1 <> ch2 then begin
          Result := False;
          Exit;
        end;
      end
      else
        Result := False;
  end
  else
    Result := False;
end;

{ TStrings }

function TStrings.StringBuilder(out output: IStringBuilder): Boolean;
begin
  Result := True;
  output := TStringBuilder.Create;
end;

function TStrings.EqualString(out output: IahaBinaryRelation): Boolean;
begin
  Result := True;
  output := TEqualString.Create;
end;

{ TStringBuilder }

procedure TStringBuilder.Grow;
begin
  SetLength(FString, Length(FString) + 1);
  ReallocMem(FString[High(FString)], BlockSize + BlockSize);
end;

function TStringBuilder.Copy: TStringBuilder;
begin

end;

procedure TStringBuilder.Shift(index, count: Int64);
begin

end;

function TStringBuilder.getState(out output: IahaArray): Boolean;
begin
  Result := True;
  //output := Self;
end;

function TStringBuilder.add(new: Boolean; ch: WideChar; out output: IStringBuilder): Boolean;
begin

end;

function TStringBuilder.put(new: Boolean; const index: Int64; ch: WideChar; out
  output: IStringBuilder): Boolean;
begin

end;

function TStringBuilder.append(new: Boolean; const str: IahaArray; out
  output: IStringBuilder): Boolean;
begin

end;

function TStringBuilder.insert(new: Boolean; const index: Int64; const str: IahaArray;
  out output: IStringBuilder): Boolean;
begin

end;

function TStringBuilder.replace(new: Boolean; const index, count: Int64;
  const str: IahaArray; out output: IStringBuilder): Boolean;
begin

end;

function TStringBuilder.delete(new: Boolean; const index, count: Int64; out
  output: IStringBuilder): Boolean;
begin

end;

function TStringBuilder.extract(new: Boolean; const index, count: Int64; out output: IStringBuilder
  ): Boolean;
begin

end;

function Module(out output: IStrings): Boolean;
begin
  Result := True;
  output := TStrings.Create;
end;

end.

