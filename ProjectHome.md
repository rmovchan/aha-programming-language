# Introduction #

Aha! is a new general-purpose programming language aimed to raise software quality to a higher level.

It is:
  * declarative (programs consist of statements that set goals, but do not directly specify steps needed to achieve them)
  * pure (computations have no side effects)
  * strict (computations are 'eager' by default)
  * total (every computation terminates)
  * parallel (program logic easily translates to multiple concurrent processes)
  * statically typed
  * modular

It features powerful data structures (arrays, sequences, first-class functions, objects and composites), success/failure logic, sequence generators, high-level array/sequence operations, generic modules with separate code and specification, abstract data types, type inference and much more, all within elegant and highly readable syntax. Aha! programs are intrinsically parallel.

Aha! differs from other programming languages in many ways, for example:
  * success/failure logic is used instead of the Boolean data type and the exception mechanism
  * infinite sequences can be easily defined, but non-terminating computations are not possible
  * complex values, such as arrays or composites (a.k.a. structures), can only be defined as a whole, i.e. can't be partially undefined
  * to create an object, no class definition is needed
  * operators are just an alternative form of variables of a functional type
  * modules, but not individual data types, can be generic

Although in Aha! side effects during computations are not possible, programs can generate jobs that affect the environment and receive the system's feedback through custom events (using the API).

Aha! is not a scripting language and is not intended for ad hoc programming. It is designed for the professional application development.

Aha! is a language for those who:
  * are looking for ways to reduce complexity in software development
  * believe that the language shouldn't facilitate making bugs but should facilitate their prevention
  * are open to new ideas

If you are one of these, you will enjoy browsing the Wiki pages and will surely go "Aha!" a few times.

Please use the [Discussion Group](https://groups.google.com/forum/?fromgroups#!forum/aha-programming-language-discuss) to submit your feedback.
# Current Status #

The Wiki pages contain some [examples](Examples.md) of code, the specifications of the Aha! [Base Library](BaseLibrary.md) and the [API](API.md) and a [Getting Started guide](GettingStarted.md) that explains the ideas behind Aha!; the [Language Reference](https://drive.google.com/file/d/0B7s-cEATWx1ncUdVa3dsbU1PNzg/edit?usp=sharing) contains a more detailed description of the language.

The work on a 'proof-of-concept' implementation (Aha! for .NET) is currently in progress. It will include a compiler, base libraries and a simple framework (console). Watch this site for future updates!

# News #

## 2014-10-25 ##

Another syntax cleanup: statements and expressions having a fixed number of parts (i.e. **unless**, **first**, **array**, **list**, **enum**, **such**, **select**, **count**, **foldl**, **foldr**, **sort**) no longer have terminating **end**. Additionally, **where**/**when** clauses now follow statements rather than preceed **end**.

## 2014-09-28 ##

In the API, the structure of applications and components has been changed: instead of using the `Receive` function, they receive input directly to the behavior's new `handleInput` action. `handle` has been renamed to `handleEvent`.

## 2014-09-24 ##

The API has been simplified in terms of defining applications and components: now there's only one way to implement an application ([ConsoleApp](ConsoleApp.md)) and only one way to use ([Component](Component.md)) or define ([ComponentDef](ComponentDef.md)) a component. Also, the job engine now can enable or disable input events.

## 2014-09-22 ##

The Aha! Console framework has been fully implemented.

## 2014-09-18 ##

The syntax of modules has been changed. The **import** declarations and **export** groups have been abolished, and only one **export** per module declares/defines all the data exported by the module. Accordingly, all types defined in external modules must be prefixed with the module's nickname (e.g. `Time::Interval`), and likewise references to the module's exported data (e.g. `@Time.Second`).

New **with** expression has been introduced for auto-resolving references to a composite's attributes.

The Wiki pages and Language Reference have been updated to reflect the changes.
