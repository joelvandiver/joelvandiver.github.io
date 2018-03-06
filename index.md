---
layout: default
---

# [](#header-1)Fractals

## [](#header-2)F# Snippets

### [](#header-3)Printing Properties in FSI

Printing Properties of any given type to the FSI console for quick coding.

```fs
let printProperities<'T> () =
	typedefof<'T>
    	.GetProperties()
    	|> Seq.map(fun y -> y.Name)
    	|> Seq.iter(fun z -> printfn "%s" z)

type MyType =
  { PropA     : string
    PropB     : int
    PropC     : System.Guid  }

// FSI
> printProperities<MyType>();;
PropA
PropB
PropC
val it : unit = ()
```

## [](#header-6)Haskell Snippets

Basic factorial implementation

```hs
fac n = if n == 0 then 1 else n * fac (n-1)

main = print (fac 42)
```

## [](#header-6)Functional TS Snippets

Add a value to the beginning of a copy of an array.
Return a new array to avoid mutation of the source array.

```ts
export default function add<T>(values: T[], value: T) {
  return [value].concat(values);
}
```

## [](#header-6)React Snippets

React Seed: index.ts

```tsx
import * as React from "react";
import * as ReactDOM from "react-dom";

import { APP } from "./components/App";

ReactDOM.render(<APP name="React App" />, document.getElementById("app"));
```

React Seed: ./components/App

```tsx
import * as React from "react";

export interface IApp {
  readonly name: string;
}

export const APP = (props: IApp) => (
  <div>
    <h1>{props.name}</h1>
  </div>
);
```

## [](#header-5)Daily Posts

#### [](#header-4)2/18/2018

Today, I'm changing some parameters with color in HTML5 Canvas:

* [1](/daily/2018/02/18/fractals/1.html)
* [2](/daily/2018/02/18/fractals/2.html)
* [3](/daily/2018/02/18/fractals/3.html)
* [4](/daily/2018/02/18/fractals/4.html)
