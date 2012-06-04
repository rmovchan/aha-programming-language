unit CColl;

{$mode objfpc}{$H+}

interface

uses
  AhaCore, BColl;

type
  TItem = WideChar;

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
  AhaCharUtils;

{$I collections.inc}

end.

