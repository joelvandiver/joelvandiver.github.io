(**
# Implicit vs Explicit Type Declarations
06-18-2019

## Implicit

F# supports implicit type declarations.  This can be a powerful technique because the F# type system will:
    1. Infer the Type - Inference allows for more succinct code with less `code noise`.
    2. Auto-Generalize Variable Types - Auto-Generalization leads to more efficient `code reuse`.

### Inference Example:
*)
let inf1 x = 3 * x
(**
> Output:
```fsharp
val inf1 : x:int -> int
```
> Note:  The `x` has been inferred to be of type `int`.  The F# compiler is smart enough to know that only an `int` parameter type for `x` will suffice.

### Auto-Generalization Example:
*)
let auto1 x y = x + y
(**
> Output:
```fsharp
val auto1 : x:int -> y:int -> int
```
> Note:  The types for `x` and `y` were declared to be of type `int`.  However, providing an implementation of float values `1.` and `2.` values below will resolve the types as `float`.
*)
let auto2 = auto1 1. 2.
(**
> Output:
```fsharp
val auto1 : x:float -> y:float -> float
```
> Note:  The FSI environment will throw an error when defining a `resolved` generic function:
> Output:
```fsharp
index.fsx(6,19): error FS0001: This expression was expected to have type
    'int'    
but here has type
    'float'  
```
To avoid the error, send both the `auto` function value and the computed `auto2` value to FSI.
*)


