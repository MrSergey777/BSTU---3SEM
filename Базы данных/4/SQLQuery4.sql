USE UNIVER
--1
SELECT AUDITORIUM.AUDITORIUM, AUDITORIUM_TYPE.AUDITORIUM_TYPENAME
	FROM AUDITORIUM INNER JOIN AUDITORIUM_TYPE
		ON AUDITORIUM.AUDITORIUM_TYPE = AUDITORIUM_TYPE.AUDITORIUM_TYPE
--2
USE UNIVER
	SELECT AUDITORIUM.AUDITORIUM, AUDITORIUM_TYPE.AUDITORIUM_TYPENAME
	FROM AUDITORIUM INNER JOIN AUDITORIUM_TYPE
		ON AUDITORIUM.AUDITORIUM_TYPE = AUDITORIUM_TYPE.AUDITORIUM_TYPE AND
			AUDITORIUM_TYPE.AUDITORIUM_TYPENAME LIKE 'компьют%ер%'
--3.
	SELECT 
    FACULTY.FACULTY_NAME AS [Факультет],
    PULPIT.PULPIT_NAME AS [Кафедра],
    PROFESSION.PROFESSION_NAME AS [Специальность],
    SUBJECT.SUBJECT_NAME AS [Дисциплина],
    STUDENT.NAME AS [Имя Студента],
    CASE PROGRESS.NOTE
        WHEN 6 THEN 'шесть'
        WHEN 7 THEN 'семь'
        WHEN 8 THEN 'восемь'
    END AS [Оценка]
FROM PROGRESS
INNER JOIN STUDENT ON PROGRESS.IDSTUDENT = STUDENT.IDSTUDENT
INNER JOIN [GROUP] ON STUDENT.IDGROUP = [GROUP].IDGROUP
INNER JOIN PROFESSION ON [GROUP].PROFESSION = PROFESSION.PROFESSION
INNER JOIN SUBJECT ON PROGRESS.SUBJECT = SUBJECT.SUBJECT
INNER JOIN PULPIT ON SUBJECT.PULPIT = PULPIT.PULPIT
INNER JOIN FACULTY ON PROFESSION.FACULTY = FACULTY.FACULTY
WHERE PROGRESS.NOTE BETWEEN 6 AND 8
ORDER BY PROGRESS.NOTE DESC;
--4
    SELECT 
    PULPIT.PULPIT_NAME AS [Кафедра],
    ISNULL(TEACHER.TEACHER_NAME, '***') AS [Преподаватель]
    FROM PULPIT
    LEFT OUTER JOIN TEACHER ON PULPIT.PULPIT = TEACHER.PULPIT
    ORDER BY PULPIT.PULPIT_NAME, [Преподаватель];
--5
    -- TEACHER слева, PULPIT справа
SELECT T.TEACHER_NAME, P.PULPIT_NAME
FROM TEACHER T
FULL OUTER JOIN PULPIT P ON T.PULPIT = P.PULPIT;

-- PULPIT слева, TEACHER справа
SELECT T.TEACHER_NAME, P.PULPIT_NAME
FROM PULPIT P
FULL OUTER JOIN TEACHER T ON T.PULPIT = P.PULPIT;
--6
use UNIVER
    SELECT 
    A.AUDITORIUM_TYPE,
    AT.AUDITORIUM_TYPENAME,
    A.AUDITORIUM,
    A.AUDITORIUM_CAPACITY,
    A.AUDITORIUM_NAME
FROM AUDITORIUM_TYPE AT
CROSS JOIN AUDITORIUM A
WHERE AT.AUDITORIUM_TYPE = A.AUDITORIUM_TYPE;
---------------------------------------------7
--1
use [Курсы_повышения_квалификации]
SELECT [Курсы].Предмет, [Группы].Количество_студентов 
	FROM [Занятия]
    INNER JOIN [Курсы] ON [Занятия].Код_курса = [Курсы].Код_курса
    INNER JOIN [Группы] ON [Занятия].Номер_группы = [Группы].Номер_группы
    ORDER BY [Группы].Количество_студентов DESC;
--2
SELECT [Курсы].Предмет, [Группы].Количество_студентов
    FROM[Занятия]
    INNER JOIN [Курсы] ON [Занятия].Код_курса = [Курсы].Код_курса
    INNER JOIN [Группы] ON [Занятия].Номер_группы = [Группы].Номер_группы
    AND [Курсы].Предмет LIKE '%Математика%'
--3
SELECT [Занятия].Код_курса,[Курсы].Предмет
    FROM[Курсы]
     Left OUTER JOIN [Занятия] ON [Занятия].Код_курса = [Курсы].Код_курса
--4 
    SELECT [Занятия].Код_курса,[Курсы].Предмет
    FROM[Курсы]
    FULL OUTER JOIN [Занятия] ON [Занятия].Код_курса = [Курсы].Код_курса
-- 
    SELECT [Занятия].Код_курса,[Курсы].Предмет
    FROM[Занятия]
    FULL OUTER JOIN [Курсы] ON [Занятия].Код_курса = [Курсы].Код_курса
--5
SELECT [Курсы].Предмет, ISNULL([Занятия].Код_курса,'1') AS Код_курса
    FROM[Курсы]
     Left OUTER JOIN [Занятия] ON [Занятия].Код_курса = [Курсы].Код_курса
--6
    use Курсы_повышения_квалификации
    SELECT [Занятия].Код_курса,[Курсы].Предмет
    FROM[Курсы] CROSS JOIN [Занятия]
    WHERE Занятия.Код_курса = Курсы.Код_курса;