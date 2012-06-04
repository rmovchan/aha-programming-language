unit NColl;

{$mode objfpc}{$H+}

interface

uses
  AhaCore, BColl, BigLists;

type
  TItem = TahaNil;

  TItems = specialize TBigList<TItem>;

{$I collections.inc}

end.

