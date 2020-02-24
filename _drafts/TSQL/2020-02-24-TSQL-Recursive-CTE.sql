/*
    Create a simple `Category` table for storing a 
    tree structure of categories in a flat table.

    Use the `ParentId` to find the parent of the 
    category.
*/
CREATE TABLE Category (
    Id uniqueidentifier NOT NULL
    , ParentId uniqueidentifier NULL
    , Title nvarchar(75) NOT NULL
);
GO

/*
    Use a CTE to order the categories
    by level and parent.
*/
;WITH Tree(Id, ParentId, Title, [Level])
AS (
    SELECT Id
        , ParentId
        , Title
        , 0 AS [Level]
    FROM Category p
    WHERE ParentId IS NULL
    UNION ALL 
    SELECT t.Id
        , t.ParentId
        , t.Title
        , tree.Level + 1 [Level]
    FROM Category t
    JOIN Tree tree ON tree.Id = t.ParentId
)
SELECT * 
FROM Tree
ORDER BY [Level], ParentId
