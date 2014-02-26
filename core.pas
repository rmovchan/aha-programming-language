unit core;

{$mode objfpc}{$H+}

interface

uses
  Classes, SysUtils;

type
  PahaInteger =^TahaInteger;
  TahaInteger = Int64;

  PahaCharacter = ^TahaCharacter;
  TahaCharacter = WideChar;

  TahaObject = TInterfacedObject;

  TahaComposite = TInterfacedObject;

  TahaOpaque = TInterfacedObject;

  IahaArray = interface
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
  end;

  TahaFooArrayWrapper = class(TInterfacedObject, IahaArray)
  private
    FSize: TahaInteger;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
  public
    constructor Create(const content: TahaInteger);
  end;

  TahaCharArray = UnicodeString;

  TahaCharArrayWrapper = class(TInterfacedObject, IahaArray)
  private
    FSize: TahaInteger;
    FItems: PahaCharacter;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
  public
    constructor Create(const content: TahaCharArray);
  end;

  TahaIntArray = array of TahaInteger;

  TahaIntArrayWrapper = class(TInterfacedObject, IahaArray)
  private
    FSize: TahaInteger;
    FItems: PahaInteger;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
  public
    constructor Create(const content: TahaIntArray);
  end;

  TahaOtherArray = array of IUnknown;

  TahaOherArrayWrapper = class(TInterfacedObject, IahaArray)
  private
    FArray: TahaOtherArray;
  protected
    function size(out value: TahaInteger): Boolean;
    function at(const index: TahaInteger; out value): Boolean;
  public
    constructor Create(const content: TahaOtherArray);
  end;

  TahaFooRelation = interface
    function Check: Boolean;
  end;

  TahaUnaryRelation = interface
    function Check(const param): Boolean;
  end;

  TahaBinaryRelation = interface
    function Check(const param1, param2): Boolean;
  end;

  TahaUnaryFunction = interface
    function Get(const param; out value): Boolean;
  end;

  TahaBinaryFunction = interface
    function Get(const param1, param2; out value): Boolean;
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

