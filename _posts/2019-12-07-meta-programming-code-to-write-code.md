---
title: Meta Programming - Code to Write Code
categories: [Fs-Topics]
tags: []
---

# Meta Programming - Code to Write Code

As I'm sure any .NET programmer would know, .NET offers a pretty expansive reflection api.  Asking a system to provide reflection information is both a powerful and expensive technique.  It can dramatically reduce the quantity of code, but it comes with the extra overhead in runtime performance.  

I have come across situations many times where the code I am writing is very repetitive, and the only way to stop the repetition *at runtime* is with reflection.  

## Validation Example

One common example is validation against table schemas.  If you need to ensure the incoming .NET object will not raise a SQL exception, then you will typically have to check each of the properties on the record *per table*.  

Take the following `dbo.visits` table:

```sql
CREATE TABLE dbo.visits (
    visit_id INT PRIMARY KEY IDENTITY (1, 1),
    first_name VARCHAR (50) NOT NULL,
    last_name VARCHAR (50) NOT NULL,
    visited_at DATETIME,
    phone VARCHAR(20)
);
```

and corresponding F# record:

```fsharp
type Visits =
    {   visit_id: int
        first_name: string
        last_name: string
        visite_at: DateTime
        phone: string
        store_id: int
        }
```

To ensure a successful transaction with this record against the database, then you will need to check:

1. `first_name` is not null and is 50 characters or less.
2. `last_name` is not null and is 50 characters or less.
3. `visited_at` is greater than the SQL minimum date.
4. ...

This might look something like:

```fsharp
let validate visit = 
    let errors = 
        [
            "first_name", visit.first_name <> null && visit.first_name.Length <= 50, "The first_name is required and must be less than or equal to 50 characters."
            "last_name", visit.last_name <> null && visit.last_name.Length <= 50, "The last_name is required and must be less than or equal to 50 characters."
            // ...
        ]

    match errors with 
    | [] -> Ok visit
    | _ -> Error errors
```

> Note:  You will need to include unit tests of the validation rules from the table schema.

You can see from the `validate` function above that the pattern of checking each of the fields is *very* repetitive.

> ***'Ain't no body got time for that!'***

## Keep it D.R.Y.

### INFORMATION_SCHEMA

The information that we are extracting from the `CREATE TABLE` can be found in the `INFORMATION_SCHEMA` in SQL SERVER.  (*Other database providers have their own schema querying views.*)

```sql
SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
```

| COLUMN_NAME | IS_NULLABLE | DATA_TYPE | CHARACTER_MAXIMUM_LENGTH |
| ----------- | ----------- | --------- | ------------------------ |
| visit_id    | NO          | int       | NULL                     |
| first_name  | NO          | varchar   | 50                       |
| last_name   | NO          | varchar   | 50                       |
| visited_at  | YES         | datetime  | NULL                     |
| phone       | YES         | varchar   | 20                       |

### F# Schema

Use your standard ORM to query this data out in an `fsx` script into an F# type:

```fsharp
type ColumnSchema =
    {   COLUMN_NAME: string
        IS_NULLABLE: string
        DATA_TYPE: string
        CHARACTER_MAXIMUM_LENGTH: int option
        }

let meta() = 
    
    // TODO: Run query...
    
    [
        {   COLUMN_NAME = "visit_id"
            IS_NULLABLE = "NO"
            DATA_TYPE = "int"
            CHARACTER_MAXIMUM_LENGTH = None
            }
        {   COLUMN_NAME = "first_name"
            IS_NULLABLE = "NO"
            DATA_TYPE = "varchar"
            CHARACTER_MAXIMUM_LENGTH = Some 50
            }
        {   COLUMN_NAME = "last_name"
            IS_NULLABLE = "NO"
            DATA_TYPE = "varchar"
            CHARACTER_MAXIMUM_LENGTH = Some 50
            }
        {   COLUMN_NAME = "visited_at"
            IS_NULLABLE = "YES"
            DATA_TYPE = "datetime"
            CHARACTER_MAXIMUM_LENGTH = None
            }
        {   COLUMN_NAME = "phone"
            IS_NULLABLE = "YES"
            DATA_TYPE = "varchar"
            CHARACTER_MAXIMUM_LENGTH = Some 20
            }
    ]
```

## Let's Get META

Convert the repetitive code above into a template.  This is where we treat ***code as data***!

```fsharp
let requiredStringTemplate (x: ColumnSchemaColumnSchema list = 
    let name = x.COLUMN_NAME
    let max = x.CHARACTER_MAXIMUM_LENGTH.Value
    sprintf """
    "%s", x.%s <> null && x.%s.Length <= %i, "The %s is required and must be less than or equal to %i characters."
    """ name name name max name max

// TODO:  Provide other rules.

let template (xs: ColumnSchema list) = 
    let rules = 
        xs 
        |> List.map(fun x -> 
            match (x.DATA_TYPE, x.IS_NULLABLE, x.CHARACTER_MAXIMUM_LENGTH) with 
            | ("varchar", "NO", Some _) -> requiredStringTemplate x |> Some
            // TODO:  Call other rules.
            | _ -> None)
        |> List.filter Option.isSome
        |> List.map Option.get
        |> List.fold (fun a b -> a + "\r\n            " + b) ""
    """
let validate x = 
    let errors = 
        [
            %s
        ]

    match errors with 
    | [] -> Ok x
    | _ -> Error errors
""" rules
```

## META Benefits

1. **META Test** - We can now safely delete our unit tests since we would only need to test the META code.
2. **Schema Changes** - As the schema evolves in the future, you can simply run this job to generate the final validation code.  This ensures that the database schema is the lone, authoritative source of schema information.

## Side Note

If you prefer to keep your database schema *dumb*, then you can move the authoritative source to another format such as `yaml`.  Then, use that source as your input into the `ColumnSchema` above.

***Pretty sweet!***