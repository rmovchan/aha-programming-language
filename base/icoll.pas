unit IColl;

{$mode objfpc}{$H+}

interface

uses
  AhaCore, BColl, BigLists;

type
  TItem = Int64;

  { TCollections }

  TCollections = class(TInterfacedObject, ICollections)
  protected
    function DynamicArray(out output: IDynamicArray): Boolean;
    function Queue(out Output: IInOutStorage): Boolean;
    function Stack(out Output: IInOutStorage): Boolean;
  public
    constructor Create;
  end;

implementation

uses
  AhaIntUtils;

{$I collections.inc}

end.

