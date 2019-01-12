# T-SQL Introduction

```sql
CREATE DATABASE TestDB
GO

USE TestDB;
GO

CREATE TABLE TestTable (Id INT)
GO

INSERT INTO TestTable(Id) VALUES (1), (2), (3), (4), (5)
GO

SELECT * FROM TestTable

DELETE FROM TestTable
```

| Id |
| - |
| 1 |
| 2 |
| 3 |
| 4 |
| 5 |