library base;

{$mode objfpc}{$H+}

uses
  Classes, collections, core, metatypes, dictionaries
  { you can add units after this };

exports
  collections.GetModuleData name 'Collections',
  dictionaries.GetModuleData name 'Dictionaries',
  metatypes.GetModuleData name 'MetaTypes';

begin
end.

