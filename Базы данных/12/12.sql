USE UNIVER;
GO

-- 1 -----неявная трагзакция------->
SET IMPLICIT_TRANSACTIONS ON;

CREATE TABLE #TestTable (
    ID INT PRIMARY KEY,
    Name VARCHAR(50)
);

INSERT INTO #TestTable VALUES (1, 'Test1');
INSERT INTO #TestTable VALUES (2, 'Test2');

SELECT * FROM #TestTable;

COMMIT;

DROP TABLE #TestTable;

SET IMPLICIT_TRANSACTIONS OFF;
GO

-- 2 -----явная транзакция ------->>>>
BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO FACULTY (FACULTY, FACULTY_NAME) VALUES ('TEST', N'Тестовый факультет');
    
    UPDATE FACULTY SET FACULTY_NAME = N'Обновленное название' WHERE FACULTY = 'TEST';
    
    DELETE FROM FACULTY WHERE FACULTY = 'TEST';
    
    COMMIT TRANSACTION;
    PRINT N'Транзакция успешно завершена';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    PRINT N'Ошибка: ' + ERROR_MESSAGE();
    PRINT N'Код ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
    PRINT N'Строка: ' + CAST(ERROR_LINE() AS VARCHAR(10));
END CATCH
GO

BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO FACULTY (FACULTY, FACULTY_NAME) VALUES ('TEST2', N'Тестовый факультет 2');
    
    INSERT INTO FACULTY (FACULTY, FACULTY_NAME) VALUES ('TEST2', N'Дубликат');
    
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    PRINT N'Ошибка: ' + ERROR_MESSAGE();
    PRINT N'Транзакция отменена';
END CATCH
GO

-- 3 --------> ТОЧКИ --> SAVE TRANSACTION 


BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO FACULTY (FACULTY, FACULTY_NAME) VALUES ('SAVE1', N'Точка сохранения 1');
    
    SAVE TRANSACTION SavePoint1;
    
    INSERT INTO FACULTY (FACULTY, FACULTY_NAME) VALUES ('SAVE2', N'Точка сохранения 2');
    
    SAVE TRANSACTION SavePoint2;
    
    UPDATE FACULTY SET FACULTY_NAME = N'Обновлено' WHERE FACULTY = 'SAVE2';
    
    ROLLBACK TRANSACTION SavePoint2;
    
    SELECT * FROM FACULTY WHERE FACULTY IN ('SAVE1', 'SAVE2');
    
    ROLLBACK TRANSACTION SavePoint1;
    
    SELECT * FROM FACULTY WHERE FACULTY IN ('SAVE1', 'SAVE2');
    
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    PRINT 'Ошибка: ' + ERROR_MESSAGE();
END CATCH
GO

DELETE FROM FACULTY WHERE FACULTY IN ('SAVE1', 'SAVE2');
GO

-- 4 --------> READ UNCOMMITID ---------> ВСЕ МОЖНО
IF OBJECT_ID('tempdb..##TestIsolation4') IS NOT NULL DROP TABLE ##TestIsolation4;
CREATE TABLE ##TestIsolation4 (
    ID INT PRIMARY KEY,
    Value INT,
    Name NVARCHAR(50)
);

INSERT INTO ##TestIsolation4 VALUES (1, 100, N'Исходное');
INSERT INTO ##TestIsolation4 VALUES (2, 200, N'Исходное');
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
BEGIN TRANSACTION IsolationA;

SELECT @@SPID AS [SPID], * FROM ##TestIsolation4;
-------------------------- t1 ------------------

SELECT @@SPID AS [SPID], * FROM ##TestIsolation4;

-------------------------- t2 ------------------

COMMIT TRANSACTION;
GO

DROP TABLE ##TestIsolation4;
GO

-- 5 ------read committed ------> только закоммиченные

IF OBJECT_ID('tempdb..##TestIsolation5') IS NOT NULL DROP TABLE ##TestIsolation5;
CREATE TABLE ##TestIsolation5 (
    ID INT PRIMARY KEY,
    Value INT,
    Name NVARCHAR(50)
);

INSERT INTO ##TestIsolation5 VALUES (1, 100, N'Исходное');
INSERT INTO ##TestIsolation5 VALUES (2, 200, N'Исходное');
GO

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRANSACTION IsolationA2;

SELECT @@SPID AS [SPID], COUNT(*) AS [Количество]
FROM ##TestIsolation5 WHERE Name = N'Обновлено';

-------------------------- t1 ------------------

-------------------------- t2 -----------------

SELECT @@SPID AS [SPID], N'update ##TestIsolation5' [результат], COUNT(*) AS [Количество]
FROM ##TestIsolation5 WHERE Name = N'Обновлено';

COMMIT TRANSACTION;
GO

DROP TABLE ##TestIsolation5;
GO

-- 6 -------> REAPETBLE READ
IF OBJECT_ID('tempdb..##TestIsolation6') IS NOT NULL DROP TABLE ##TestIsolation6;
CREATE TABLE ##TestIsolation6 (
    ID INT PRIMARY KEY,
    Value INT,
    Name NVARCHAR(50)
);

INSERT INTO ##TestIsolation6 VALUES (1, 100, N'Исходное1');
INSERT INTO ##TestIsolation6 VALUES (2, 200, N'Исходное2');
GO

SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
BEGIN TRANSACTION IsolationA3;

SELECT @@SPID AS [SPID], Name AS [Заказчик] FROM ##TestIsolation6 WHERE Value = 100;

-------------------------- t1 ------------------
-------------------------- t2 -----------------

SELECT @@SPID AS [SPID], 
    CASE WHEN Name = N'Новое' THEN N'insert ##TestIsolation6' ELSE N' ' END [результат],
    Name AS [Заказчик]
FROM ##TestIsolation6 WHERE Value = 200;

COMMIT TRANSACTION;
GO

DROP TABLE ##TestIsolation6;
GO

-- 7
IF OBJECT_ID('tempdb..##TestIsolation7') IS NOT NULL DROP TABLE ##TestIsolation7;
CREATE TABLE ##TestIsolation7 (
    ID INT PRIMARY KEY,
    Value INT,
    Name NVARCHAR(50)
);

INSERT INTO ##TestIsolation7 VALUES (1, 100, N'Исходное');
INSERT INTO ##TestIsolation7 VALUES (2, 200, N'Исходное');
GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION IsolationA4;

DELETE FROM ##TestIsolation7 WHERE Name = N'Новое';
INSERT INTO ##TestIsolation7 VALUES (3, 100, N'Новое');
UPDATE ##TestIsolation7 SET Name = N'Новое' WHERE Value = 100;

SELECT @@SPID AS [SPID], Name AS [Заказчик] FROM ##TestIsolation7 WHERE Value = 100;

-------------------------- t1 -----------------


SELECT @@SPID AS [SPID], Name AS [Заказчик] FROM ##TestIsolation7 WHERE Value = 100;

-------------------------- t2 ------------------

COMMIT TRANSACTION;
GO

DROP TABLE ##TestIsolation7;
GO

-- 8
BEGIN TRANSACTION OuterTransaction;
PRINT @@TRANCOUNT; 

BEGIN TRANSACTION InnerTransaction1;
PRINT @@TRANCOUNT;

BEGIN TRANSACTION InnerTransaction2;
PRINT @@TRANCOUNT;

SAVE TRANSACTION SavePointNested;
ROLLBACK TRANSACTION SavePointNested; 

COMMIT TRANSACTION InnerTransaction2; 
PRINT @@TRANCOUNT;
COMMIT TRANSACTION InnerTransaction1; 
PRINT @@TRANCOUNT;
ROLLBACK TRANSACTION OuterTransaction;
GO
