library base;

{$mode objfpc}{$H+}

uses
  Classes, collections, core, metatypes, dictionaries, rational
  { you can add units after this };

exports
  collections.GetModuleData name 'Collections',
  dictionaries.GetModuleData name 'Dictionaries',
  rational.GetModuleData name 'Rational',
  metatypes.GetModuleData name 'MetaTypes';

begin
end.

