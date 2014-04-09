unit rational;
(*
doc
    Title:   "Rational"
    Package: "Aha! Base Library"
    Purpose: "Rational numbers"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2012-06-02"
end

export Types:
    type Rational: opaque "a rational number"
    type RatioStruc:
        [
            num: integer "numerator"
            den: integer "denominator"
        ] "rational as composite"
end

export Operators:
    (integer / integer): { integer, integer -> Rational } "divide integers to get Rational"
    (~struc Rational): { Rational -> RatioStruc } "convert Rational to RatioStruc"
    (Rational + Rational): { Rational, Rational -> Rational } "sum of two rationals"
    (Rational - Rational): { Rational, Rational -> Rational } "difference between two rationals"
    (Rational * Rational): { Rational, Rational -> Rational } "product of two rationals"
    (Rational / Rational): { Rational, Rational -> Rational } "quotient of two rationals"
    (Rational < Rational): { Rational, Rational } "is first rational less than second?"
    (Rational <= Rational): { Rational, Rational } "is first rational less than or equal to second?"
    (Rational = Rational): { Rational, Rational } "is first rational equal to second?"
    (Rational /= Rational): { Rational, Rational } "is first rational not equal to second?"
    (Rational >= Rational): { Rational, Rational } "is first rational greater than or equal to second?"
    (Rational > Rational): { Rational, Rational } "is first rational greater than second?"
end
*)
{$mode objfpc}{$H+}

interface

uses
  core;

type
  IModuleData = interface
    function Ratio(out value: IahaBinaryFunction): Boolean;
    function RatioStruc(out value: IahaUnaryFunction): Boolean;
    function RationalSum(out value: IahaBinaryFunction): Boolean;
    function RationalDiff(out value: IahaBinaryFunction): Boolean;
    function RationalProd(out value: IahaBinaryFunction): Boolean;
    function RationalDiv(out value: IahaBinaryFunction): Boolean;
    function RationalLess(out value: IahaBinaryRelation): Boolean;
    function RationalLessEqual(out value: IahaBinaryRelation): Boolean;
    function RationalEqual(out value: IahaBinaryRelation): Boolean;
    function RationalNotEqual(out value: IahaBinaryRelation): Boolean;
    function RationalGreaterEqual(out value: IahaBinaryRelation): Boolean;
    function RationalGreater(out value: IahaBinaryRelation): Boolean;
  end;

function GetModuleData(out value: IModuleData): Boolean;

implementation

type
  IRatio = interface
    function num(out value: TahaInteger): Boolean;
    function den(out value: TahaInteger): Boolean;
  end;

  { TModuleData }

  TModuleData = class(TahaComposite, IModuleData)
  private
    function Ratio(out value: IahaBinaryFunction): Boolean;
    function RatioStruc(out value: IahaUnaryFunction): Boolean;
    function RationalSum(out value: IahaBinaryFunction): Boolean;
    function RationalDiff(out value: IahaBinaryFunction): Boolean;
    function RationalProd(out value: IahaBinaryFunction): Boolean;
    function RationalDiv(out value: IahaBinaryFunction): Boolean;
    function RationalLess(out value: IahaBinaryRelation): Boolean;
    function RationalLessEqual(out value: IahaBinaryRelation): Boolean;
    function RationalEqual(out value: IahaBinaryRelation): Boolean;
    function RationalNotEqual(out value: IahaBinaryRelation): Boolean;
    function RationalGreaterEqual(out value: IahaBinaryRelation): Boolean;
    function RationalGreater(out value: IahaBinaryRelation): Boolean;
  end;

  { TRatio }

  TRatio = class(TahaComposite, IRatio)
  private
    fnum: TahaInteger;
    fden: TahaInteger;
    function num(out value: TahaInteger): Boolean;
    function den(out value: TahaInteger): Boolean;
  end;

  { TRatioFunc }

  TRatioFunc = class(TahaFunction, IahaBinaryFunction)
  private
    function Get(const param1, param2; out value): Boolean;
  end;

  { TRatioStrucFunc }

  TRatioStrucFunc = class(TahaFunction, IahaUnaryFunction)
  private
    function Get(const param; out value): Boolean;
  end;

  { TRationalSum }

  TRationalSum = class(TahaFunction, IahaBinaryFunction)
  private
    function Get(const param1, param2; out value): Boolean;
  end;

  { TRationalDiff }

  TRationalDiff = class(TahaFunction, IahaBinaryFunction)
  private
    function Get(const param1, param2; out value): Boolean;
  end;

  { TRationalProd }

  TRationalProd = class(TahaFunction, IahaBinaryFunction)
  private
    function Get(const param1, param2; out value): Boolean;
  end;

  { TRationalDiv }

  TRationalDiv = class(TahaFunction, IahaBinaryFunction)
  private
    function Get(const param1, param2; out value): Boolean;
  end;

  { TRationalLess }

  TRationalLess = class(TahaFunction, IahaBinaryRelation)
  private
    function Check(const param1, param2): Boolean;
  end;

  { TRationalLessEqual }

  TRationalLessEqual = class(TahaFunction, IahaBinaryRelation)
  private
    function Check(const param1, param2): Boolean;
  end;

  { TRationalEqual }

  TRationalEqual = class(TahaFunction, IahaBinaryRelation)
  private
    function Check(const param1, param2): Boolean;
  end;

  { TRationalNotEqual }

  TRationalNotEqual = class(TahaFunction, IahaBinaryRelation)
  private
    function Check(const param1, param2): Boolean;
  end;

  { TRationalGreaterEqual }

  TRationalGreaterEqual = class(TahaFunction, IahaBinaryRelation)
  private
    function Check(const param1, param2): Boolean;
  end;

  { TRationalGreater }

  TRationalGreater = class(TahaFunction, IahaBinaryRelation)
  private
    function Check(const param1, param2): Boolean;
  end;

function GetModuleData(out value: IModuleData): Boolean;
begin
  try
    value := TModuleData.Create;
    Result := True;
  except
    Result := False;
  end;
end;

{ TRationalLessEqual }

function TRationalLessEqual.Check(const param1, param2): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
begin
  try
    Result :=
      IRatio(param1).num(n1) and IRatio(param1).den(d1) and
      IRatio(param2).num(n2) and IRatio(param1).den(d2) and
      (n1 * d2 <= n2 * d1);
  except
    Result := False;
  end;
end;

{ TRationalEqual }

function TRationalEqual.Check(const param1, param2): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
begin
  try
    Result :=
      IRatio(param1).num(n1) and IRatio(param1).den(d1) and
      IRatio(param2).num(n2) and IRatio(param1).den(d2) and
      (n1 * d2 = n2 * d1);
  except
    Result := False;
  end;
end;

{ TRationalNotEqual }

function TRationalNotEqual.Check(const param1, param2): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
begin
  try
    Result :=
      IRatio(param1).num(n1) and IRatio(param1).den(d1) and
      IRatio(param2).num(n2) and IRatio(param1).den(d2) and
      (n1 * d2 <> n2 * d1);
  except
    Result := False;
  end;
end;

{ TRationalGreaterEqual }

function TRationalGreaterEqual.Check(const param1, param2): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
begin
  try
    Result :=
      IRatio(param1).num(n1) and IRatio(param1).den(d1) and
      IRatio(param2).num(n2) and IRatio(param1).den(d2) and
      (n1 * d2 >= n2 * d1);
  except
    Result := False;
  end;
end;

{ TRationalGreater }

function TRationalGreater.Check(const param1, param2): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
begin
  try
    Result :=
      IRatio(param1).num(n1) and IRatio(param1).den(d1) and
      IRatio(param2).num(n2) and IRatio(param1).den(d2) and
      (n1 * d2 > n2 * d1);
  except
    Result := False;
  end;
end;

{ TRationalLess }

function TRationalLess.Check(const param1, param2): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
begin
  try
    Result :=
      IRatio(param1).num(n1) and IRatio(param1).den(d1) and
      IRatio(param2).num(n2) and IRatio(param1).den(d2) and
      (n1 * d2 < n2 * d1);
  except
    Result := False;
  end;
end;

{ TRationalDiv }

function TRationalDiv.Get(const param1, param2; out value): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
  rat: TRatio;
begin
  Result := IRatio(param1).num(n1) and IRatio(param1).den(d1) and
    IRatio(param2).num(n2) and IRatio(param1).den(d2);
  if Result then
    try
      rat := TRatio.Create;
      rat.fnum := n1 * d2;
      rat.fden := d1 * n2;
      IRatio(value) := rat;
    except
      Result := False;
    end;
end;

{ TRationalProd }

function TRationalProd.Get(const param1, param2; out value): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
  rat: TRatio;
begin
  Result := IRatio(param1).num(n1) and IRatio(param1).den(d1) and
    IRatio(param2).num(n2) and IRatio(param1).den(d2);
  if Result then
    try
      rat := TRatio.Create;
      rat.fnum := n1 * n2;
      rat.fden := d1 * d2;
      IRatio(value) := rat;
    except
      Result := False;
    end;
end;

{ TRationalDiff }

function TRationalDiff.Get(const param1, param2; out value): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
  rat: TRatio;
begin
  Result := IRatio(param1).num(n1) and IRatio(param1).den(d1) and
    IRatio(param2).num(n2) and IRatio(param1).den(d2);
  if Result then
    try
      rat := TRatio.Create;
      rat.fnum := (n1 * d2) - (n2 * d1);
      rat.fden := d1 * d2;
      IRatio(value) := rat;
    except
      Result := False;
    end;
end;

{ TModuleData }

function TModuleData.Ratio(out value: IahaBinaryFunction): Boolean;
begin
  try
    value := TRatioFunc.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RatioStruc(out value: IahaUnaryFunction): Boolean;
begin
  try
    value := TRatioStrucFunc.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalSum(out value: IahaBinaryFunction): Boolean;
begin
  try
    value := TRationalSum.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalDiff(out value: IahaBinaryFunction): Boolean;
begin
  try
    value := TRationalDiff.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalProd(out value: IahaBinaryFunction): Boolean;
begin
  try
    value := TRationalProd.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalDiv(out value: IahaBinaryFunction): Boolean;
begin
  try
    value := TRationalDiv.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalLess(out value: IahaBinaryRelation): Boolean;
begin
  try
    value := TRationalLess.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalLessEqual(out value: IahaBinaryRelation): Boolean;
begin
  try
    value := TRationalLessEqual.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalEqual(out value: IahaBinaryRelation): Boolean;
begin
  try
    value := TRationalEqual.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalNotEqual(out value: IahaBinaryRelation): Boolean;
begin
  try
    value := TRationalNotEqual.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalGreaterEqual(out value: IahaBinaryRelation
  ): Boolean;
begin
  try
    value := TRationalGreaterEqual.Create;
    Result := True;
  except
    Result := False;
  end;
end;

function TModuleData.RationalGreater(out value: IahaBinaryRelation): Boolean;
begin
  try
    value := TRationalGreater.Create;
    Result := True;
  except
    Result := False;
  end;
end;

{ TRationalSum }

function TRationalSum.Get(const param1, param2; out value): Boolean;
var
  n1, n2, d1, d2: TahaInteger;
  rat: TRatio;
begin
  Result := IRatio(param1).num(n1) and IRatio(param1).den(d1) and
    IRatio(param2).num(n2) and IRatio(param1).den(d2);
  if Result then
    try
      rat := TRatio.Create;
      rat.fnum := (n1 * d2) + (n2 * d1);
      rat.fden := d1 * d2;
      IRatio(value) := rat;
    except
      Result := False;
    end;
end;

{ TRatioStrucFunc }

function TRatioStrucFunc.Get(const param; out value): Boolean;
begin
  IRatio(value) := IRatio(param);
  Result := True;
end;

{ TRatioFunc }

function TRatioFunc.Get(const param1, param2; out value): Boolean;
var
  rat: TRatio;
begin
  try
    rat := TRatio.Create;
    rat.fnum := TahaInteger(param1);
    rat.fden := TahaInteger(param2);
    IRatio(value) := rat;
    Result := True;
  except
    Result := False;
  end;
end;

{ TRatio }

function TRatio.num(out value: TahaInteger): Boolean;
begin
  value := fnum;
  Result := True;
end;

function TRatio.den(out value: TahaInteger): Boolean;
begin
  value := fden;
  Result := True;
end;

end.

