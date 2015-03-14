**_Q. What is the main motivation behind Aha! ? How is it different from other languages? What are its main benefits?_**

A. Our main focus was on:
  * reducing software complexity
  * improving readability
  * achieving high reliability
  * supporting the team work

While keeping the language simple, we also aimed to ensure that it has enough power and flexibility to solve any practical task and that at least decent performance, comparable with the mainstream compiled languages, can be achieved.

We see Aha!'s biggest benefits in its:
  * declarative programming paradigm
  * compact and clean syntax
  * high level of abstraction
  * advanced type system with static typing and type inference
  * well-designed Base Library and API

**_Q. Why do relational operators return 'void' ? Where is the Boolean data type?_**

A. Any computation in Aha! (an expression or statement) either succeeds and returns any values it's supposed to return, or fails (and the return values are undefined). This implicit success/failure result, combined with the statement logic, behaves exactly like the Boolean data type; complex statements built using the **all**, **any** and **not** constructs become equivalents of Boolean expressions (except that along with the 'true' value (success) they additionally return their output variables). **void** return value simply means that no meaningful result is returned apart from the success/failure. Note that **void** in Aha! is not just a value, it's, in fact, a separate data type that is completely neutral: there are no operations defined for it, not even the equality comparison.

**_Q. What will happen if a program falls in error?_**

A. There are commonly two types of exceptions: internal (caused by the computations themselves) and external (caused by some conditions outside the control of the program, such as I/O errors). Internal exceptions are replaced in Aha! with a more general concept of _failure_, and external ones are handled through the API (using callback routines).

One of the main goals of Aha! is to help building robust applications. The language doesn't introduce a concept of 'exception' or 'error'; instead, it pronounces that any program or its part can either succeed (i.e. reach its goal) or fail. There is no exception handling mechanism that can be optionally used; the entire language is built on the premises that any computation can either succeed or fail. This contrasts to the traditional approach, where a program is first written based on the assumption that nothing will ever fail, and exception handling is added later on, if ever.

**_Q. Isn't it too annoying to have to specify the "to" clause in every "loop" or a recursive definition?_**

A. First, not _every_ loop - Aha! has several loop-like constructs, such as **foldl**, where the **to** clause is not used.

Second, we believe that in the situations where the computations are potentially infinite, the programmer needs to think about the ways to ensure termination, and estimating the maximum number of iterations is a simple method of doing this. In most cases, a simple estimate exists - please have a look at our code [examples](Examples.md).

Moreover, having to think of such estimates often makes the programmer find a better algorithm or avoid a bug. Yes, this means a little more work for the programmer, but results in a better program with fewer problems.

**_Q. Why aren't inheritance and other object-oriented features supported?_**

A. It is well known that using class inheritance automatically introduces dependencies in the program and therefore may lead to maintenance problems later on. We didn't want to include any features that may reduce the maintainability or security in future programs.

We believe that the very simple, almost minimalistic concept of object in Aha! is what is really needed for development. Compared to the object-oriented languages, in Aha!:
  * there are no 'classes'; an 'obj' expression describes the object's 'current' value and the class of all possible values  that can be obtained by applying the actions
  * an object's type is its 'interface' and therefore objects with completely different implementation may belong to the same type
  * 'constructors' are simply functions that return objects; there is no need for destructors as memory management is automatic, and releasing other types of resources can/should be done using the API (on certain events)
  * the internal state of an object is not accessible in any way other than by applying the actions; in other words, all data members are 'private' and all 'methods' are 'public'
  * inheritance by implementation can be modelled by embedding the 'base' object into the 'derived' object; this also leaves the developer a lot more flexibility, while keeping the 'base' object's data members secure

**_Q. Since the Language Reference doesn't specify the precise range for the integer data type (only recommends it), does it mean that the same program may produce different results on different implementations of Aha! ?_**

A. Yes. The **integer** data type is an abstraction meant to suitably model the set of all integers. It's beyond the language scope to enforce some particular representation of the **integer** data type: no part of the language relies on any assumptions about the range of integers to be used. We assume that particular implementations of Aha! will allow the users to choose the most suitable representation for integers (e.g. 32-bit, 64-bit or even 'arbitrary precision').

_**Q. Why is the current time returned by sending a job with the API? Could it be done more easily, e.g. with a function in the Base Library?**_

A. No, it couldn't. This would mean that the function returns different values every time it's called and, more importantly, it has a side effect because every returned value implicitly depends on previous values. This is not possible in a declarative language.

**_Q. When is Aha! going to be implemented? What platforms will it support?_**

A. Our current plan is to release the first version of Aha! for .NET by the end of this year.

The core modules, runtime framework, compiler and Base Library are being written in C#. The compiler will convert Aha! source code into IL. Therefore, all the platforms supporting .NET applications will be covered.