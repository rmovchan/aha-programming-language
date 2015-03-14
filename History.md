# News #

## 2014-04-01 ##

The Aha! implementation for .Net commenced. The old Free Pascal code of the Base Library is now under the branch 'Pascal', the master branch now contains the C# code.

## 2014-03-14 ##

Module `Float` was superseded by [Math](Math.md).

## 2014-01-17 ##

The syntax of literal arrays was changed: values are now comma-separated, instead of semicolon-terminated. The syntax of composites didn't change.

Also, the module `Variants` and the `Variant` data type were changed to [Dynamic](Dynamic.md).

## 2013-11-10 ##

Example [TypeAsString](TypeAsString.md) was added.

## 2013-10-16 ##

A minor change to the syntax was made: **where** is replaced with **when** in cases when the following statement defines no variables.

## 2013-10-04 ##

The structure of the Wiki pages was improved.

## 2013-09-20 ##

Actors were renamed to appliances for clarity; accordingly, modules `Actor` and `ActorDef` were renamed to [Appliance](Appliance.md) and [ApplianceDef](ApplianceDef.md).

## 2013-09-19 ##

The [syntax definition file](https://drive.google.com/file/d/0B7s-cEATWx1nVzZVd1pKWWo2cXM/edit?usp=sharing) for use with [Notepad++](http://notepad-plus-plus.org/download) (Windows only) was published.

## 2013-09-12 ##

Module `FileIO` was superseded by [FileAccess](FileAccess.md).

## 2013-09-10 ##

Modules `Actor` and `ActorDef` were added to the API.

## 2013-09-06 ##

Modules [Process](Process.md) and [ProcessDef](ProcessDef.md) were added to the API.

## 2013-09-05 ##

Example [SortFile](SortFile.md) was reworked to match the new API.

## 2013-09-03 ##

Module `FileIO` has been added to the [API](API.md).

## 2013-08-27 ##

A brief introduction to the [API](API.md) has been published. Important parts of the API, such as file I/O and GUI, are still being worked upon.

## 2013-08-22 ##

The first API module, [Environment](Environment.md), has been published.

## 2013-08-15 ##

Module [Conversions](Conversions.md) has been added to the Base Library.

## 2013-08-14 ##

The Language Reference has been updated to reflect the recent changes to the syntax.

Module [Trees](Trees.md) has been reworked.

Example [Meta](Meta.md) has been added demonstrating access to the type information in Aha! at run time.

## 2013-08-01 ##

The following changes have been made to the syntax:

  * definition statement now can be used to define an operator; an operator (pattern) is now considered an alternative form of a function-type variable
  * a definition now must be terminated with an exclamation mark
  * composite expressions and composite type references are now enclosed in square brackets, instead of **com**-**end** pairs
  * expressions now must be terminated with semicolons in the following cases: field values in composite expressions (including **alter** and **extend**), the initial state and an action body in **obj** expressions, values of items in array literals
  * operators don't need to be enclosed in parentheses, unless they are nested in other operator expressions
  * **return** expression is replaced with new syntax: ( _expression_ **where** _statement_ )
  * **let** statement has been deprecated; instead, **where** clauses now can be added to **all**, **any** and **unless** statements before the **end**
  * the syntax of **seq** expressions has been simplified (**then** replaced by a comma and **end** removed): **seq** _first_,_next_
  * commas between items in **join** expressions are now mandatory
  * semicolon between statements in **all** and **any** now isn't used
  * comma in composite type references now isn't used

The Wiki and Language Reference are being updated to reflect the changes in syntax.

## 2013-06-04 ##

The Language Reference was updated. There are minor changes to the syntax, including new notation for external module references, empty sequences, composite extension, plus change to module's code structure (must contain an expression rather than a statement).

Since the Downloads feature has been deprecated by Google, the Language Reference now resides at Google Drive.

## 2013-05-31 ##

Example [Merge](Merge.md) was added.

## 2013-05-28 ##

The keyword **curr** ("current") was changed to **prev** ("previous").

## 2013-05-23 ##

The keyword **get** was changed to **return** for better readability.

## 2013-04-10 ##

The specification of module [Variants](Variants.md) was added to Wiki.

## 2012-10-25 ##

The [Tokenizer](Tokenizer.md) example was added.

## 2012-06-19 ##

The Frequently Asked Questions ([FAQ](FAQ.md)) were added to Wiki.

## 2012-06-14 ##

The spec of module [Trees](Trees.md) was added to Wiki.

## 2012-06-09 ##

The Language Reference was added to Downloads.

## 2012-06-08 ##

[Getting Started guide](GettingStarted.md) was reworked to make it shorter and easier to digest.

## 2012-06-07 ##

The specificatiom of module [Bits](Bits.md) was added to Wiki.

## 2012-06-03 ##

The specifications of the following modules of the Base Library were added to Wiki:
  * [Collections](Collections.md)
  * [Rational](Rational.md)
  * [Float](Float.md)
  * [Time](Time.md)
  * [Money](Money.md)
  * [Formatting](Formatting.md)
  * [Dictionaries](Dictionaries.md)
  * [StrUtils](StrUtils.md)
  * [Miscellaneous](Miscellaneous.md)