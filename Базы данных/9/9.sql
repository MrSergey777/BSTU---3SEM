--1 -- обявить переменные, присвоить значения, вывести
DECLARE -->
 @ch char(1) = '1',
 @text varchar(50) = 'text',
 @i int,
 @d datetime,
 @timeVar time,  
 @si smallint,
 @ti tinyint,
 @nu numeric(12,5);
 SELECT -->
    @ch   AS ch,
    @text AS text_value,
    @i    AS int_value,
    @d    AS datetime_value,
    @timeVar AS time_value,
    @si   AS smallint_value,
    @ti   AS tinyint_value,
    @nu   AS numeric_value;
SET @d = GETDATE();-->
SET @i = 11; -->
SELECT @ti = 12, @si = 12;
PRINT N'Питон' + N'Одобряет' + @text; -->
PRINT @ti;
--2 -- Определить общую вместимость и условие
DECLARE @VM INT = (SELECT SUM(AUDITORIUM_CAPACITY) FROM AUDITORIUM);
PRINT N'ОБЩАЯ ВМЕСТИМОСТЬ ' + CAST(@VM AS varchar(50));

DECLARE @KOL INT = (SELECT COUNT(*) FROM AUDITORIUM);
PRINT N'Количество аудиторий ' + CAST(@KOL AS varchar(50));

IF @VM > 200
BEGIN
    DECLARE @men INT = (SELECT COUNT(*) FROM AUDITORIUM WHERE AUDITORIUM_CAPACITY < (@VM / @KOL));
    DECLARE @percent DECIMAL(5,2) = CASE WHEN @KOL = 0 THEN 0
                                        ELSE CAST(@men AS DECIMAL(9,4)) / CAST(@KOL AS DECIMAL(9,4)) * 100 END;
    PRINT N'Меньше средней: ' + CAST(@men AS varchar(50)) + N' Процент: ' + CAST(@percent AS varchar(10)) + N'%';
END;
ELSE 
BEGIN
    PRINT N'ОБЩАЯ ВМЕСТИМОСТЬ ' + CAST(@VM AS varchar(50));
END;
--3 -- вывод глобальных переменных
print N'Количество обработанных строк '+ cast (@@ROWCOUNT as varchar(50))
print @@VERSION
print @@SPID
print @@ERROR
print @@SERVERNAME
print @@TRANCOUNT
print @@FETCH_STATUS 
print @@NESTLEVEL 
--4 -- 
--4.1 -- система уравнрений
Declare @t int = 0, @x int = 1 -- например;
if(@t > @x)
    begin 
    declare @z float = sin(@t) * sin(@t);
    print @z;
    end;
else if(@x > @t) 
    begin 
    declare @z1 float  = 4*(@t + @x);
    print @z1
    end;
else
    begin
    declare @z2 float = 1 - exp(@x-2);
    print @z2;
    end;
--4.2 -- преобразование ФИО
-- 4.2 Преобразование ФИО
SELECT 
    [NAME],
    LEFT([NAME], CHARINDEX(' ', [NAME]) - 1) 
    + ' ' +
    SUBSTRING([NAME], CHARINDEX(' ', [NAME]) + 1, 1) + '. ' +
    SUBSTRING([NAME], CHARINDEX(' ', [NAME], CHARINDEX(' ', [NAME]) + 1) + 1, 1) + '.'
AS ShortName
FROM STUDENT;
--4.3 Поиск студентов у которых день рождения в след месяце и возраст
SELECT 
    NAME,
    BDAY,
    DATEDIFF(YEAR, BDAY, GETDATE()) AS Age
FROM STUDENT
WHERE MONTH(BDAY) = MONTH(DATEADD(MONTH, 1, GETDATE()));
--4.4 -- день сдачи эк. по бд
SELECT 
    PDATE,
    DATENAME(WEEKDAY, PDATE) AS WeekDayName
FROM PROGRESS
INNER JOIN STUDENT 
    ON STUDENT.IDSTUDENT = PROGRESS.IDSTUDENT
WHERE STUDENT.IDGROUP = 22 
  AND PROGRESS.SUBJECT = N'СУБД';
  
  --5 -- IF - ELSE
  
  if((select count(*) from STUDENT s  where s.IDGROUP = 22) > 0) 
   begin 
   print 'Пока не всех отчислили'
   end;
  else 
    begin 
    print ':(';
    end;

--6 -- CASE - оценки
SELECT 
    CASE 
        WHEN NOTE BETWEEN 1 AND 3 THEN 'анлак'
        WHEN NOTE BETWEEN 4 AND 5 THEN 'сильнейший'
        WHEN NOTE BETWEEN 6 AND 10 THEN 'Слабовато'
    END AS Результат,
    COUNT(*) AS Количество
FROM PROGRESS p
INNER JOIN STUDENT s ON p.IDSTUDENT = s.IDSTUDENT
WHERE s.IDGROUP = 22
GROUP BY 
    CASE 
        WHEN NOTE BETWEEN 1 AND 3 THEN 'анлак'
        WHEN NOTE BETWEEN 4 AND 5 THEN 'сильнейший'
        WHEN NOTE BETWEEN 6 AND 10 THEN 'Слабовато'
    END;

--7 -- временная таблица и while 
CREATE TABLE #TempStudents (
    ID INT,
    Name NVARCHAR(50),
    Score INT
);

DECLARE @i1 INT = 1;
WHILE @i1 <= 10
BEGIN
    INSERT INTO #TempStudents (ID, Name, Score)
    VALUES (@i1, CONCAT(N'Студент_', @i1), @i1 * 10);

    SET @i1 = @i1 + 1;
END;
SELECT * FROM #TempStudents;

DROP TABLE #TempStudents;

--8 -- return
--хаахахах
CREATE PROCEDURE dbo.FindSqrt
    @x1 INT
AS
BEGIN
    DECLARE @a1 INT = 0, @b1 INT = @x1, @mid INT;

    WHILE (@a1 <= @b1)
    BEGIN
        SET @mid = (@a1 + @b1) / 2;

        IF (@mid * @mid = @x1)
        BEGIN
            RETURN @mid;
        END
        ELSE IF (@mid * @mid < @x1)
            SET @a1 = @mid + 1;
        ELSE
            SET @b1 = @mid - 1;
    END;
END;
GO
DECLARE @result INT;
EXEC @result = dbo.FindSqrt @x1 = 144;
PRINT CONCAT('Результат: ', @result);
--------------------------------------
DECLARE @n INT = 5;

PRINT 'Начало выполнения';
PRINT CONCAT('n + 1 = ', @n + 1);
PRINT CONCAT('n * 2 = ', @n * 2);

RETURN;

PRINT CONCAT('n ^ 2 = ', @n * @n);
PRINT 'Конец выполнения';
--8 -- try - catch - for - beginers
BEGIN TRY
    SELECT 
        IDSTUDENT,
        NOTE,
        NOTE / 0
    FROM PROGRESS;
END TRY
BEGIN CATCH
    PRINT 'Произошла ошибка!';
    SELECT 
        ERROR_NUMBER()   AS ErrorNumber,     -- код ошибки
        ERROR_MESSAGE()  AS ErrorMessage,    -- текст ошибки
        ERROR_LINE()     AS ErrorLine,       -- строка, где ошибка
        ERROR_PROCEDURE() AS ErrorProcedure, -- имя процедуры или NULL
        ERROR_SEVERITY() AS ErrorSeverity,   -- уровень серьезности
        ERROR_STATE()    AS ErrorState;      -- состояние ошибки
END CATCH;

