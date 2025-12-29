--1  -------. Разработать скалярную функцию с именем COUNT_STUDENTS, которая вычисляет
----количе-ство студентов на факультете, код которого задается параметром типа varchar(20) с именем @faculty.
USE UNIVER;
GO

CREATE FUNCTION dbo.COUNT_STUDENTS (@faculty VARCHAR(20))
RETURNS INT
AS
BEGIN
    DECLARE @count INT;

    SELECT @count = COUNT(DISTINCT s.IDSTUDENT)
    FROM FACULTY f
    INNER JOIN [GROUP] g ON f.FACULTY = g.FACULTY
    INNER JOIN STUDENT s ON g.IDGROUP = s.IDGROUP
    WHERE f.FACULTY = @faculty;

    RETURN @count;
END;
GO

SELECT dbo.COUNT_STUDENTS('ИЭФ') AS 'Количество студентов по факультету ИЭФ';

ALTER FUNCTION dbo.COUNT_STUDENTS 
(
    @faculty VARCHAR(20) = NULL,
    @prof VARCHAR(20) = NULL -- код PROFESSION для фильтрации
)
RETURNS INT
AS
BEGIN
    DECLARE @count INT;

    SELECT @count = COUNT(DISTINCT s.IDSTUDENT)
    FROM FACULTY f
    INNER JOIN [GROUP] g ON f.FACULTY = g.FACULTY
    INNER JOIN PROFESSION sp ON g.PROFESSION = sp.PROFESSION
    INNER JOIN STUDENT s ON g.IDGROUP = s.IDGROUP
    WHERE (@faculty IS NULL OR f.FACULTY = @faculty)
      AND (@prof IS NULL OR sp.PROFESSION = @prof);

    RETURN @count;
END;
GO
SELECT dbo.COUNT_STUDENTS('ИЭФ', NULL) AS 'Студенты по факультету FA1';
DROP FUNCTION dbo.COUNT_STUDENTS;
GO

--2 ---Разработать скалярную функцию с именем FSUB-JECTS,
--принимающую параметр @p типа varchar(20), значение которого задает код кафедры (столбец SUBJECT.PULPIT). 
--Использовать локальный статический курсор на основе SELECT-запроса к таблице SUBJECT.
USE UNIVER;
GO

CREATE FUNCTION dbo.FSUBJECTS (@p VARCHAR(20))
RETURNS VARCHAR(300)
AS
BEGIN
    DECLARE @discipline_name VARCHAR(100);
    DECLARE @result VARCHAR(300) = 'SUBJECTS';
    DECLARE @first BIT = 1; -- флаг для первой записи
    
    DECLARE discipline_cursor CURSOR LOCAL STATIC
    FOR 
    SELECT d.SUBJECT_NAME
    FROM SUBJECT d
    WHERE d.PULPIT = @p
    ORDER BY d.SUBJECT_NAME;
    
    OPEN discipline_cursor;
    FETCH NEXT FROM discipline_cursor INTO @discipline_name;
    
    IF @@FETCH_STATUS = 0
    BEGIN
        SET @result = @result + ': ' + @discipline_name;
        SET @first = 0;
    END
    
    FETCH NEXT FROM discipline_cursor INTO @discipline_name;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @result = @result + ', ' + @discipline_name;
        FETCH NEXT FROM discipline_cursor INTO @discipline_name;
    END
    
    CLOSE discipline_cursor;
    DEALLOCATE discipline_cursor;
    IF @first = 1
        SET @result = @result + '.';
    
    RETURN @result;
END;
GO
SELECT dbo.FSUBJECTS('ИСиТ') AS [Дисциплины кафедры IT];

--3----Использует SELECT-запрос c левым внешним соеди-нением между таблицами FACULTY и PULPIT. 
USE UNIVER;
GO

CREATE FUNCTION dbo.FFACPUL 
(
    @faculty_code VARCHAR(20) = NULL,
    @pulpit_code VARCHAR(20) = NULL
)
RETURNS TABLE
AS
RETURN
    SELECT 
        f.FACULTY AS FACULTY,
        k.PULPIT AS PULPIT
    FROM FACULTY f
    LEFT OUTER JOIN PULPIT k ON f.FACULTY = k.FACULTY
    WHERE 
        (@faculty_code IS NULL OR f.FACULTY = @faculty_code)
        AND
        (@pulpit_code IS NULL OR k.PULPIT = @pulpit_code)
        AND
        NOT (@faculty_code IS NULL AND @pulpit_code IS NOT NULL AND k.PULPIT IS NULL);
GO
SELECT * FROM dbo.FFACPUL('ИЭФ', NULL)
ORDER BY PULPIT;

SELECT * FROM dbo.FFACPUL(NULL, 'ИСиТ');
GO

--4 ---На рисунке ниже показан сценарий, демонстрирую-щий работу скалярной функции FCTEACHER. 
--Функция принимает один параметр, задающий код кафедры. Функция возвращает количество препода-вателей на заданной параметром кафедре.

CREATE FUNCTION dbo.FCTEACHER(@pulpit_code CHAR(20))
RETURNS INT
AS
BEGIN
    DECLARE @teacher_count INT;
    
    IF @pulpit_code IS NULL
        SELECT @teacher_count = COUNT(*) 
        FROM TEACHER;
    ELSE
        SELECT @teacher_count = COUNT(*) 
        FROM TEACHER 
        WHERE PULPIT = @pulpit_code;
    
    RETURN @teacher_count;
END;
GO
SELECT PULPIT, dbo.FCTEACHER(PULPIT) AS [Количество преподавателей] 
FROM PULPIT;
SELECT dbo.FCTEACHER(NULL) AS [Всего преподавателей];
GO
--5 
IF OBJECT_ID('dbo.FACULTY_REPORT', 'TF') IS NOT NULL
    DROP FUNCTION dbo.FACULTY_REPORT;
GO

IF OBJECT_ID('dbo.FACULTY_PROFESSION_COUNT', 'FN') IS NOT NULL
    DROP FUNCTION dbo.FACULTY_PROFESSION_COUNT;
GO

IF OBJECT_ID('dbo.COUNTS_STUDENTS', 'FN') IS NOT NULL
    DROP FUNCTION dbo.COUNTS_STUDENTS;
GO

IF OBJECT_ID('dbo.FACULTY_GROUP_COUNT', 'FN') IS NOT NULL
    DROP FUNCTION dbo.FACULTY_GROUP_COUNT;
GO

IF OBJECT_ID('dbo.FACULTY_PULPIT_COUNT', 'FN') IS NOT NULL
    DROP FUNCTION dbo.FACULTY_PULPIT_COUNT;
GO
-- 1. 
CREATE FUNCTION dbo.FACULTY_PULPIT_COUNT(@faculty_code CHAR(10))
RETURNS INT
AS
BEGIN
    DECLARE @count INT;
    SELECT @count = COUNT(*) 
    FROM PULPIT 
    WHERE FACULTY = @faculty_code;
    RETURN @count;
END;
GO

-- 2. 
CREATE FUNCTION dbo.FACULTY_GROUP_COUNT(@faculty_code CHAR(10))
RETURNS INT
AS
BEGIN
    DECLARE @count INT;
    SELECT @count = COUNT(*) 
    FROM [GROUP] 
    WHERE FACULTY = @faculty_code;
    RETURN @count;
END;
GO

-- 3. 
CREATE FUNCTION dbo.COUNTS_STUDENTS(@faculty_code CHAR(10), @year CHAR(10) = NULL)
RETURNS INT
AS
BEGIN
    DECLARE @count INT;
    
    IF @year IS NULL
        SELECT @count = COUNT(s.IDSTUDENT)
        FROM STUDENT s
        JOIN [GROUP] g ON s.IDGROUP = g.IDGROUP
        WHERE g.FACULTY = @faculty_code;
    ELSE
        SELECT @count = COUNT(s.IDSTUDENT)
        FROM STUDENT s
        JOIN [GROUP] g ON s.IDGROUP = g.IDGROUP
        WHERE g.FACULTY = @faculty_code 
          AND g.YEAR_FIRST = @year;
    
    RETURN @count;
END;
GO

-- 4. 
CREATE FUNCTION dbo.FACULTY_PROFESSION_COUNT(@faculty_code CHAR(10))
RETURNS INT
AS
BEGIN
    DECLARE @count INT;
    SELECT @count = COUNT(*) 
    FROM PROFESSION 
    WHERE FACULTY = @faculty_code;
    RETURN @count;
END;
GO
CREATE FUNCTION dbo.FACULTY_REPORT(@min_students INT)
RETURNS @fr TABLE 
(
    [Факультет] VARCHAR(50),
    [Количество кафедр] INT,
    [Количество групп] INT,
    [Количество студентов] INT,
    [Количество специальностей] INT
)
AS
BEGIN
    INSERT INTO @fr
    SELECT 
        f.FACULTY_NAME AS [Факультет],
        dbo.FACULTY_PULPIT_COUNT(f.FACULTY) AS [Количество кафедр],
        dbo.FACULTY_GROUP_COUNT(f.FACULTY) AS [Количество групп],
        dbo.COUNTS_STUDENTS(f.FACULTY, DEFAULT) AS [Количество студентов],
        dbo.FACULTY_PROFESSION_COUNT(f.FACULTY) AS [Количество профессий]
    FROM FACULTY f
    WHERE dbo.COUNTS_STUDENTS(f.FACULTY, DEFAULT) > @min_students;
    
    RETURN;
END;
GO
SELECT * FROM dbo.FACULTY_REPORT(3);