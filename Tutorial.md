# Overview #
Aha! is a new general-purpose programming language designed to solve many of the problems that software developers face every day. It is meant to make the software more predictable and reliable, the source code - more readable and easier to maintain, and the development process - more productive and enjoyable.

Aha! is a declarative language. This means that programs in Aha! describe the desired results (e.g. the behavior of an interactive application) and not sequences of commands that might produce the desired results. Programs in Aha! only compute the results and can't have any side effects. One could say that Aha! is a purely functional language, however it differs from such languages syntactically and in some approaches.

Programs in Aha! are highly structured. From the program code, it is always clear from where each value comes and of what data type it is. It is not possible under any circumstances to use a value that may be undefined, even partially.

Aha! provides a simple but effective and safe mechanism for defining objects. However, it intentionally lacks the mechanism of inheritance and, therefore, can't be considered an object-oriented language.

Here is a brief summary of the features of Aha!:
  * modular structure designed to avoid any name conflicts and separate the implementation from the specification
  * generic modules, as opposed to generic data types
  * abstract (opaque) data types
  * advanced data type system with static typing
  * no pointers, no _null_ or _nil_ values except the special **nil** data type
  * first-class functions (i.e. functions are values)
  * sequence generators for 'lazy' computations
  * objects are easy to define and use; object types are 'interfaces', not 'classes'
  * all values are immutable (i.e. never change) and side effects are not possible; therefore, different parts of a program can't implicitly affect each other
  * data types of expressions are always inferred and local variables are never declared
  * expressions and statements can produce their output or fail, in which case their output can't (and won't) be used
  * statement success/failure logic replaces both the _Boolean_ data type and the exception mechanism
  * programs are intrinsically parallel: any computations can be performed in parallel unless they require data from other computations
  * expressions and statements with **where** clauses that define substitutions/preconditions
  * high-level array/sequence operations
  * no constructs that could cause infinite computations
  * user-defined operators; operators have no precedence rules
  * automatic memory management

# Data Types #

Every value in Aha! belongs to a specific, and only one, data type. No implicit conversions or explicit typecasts are provided; explicit type conversion functions must be invoked where necessary.

Only three primitive data types are built into the language: **nil**, **integer** and **character**.

The **nil** data type means void (irrelevant) data. **nil** is also the only value of this data type.

The **integer** data type represents integers; we assume 64-bit representation on most implementations. All the standard arithmetic (+, -, `*`, / and //(modulo)) and comparison (<, <=, =, /=, >=, >) operators are built into the language.

The **character** data type consists of arbitrary characters. Character values can be compared for equality (=).

In addition, various data types are provided through the standard library, called Aha! Base Library: floating-point and unlimited precision numbers, date and time, dynamic collections and strings, trees, etc.

Complex data types are built using the following data structures:

  * arrays
  * composites (records)
  * functions
  * sequences
  * objects

## Arrays ##

Arrays are finite collections of uniform data of any type.

An array's size is denoted `<`array`>`**#**, and its items are accessed via integer indexes (offsets): `<`array`>`(`<`index`>`). Indexes must be from 0 to array's size minus one; otherwise, the item access operation fails.

Instead of dealing with individual items, it is often possible to use group operations on arrays, such as **select**, **foreach** etc. (see "Expressions" and "Statements"). Such operations can be implemented very efficiently on multi-processor systems.

The type of arrays with items of type T is referred to as `[`T`]`, e.g.: **`[`character`]`**, **`[[`integer`]]`**.
## Composites ##
A composite value consists of several named values, called fields, possibly of different types. A simple composite data type describes a class of composite values with identical field names and their corresponding data types. For example, the values ( name: “Aha!”, number: 9999 ) and (number: 1, name: “FORTRAN” ) both belong to the type (name: **`[`character`]`,** number: **integer**). The order of the fields in a composite value is irrelevant.

A composite type with variants is a generalisation of a simple composite type where several distinct field sets exist. For example, the values ( value: 111, extra: `[`1 2 3`]` ) and ( novalue: **nil** ) both belong to the variant type ( value: **integer,** extra: **`[`integer`]`** | novalue: **nil** ). For every value of such a type, only one set of fields actually exists, but all the fields in the set must be present. A value of a composite type with variants can be defined using a disjunctive statement (**any** or **unless**, see section "Statements"), where different branches define composite values with different sets of fields. An attempt to extract a field from a field set that doesn't belong to the actual value of a variant composite will fail.
## Functions ##
Functions in Aha! are first-class values, i.e. can be part of an expression like any other value. One can be assigned to a variable (or variables) if desired, but can also be used anonymously.

A function consists of parameter(s) and an expression that computes the result based on them. The type(s) of the parameter(s) must always be specified; the type of the result is always inferred from the expression. There is no way for a function to call itself except by using a special **letrec** construct (see below).

Functions in Aha! can have only one or two parameters. Parameters of composite types can be used to simulate a larger number of parameters. The reason for this approach is that the syntax for composites with many fields is more readable than the syntax for functions with many parameters, while semantically a composite parameter is equivalent to multiple parameters. On the other hand, functions with two parameters are useful for representing binary operators. For an example of a function with a variant composite type parameter, see _Search_ in Base/[StrUtils](StrUtils.md).

A functional type is referred to as `{ <param type> -> <result type> }` or `{ <param type>, <param type> -> <result type> }`, where `<param type>` is the type of the function's parameter and `<result type>` is the type of its result.
## Sequences ##
Sequences are similar to arrays in that they contain uniform data. However, their items can’t be accessed by indexes; they have to be extracted one by one. Aha provides two basic operations on sequences: extracting the first item (**^**) and getting the rest of the sequence, without the first item (**.skip**).

The biggest benefit of sequences is that their items can be computed on demand, i.e. lazily. It is therefore possible to define infinite sequences without causing the risk of infinite computations. Indeed, Aha! doesn’t contain any constructs that could cause infinite computations.

The most common way to define a sequence is using a **seq**-**then** expression (a _sequence generator_). The programmer needs to specify the initial sequence item and the rule by which subsequent items are computed (an expression where the previous item is denoted **curr**).

Alternatively, a (finite) sequence can be obtained from an array.

Finally, a sequence can be defined as an object; see below section “Objects”.

The type of sequences of items of type T is referred to as T**`*`**.

## Objects ##
Objects are used to simulate values that change over time. An object has a state of an arbitrary type and a set of actions that change the state. This doesn’t mean that objects are mutable values: an object with a changed state is considered a new object.

Object values are created using the **obj**-**end** expression. Unlike most so-called object-oriented languages, in Aha! it is not necessary to declare a class first in order to create an object.

Objects are abstract. They encapsulate some ‘internal’ state that is mapped into ‘external’ state that is visible to the users of the object. An object’s actions are essentially functions that convert its internal state into some new internal state. Since an object type reference includes its external, but not internal, state, two objects with the internal state of completely different types can belong to the same object type. An object type, therefore, serves as the ‘interface’ for all values of that type.

When defining an object, the developer has to specify:

  * a function that maps the object’s internal state into its external state
  * the initial internal state
  * for each action, an expression that computes a new state based on the previous state (denoted by pseudo-variable **curr**) and the action’s parameters, if present

The state of an object is denoted with the caret symbol: `<`object`>`**^**; actions are applied using the dot-notation: `<`object`>`**.**`<`action`>[<`parameters`>]`. It’s easy to see that a sequence is just a case of an object, specifically an object with one (parameter-less) action, **skip**. Therefore, yet another way to define a sequence is by defining an object with one action, **skip**.

The type of an object includes the type of its state and the names of its actions, together with the types of their parameters (if any).

# Program logic #

Unlike imperative programming languages, where variables are names of memory locations for values, in Aha! variables are names for values; once assigned to a variable, the value can't be changed. In that sense, 'variables' could be called 'constants' for clarity.

Aha! code consists of statements and expressions; both describe computations and may have output. A statement's output consists of zero or more variables, and an expression uses some variables but its output is a single anonymous value. Any computation can succeed (i.e. achieve its goal) or fail, in which case its output is undefined. Aha! is designed in such a way that if a computation fails, its output will not be used.

Statements can contain expressions, and expressions can contain statements.
## Expressions ##
Expressions can use pre-defined or user-defined operators, both have the same syntax; the difference is that pre-defined ones don't need to be imported.

Operators have no precedence rules and are always enclosed in parenthesis:
```
(a + b) `returns the sum of a and b; fails if result is too big`

(a < b) `returns nil if a < b, otherwise fails`

(~float 0) `returns floating-point 0 (defined in module Base/Float)`
```
Below we list examples of expressions in Aha!, grouped by the relevant data type.

**nil**:
```
nil
```
integers:
```
123
 
-777
```
characters:
```
$a `English letter a`

$1 `digit 1`

$Э `Russian letter Э` 

$$ `$`

$space `space`

$CR `carriage return`
```

arrays:
```
[1 2 3 (5 * 5)] `[integer]`

integer[] `[integer] (no items)`

[$A $h $a $!] `[character]`

"Aha!" `same as above`

"" `same as character[]`

[[1] [1 2] [1 2 3]] `[[integer]]`

a# `returns size of array a (integer)`

a(0) `returns first item of array a; fails if a is empty`

[0 .. n] `returns [0 1 2 ... n], of type [integer]`

[0 .. n) `returns [0 1 2 ... (n - 1)], of type [integer]`
```
composites:
```
( name: "Aha!", rank: 999 ) `( name: [character], rank: integer )`

( state; index: (state.index + 1) ) `composite value state with field index incremented by 1`

rec.name `name field of composite rec`
```
functions:
```
{ i: integer -> (i * i) } `{ integer -> integer }`

{ x: integer, y: integer -> (x < y) } `{ integer, integer -> nil }`

{ x: integer -> { y: integer -> (x * y) } } 
  `{ integer -> { integer -> integer } }; for any integer x, returns a function that multiplies its argument by x`

{ t: ( x: integer, y: integer z: integer ) -> ((t.x * t.y) * t.z) } 
  `{ ( x: integer, y: integer z: integer ) -> integer }; multiplies x, y and z fields of its composite argument`

f(x, y) `apply function f to x and y`
```
sequences:
```
seq 1 then (curr + 3) end `integer* (1, 4, 7, ...)`

seq [1 .. 100) end `integer* (1, 2, 3, ... , 99)`

s^ `first item of sequence s`

s.skip `sequence s without the first item`
```
expressions with preconditions/substitutions (**get** `<expression>` **where** `<statement>` **end**; see "Statements"):
```
get max `returns max as result`
where `define max`
  any 
    max = get x where (x >= y)? end `fails if x < y`
    max = get y where (x <= y)? end `fails if x > y`
  end
end `obtain the maximum of x and y`
```
objects:
```
acct =
  obj { b: integer -> ( balance: b ) } `internal state is integer, external state is composite ( balance: integer )`
    0 `initial balance is 0`
  deposit(sum: integer): 
    get (curr + sum) `add sum to current balance`
    where (sum > 0)? `sum must be positive, otherwise fail`
    end 
  withdraw(sum: integer): 
    get (curr - sum) `subtract sum from current balance`
    where 
      all 
        (sum > 0)? `sum must be positive`
        (sum <= curr)? `but not exceed balance, otherwise fail`
      end  
    end 
  end
` object of type:
obj ( balance: integer )
  deposit(integer)
  withdraw(integer)
end
`

acct^.balance `obtain balance field of the object's state (0)`

acct.deposit(100).withdraw(50) `deposit 100 then withdraw 50, return an object with balance 50`
```
array/sequence operations:
```
array { i: integer -> ((i * 3) + 1) } to 5 end `returns [1 4 7 10 13]`

array seq 1 then (curr + 3) end to 5 end `returns [1 4 7 10 13]`

join "Hello" [$, $space] "World" [$!] end `returns "Hello, World!"`

select x in [0 .. 100) that (x < 10)? end `returns [0 1 2 3 4 5 6 7 8 9]`

count x in [0 .. 100) that ((x // 2) = 0)? end `returns 50`

such ch in "amazing!" that any (ch = $a)? (ch = $A)? end end `returns $a`

first p in Primes to 1000 that (p > 1000)? end `the first among first 1000 items in sequence Primes that exceeds 1000`

foldl x, y in [1 .. 100) to (x + y) end `returns the sum 1 + 2 + ... + 99`

foldr a, b in ["a" "b" "c" "d"] to join a ", " b end end `returns "a, b, c, d"`

sort x, y in [1 .. 100) that (x >= y)? end `returns sequence: 99, 98, ... , 1`

group x, y in seq "ag77ttrdddeff" end that (x = y)? end `returns sequence: "a", "g", "77", "tt", "r", "ddd", "e", "ff"`
```
_explanations_:
  * **array** creates and fills an array of given size (**to**) using either a function that maps **integer** into the desired type or a sequence of items of that type
  * **join** joins arrays of the same type (not only characters)
  * **select** filters an array (**in**) by given criteria (**that**)
  * **such** finds any item in an array (**in**) that satisfies given griteria (**that**); can be non-deterninistic: if several items satisfied the condition, any of them could be returned (e.g. if the string was "Amazing!", then either $A or $a would be a possible result)
  * **first** finds the first item in a sequence (**in**) that satisfies given criteria (**that**); applied to a sequence generator, acts much like a **while**-(or **until**-)loop in imperative languages, but always has an explicit limit on the number of iterations (**to**)
  * **foldl** and **foldr** apply the binary operator represented by an expression (**to**) to all items of an array (**in**) either from left to right (**foldl**) or from right to left (**foldr**); give the same result if the expression is a commutative operator
  * **sort** performs stable sorting according to given criteria (**that**); it accepts an array (**in**) and returns a sequence
  * **group** accepts a sequence (**in**) and returns a sequence of arrays where adjacent items are grouped by given criteria (**that**); is often used after **sort**

recursive functions:
```
            letrec `define fact recursively`
                fact: integer =
                    { i: integer ->
                        get f where unless all (i = 1)? f = 1 end then f = (i * fact((i - 1))) end end
                    }
            to n `limit the number of recursive calls to n`
            end
```

## Statements ##
A statement can have both input and output variables. It enforces some condition about these variables. If it has no output, the condition must be true about its inputs; otherwise, the output variables are defined in accordance with the condition.

Some expressions are evaluated just to find out if some condition is valid about its variables, and their output is of no interest. If the evaluation succeeds, we consider the condition true, otherwise false. The statement that evaluates an expression but ignores its output, in Aha! looks like this: `<`expression`>`**?**. For example:
```
(x < y)? `succeeds when x is less than y; no output`

(x / y)? `succeeds when y is not zero; no output`

rec.date? `succeeds when field date is present in variant composite value rec`
```
Alternatively, the output of an expression can be used in another expression (nested expressions) or given a name: `<`variable`>` = `<`expression`>`; such a statement is called a _definition_. The type of any expression is always inferred automatically, so variables never need to be declared. For example:
```
str = "Hello, World!" `defines variable str of type [character]`
```
Such basic statements can be combined into programs using the _statement logic_. First, if we need the outputs of several independent statements, we can write:
```
all
  x = 2
  y = 3
  z = 5
end `defines three integer variables: x, y and z`
```

The **all** statement fails if one of the statements in it fails. Therefore, we could write:
```
all
  (i >= 0)?
  (i < 100)?
end
```
to check that variable i is not negative _and_ is less than 100.

The **any** statement works the opposite, dual to the **all** statement, way. It fails if each of the statements in it fails and succeeds if one of them succeeds, retaining its output. For example:
```
any
  a = $a
  a = $A
end `defines variable a as either lowercase or uppercase character 'a'`

any
  (a = $a)?
  (a = $A)?
end `verifies that the value of variable a is either lowercase or uppercase character 'a', defines no output`
```
There can be any number of statements enclosed in the **all** or **any** brackets. The output of the **all** statement is the combination (union) of outputs of the statements in it, and the output of the **any** statement is the common part (intersection) of the outputs of the statements in it.

Note that the order of evaluation of statements in **all** and **any** constructs is not known (non-deterministic). The statements in them can't use one another's outputs.

**not** `<`statement`>` succeeds if `<`statement`>` fails and fails if it succeeds, retaining no output:
```
not IsLetter(a)? `succeeds when function IsLetter fails`
```
A variation of **any** is the **unless** statement. It always contains only two statements, and the second one is evaluated only if the first one failed:
```
unless
  x = (a / b)
then
  x = 1
end `defines variable x as the ratio or, if this fails (e.g. b = 0), then 1`
```
Often, complex computations have to be performed in stages. In Aha!, one way to accomplish this is using the **let** statement, similar to the **get** expression:
```
let
  all
    i = nn 
    j = f((nn - 1))
  end
where
  nn = (n + f(n))
where
  f = { x: integer -> x * x }
end `defines two variables, i and j; nn and f are local for the let statement and not visible outside`
```
**where** clauses are evaluated from bottom to top, and any variables they define can be used only by statements above in the **let** statement. **where** clauses don't have to define any variables; they can be used to verify some pre-conditions.

Finally, Aha! provides special statements that verify conditions on arrays of data, **foreach** and **forsome**:
```
foreach x in A, (x > 0)? end `check if all items in array A are positive`

forsome vowel in "aeiouyAEIOUY", (ch = vowel)? end `check if ch is a vowel`
```
Some statements (**all**, **any**, **foreach** and **forsome**) are intrinsically parallel and potentially non-deterministic; however, it is up to the implementer and depends on the underlying hardware whether to take advantage of this. Other statements (**unless**, **let**) enforce certain order of evaluation. Where possible, non-deterministic statements should be used in favour of similar deterministic ones (e.g. **any** instead of **unless**): they usually make the code more readable and elegant and leave more freedom to the compiler for possible optimizations.
# Modules #

Aha! programs consist of modules, grouped into packages. Each module consists of code and the specification (_spec_ for brevity). A module's specification defines data types, variables and operators published by the module and may look like this:
```
doc 
    Title:   "Time"
    Package: "Aha! Base Library"
    Purpose: "Date and time manipulation"
    Author:  "Roman Movchan, Melbourne, Australia"
    Created: "2011-10-11"
end

export Types:
    type Timestamp: opaque "Date and time"
    type Interval: opaque "Time interval"
    .   .   .
end

export Utils:
    the DayOfWeek:
        (
            monday: { Timestamp -> nil } "is the date a Monday?"
            tuesday: { Timestamp -> nil } "is the date a Tuesday?"
            .   .   .
        ) "a set of functions to determine the day of week"
    .   .   .
end

the TimestampDiff: {Timestamp, Timestamp -> Interval} "difference between two timestamps"
the IntervalSum: {Interval, Interval -> Interval} "sum of intervals"
    .   .   .

export Operators:
    (Timestamp - Timestamp): TimestampDiff
    (Interval + Interval): IntervalSum
    .   .   .
end
```

The spec always begins with a header, consisting of the brackets **doc**-**end** and documentation entries between them. What entries are required is determined by the owner of the package containing the module.

The above module spec declares two **opaque** data types, _Timestamp_ and _Interval_, and some operations on them. Being opaque means that the types are fully defined in the code, not in the spec; it is not relevant to the module's user how these types are implemented.

Finally, the spec declares two binary operators: -(minus) for two timestamps and +(plus) for two intervals. This means that when these operators are used anywhere with operands of the appropriate types, the corresponding functions will be called instead.

Some declarations are grouped into **export**-**end** blocks. Such groups can be imported by other modules (see below).

Note the text in double quotes after each declaration. These are not (optional) comments, these are (mandatory) descriptions. Not only each declaration must have a description, but each relevant part in each declaration (e.g. a field in a composite type) must have one. It is crucial for a module to be well documented. Individual development teams can establish their own rules about using descriptions, but at least having one after each declaration, each composite field and each object action is a requirement of the Aha! syntax.

Modules can be generic, i.e. use data types provided by the user of the module. Such data types have to be introduced with the keyword **arbitrary**:

```
arbitrary Key "lookup key", Value "lookup value"
```

The name is assigned to a module when it's being included into a package; this helps avoid name conflicts and even allows for several versions of the same module to be included in a project. Aha! doesn't specify any syntax for packages.

Any existing module can be used by any other module's code or spec. If the module is generic, its type parameters must be substituted with types known to the client module:
```
use Dict: Base/Dictionaries<Key: String, Value: String>
```
This declaration instantiates module _Dictionaries_ from package _Base_ (Base Library), substitutes its type parameters with _String_ and assigns this instance of the module a nickname, _Dict_. Any types or public variables declared in that module can be referred to using the format `<`nickname`>`**$**`<`name`>`, for example:
```
Dict$HashTable
```
In order to be able to refer to the module's types or variables that belong to an export group without using the nickname, the group needs to be imported:
```
import Dict(Types)
```
User-defined operators always need to be imported, otherwise they can't be used. Operators can be overloaded, i.e. the same symbol can be used for operators on different data types.

A module's code always begins with the **implement** line, for example:
```
implement Base/Time
```

This line specifies the module being implemented (its 'owner' package and the module name inside it). Packages can be nested, so the 'path' may contain more than two parts, e.g.:
```
implement API/Extensions/Scheduler
```
Then the code may contain some **use**, **import** and **type** declarations (including the opaque type definitions, now without the word **opaque**). All the non-opaque type declarations from the spec being implemented are implicitly imported into the code.

Finally, if the spec declared any public variables, the code must contain a statement that defines these variables.

Code can contain optional comments - arbitrary text between the symbols `````. Comments have no effect on the semantics of the code.

The code of the above module would have to contain, at least:
  * definitions of the types _Timestamp_ and _Interval_
  * a statement defining (i.e. assigning) the public variables _DayOfWeek_ (a composite), _TimestampDiff_ (a function) and _IntervalSum_ (a function)

The full source of this module spec can be found at: Base/[Time](Time.md)

# API #

Aha! programs don't run by themselves, they only describe how an application behaves when run. To actually run them, a special framework, called Aha! Runtime Framework (_ARF_), needs to be used; it creates the runtime environment, instantiates the application and performs the communication between the user and the application. Unlike a 'virtual machine', ARF doesn't provide primitive commands that applications can call, it works the other way around: it calls the functions defined by the Aha! program that determine how ARF should behave. This approach is known as _inversion of control_.

Aha! comes with a special package, called _API_, that declares data types and variables (functions, objects) needed to write interactive applications that run under ARF.

To define an application, the developer needs to implement the code for a special module, _Application_. The spec of this module is pre-defined and declares only one variable, _Application_, of type _Application_.

Generic API module _AppTemplates_ contains a few templates of applications, such as console, Web and GUI applications. The only type parameter of this module is the type of the state of the application - an arbitrary type that the programmer chooses to represent the application-specific data; we will call this type _State_. The type { _State_ -> _State_ } is called _Action_; values of this type are applied to the current state to produce a new state.

## Console applications ##
The simplest type of an application is a console application. It has simple input - an array of strings that are displayed in the console window as separate lines - and simple output - a string entered by the end user. This type of application is generated using function _ConsoleApp_.

_ConsoleApp_ is a function that returns an object (an object constructor). The function has a single parameter of type _State_ - the initial state of the application.

The result object is used to obtain a value of type _Application_ that can be assigned to the variable _Application_. The object has the following actions:
  * _input_ - specifies a function of type { _State_ -> **`[[character]]`** } that generates the input for the console (i.e. displayed lines of text) from the current state
  * _output_ - specifies a function of type { **`[character]`** -> _Action_ } that produces an action to be performed from the user input
  * _extensions_ - specifies a collection (array) of extensions linked to the application (see "Extensions")
  * _quit_ - specifies a function of type { _State_ -> **nil** } that indicates when to terminate the application; if it succeeds, the application quits

The main benefits of this approach to building interactive applications are:
  * there is only one function that generates the text displayed depending on the current state, so it's easy to ensure consistency in what's displayed
  * there is only one function that processes the user input, so it's easy to ensure predictable and, in the end, correct behaviour
  * since the code for processing the input is separated from the code that generates the response, it's easier to localize any bugs

An example of a console application: [HelloWorld](HelloWorld.md).
## GUI and Web applications ##
GUI and Web applications are created using functions _GUIApp_ and _WebApp_. They are somewhat similar to _ConsoleApp_ and, too, produce objects with the state that can be assigned to the variable _Application_. Building GUI and Web applications with Aha! API is described in detail in a separate document.

## Extensions ##
Apart from communicating with the end user, applications often need to access and modify the environment they run in, e.g. read/write some files, access the database, get some system information like the current date and time, etc. ARF makes this possible through the mechanism of extensions.

Each extension is an independent generic object that processes the current application state at various events. For example, the _Persistence_ module defines an extension that is used when the application starts and terminates; it has 'slots' for custom functions (_callbacks_) that convert the application state to/from some persistent format.

There are numerous benefits in such architecture. Extensions can be added or removed at any stage without affecting other parts of the application. Different aspects of the application (e.g. file or database access) are localized in certain places and are not spread over the application's code, which makes it easier to test, debug and maintain the application.

When implemented, each extension can be run asynchronously in a separate OS thread. Synchronization is needed only when the extension produces an _Action_. For example, the _FileReader_ extension utilizes a user-supplied callback function by calling it whenever a piece of data is retrieved from the associated external file; this function returns an _Action_. Since the application has only one (shared) state, all the actions generated by the application or its extensions are put in a queue and applied synchronously.

An important application of extensions is integration with third-party libraries written in other languages. Since such libraries can have side effects, it would not be possible to call them directly from Aha! programs without jeopardizing their integrity. Extensions provide a safe communication channel between a declarative program and the mutable environment.

An example of a GUI application with an extension: [Clock](Clock.md).
## GUI Embedding ##
GUI applications often are too complex to be represented with a single state structure. They may consist of several windows with individual purpose, or they may contain groups of visual elements that behave like separate applications.

The API provides a solution for this problem: GUI embedding. With this technique, parts of a large GUI application can be developed as separate applications and then 'glued' together. The difference between a standalone application and an embeddable application is that an embeddable one has some inputs and outputs. When such an embeddable application is embedded into a GUI or another embeddable application, its inputs and outputs are linked to the state of the 'owner' application.

Let's assume, we have developed a standalone GUI application that simulates a calculator. Then we decide to use it as a component in other GUI application(s). First, we need to decide what data types will be used to represent the calculator's input and output; say, we choose the _Float_ data type for both (the initial value displayed and the final result). We create the spec for our _Calculator_ module that declares _Calculator_ as an instance of the _Embeddable_ data type with the _Float_ type for its two type parameters. Then, we create the code of our module _Calculator_ where we modify the definition of our application so it uses constructor _Embeddable_ instead of _GUIApp_. The _Embeddable_ object has additional 'slots' for input and output functions that convert the state of the embeddable object to/from its inputs/outputs; it's used to create a GUI control that can be placed inside any GUI application. When the module _Calculator_ is used by an application, this application has to supply the input/output functions that convert the application's state to/from the embeddable object's inputs/outputs.

Very large applications can be constructed this way, without the danger of becoming too perplexed. Very little modifications are needed to make a standalone application embeddable, and very little modifications are needed for the client application to actually embed it.

# Conclusion #
Aha! was designed to be compact and elegant, easy to learn and pleasant to use, while providing everything needed for real-life application programming. It doesn't have any unsafe features and its clear, unambiguous syntax should help the developers write robust, readable and maintainable programs.