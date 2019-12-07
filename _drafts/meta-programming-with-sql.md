---
title: Meta Programming with SQL
categories: [Fs-Topics]
tags: []
---

# Meta Programming with SQL

As I'm sure any .NET programmer would know, .NET offers a pretty expansive reflection api.  Asking a system to provide reflection information is both a powerful and expensive technique.  It can dramatically reduce the quantity of code, but it comes with the extra overhead in runtime performance.  

I have come across situations many times where the code I am writing is very repetitive, and the only way to stop the repetition is with reflection.  

## Ex: Table Schema Validation

One common example must developers will recognize is validation against table schemas.  If you need to ensure the incoming .NET object will not raise a SQL exception, then you will typically have to check each of the properties on the record *per table*.  

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

If we had a corresponding F# record:

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

To ensure a successful command with this record against the database, then you will need to check:

1. `first_name` is not null and is 50 characters or less.
2. `last_name` is not null and is 50 characters or less.
3. `visited_at` is greater than the SQL minimum date.
4. ...

<!-- TODO:  Setup a Docker SQL Host and query against the INFORMATION_SCHEMA. -->

## Keep it D.R.Y.
