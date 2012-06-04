unit AhaUDTUtils;

{$mode objfpc}{$H+}

interface

uses
  AhaCore;

type
  TItem = IUnknown;
  TArray = array of TItem;

function AhaArrayOf(arr: array of TItem; out output: IahaArray): Boolean;

function AhaArrayFromArray(arr: TArray; out output: IahaArray): Boolean; inline;

function AhaArrayAppend(arr: array of IahaArray; out output: IahaArray): Boolean;

function AhaArrayFromFunc(const func: IahaUnaryFunction; const count: Int64;
  out output: IahaArray): Boolean; inline;

function AhaArrayFromSeq(const seq: IahaSequence; const count: Int64;
  out output: IahaArray): Boolean; inline;

function AhaSeqSelect(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: IahaArray): Boolean;

function AhaSeqSuch(const seq: IahaSequence; const func: IahaUnaryRelation;
  const count: Int64; out output: TItem): Boolean;

implementation

{$I ahautils.inc}

end.

