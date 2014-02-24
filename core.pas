unit core;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils;

type
  TahaInteger = Int64;

  TahaCharacter = WideChar;

  TahaObject = TInterfacedObject;

  TahaComposite = TInterfacedObject;

  TahaArray = TInterfacedObject;

  TahaOpaque = TInterfacedObject;

  IahaArray = interface
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
  end;

function IntPlus(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntMinus(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntMult(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntDiv(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntMod(const a, b: TahaInteger; out c: TahaInteger): Boolean; inline;
function IntLess(const a, b: TahaInteger): Boolean; inline;
function IntLessEqual(const a, b: TahaInteger): Boolean; inline;
function IntEqual(const a, b: TahaInteger): Boolean; inline;
function IntNotEqual(const a, b: TahaInteger): Boolean; inline;
function IntGreaterEqual(const a, b: TahaInteger): Boolean; inline;
function IntGreater(const a, b: TahaInteger): Boolean; inline;
function CharEqual(const a, b: TahaCharacter): Boolean; inline;

implementation

end.

