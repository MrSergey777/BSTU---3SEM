USE UNIVER;
GO

-- 4
BEGIN TRANSACTION IsolationB;
--SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SELECT @@SPID AS [SPID];

UPDATE ##TestIsolation4 SET Value = 150, Name = N'Обновлено' WHERE ID = 1;

-------------------------- t1 --------------------


UPDATE ##TestIsolation4 SET Value = 250, Name = N'Обновлено' WHERE ID = 2;

INSERT INTO ##TestIsolation4 VALUES (3, 300, N'Новое');

-------------------------- t2 --------------------

ROLLBACK TRANSACTION;
GO

-- 5
BEGIN TRANSACTION IsolationB2;

-------------------------- t1 --------------------

UPDATE ##TestIsolation5 SET Name = N'Обновлено' WHERE Name = N'Исходное';

COMMIT TRANSACTION;

-------------------------- t2 --------------------

GO

-- 6
BEGIN TRANSACTION IsolationB3;

-------------------------- t1 --------------------



UPDATE ##TestIsolation6 SET Name = N'Обновлено' WHERE Name = N'Исходное2';

SELECT @@SPID AS [SPID], 
    CASE WHEN Name = N'Новое' THEN N'insert ##TestIsolation6' ELSE N' ' END [результат],
    Name AS [Заказчик]
FROM ##TestIsolation6 WHERE Value = 200;

INSERT INTO ##TestIsolation6 VALUES (3, 100, N'Новое');

COMMIT TRANSACTION;

-------------------------- t2 --------------------

GO

-- 7
BEGIN TRANSACTION IsolationB4;

DELETE FROM ##TestIsolation7 WHERE Name = N'Новое';
INSERT INTO ##TestIsolation7 VALUES (4, 100, N'Новое');
UPDATE ##TestIsolation7 SET Name = N'Новое' WHERE Value = 200;

SELECT @@SPID AS [SPID], Name AS [Заказчик] FROM ##TestIsolation7 WHERE Value = 100;

-------------------------- t1 --------------------

COMMIT TRANSACTION;

SELECT @@SPID AS [SPID], Name AS [Заказчик] FROM ##TestIsolation7 WHERE Value = 100;

-------------------------- t2 --------------------

GO