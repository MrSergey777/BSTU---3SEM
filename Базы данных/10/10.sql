--1 - создать вр. табл. --> План запроса --> Кл.Индекс
CREATE Table #TimeTable(
	Name varchar(50),
	Surname varchar(50),
	ID int
)
CREATE CLUSTERED INDEX IX_TimeTable_ID ON #TimeTable(ID);
Declare @i int = 1;
while(@i < 10000)
begin
	INSERT INTO #TimeTable Values('Человек_'+cast(@i AS varchar(50)),
	'Фамилия_'+ cast(@i AS varchar(50)), @i);
	set @i = @i+1
End
Select Name 
from #TimeTable t
where t.ID  = 15;

CREATE CLUSTERED INDEX IX_TimeTable_ID ON #TimeTable(ID);

Select Name 
from #TimeTable t
where t.ID  = 15;

--DROP TABLE #TimeTable;

--2 ---- создать вр. табл. --> План запроса --> НЕ.Кл.Индекс
CREATE TABLE #ProgressTest (
    ID INT,
    StudentName NVARCHAR(50),
    Subject NVARCHAR(50),
    Note INT
);
DECLARE @i3 INT = 1;

WHILE @i3 <= 10000
BEGIN
    INSERT INTO #ProgressTest (ID, StudentName, Subject, Note)
    VALUES (
        @i,
        CONCAT(N'Студент_', @i3),
        CASE WHEN @i3 % 2 = 0 THEN N'СУБД' ELSE N'Математика' END,
        @i3 % 10 + 1
    );
    SET @i3 = @i3 + 1;
END;

SELECT StudentName, Note
FROM #ProgressTest
WHERE Subject = N'СУБД' AND Note >= 8;

CREATE NONCLUSTERED INDEX IX_Progress_SubjectNote
ON #ProgressTest (Subject, Note);

SELECT StudentName, Note
FROM #ProgressTest
WHERE Subject = N'СУБД' AND Note >= 8;

--DROP TABLE #ProgressTest;
--DROP INDEX IX_Progress_SubjectNote ON #ProgressTest;
--3 -- Некластеризованный индекс покрытия 
CREATE TABLE #ProgressTest1 (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    StudentName NVARCHAR(100),
    Subject NVARCHAR(50),
    Note INT
);

DECLARE @i2 INT = 1;
WHILE @i2 <= 10000
BEGIN
    INSERT INTO #ProgressTest1 (StudentName, Subject, Note)
    VALUES (
        CONCAT(N'Студент_', @i2),
        CASE WHEN @i2 % 3 = 0 THEN N'СУБД' WHEN @i2 % 3 = 1 THEN N'Математика' ELSE N'Физика' END,
        (@i2 % 10) + 1
    );
    SET @i2 = @i2 + 1;
END;

PRINT '--- До создания индекса ---';

SELECT StudentName, Note
FROM #ProgressTest1
WHERE Subject = N'СУБД' AND Note >= 8;

CREATE NONCLUSTERED INDEX IX_Progress_Subject_Note_Covering
ON #ProgressTest1 (Subject, Note)
INCLUDE (StudentName); ----->

PRINT '--- После создания покрывающего индекса ---';

SELECT StudentName, Note
FROM #ProgressTest1
WHERE Subject = N'СУБД' AND Note >= 8;

-- DROP INDEX IX_Progress_Subject_Note_Covering ON #ProgressTest1;
-- DROP TABLE #ProgressTest1;

--4 ---- некластеризованный фильтруемый индекс

CREATE TABLE #StudyProgress (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    StudentName NVARCHAR(100),
    Course NVARCHAR(50),
    Score INT,
    ExamDate DATE
);

DECLARE @i4 INT = 1;
WHILE @i4 <= 12000
BEGIN
    INSERT INTO #StudyProgress (StudentName, Course, Score, ExamDate)
    VALUES (
        CONCAT(N'Студент_', @i4),
        CASE 
            WHEN @i4 % 4 = 0 THEN N'БазыДанных' 
            WHEN @i4 % 4 = 1 THEN N'Алгоритмы' 
            WHEN @i4 % 4 = 2 THEN N'Сети' 
            ELSE N'Математика' 
        END,
        (@i4 % 10) + 1,
        DATEADD(day, @i4 % 365, '2023-01-01')
    );
    SET @i4 = @i4 + 1;
END;

PRINT '--- До создания фильтруемого индекса ---';
SELECT StudentName, Score, ExamDate
FROM #StudyProgress
WHERE Course = N'БазыДанных' AND Score >= 8;

CREATE NONCLUSTERED INDEX IX_StudyProgress_Filtered_Score
ON #StudyProgress (Score)
WHERE Course = N'БазыДанных' AND Score >= 8;
 
PRINT '--- После создания фильтруемого индекса ---';
SELECT StudentName, Score, ExamDate
FROM #StudyProgress
WHERE Course = N'БазыДанных' AND Score >= 8;

-- DROP INDEX IX_StudyProgress_Filtered_Score ON #StudyProgress;
-- DROP TABLE #StudyProgress;

--5 -----Фрагментация

-- 1. Создание временной таблицы
CREATE TABLE #FragTest (
    ID INT IDENTITY(1,1) PRIMARY KEY,   -- кластерный индекс по умолчанию
    StudentName NVARCHAR(100),
    Score INT
);
-- 2. Заполнение 20000 строк
set @i4 = 1;
WHILE @i4 <= 20000
BEGIN
    INSERT INTO #FragTest (StudentName, Score)
    VALUES (CONCAT(N'Студент_', @i4), (@i4 % 100) + 1);
    SET @i4 = @i4 + 1;
END;

-- 3. Создание некластеризованного индекса по Score
CREATE NONCLUSTERED INDEX IX_Frag_Score ON #FragTest(Score);


CREATE PROCEDURE #GetFragInfo
AS
BEGIN
    SELECT 
        db_name(ps.database_id) AS DatabaseName,
        OBJECT_NAME(ps.object_id, ps.database_id) AS ObjectName,
        i.name AS IndexName,
        ps.index_id,
        ps.avg_fragmentation_in_percent,
        ps.page_count
    FROM sys.dm_db_index_physical_stats(DB_ID('tempdb'), OBJECT_ID('tempdb..#FragTest'), NULL, NULL, 'LIMITED') ps
    JOIN tempdb.sys.indexes i
        ON ps.object_id = i.object_id AND ps.index_id = i.index_id
    WHERE OBJECT_NAME(ps.object_id, ps.database_id) = '#FragTest';
END;
GO

-- 4. Оценка уровня фрагментации до искусственных изменений
PRINT '--- Фрагментация до изменений ---';
EXEC #GetFragInfo;

-- 5. Сценарий, повышающий фрагментацию
-- Идея: многократно удаляем часть строк и вставляем новые с рандомным ключом,
-- что вызывает page splits и фрагментацию. Повторяем несколько циклов.
DECLARE @cycle INT = 1;
WHILE @cycle <= 40  -- увеличьте число циклов, если не достигли высокой фрагментации
BEGIN
    -- Удаляем примерно 30% строк (по условию ID % 10 < 3)
    DELETE TOP (600) FROM #FragTest WHERE ID % 10 < 3;

    -- Вставляем 600 новых строк с рандомным Score (неупорядоченно)
    DECLARE @j INT = 1;
    WHILE @j <= 600
    BEGIN
        INSERT INTO #FragTest (StudentName, Score)
        VALUES (CONCAT(N'NewStudent_', NEWID()), ABS(CHECKSUM(NEWID())) % 100 + 1);
        SET @j = @j + 1;
    END;

    SET @cycle = @cycle + 1;
END;

-- 6. Оценка уровня фрагментации после сценария
PRINT '--- Фрагментация после сценария (ожидается рост) ---';
EXEC #GetFragInfo;

-- 7. Реорганизация индекса (online, менее затратная операция)
PRINT '--- Выполняем REORGANIZE ---';
ALTER INDEX IX_Frag_Score ON #FragTest REORGANIZE;

-- 8. Оценка фрагментации после REORGANIZE
PRINT '--- Фрагментация после REORGANIZE ---';
EXEC #GetFragInfo;

-- 9. Переcтройка индекса (REBUILD)
PRINT '--- Выполняем REBUILD ---';
ALTER INDEX IX_Frag_Score ON #FragTest REBUILD;

-- 10. Оценка фрагментации после REBUILD
PRINT '--- Фрагментация после REBUILD ---';
EXEC #GetFragInfo;


DROP PROCEDURE #GetFragInfo;
-- DROP INDEX IX_Frag_Score ON #FragTest;
-- DROP TABLE #FragTest;

---6 ----- Falicator

CREATE TABLE #FFTest (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Val INT
);

DECLARE @i4 INT = 1;
WHILE @i4 <= 5000
BEGIN
    INSERT INTO #FFTest (Val) VALUES (@i4 % 100);
    SET @i4 = @i4 + 1;
END;

CREATE NONCLUSTERED INDEX IX_FF_Default ON #FFTest(Val);

PRINT '--- Статистика для индекса без FILLFACTOR ---';
SELECT 
    i.name AS IndexName,
    ps.index_id,
    ps.page_count,
    ps.avg_fragmentation_in_percent,
    ps.avg_page_space_used_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID('tempdb'), OBJECT_ID('tempdb..#FFTest'), NULL, NULL, 'LIMITED') ps
JOIN tempdb.sys.indexes i
    ON ps.object_id = i.object_id AND ps.index_id = i.index_id
WHERE i.name = 'IX_FF_Default';

DROP INDEX IX_FF_Default ON #FFTest;
CREATE NONCLUSTERED INDEX IX_FF_70 ON #FFTest(Val) WITH (FILLFACTOR = 70);

PRINT '--- Статистика для индекса с FILLFACTOR = 70 ---';
SELECT 
    i.name AS IndexName,
    ps.index_id,
    ps.page_count,
    ps.avg_fragmentation_in_percent,
    ps.avg_page_space_used_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID('tempdb'), OBJECT_ID('tempdb..#FFTest'), NULL, NULL, 'LIMITED') ps
JOIN tempdb.sys.indexes i
    ON ps.object_id = i.object_id AND ps.index_id = i.index_id
WHERE i.name = 'IX_FF_70';

DROP INDEX IX_FF_70 ON #FFTest;
DROP TABLE #FFTest;

