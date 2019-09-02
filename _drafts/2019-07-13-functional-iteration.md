---
title: 
categories: [Guide-F#]
tags: []
---

# Functional Iteration
07-13-2019

.NET has a few instances of collections types that do not inherit from IEnumberable<'T>.  The [Seq<'T> is an alias of the IEnumerable<'T>](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/sequences).  In dealing with these collection types that do not have a correlation to sequences, you will likely have to iterate through the collection.

One way to do so without maintaining mutable state is to use the `yield` keyword.

For example, the .NET type, DataTable and DataTableCollection, do not support IEnumerable.

Suppose, we had loaded the following data from a remote data connection:


let set = new System.Data.DataSet("Superheros")
let table = new System.Data.DataTable("Avengers")
set.Tables.Add(table)
table.Columns.Add("id")
table.Columns.Add("name")
table.Rows.Add(1, "Tony Stark")
table.Rows.Add(2, "Captain America")
table.Rows.Add(3, "Thor")
table.Rows.Add(4, "4")


Then, to process the data into a normal F# type, we can use the `yield` to extract the values we desire:


let heros = 
    seq { 
        for i in 0 .. set.Tables.Count - 1 do 
            let table = set.Tables.[i]
            for j in 0 .. table.Rows.Count - 1 do
                let row = table.Rows.[j]
                yield 
                    seq { 
                        let cells = row.ItemArray
                        for k in 0 .. cells.Length - 1 do
                            yield cells.[k] }
                    |> List.ofSeq
        }
    |> List.ofSeq

printfn "%A" heros


> Output:
```fsharp
val heros : obj list list =
  [["1"; "Tony Stark"]; ["2"; "Captain America"]; ["3"; "Thor"]; ["4"; "4"]]
```
