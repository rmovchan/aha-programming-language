unit BColl;

{$mode objfpc}{$H+}

interface

uses
  AhaCore;

type
  IDynamicArray = interface
    function getState(out output): Boolean;
    function add(new: Boolean; const item; out output: IDynamicArray): Boolean;
  end;

  IInOutStorage = interface
    function getState(out output): Boolean;
    function pop(new: Boolean; out output: IInOutStorage): Boolean;
    function push(new: Boolean; const item; out output: IInOutStorage): Boolean;
  end;

  ICollections = interface
    function DynamicArray(out Output: IDynamicArray): Boolean;
    function Queue(out Output: IInOutStorage): Boolean;
    function Stack(out Output: IInOutStorage): Boolean;
  end;

implementation

end.

