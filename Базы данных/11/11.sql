
----1 список дисциплин на исит
DECLARE 
    @shortname NVARCHAR(200),
    @list NVARCHAR(MAX) = N'';

DECLARE subj_cursor CURSOR FOR
    SELECT s.SUBJECT
    FROM SUBJECT s
    WHERE s.PULPIT = N'ИСиТ'  
    ORDER BY s.SUBJECT     
OPEN subj_cursor;
FETCH  subj_cursor INTO @shortname;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF @shortname IS NOT NULL
    BEGIN
        -- Используем RTRIM для удаления пробелов справа
        SET @list = @list + RTRIM(@shortname) + N',';
    END
    FETCH subj_cursor INTO @shortname;
END
CLOSE subj_cursor;
DEALLOCATE subj_cursor;
SELECT @list AS DisciplinesList;

--2 -- Отличии глобального от локального
---------------------------
-- ГЛОБАЛЬНЫЙ КУРСОР
---------------------------
DECLARE @subjName NVARCHAR(200), @globalList NVARCHAR(MAX) = N'';

DECLARE curSubjectsGlobal CURSOR FOR
    SELECT SUBJECT FROM SUBJECT WHERE PULPIT = N'ИСиТ' ORDER BY SUBJECT;

OPEN curSubjectsGlobal;

FETCH NEXT FROM curSubjectsGlobal INTO @subjName;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF @subjName IS NOT NULL SET @globalList = @globalList + RTRIM(@subjName) + N',';
    FETCH NEXT FROM curSubjectsGlobal INTO @subjName;
END

PRINT N'GLOBAL batch 1: ' + ISNULL(@globalList, N'(empty)');
GO

-- Второй патч: глобальный курсор ещё существует
DECLARE @subjName2 NVARCHAR(200), @globalList2 NVARCHAR(MAX) = N'';
FETCH NEXT FROM curSubjectsGlobal INTO @subjName2;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF @subjName2 IS NOT NULL SET @globalList2 = @globalList2 + RTRIM(@subjName2) + N',';
    FETCH NEXT FROM curSubjectsGlobal INTO @subjName2;
END

PRINT N'GLOBAL batch 2 (продолжение того же курсора): ' + ISNULL(@globalList2, N'(нет строк, курсор уже выбран)');
CLOSE curSubjectsGlobal;
DEALLOCATE curSubjectsGlobal;
GO

---------------------------
-- ЛОКАЛЬНЫЙ КУРСОР
---------------------------
DECLARE @subjNameLoc NVARCHAR(200), @localList NVARCHAR(MAX) = N'';

DECLARE curSubjectsLocal CURSOR LOCAL FOR
    SELECT SUBJECT FROM SUBJECT WHERE PULPIT = N'ИСиТ' ORDER BY SUBJECT;

OPEN curSubjectsLocal;

FETCH NEXT FROM curSubjectsLocal INTO @subjNameLoc;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF @subjNameLoc IS NOT NULL SET @localList = @localList + RTRIM(@subjNameLoc) + N',';
    FETCH NEXT FROM curSubjectsLocal INTO @subjNameLoc;
END

PRINT N'LOCAL batch 1: ' + ISNULL(@localList, N'(empty)');
GO

-- Второй патч: локального курсора уже нет
FETCH NEXT FROM curSubjectsLocal;
CLOSE curSubjectsLocal;
DEALLOCATE curSubjectsLocal;
GO

----3 --- static and dynamic


-- Статический курсор (снимок данных)
DECLARE @name NVARCHAR(200);
DECLARE curStatic CURSOR STATIC FOR
    SELECT SUBJECT FROM SUBJECT WHERE PULPIT = N'ИСиТ' ORDER BY SUBJECT;
OPEN curStatic;

FETCH NEXT FROM curStatic INTO @name;
PRINT 'STATIC before insert: ' + ISNULL(@name,'(null)');

-- Вставка новой строки
INSERT INTO SUBJECT (SUBJECT, PULPIT) VALUES (N'Новый_статик', N'ИСиТ');

FETCH NEXT FROM curStatic INTO @name;
PRINT 'STATIC after insert (не увидит новую): ' + ISNULL(@name,'(null)');

CLOSE curStatic; DEALLOCATE curStatic;
GO

-- Динамический курсор (видит изменения)
DECLARE @name2 NVARCHAR(200);
DECLARE curDynamic CURSOR DYNAMIC FOR
    SELECT SUBJECT FROM SUBJECT WHERE PULPIT = N'ИСиТ' ORDER BY SUBJECT;
OPEN curDynamic;

FETCH NEXT FROM curDynamic INTO @name2;
PRINT 'DYNAMIC before insert: ' + ISNULL(@name2,'(null)');

-- Вставка новой строки
INSERT INTO SUBJECT (SUBJECT, PULPIT) VALUES (N'Новый_динамик', N'ИСиТ');

FETCH NEXT FROM curDynamic INTO @name2;
PRINT 'DYNAMIC after insert (увидит новую): ' + ISNULL(@name2,'(null)');

CLOSE curDynamic; DEALLOCATE curDynamic;
GO

-----4 Scroll

DECLARE @s NVARCHAR(200);

DECLARE curScroll CURSOR STATIC SCROLL FOR
    SELECT SUBJECT FROM SUBJECT WHERE PULPIT = N'ИСиТ' ORDER BY SUBJECT;

OPEN curScroll;

FETCH FIRST  FROM curScroll INTO @s; PRINT 'FIRST: '    + ISNULL(@s,'(null)');
FETCH NEXT   FROM curScroll INTO @s; PRINT 'NEXT: '     + ISNULL(@s,'(null)');
FETCH PRIOR  FROM curScroll INTO @s; PRINT 'PRIOR: '    + ISNULL(@s,'(null)');
FETCH LAST   FROM curScroll INTO @s; PRINT 'LAST: '     + ISNULL(@s,'(null)');
FETCH ABSOLUTE 2 FROM curScroll INTO @s; PRINT 'ABS 2: ' + ISNULL(@s,'(null)');
FETCH RELATIVE 1 FROM curScroll INTO @s; PRINT 'REL +1: ' + ISNULL(@s,'(null)');
FETCH RELATIVE -2 FROM curScroll INTO @s; PRINT 'REL -2: ' + ISNULL(@s,'(null)');

CLOSE curScroll; DEALLOCATE curScroll;
GO

----5


BEGIN TRAN;

-- Подготовка демо-данных
INSERT INTO SUBJECT (SUBJECT, PULPIT) VALUES (N'TEST_CUR_1', N'TEST_CUR');
INSERT INTO SUBJECT (SUBJECT, PULPIT) VALUES (N'TEST_CUR_2', N'TEST_CUR');

DECLARE @name NVARCHAR(200);

-- Курсор допускает обновление/удаление
DECLARE curUpdDel CURSOR KEYSET FOR
    SELECT SUBJECT FROM SUBJECT
    WHERE PULPIT = N'TEST_CUR'
    FOR UPDATE;

OPEN curUpdDel;

-- UPDATE текущей строки
FETCH NEXT FROM curUpdDel INTO @name;
UPDATE SUBJECT
    SET SUBJECT = @name + N'_upd'
WHERE CURRENT OF curUpdDel;

-- DELETE текущей строки (следующая)
FETCH NEXT FROM curUpdDel INTO @name;
DELETE FROM SUBJECT
WHERE CURRENT OF curUpdDel;

CLOSE curUpdDel;
DEALLOCATE curUpdDel;

-- Проверка результата внутри транзакции
SELECT SUBJECT, PULPIT
FROM SUBJECT
WHERE PULPIT = N'TEST_CUR';

ROLLBACK TRAN; -- откат, чтобы не менять боевые данные
GO

----6
Ниже два коротких сценария с курсорами (UNIVER). Каждый завернут в транзакцию и ROLLBACK, чтобы не испортить данные. Уберите ROLLBACK → поставьте COMMIT для реального выполнения.

```sql
USE UNIVER;
GO

-------------------------------------------------
-- 1) Удалить из PROGRESS оценки < 4 (через join)
-------------------------------------------------
BEGIN TRAN;

DECLARE @idp int;

DECLARE curDel CURSOR KEYSET FOR
    SELECT p.IDSTUDENT
    FROM PROGRESS p
    JOIN STUDENT s ON s.IDSTUDENT = p.IDSTUDENT
    JOIN [GROUP] g ON g.IDGROUP = s.IDGROUP
    WHERE p.NOTE < 4;

OPEN curDel;

FETCH NEXT FROM curDel INTO @idp;
WHILE @@FETCH_STATUS = 0
BEGIN
    DELETE FROM PROGRESS WHERE CURRENT OF curDel;
    FETCH NEXT FROM curDel INTO @idp;
END

CLOSE curDel; DEALLOCATE curDel;

-- Проверка
SELECT * FROM PROGRESS WHERE NOTE < 4;

ROLLBACK TRAN; -- заменить на COMMIT для реального удаления
GO

-------------------------------------------------
-- 2) Увеличить оценку на 1 для конкретного студента
-------------------------------------------------
BEGIN TRAN;

DECLARE @idp2 int, @note int;
DECLARE @targetID int = 123; -- нужный IDSTUDENT

DECLARE curUpd CURSOR KEYSET FOR
    SELECT p.IDSTUDENT, p.NOTE
    FROM PROGRESS p
    WHERE p.IDSTUDENT = @targetID
    FOR UPDATE;

OPEN curUpd;

FETCH NEXT FROM curUpd INTO @idp2, @note;
WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE PROGRESS
       SET NOTE = NOTE + 1
     WHERE CURRENT OF curUpd;

    FETCH NEXT FROM curUpd INTO @idp2, @note;
END

CLOSE curUpd; DEALLOCATE curUpd;

-- Проверка
SELECT * FROM PROGRESS WHERE IDSTUDENT = @targetID;

ROLLBACK TRAN; -- заменить на COMMIT для реального обновления
GO