unit UColl;

{$mode objfpc}{$H+}

interface

uses
  AhaCore, BColl, BigLists;

type
  TItem = IUnknown;

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
  AhaUDTUtils;

{$I collections.inc}

end.

