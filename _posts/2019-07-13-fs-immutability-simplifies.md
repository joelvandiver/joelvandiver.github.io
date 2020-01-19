# Immutability Reduces Information Domain

I have heard many developers who come from non-functional programming languages such as C++ and C# claim that immutability is overly restrictive.  It is true that immutability is a restrictive, but this restriction comes at one huge benefit:  reduction of possible outcomes.  

`Information` may be informally defined as the element of surprise.  The more surprises a system has, the more information it contains.  Note the distinction between actual information and possible information.  Typically as developers we focus our algorithms on the actual information that our system processes.  But, of equal (or greater) concern is of the possible information the system will proccess.  Let's call this the `Information Domain`.  Effectively, it is the concern of the developer to process the Information Domain.  We have to anticipate all of the possible outcomes of our system.  

Mutability at its core allows for surprise.  

Let's take a simple mathematical function and restricted domain for example.

```fsharp
let f x = x + 3
let domainX = [-2;-1;0;1;2]
let table = domainX |> List.map f
```
 

> Output:
```fsharp
val f : x:int -> int
val domainX : int list = [-2; -1; 0; 1; 2]
val table : int list = [1; 2; 3; 4; 5]
```

Now, let's explore the same function with a mutable side-effect:

 
```fsharp
let mutable X = 4
 
let f' x =
    X <- X + 1
    X + x + 3
 
let table' = domainX |> List.map f'
let table'2 = domainX |> List.map f'
```


> Output:
```fsharp
val mutable X : int = 14
val f' : x:int -> int
val table' : int list = [6; 8; 10; 12; 14]
val table'2 : int list = [11; 13; 15; 17; 19]
```
Note the `table'2` has different values from the values in `table'` as expected.

This may seem simple when you can *read* the source code as we are doing here, but if you did not have access to the source code, this behavior would surely be a **surprise**.

The `f'` can be restructured to make the behavior less of a surprise:

 
```fsharp
let f'' x y = y + x + 3
```


Here, we've removed the mutable call, and instead declared a new param, `y`, that declares the dependency for `f''` to operate.  

Now, we can provide another restricted domain for the `y` param:


```fsharp
let domainY = [4;5;6;7;8;9]
```


This has the effect of combining both `domainX` and `domainY` through a cartesian product:

 
```fsharp
let table'3 =
    domainX
    |> List.map(fun x -> domainY |> List.map (f'' x))
    |> List.concat
```
 

> Output:
```fsharp
val f'' : x:int -> y:int -> int
val domainY : int list = [4; 5; 6; 7; 8; 9]
val table'3 : int list =
  [5; 6; 7; 8; 9; 10; 6; 7; 8; 9; 10; 11; 7; 8; 9; 10; 11; 12; 8; 9; 10; 11;
   12; 13; 9; 10; 11; 12; 13; 14]
```

By allowing mutation, we've increased the element of surprise.  I have found this aspect of mutation especially difficult to reason about in large code bases.  

Restrictions are not always an inhibitor, rather they can play a central role in improving the speed and effeciency in development.


