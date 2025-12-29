--1 --------->>>Процедура формирует результирующий набор на ос-нове таблицы SUBJECT
USE UNIVER;
GO
-----DROP PROCEDURE PSUBJECT;
CREATE PROCEDURE PSUBJECT
AS
BEGIN
    DECLARE @row_count INT;

    SELECT 
        d.SUBJECT,
        d.SUBJECT_NAME,
        k.PULPIT
    FROM SUBJECT d
    INNER JOIN PULPIT k ON d.PULPIT = k.PULPIT
    ORDER BY d.SUBJECT;

    SELECT @row_count = COUNT(*)
    FROM SUBJECT;

    RETURN @row_count;
END;
GO
DECLARE @count INT;
EXEC @count = PSUBJECT;
PRINT 'Количество дисциплин = ' + CAST(@count AS VARCHAR(10));
GO
--2 ----->Изменить, чтобы она принимала два па-раметра с именами @p и @c.
ALTER PROCEDURE PSUBJECT
@p VARCHAR(20) =NULL ,
@c INT OUTPUT
AS
BEGIN
DECLARE @k int;
 PRINT 'параметры: @p = ' + ISNULL(@p, 'NULL') + ', @c = ' + CAST(@c AS VARCHAR(3));
 SELECT
 d.SUBJECT ,
 d.SUBJECT_NAME,
 k.PULPIT
    FROM SUBJECT d
    INNER JOIN PULPIT k ON d.PULPIT = k.PULPIT
    WHERE @p IS NULL OR k.PULPIT = @p
    ORDER BY d.SUBJECT;
SET @c = @@ROWCOUNT;

    SELECT @k = COUNT (*)
    FROM SUBJECT;

    RETURN @k;
    END;
    GO

   DECLARE @k INT = 0, @c INT = 0, @p VARCHAR(20) = 'КАФ_ПО';
   EXEC @k = PSUBJECT @p =@p ,@c = @c OUTPUT;
   PRINT 'Количество дисциплин всего = ' + CAST(@k AS VARCHAR(10));
PRINT 'Количество дисциплин для PULPIT ' + @p + ' = ' + CAST(@c AS VARCHAR(10));
GO
--3 ------->>>>Создать временную локальную таблицу с именем #SUBJECT.------INSERT…EXECUTE 
CREATE TABLE #SUBJECT (
    SUBJECT CHAR(10),
    SUBJECT_NAME VARCHAR(100),
    Название_кафедры VARCHAR(100)
);
GO
ALTER PROCEDURE PSUBJECT
@p VARCHAR(20) = NULL
AS
BEGIN
PRINT 'параметр: @p = ' + ISNULL(@p, 'NULL');

SELECT
 d.SUBJECT ,
 d.SUBJECT_NAME,
 k.PULPIT
    FROM SUBJECT d
    INNER JOIN PULPIT k ON d.PULPIT = k.PULPIT
    WHERE @p IS NULL OR k.PULPIT = @p
    ORDER BY d.SUBJECT;

    DECLARE @k INT;
    SELECT @k = COUNT(*);
    RETURN @k;
END;
GO

DECLARE @result INT;
INSERT INTO #SUBJECT
EXEC @result = PSUBJECT @p = 'КАФ_ПО';
PRINT 'Добавлено SUBJECT для PULPIT КАФ_ПО, всего в базе: ' + CAST(@result AS VARCHAR(10));

INSERT INTO #SUBJECT
EXEC @result = PSUBJECT @p= NULL;
PRINT 'Добавлено всех SUBJECT, всего в базе: ' + CAST(@result AS VARCHAR(10));

SELECT * FROM #SUBJECT;
DROP  PROCEDURE PSUBJECT;
GO
--4 ------->>>Процедура PAUDITORIUM_INSERT должна при-менять механизм TRY/CATCH для обработки ошибок. В случае возникновения ошибки, процедура должна формировать сообщение, содержащее код ошибки, 
------уровень серьезности и текст сообщения в стандартный выходной поток. 
CREATE PROCEDURE PAUDITORIUM_INSERT
@a CHAR(20),
@n VARCHAR(50),
@c INT = 0 ,
@t CHAR(10)
AS
DECLARE @rc  INT = 1;
BEGIN TRY
IF @c < 1 OR @c > 300
BEGIN
        RAISERROR('Вместимость должна быть в диапазоне от 1 до 300', 16, 1);
        RETURN -1;
    END
    
    -- Проверка существования кода типа аудитории
    IF NOT EXISTS (SELECT 1 FROM AUDITORIUM_TYPE WHERE AUDITORIUM_TYPE = @t)
    BEGIN
        RAISERROR('AUDITORIUM_TYPE не существует в таблице AUDITORIUM_TYPE', 16, 1);
        RETURN -1;
    END
    
    -- Выполнение вставки
    INSERT INTO AUDITORIUM (AUDITORIUM, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY, AUDITORIUM_NAME)
    VALUES (@a, @t, @c, @n);
    
    PRINT 'Запись успешно добавлена в таблицу AUDITORIUM';
    RETURN @rc;
END TRY
BEGIN CATCH
    -- Обработка ошибки
    PRINT '--- ОШИБКА ПРИ ВСТАВКЕ ДАННЫХ ---';
    PRINT 'Код ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(6));
    PRINT 'Сообщение: ' + ERROR_MESSAGE();
    PRINT 'Уровень серьезности: ' + CAST(ERROR_SEVERITY() AS VARCHAR(6));
    PRINT 'Состояние: ' + CAST(ERROR_STATE() AS VARCHAR(8));
    PRINT 'Номер строки: ' + CAST(ERROR_LINE() AS VARCHAR(8));
    
    IF ERROR_PROCEDURE() IS NOT NULL
        PRINT 'Имя процедуры: ' + ERROR_PROCEDURE();
    
    RETURN -1;
END CATCH;
GO

DECLARE @rc INT;
EXEC @rc = PAUDITORIUM_INSERT 
    @a = 'K-105', 
    @n = 'Аудитория 105', 
    @c = 50, 
    @t = 'ЛК';
PRINT 'Результат теста 1 (ожидается 1): ' + CAST(@rc AS VARCHAR(3));

-- Тест 2: Ошибка - нарушение ограничения CHECK (вместимость > 300)
EXEC @rc = PAUDITORIUM_INSERT 
    @a = 'А-106', 
    @n = 'Аудитория 106', 
    @c = 350, 
    @t = 'ЛК';
PRINT 'Результат теста 2 (ожидается -1): ' + CAST(@rc AS VARCHAR(3));
-- Тест 3: Ошибка - нарушение первичного ключа (дубликат кода аудитории)
EXEC @rc = PAUDITORIUM_INSERT 
    @a = 'А-101',  -- Код уже существует
    @n = 'Аудитория 101-доп', 
    @c = 60, 
    @t = 'ЛК';
PRINT 'Результат теста 3 (ожидается -1): ' + CAST(@rc AS VARCHAR(3));

SELECT * FROM AUDITORIUM ORDER BY AUDITORIUM;

DROP PROCEDURE PAUDITORIUM_INSERT;
GO
--5 -----RTRIM ---->Разработать процедуру с именем SUBJECT_REPORT, формирующую в стандартный 
----выходной поток отчет со списком дисциплин на кон-кретной кафедре
CREATE PROCEDURE SUBJECT_REPORT 
    @p CHAR(10)  -- входной параметр: PULPIT
AS
    DECLARE @rc INT = 0;  -- счетчик SUBJECT                            
    DECLARE @subject_name VARCHAR(100);
    DECLARE @report_text VARCHAR(MAX) = '';
    
BEGIN TRY
    -- Проверка существования PULPIT с заданным кодом
    IF NOT EXISTS (SELECT 1 FROM PULPIT WHERE PULPIT = @p)
    BEGIN
        -- Генерация ошибки с уровнем серьезности 11
        RAISERROR('ошибка в параметрах', 11, 1);
    END
    
    -- Объявление курсора для получения SUBJECT PULPIT
    DECLARE SubjectCursor CURSOR FOR 
    SELECT SUBJECT_NAME 
    FROM SUBJECT 
    WHERE PULPIT = @p
    ORDER BY SUBJECT_NAME;
    
    
    OPEN SubjectCursor;
    
    FETCH SubjectCursor INTO @subject_name;
    
    -- Формирование заголовка отчета
    DECLARE @faculty_name VARCHAR(50);
    SELECT @faculty_name = f.FACULTY_NAME
    FROM PULPIT k
    INNER JOIN FACULTY f ON k.FACULTY = f.FACULTY
    WHERE k.PULPIT = @p;
    
    PRINT '========================================';
    PRINT 'ОТЧЕТ ПО SUBJECT ДЛЯ PULPIT';
    PRINT '----------------------------------------';
    PRINT 'PULPIT: ' + @p;
    
    
    DECLARE @department_name VARCHAR(100);
    SELECT @department_name = PULPIT_NAME
    FROM PULPIT 
    WHERE PULPIT = @p;
    
    PRINT 'Название PULPIT: ' + @department_name;
    PRINT 'Факультет: ' + @faculty_name;
    PRINT '----------------------------------------';
    
  
    WHILE @@FETCH_STATUS = 0                                     
    BEGIN 
        -- Добавление названия дисциплины к отчету
        IF @rc > 0
            SET @report_text = @report_text + ', ' + RTRIM(@subject_name);
        ELSE
            SET @report_text = RTRIM(@subject_name);
        
        
        SET @rc = @rc + 1;
        
        
        FETCH SubjectCursor INTO @subject_name;
    END;
    
    -- Вывод отчета
    IF @rc > 0
    BEGIN
        PRINT 'Список SUBJECT (' + CAST(@rc AS VARCHAR(3)) + '):';
        PRINT '----------------------------------------';
        PRINT @report_text;
        PRINT '========================================';
    END
    ELSE
    BEGIN
        PRINT 'Для PULPIT нет SUBJECT';
        PRINT '========================================';
    END;
    
    -- Закрытие курсора
    CLOSE SubjectCursor;
    DEALLOCATE SubjectCursor;
    
    -- Возврат количества дисциплин
    RETURN @rc;
END TRY  
BEGIN CATCH
    -- Обработка ошибки
    PRINT '========================================';
    PRINT 'ОШИБКА: ошибка в параметрах';
    PRINT '----------------------------------------';
    PRINT 'Неверный PULPIT: ' + @p;
    PRINT 'Проверьте правильность введенного PULPIT';
    PRINT '========================================';
    
    -- Дополнительная информация об ошибке
    IF ERROR_PROCEDURE() IS NOT NULL   
        PRINT 'Имя процедуры: ' + ERROR_PROCEDURE();
    
    RETURN @rc;
END CATCH;
GO

-- Тест 1: Успешный вызов для кафедры с дисциплинами
DECLARE @rc INT;
EXEC @rc = SUBJECT_REPORT @p = 'КАФ_ПО';
PRINT 'Количество дисциплин = ' + CAST(@rc AS VARCHAR(3));
PRINT '';
-- Тест 2: Ошибочный вызов - несуществующий код кафедры
EXEC @rc = SUBJECT_REPORT @p = 'НЕ_СУЩ';
PRINT 'Количество дисциплин = ' + CAST(@rc AS VARCHAR(3));
DROP PROCEDURE SUBJECT_REPORT;
GO
--6 ------->>>>Разработать процедуру с именем PAUDITORI-UM_INSERTX. 
---Процедура принимает пять входных параметров: @a, @n, @c, @t и @tn. 
---Процедура добавляет две строки. Первая строка добавляется в таблицу AUDITORIUM_TYPE. Значе-ния столбцов AUDITORIUM_TYPE и AUDITORI-UM_ TYPENAME задаются соответственно парамет-рами @t и @tn.
---Вторая строка добавляется путем вы-зова процедуры PAUDITORIUM_INSERT.
---Добавление строки  должны выполняться в рамках одной транзакции
CREATE PROCEDURE PAUDITORIUM_INSERTX
    @a CHAR(20),           -- AUDITORIUM
    @n VARCHAR(50),        -- Название_аудитории
    @c INT = 0,           -- Вместимость (значение по умолчанию 0)
    @t CHAR(10),           -- AUDITORIUM_TYPE (для таблицы AUDITORIUM_TYPE)
    @tn VARCHAR(50)       -- Название_типа (для таблицы AUDITORIUM_TYPE)
AS
    DECLARE @rc INT = 1;
BEGIN TRY
    -- Установка уровня изолированности транзакции
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    
    BEGIN TRANSACTION;
    
    PRINT 'Добавление нового типа аудитории:';
    PRINT 'AUDITORIUM_TYPE: ' + @t;
    PRINT 'Название типа: ' + @tn;
    
    INSERT INTO AUDITORIUM_TYPE (AUDITORIUM_TYPE, AUDITORIUM_TYPENAME)
    VALUES (@t, @tn);
    
    PRINT 'Тип аудитории успешно добавлен';
    PRINT '----------------------------------------';
    
    -- 2. Вызов процедуры PAUDITORIUM_INSERT для добавления аудитории
    PRINT 'Добавление новой аудитории:';
    PRINT 'AUDITORIUM: ' + @a;
    PRINT 'Название аудитории: ' + @n;
    PRINT 'Вместимость: ' + CAST(@c AS VARCHAR(10));
    PRINT 'AUDITORIUM_TYPE: ' + @t;
    
    DECLARE @insert_result INT;
    EXEC @insert_result = PAUDITORIUM_INSERTX
        @a = @a, 
        @n = @n, 
        @c = @c, 
        @t = @t;
    
    -- Проверка результата вызова процедуры
    IF @insert_result = -1
    BEGIN
        RAISERROR('Ошибка при добавлении аудитории', 16, 1);
    END
    
    -- Фиксация транзакции
    COMMIT TRANSACTION;
    
    PRINT '========================================';
    PRINT 'Транзакция успешно завершена';
    PRINT 'Добавлен новый тип аудитории и аудитория';
    PRINT '========================================';
    
    RETURN @rc;
END TRY
BEGIN CATCH
    -- Обработка ошибки
    PRINT '========================================';
    PRINT 'ОШИБКА В ТРАНЗАКЦИИ';
    PRINT '----------------------------------------';
    PRINT 'Код ошибки     : ' + CAST(ERROR_NUMBER() AS VARCHAR(6));
    PRINT 'Сообщение      : ' + ERROR_MESSAGE();
    PRINT 'Уровень        : ' + CAST(ERROR_SEVERITY() AS VARCHAR(6));
    PRINT 'Метка          : ' + CAST(ERROR_STATE() AS VARCHAR(8));
    PRINT 'Номер строки   : ' + CAST(ERROR_LINE() AS VARCHAR(8));
    
    IF ERROR_PROCEDURE() IS NOT NULL   
        PRINT 'Имя процедуры  : ' + ERROR_PROCEDURE();
    
    PRINT '----------------------------------------';
    
    -- Откат транзакции, если она активна
    IF @@TRANCOUNT > 0 
    BEGIN
        PRINT 'Выполняется откат транзакции...';
        ROLLBACK TRANSACTION;
        PRINT 'Транзакция откачена';
    END
    
    PRINT '========================================';
    
    RETURN -1;
END CATCH;
GO
PRINT '=== ТЕСТ 1: Успешное добавление ===';
DECLARE @rc INT;
EXEC @rc = PAUDITORIUM_INSERTX 
    @a = 'К-304',                 -- код аудитории
    @n = 'Компьютерный класс 304-1', -- название аудитории
    @c = 18,                      -- вместимость
    @t = 'КЛП',                    -- код типа (новый)
    @tn = 'Компьютерный класс с проектором !'; -- название типа (новое)

PRINT 'Результат теста 1 (ожидается 1): ' + CAST(@rc AS VARCHAR(3));
PRINT '';

-- Проверка результатов
SELECT 'Типы аудиторий:' AS Таблица, * FROM AUDITORIUM_TYPE;
PRINT '';
SELECT 'Аудитории:' AS Таблица, * FROM AUDITORIUM ;
PRINT '';

-- Тест 2: Ошибка - дубликат кода типа аудитории
PRINT '=== ТЕСТ 2: Ошибка - дубликат кода типа ===';
EXEC @rc = PAUDITORIUM_INSERTX 
    @a = 'А-109',
    @n = 'Аудитория 109',
    @c = 60,
    @t = 'ЛК',  -- код типа уже существует
    @tn = 'Лекционная с проектором';

PRINT 'Результат теста 2 (ожидается -1): ' + CAST(@rc AS VARCHAR(3));
PRINT '';
ALTER TABLE AUDITORIUM_TYPE
ALTER COLUMN AUAUDITORIUM VARCHAR(50);
DROP PROCEDURE PAUDITORIUM_INSERTX;