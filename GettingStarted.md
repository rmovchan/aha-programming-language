Aha! is a new general-purpose programming language designed to solve many of the problems that software developers face every day. It is meant to make the software more predictable and reliable, the source code - more readable and easier to maintain, and the development process - more productive and enjoyable.

The main problem that exists nowadays in software development and that Aha! tries to address is complexity. It does that mainly by offering a structured, goal-directed approach, where every piece of code has a clear end goal. Instead of trying to make the code as short as possible, like many other programming languages do, it makes the developer be explicit in every decision, which results in much clearer code that can be understood and maintained by other people.

Aha! is not like any other programming language. It's innovative in almost every area and affects the way people need to think about programs.

This document is meant for introduction only. For a more detailed description of the language, see the [Language Reference](https://drive.google.com/file/d/0B7s-cEATWx1nX1k5LXVYVlZyQWc/edit?usp=sharing).

# Semantics #

Aha! is a declarative language. This means that programs in Aha! describe the desired results and not the sequences of commands that might produce the desired results (or might not). When run, programs in Aha! only compute the results and can't have any side effects during the computations. One could say that Aha! is a purely functional language, however the term "functional programming" is a little vague so we prefer not to use it.

The declarative programming paradigm is crucial for developing reliable, stable programs that match their specifications and have no serious bugs.

Since it's a declarative language, its concept of a variable is different to the imperative programming languages. While in such languages a variable is the name of some memory location used to store values, in Aha!
**variables are names for values, not for memory cells**,
i.e., every variable is attached to a (immutable) value. We could say that a variable is in fact a constant, but this would be misleading in some situations. The value attached to a variable can actually be different at different times; for example, a function parameter will refer to a value specified when the function is called. What's important is that once a variable is defined, the code that uses it never attempts to modify it, i.e. treats it as a constant.

Since a variable is not considered a memory location, the programmer is free from the burden of managing the memory; once a value is not referenced by any variable, the memory it takes is automatically released.

The most important thing to do in order to learn successfully a declarative language is to **stop thinking of programs in terms of their control flow**. Declarative programs, unlike imperative ones, describe computations in terms of the results they have to produce and not the actual sequences of actions needed to produce the results. This provides ultimate flexibility to the language implementer in terms of possible optimizations and makes them completely transparent to the programmer. Moreover, liberating the program semantics from non-essential behavioral details helps the developer concentrate on the actual task at hand.

**Programming in Aha! is not about executing commands, it's about setting goals.** Aha! will help the developer express large, complex tasks via a number of elementary, achievable goals.

# Syntax #

We strove to make the syntax of Aha! as readable and unambiguous as possible while keeping it reasonably brief and expressive.

The Aha! syntax is free-format, i.e. spaces and line breaks can be freely inserted between tokens without affecting the program semantics. In the examples, we follow certain guidelines when formatting the code in order to make it more readable.

Names and keywords in Aha! are case-sensitive.

# Computation logic #

Aha! is based on some simple principles and follows them faithfully and consistently.

The first and most important principle is that **every program or its part formally expresses the goal that it is supposed to achieve**. This is not true when we talk about programs in the traditional languages: most programs have no explicit, adequately expressed, goal and simply describe some computations that are not even guaranteed to give _any_ result. The goal is usually buried in the developer's head and, in the very best case, is informally described in the program documentation, such as comments.

Second, we want our programs to be so good that people can depend on them. Ideally, we would like to be able to write a program that always achieves its goal. Unfortunately, it is generally impossible to guarantee that. It is possible, however, to guarantee that the program **either achieves its goal or signals that it failed**. Under no circumstances it can yield wrong results without letting the user/calling program know it.

Third, **if a computation has failed, its results can't be used**. It's not enough to indicate that the computation failed, it must be guaranteed that no other computation uses its, probably invalid, results (for example, by discarding them).

These are important and natural requirements that the programming practice has been ignoring for many years. Programs or their parts that fail to achieve their goal often hide such a fact (or, even if they indicate it, the client program is free to ignore it) and their results continue to be used when they shouldn't be.

In Aha!, programs' code is built from _computations_ of two types: _statements_ and _expressions_.

A statement only defines zero or more variables and/or verifies some conditions and has no side effects whatsoever. In relation to statements, our first two fundamental principles can be formulated as:

## every statement expresses some condition that it must ensure or, if this is impossible, explicitly fail ##

There are only two basic types of statements in Aha!: _definition_ and _assertion_.

A definition has the form `<variable> = <expression>!` and associates the variable with the value of the expression. For example, we may write a definition
```
x = y / z!
```
and expect that the equality we stated holds after the statement is evaluated, i.e. variable x receives the value that ensures the condition. If however z = 0 then the equality cannot be ensured by any value of x. In such a case, the statement _fails_, the run-time system receives an appropriate signal and the value of x cannot be used.

Unlike most programming languages, where the failure of any part of a program is considered a bad thing, an abnormal behavior leading to 'panic', special recovery (if specified) and, often, to inconsistent results, in Aha! a **failure is a normal outcome of any computation** and the basis of its computation logic.

If we were interested not in the actual result of an expression (e.g. `x / y`) but only in the fact that it succeeded or failed, we could write a statement by adding a question mark to it: `x / y?`. Such a statement defines no variables and serves solely to verify some condition; we call such statements _assertions_. A common example of an assertion is checking for a relation between two numbers, such as `x < y?`; such an assertion will succeed when the relation holds (i.e. x is less than y in the example) and will fail when it doesn't. `x < y` itself (without the question mark) is an example of a relational operator (others include "=", "/=", ">", ">=", etc.) and is considered a function whose return value is irrelevant (since only the success/failure status matters). It's easy to define such functions, as well as operators, in Aha!.

Expressions are different from statements in that they define no variables; an expression simply returns an anonymous value that can be used to define a variable or as an argument in another expression. Expressions are just another kind of computations, only with different syntax, and are subject to the analogous principles. In particular,

## every expression must return a valid and appropriate value or fail ##

This means what it says: that an expression has to either return the expected value or fail. The value of a failed expression cannot be used; e.g if a statement defines a variable using such an expression, it will fail too.

The semantic requirement that every computation must either succeed or fail, obviously, implies that every computation must terminate. Within our system, therefore,

## non-terminating computations are semantic nonsense ##

To satisfy this requirement, Aha! only has iterative constructs that are guaranteed to terminate. Recursion is also very limited (see chapter "Recursion"). It is not possible however to ensure that the time that a computation takes in reality is acceptable to the user; the user can send an interruption signal when he/she wants. Such a signal will cause the current computation(s) (not necessarily the entire program) to fail.

This doesn't mean that _applications_ written in Aha! are bound to terminate. An application itself is not a computation, it's rather the result of one (as well as the source of further computations); see chapter "Applications" for understanding of how applications are written in Aha!.

Instead of the imperative control-flow constructs, Aha! uses

## statement composition rules based on Boolean logic ##

Statements in Aha! are similar to Boolean statements (ones that can be either true or false), like "2 x 2 = 4" or "I like apples"; the difference is, they describe desirable, and not factual, conditions, where some variables may not be known yet. For example, the statement `x = y / z!` defines a computation that finds some value "x" that satisfies the equation; by the syntax convention, the variable to the left from the "=" is viewed as the one to be defined, and all the other variables ("y" and "z" in the example) must be known in the context of the statement.

Similarly to Boolean statements, Aha! statements can be combined together using the analogues of the Boolean operators; in the Aha! programming language, they have special - different to the mathematical logic - syntax that makes such statements more readable. For example:
```
all
    x = a + b!
    y = a - b!
end
```
succeeds when all of the statements between **all** and **end** succeed (i.e. when both "x" and "y" get the appropriate values). Similarly,
```
any
    x = a + b!
    x = a - b!
end
```
means that we want only one of the conditions between **any** and **end** (i.e. either `x = a + b` or `x = a - b`) to be true. By the analogy with the Boolean logic, we call the **all** statement _conjunction_ and the **any** statement - _disjunction_.

Note however that we can't combine just arbitrary statements into a conjunction or disjunction; they must conform certain rules. For example, we could not write
```
all
    x = a + b!
    x = a - b!
end
```
because both conditions can't be satisfied at the same time. Neither we could write
```
any
    x = a + b!
    y = a - b!
end
```
because that would mean that either x or y remains undefined. Aha! compiler will produce error messages and will not compile statements like these.

In addition, the compiler will reject a statement like
```
all
    x = a + b!
    y = x - b!
end
```
because variable x is not defined yet in the context of the statement.

Since conjunction requires that the goals of all the statements must be satisfied, it will fail if any of the statements fails. Disjunction will fail only when all the statements in it fail.

Other logical constructs in Aha! include negation (**not**) and alternative (**unless**). The former is a statement that fails when a given statement succeeds and vice versa. The latter can be expressed using the negation, conjunction and disjunction. For example:
```
unless
    x = y / z!
then
    x = 0!
```
This statement defines variable `x` as `y / z` or, if `z = 0` (and, therefore, the first definition fails), zero. This is semantically equivalent to
```
any
    x = y / z!
    all
        not x = y / z!
        x = 0!
    end
end
```

The statement composition rules are designed to ensure that complex statements satisfy the principles we mentioned above. In addition, they guarantee that

  1. by looking at any statement, one can always figure out what variables the statement defines (its _output_)
  1. when a statement succeeds, all its output variables are defined
  1. if a statement has failed, any statements using its outputs will not be evaluated; no special checking for undefined variables is needed

This is how Aha! deals with conditions, without even introducing the Boolean data type: every statement/expression "returns" an implicit Boolean-like value (success/failure).

# Failure vs Exceptions #
Failure in Aha! is the natural way of informing the rest of the program that a computation didn't reach its goal. Many modern programming languages for the similar purposes introduce the concept of exception.

There are two distinct types of exceptions:
  1. internal - those caused by the program logic (index out of bounds, invalid arguments, etc.)
  1. external - those caused by external conditions (I/O errors, invalid user input etc.)

We argue that these types of conditions should be treated completely differently. Internal exceptions should be fully handled by the program logic and should _never_ be presented to the end user, whereas external exceptions, depending on the particular cause, can be partially handled and used for the end user notification. With the Aha! Runtime Framework, external exceptions are handled using the API (typically, by specifying a 'callback' routine).

We need to emphasise that **failure doesn't necessarily mean an error**. It just means that the computation should take a different path because some intermediate goal hasn't been reached.

Aha! doesn't need the mechanism of internal exceptions as its computation logic is a superior replacement for one: a failure never breaks the control flow, never leaves variables unassigned, never causes 'panic'.

# Parallelism and non-determinism #

From the description above it is clear that the statements in the **all** and **any** constructs can be evaluated independently. This creates great opportunities to improve the performance of Aha! programs on multi-processor (or multi-core) systems. Since all the data are immutable, they can be shared between the processors that evaluate each statement without requiring any synchronization.

In addition, Aha! offers a number of built-in array operations (see the Language Reference) that could get a performance boost on such systems. For example, the **such** construct could simultaneously check a condition on multiple elements of the array (instead of simply scanning it from start to end).

Another thing to notice is that the **all** and **any** constructs are potentially non-deterministic. On a single-processor system, it is not relevant in which order the statements are evaluated; programmers should not assume any particular order. On a multi-processor system, the statements can be evaluated in parallel, but semantically it's the same as running them one by one in an arbitrary order. What's important is that, regardless of the order and parallelization, the end result satisfies the specification, i.e. the statement either achieves its goal or fails.

The same applies to the already mentioned array operations. The **such** construct returns an arbitrary value in the array that satisfies the given statement; therefore, if several such items exist, the result is _any_ of them. To find, for instance, the first of such items (i.e. the one with the lowest index), we could convert the array to a sequence and apply the **first** construct (see below).

# Conditional computations #

Not all computations can be performed in parallel. Often a computation requires data from other computations or relies on some conditions verified by other computations and therefore can't be performed until these computations are complete.

When a computation expects some prerequisites, such as variables or preconditions, this is expressed in Aha! using either a conditional expression or a conditional statement. A conditional expression has the following syntax: _expression_ **where** _statement_ or _expression_ **when** _statement_. A conditional statement has the syntax: **let** _statement_ **where** _statement_ or **let** _statement_ **when** _statement_. Each **where**/**when** clause contains a statement that defines the variables (**where**) or verifies the conditions (**when**) required by an expression before the clause(s). If one of the clauses fails, the whole computation fails too.

Conditional computations are especially useful because they let the developer first formulate the end goal of a computation and then define various details, such as any auxiliary values (variables) and the conditions when the computation can take place. This contrasts to the imperative style of programming, where operations are normally coded in the order of their execution instead of "from general to particulars". Aha! programs are goal-directed, rather than operation-directed.

Conditional computations also serve as means of restricting the scope of variables, similar to blocks in the imperative languages. The conditional computation can reference variables not known yet in its context; they all must be defined by the **where** clauses. These variables are local to the conditional computation and therefore aren't considered its outputs.

Let's suppose `x` and `y` are integer variables. Then the following code is an expression that returns their maximum:
```
max where
  any 
    let max = x! when x >= y?
    let max = y! when x <= y?
  end
```
The outer **where** clause defines variable `max` that is returned as the result of the whole expression. The inner **when** clauses verify the assertions about `x` and `y`; one of them will fail, except when `x = y` (in which case, the choice is non-deterministic, but the result is the same regardless). Variable `max`, although its value is returned, is local to the expression and is not visible from outside of it.

Semantically, this expression is equivalent to:
```
max where 
  any 
    all x >= y? max = x! end 
    all x <= y? max = y! end 
  end
```
Any complex expression can be considered a shortened form of a conditional expression. For example, `a(j) < a(i)` could be written as `x < y where all x = a(j)! y = a(i)! end`, where `x` and `y` are just arbitrary names for the temporary variables not visible outside the conditional expression.
# Data and data types #

Aha! is a statically typed language with type inference. This means that every expression in it belongs to a specific data type that can be inferred by looking at the expression and the types of any variables in it. Variables, other than a function's parameters and a module's public variables, don't need to be declared; a variable automatically obtains the data type of the expression associated with it when defining.

Compared to other programming languages, in Aha! the type inference rules are extremely straightforward. It doesn't attempt to infer, for example, the types of a function's parameters; those must be specified explicitly. The goal of the type inference in Aha! is to avoid redundancy without making the code less readable.

The only primitive data types are **integer** and **character**.

**integer** is the data type that simulates the (infinite) set of all integer numbers. Aha! pre-defines all the standard arithmetic and relational operators for the integer datatype. It is up to the implementer what representation to choose for integers - e.g., 32-bit, 64-bit or variable-length. In reality, no implementation can guarantee the correct mapping between the finite **integer** data type and the infinite set of integer numbers; therefore, when the result of an integer operation can't be represented as **integer**, the operation will fail. For the practical reasons, it is advisable that the implementers choose for the representation of integers the maximum number of bits that can be efficiently processed by the computer's CPU (64 bits on the modern hardware); however, it is possible to have different representations of integers in different projects.

**character** covers all the characters that can be represented by the computer. There is no association between characters and integers built in the language; these are considered completely unrelated types. Characters can only be compared for equality.

## Functions ##
A function is a value that is _applied_ to another value (or two), called _argument_(s), in order to produce the _return value_. The syntax of a function call (application) in Aha! is usual both for mathematics and most programming languages: `f(x)` or `f(x, y)` (only one or two arguments are permitted).

Functions in Aha! are first-class citizens, which means a function is treated like any other value: it can be assigned to a variable, passed as a parameter to another function, returned as the result, and so on.

A function type value (_closure_) is defined using the syntax: `{ param: type -> expression }` or `{ param1: type1, param2: type2 -> expression }`. The expression is the formula used to compute the return value based on any variables available in the context, including the parameter(s); the parameters are substituted with the values of arguments supplied when the function is applied. Using a conditional expression, we can build a function body of arbitrary complexity.

The type of a closure is fully defined by the type(s) of its parameter(s) and the type of the expression in the body. E.g. `{ x: integer -> x * x }` is of type `{ integer -> integer }`. When the result type is **void**, it can be omitted, together with the arrow: `{ integer }`.

## Arrays and composites ##

Aha! also provides such traditional ways to structure data as arrays and composites (a.k.a. structures, tuples or records). Expressions of these types, such as literal arrays, are easy to write. Aha! has also special syntax for literal arrays of characters: simply putting character strings inside double quotes.

A composite value consists of several named attributes of arbitrary types; an array consists of an arbitrary number of items of the same type, referred to by indexes (offsets). The length of array `a` is referred to as `a#`.

An array can be created by enumerating the items, or using a sequence or a function with an integer parameter; in any case, it can't have any 'gaps', i.e. undefined items, within. There are several built-in array operations that make working with arrays easy and efficient; see [Language Reference](https://docs.google.com/file/d/0B7s-cEATWx1nd2lwbi1fM0FNbHM/edit?usp=sharing) for details. The type of an array of items of type `T` is referred to as `[T]`.

A composite value consists of several named values, called attributes, possibly of different types. Composite values can be introduced using a composite expression, listing the attributes together with their values, terminated with semicolons, in square brackets, e.g.: `[ name: "Aha!"; number: 9999; ]`. If the value of an attribute is omitted (together with the semicolon), **void** is assumed.

**void** stands for a composite with no attributes.

A composite data type describes a class of composite values whose set of attributes is a subset of the set of attributes of the type with the corresponding data types. For example, the values `[ name: "Aha!"; rank: [unranked:]; ]` and `[ rank: [value: 1;]; name: "C"; ]` both belong to the type `[ name: [character] rank: [value: integer | unranked:] ]`. The order of the attributes in a composite value is irrelevant.

A value of a composite type can be defined using a disjunctive statement (**any** or **unless**) where different branches define composite values with different sets of attributes. For example:
```
any
  rec = [ values: [1, 2, 3]; ]!  
  rec = [ value: 45; ]!
end
```
defines variable `rec` of type `[ value: integer | values: [integer] ]`.  See chapter “Statements” for explanation of statements.

To obtain the value of an attribute of a composite type value, the dot-notation is used: _composite.attribute_. The attribute must be present in one of the variants of the composite value. If the attribute is not present in the actual value, then the operation will fail at runtime. Given the definition above, `rec.values` may either yield array `[1, 2, 3]` or fail, depending on which branch of the any statement was chosen by the evaluator.

All the attribute names in a composite value or a composite type must be different.

Composite values and composite types can be used for a variety of purposes:

  * passing multiple arguments to a function (using named notation)
  * returning multiple values from a function
  * creating enumerations (such as `[ yes: | no: ]`)
  * creating 'nullable' values (options)
  * having simple values with different 'flavors' (e.g. `[ kg: 10; ]` for ten kilograms)
  * creating functions with several variants of parameter lists
and so on.

Module [StrUtils](StrUtils.md) contains many examples of composite data types.

## Sequences and objects ##

Sequences represent values that are computed by iteration. In other words, a sequence consists of some "initial" value and a rule how to compute the next value from the previous one. A sequence therefore can be infinite, even though we can access only one item of it at a time. An example of a sequence is the sequence of all odd natural numbers:
```
odd = seq 1, (prev + 2)!
```
This statement defines variable 'odd' as a sequence that begins with 1 and each following item is the previous item (always referred to by pseudo-variable **prev**) plus 2.

To access the first item of a sequence s, we use the notation `s^`; to access the following items, we need to first get the sequence without its first item, written as `s.skip`. Therefore, to get the sequence's second item, we can write: `s.skip^`, third - `s.skip.skip^`, and so on. When the `.skip` expression fails, the sequence ends.

The type of a sequence of items of type `T` is referred to as `T*`.

The **first** construct finds the first item in a sequence that satisfies a given condition; this construct closely resembles the while/until loop in the imperative programming languages. One important difference is that the **first** construct always contains a special **to** clause that specifies the maximum number of sequence items that will be extracted, in order to satisfy the requirement that every computation must terminate. If an item that satisfies the given condition hasn't been found within the specified number of sequence items, the **first** construct fails.

A good example of using sequences and the **first** construct is [GreatestCommonDivisor](GreatestCommonDivisor.md).

An object is a generalization of a sequence, with some additional features. An object is an abstraction characterized by a _state_ (a value of an arbitrary type) and a set of _actions_ that produce a new object of the same type (i.e. with the state of the same type and the same actions). When we say "abstraction", we mean that every object may have some additional elements that we don't care about when we work with the object; these elements are relevant only to the person who defined the object.

To access the state of object x, we use the same notation as for a sequence's first item: x^. Dot notation is used to apply an action. Since applying an action returns an object of the same type, several actions can be applied in a row, for example: `p.left(10).up(30).right(40).down(20)`. This example also demonstrates that actions can have parameters. For better readability, Aha! allows only up to one action parameter; multiple parameters can be passed using a composite value.

In most object-oriented languages, defining an object is a non-trivial task: you first need to define a class, which contains all the data members, constructors, destructors and other methods, and then create an instance using a constructor of this class. In Aha!, the process is much simpler: an object is created using an **obj** expression. The **obj** construct defines a new object and a whole class of values of the same type and consists of three main parts:
  * a function mapping some internal state into the external (visible) object state
  * the 'initial' internal state
  * one or more of action definitions, each having a name, an optional parameter and an expression (where the current state is referred to by pseudo-variable **prev**) returning a new internal state

A classic example of an object is a simple bank account:
```
acct =
  obj { b: integer -> [ balance: b; ] } `internal state is integer, external state is composite [ balance: integer ]`
    0; `initial balance is 0`
  deposit(sum: integer): 
    (prev + sum) `add sum to current balance`
    when sum > 0?; `sum must be positive, otherwise fail`
  withdraw(sum: integer): 
    (prev - sum) `subtract sum from current balance`
    when 
      all 
        sum > 0? `sum must be positive`
        sum <= prev? `but not exceed balance, otherwise fail`
      end;
  end!
```
Here `acct` is defined as an object of type:
```
obj [ balance: integer ]
  deposit(integer)
  withdraw(integer)
end
```
This object has two actions, `deposit` and `withdraw`; its state is a composite with one field, _balance_. The actions guarantee that the balance never goes below 0. We could easily define another object of the same type (i.e. with the same type of state and the same action names and parameter types) that can have a negative balance. The type of internal state and the actual implementation of the actions are not part of the object's type; the type of an object acts as its _interface_.

Since an action always returns a new object, objects are immutable, just like any other values (although the actual implementation of an object can be optimized to avoid data copying where possible).

Note that since a sequence is a particular case of an object - one with the only action, `skip` - `T*` is simply syntax sugar for `obj T skip end` and using the **obj** construct is another, more flexible (but somewhat more tedious), way to define a sequence. See [RWHamming](RWHamming.md) for a simple example of how to do it. [Tokenizer](Tokenizer.md) is a more advanced example.

## Type compatibility ##
When we talked about functions, we mentioned that every function parameter has a type, and the actual argument when applying the function must match that type. Matching types is trivial with simple types, such as **integer**: the argument (expression) must be of the exact same type. However, having parameters of complex types, such as composites and objects, makes it somewhat more difficult.

The general rule is: the type of the parameter must cover the data type of the actual argument. For instance, if the parameter is of a composite type, the argument's type must also be a composite whose attributes with the names present in the parameter's type must have appropriate types. The other attributes are simply ignored. Similarly, when matching object type parameters, the types of the state must match, and the set of actions of the argument (their names and parameter types) must cover the set of actions of the parameter.

When matching user-defined types, their names are substituted with the actual definitions. The only exception are opaque types (see chapter "Modularity"): they are always considered different types if they have different names or are defined in different modules.
# Recursion #
Recursion is a programming technique that lets developers divide a problem into sub-problems of the same type as the original, solve those problems, and combine the results. Recursive functions produce their results from intermediate results computed by applying the same function to modified values of arguments.

The normal closure syntax doesn't support recursion because any functions used inside the body must be defined in the context of the closure. To define a recursive function, a special **defrec** statement is used, with a **to** clause that limits the total number of times the function will be recursively called; this is needed to ensure that any call to the recursive function will eventually return a value or fail. The type of the function's result is pre-declared in the **defrec** statement so that it's known when the function is referenced from inside the definition. Several recursive functions can be defined in the same **defrec** statement, so that they can call one another (i.e. be mutually recursive).

A classic example of using recursion is the factorial function:
```
Factorial =
    { n: integer ->
        (   
            fact(n) `use an auxiliary function fact`
        where
            defrec `define fact recursively`
                fact: integer =
                    { i: integer ->
                        f where unless f = (1 when i = 1?)! then f = i * fact(i - 1)! 
                    }
            to n `fact is called maximum n times`
        )
        when n > 0? `argument must be positive`
    }!
```
See also example [Factorial](Factorial.md) for a non-recursive implementation of the factorial function.
# Modularity #

Aha! programs are built from modules. Each module consists of the specification, which uses only a subset of the Aha! syntax, and the implementation. The specification contains declarations of the types and data items provided by the module; the implementation is the code that computes those data items. Multiple implementations of the same module may exist, but only one is chosen when a project (e.g. an application or a library) is built.

The Aha! module system guarantees that any name (data type or variable) in the specification never conflicts with names in other modules and even several versions of the same module may exist in the same project. Modules are anonymous, never directly refer to each other and instead use 'virtual names'; mapping between virtual names and the actual code is established by external means. This prevents situations when tons of code need to be adjusted in order to, for example, bring two separate projects together; moreover, replacing a module in a project becomes trivial.

A module can be generic, i.e. can refer to some open (unspecified) data types, introduced with the keyword **arbitrary**. When the module is used, these data types are substituted with specific data types known to the module that uses that module. Examples of generic modules are [Collections](Collections.md), [Dictionaries](Dictionaries.md) and [Trees](Trees.md). Generic modules are a great way to make the code reusable where some data types (such as the type of a collection's items) can be abstracted away.

## Opaque types ##

Some user-defined data types can be declared in the specification as **opaque**. This means that only that module's code 'knows' the actual definitions of these data types because they are hidden inside the implementation part of the module. For such a type, there must be at least one function in the module that returns result of that type: there is no other way to define a value of that type.

Since the implementation of an opaque type is unknown to other modules, it can be changed without them knowing it. For example, module [Float](Float.md) contains an opaque type Float; the actual implementation of it can be, say, a 8-byte or 10-byte floating point number; changing from one to the other will not affect any programs except that the results will be computed with more or less precision.

## Base Library ##

Aha! comes with a small but powerful library, consisting of modules that implement some useful data types and common operations on them: dynamic collections and character strings, dictionaries, floating-point and rational numbers, timestamps and intervals, money sums, bit strings and trees. The specifications of the [Base Library](BaseLibrary.md) modules can be found in the Wiki pages on the Aha! web site.

## Operators ##

We have seen a few examples of operators already, such as arithmetic and comparison operators. Apart from the built-in operators defined for the primitive data types, Aha! allows the developer to define one's own operators, both unary and binary. Whenever an operator expression is used as an argument for another operator, it is enclosed in parentheses, together with its arguments; no precedence rules are assumed. This is done so to avoid ambiguity and simplify the syntax of statements.

An operator pattern (i.e. operator together with its parameters' types) can be considered a 'name' for a function type value having the same parameter types.

Operators can be polymorphic, i.e. the same operator symbol can be associated with different functions, as soon as the parameter types unambiguously identify which of the functions has to be called.

Base Library modules [Rational](Rational.md), [Float](Float.md), [Time](Time.md), [Money](Money.md) and [Bits](Bits.md) contain examples of operator declarations.

# Applications #

Although Aha! programs only describe computations and have no side effects, they can be used to control the behavior of real-world objects, such as computer hardware, and various software components. Particularly, Aha! programs can define _applications_ - special objects that are supposed to run under the control of some runtime framework. An application doesn't call any routines that may affect the environment. Instead, it computes and returns to the framework a series of _jobs_ - values that tell the framework what to do. The framework informs the application of any changes in the environment via _events_ of a data type chosen by the developer.

Aha! comes with runtime environment called Aha! Runtime Framework; programs communicate with it via a set of modules called [API](API.md).

# Conclusion #

Aha! is an ambitious project aimed to establish new 'rules of the game' in programming. It parts with many unsafe/problematic features of the modern programming languages, instead offering a goal-directed, structured and logical approach.

The main advantages of adopting the language would be much cleaner, more maintainable code, considerably fewer bugs and improved overall quality of the software, plus the potential performance benefits from the intrinsic parallelism of the language.