+++
title = "Power of Immutability"
date = 2019-07-13
description = "Immutability reduces possibilities."
slug = "2019-07-13-immutability"
taxonomies = { tags = ["Rust"] }
+++

I have heard many developers coming from imperative programming languages like Python, C#, and C++ say that immutability is overly restrictive. It is true that immutability is restrictive, but this restriction comes with one huge benefit: reduction of possible outcomes.

`Information` may be informally defined as the element of surprise.  The more surprises a system has, the more information it contains.  Note the distinction between actual information and possible information.  Typically as developers we focus our algorithms on the actual information that our system processes.  But, of equal (or greater) concern is of the possible information the system will process.  Let's call this the `Information Domain`.  Effectively, it is the concern of the developer to process the `Information Domain`.  We have to anticipate all of the possible outcomes of our system or at least the edges of our system.

## Simple Immutability

Let's take a simple mathematical function with a restricted domain for example. We'll transform it to show how mutability increases the information.

```rust
fn f(x: i32) -> i32 {
    x + 3
}
let domain = vec![-2, -1, 0, 1, 2];
let range = domain.into_iter().map(f).collect::<Vec<i32>>();
assert_eq!(range, vec![1, 2, 3, 4, 5]);
```

That's a pretty straightforward Mathematical definition:

```math
f(x) = x + 3
D: [-2, -1, 0, 1, 2]
R: [1, 2, 3, 4, 5]
```

## Problem with Mutability

> Mutability at its core allows for surprise.  

Now, let's explore the same function with a mutable side-effect:

```rust
let mut a = 4;
fn f(x: i32) -> i32 {
    x + 3
}
let domain = vec![-2, -1, 0, 1, 2];
let range = domain
    .iter()
    .map(|x| {
        a = a + 1;
        f(a + x)
    })
    .collect::<Vec<i32>>();
assert_eq!(range, vec![6, 8, 10, 12, 14]);
let range2 = domain
    .iter()
    .map(|x| {
        a = a + 1;
        f(a + x)
    })
    .collect::<Vec<i32>>();
assert_eq!(range2, vec![11, 13, 15, 17, 19]);
```

Note the `range2` vector has different values from the `range` vector. This may seem simple when you read the source code, but if you did not have access to the source, this behavior may be a surprise if you only new the definition of the `f` function.

I like how Rust's closure syntax with the `mut` keyword makes the mutation explicit!

Now, let's refactor the mutability away to get the same outcome. This makes the closure above referentially transparent.

```rust
fn f(x: i32, y: i32) -> i32 {
    x + y + 3
}
let domain_x = vec![-2, -1, 0, 1, 2];
let domain_y = vec![5, 6, 7, 8, 9, 10];
let range = domain_x
    .into_iter()
    .zip(domain_y.into_iter())
    .map(|(x, y)| f(x, y))
    .collect::<Vec<i32>>();
assert_eq!(range, vec![6, 8, 10, 12, 14]);
```

In this example, the `y` parameter was added to the `f` function to make the extra state of information transparent. It might not have been clear from the mutable example that the changing values of `a` over time were the values of the the vector, `vec![5, 6, 7, 8, 9, 10]`. But, that's the new dimension of data that the mutability introduced.

The extra complexity can also be seen by the use of the `zip` function above. The `zip` is effectively a cartesian product of `domain_x` and `domain_y`.

The Mathematical representation has gotten more complex as well. 

```math
f(x, y) = x + y + 3
D_x: [-2, -1, 0, 1, 2]
D_y: [5, 6, 7, 8, 9, 10]
R: [6, 8, 10, 12, 14]
```

From the Mathematical definition, it's plain to see that the function `f` was increased to the second dimension. Plotting this function on a 3-D graph would yield a 2-D plane.

## Benefits of Immutability

> Immutability reduces the information domain.

By allowing mutation, we've increased the element of surprise.  Mutable code requires the reader of code to mentally hold the state of a variable while reasoning through the code.

Restrictions are not always an inhibitor, they can reduce complexity!

## Caveats

The purist in me strives to keep everything immutable, but the pragmatist in me realizes that copying memory to support immutability can be prohibitively non-performant. So, in Rust, just as I try to confine the `unsafe` operations, I also try to confine the `mut` values and `.into` operations to a minimum.

## Update Log

> Update: 2026-02-12

My original post had a focus on how functional programming languages such as Haskell and F# improve legibility by requiring immutability. I decided to place the focus on immutability in general. Rust, for example, implements immutability by default without being a strict, functional programming language.
