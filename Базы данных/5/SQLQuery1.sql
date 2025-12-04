USE UNIVER;
--1
SELECT FACULTY.FACULTY_NAME, PULPIT.PULPIT_NAME, PROFESSION.PROFESSION_NAME
FROM FACULTY, PULPIT, PROFESSION
WHERE  FACULTY.FACULTY = PULPIT.FACULTY
AND FACULTY.FACULTY = PROFESSION.FACULTY
AND PROFESSION.PROFESSION_NAME IN (
    SELECT PROFESSION_NAME 
    FROM PROFESSION 
    WHERE PROFESSION_NAME LIKE '%технология%' 
       OR PROFESSION_NAME LIKE '%технологии%'
)
--2
SELECT FACULTY.FACULTY_NAME, PULPIT.PULPIT_NAME, PROFESSION.PROFESSION_NAME
FROM FACULTY 
INNER JOIN PULPIT ON FACULTY.FACULTY = PULPIT.FACULTY
INNER JOIN PROFESSION ON FACULTY.FACULTY = PROFESSION.FACULTY
WHERE PROFESSION.PROFESSION_NAME IN (
    SELECT PROFESSION_NAME 
    FROM PROFESSION 
    WHERE PROFESSION_NAME LIKE '%технология%' 
       OR PROFESSION_NAME LIKE '%технологии%'
)
--3
SELECT FACULTY.FACULTY_NAME, PULPIT.PULPIT_NAME, PROFESSION.PROFESSION_NAME
FROM FACULTY 
INNER JOIN PULPIT ON FACULTY.FACULTY = PULPIT.FACULTY
INNER JOIN PROFESSION ON FACULTY.FACULTY = PROFESSION.FACULTY
WHERE(PROFESSION_NAME LIKE '%технология%' 
OR PROFESSION_NAME LIKE '%технологии%')
--4
USE UNIVER
SELECT 
q.AUDITORIUM_TYPE,
q.AUDITORIUM_CAPACITY AS Максимальная_вместимость
FROM AUDITORIUM q
WHERE AUDITORIUM_CAPACITY = (SELECT TOP(1) AUDITORIUM_CAPACITY FROM AUDITORIUM
                          WHERE q.AUDITORIUM_TYPE = AUDITORIUM.AUDITORIUM_TYPE 
                          ORDER BY AUDITORIUM.AUDITORIUM_CAPACITY DESC)
--5
SELECT FACULTY.FACULTY_NAME FROM FACULTY
WHERE NOT EXISTS( SELECT * FROM PULPIT
                    WHERE FACULTY.FACULTY = PULPIT.FACULTY)
--6
SELECT
  (SELECT AVG(NOTE) FROM PROGRESS WHERE SUBJECT = 'ПСП') AS avg_PSP,
  (SELECT AVG(NOTE) FROM PROGRESS WHERE SUBJECT = 'ПИС')    AS avg_PIS,
  (SELECT AVG(NOTE) FROM PROGRESS WHERE SUBJECT = 'СУБД')  AS avg_SUBD;
--7
SELECT DISTINCT s.IDSTUDENT, s.NAME
FROM STUDENT s
JOIN PROGRESS p ON s.IDSTUDENT = p.IDSTUDENT
WHERE p.NOTE > ALL (
  SELECT NOTE
  FROM PROGRESS
  WHERE SUBJECT = 'СУБД'
);
--8
SELECT DISTINCT s.IDSTUDENT, s.NAME
FROM STUDENT s
JOIN PROGRESS p ON s.IDSTUDENT = p.IDSTUDENT
WHERE p.NOTE > ANY (
  SELECT NOTE
  FROM PROGRESS
  WHERE  SUBJECT = 'СУБД'
);
USE UNIVER;

/* SELECT
-- 1. Списки факультета, кафедры и профессии, где в названии профессии встречается "технология" (объединены в одну строку)
(SELECT STRING_AGG(CONCAT(f.FACULTY_NAME, ' / ', p.PULPIT_NAME, ' / ', pr.PROFESSION_NAME), '; ')
 FROM FACULTY f
 JOIN PULPIT p ON f.FACULTY = p.FACULTY
 JOIN PROFESSION pr ON f.FACULTY = pr.FACULTY
 WHERE pr.PROFESSION_NAME LIKE '%технологи%') AS tech_faculty_pulpit_profession,

-- 2. То же через явные JOIN (вернём количество найденных сочетаний)
(SELECT COUNT(*)
 FROM FACULTY f
 INNER JOIN PULPIT p ON f.FACULTY = p.FACULTY
 INNER JOIN PROFESSION pr ON f.FACULTY = pr.FACULTY
 WHERE pr.PROFESSION_NAME LIKE '%технологи%') AS count_tech_combinations,

-- 3. Результат фильтрации групп по специальности с LIKE (вернём список номеров групп, объединённый в строку)
(SELECT STRING_AGG(CONVERT(varchar(50), g.Номер_группы), '; ')
 FROM Группы g
 WHERE g.Специальность LIKE '%технология%' OR g.Специальность LIKE '%технологии%') AS groups_with_technology_specialty,

-- 4. Тип занятия с максимальным количеством часов (тип и значение)
(SELECT STRING_AGG(CONCAT(k.Тип_занятия, ' (', CONVERT(varchar(20), k.Количество_часов), 'ч)'), '; ')
 FROM Курсы k
 WHERE k.Количество_часов = (SELECT MAX(Количество_часов) FROM Курсы)
) AS max_hours_course_type_and_value,

-- 5. Факультеты без кафедр (объединённый список)
(SELECT STRING_AGG(f.FACULTY_NAME, '; ')
 FROM FACULTY f
 WHERE NOT EXISTS (SELECT 1 FROM PULPIT p WHERE p.FACULTY = f.FACULTY)
) AS faculties_without_pulpits,

-- 6. Три независимых AVG по предметам ОАиП, БД, СУБД (если нет значений — NULL)
(SELECT
   CAST((SELECT AVG(CAST(NOTE AS DECIMAL(8,2))) FROM PROGRESS WHERE SUBJECT = 'ОАиП') AS varchar(20)) + ';' +
   CAST((SELECT AVG(CAST(NOTE AS DECIMAL(8,2))) FROM PROGRESS WHERE SUBJECT = 'БД') AS varchar(20)) + ';' +
   CAST((SELECT AVG(CAST(NOTE AS DECIMAL(8,2))) FROM PROGRESS WHERE SUBJECT = 'СУБД') AS varchar(20))
) AS avg_notes_OAIP_BD_SUBD,

-- 7. Количество студентов, у которых есть оценка > ALL оценок по курсу СУБД
(SELECT COUNT(DISTINCT s.IDSTUDENT)
 FROM STUDENT s
 JOIN PROGRESS p ON s.IDSTUDENT = p.IDSTUDENT
 WHERE p.NOTE > ALL (
     SELECT NOTE FROM PROGRESS WHERE SUBJECT = 'СУБД'
 )
) AS students_with_note_greater_than_all_SUBD,

-- 8. Количество студентов, у которых есть оценка > ANY оценки по курсу СУБД
(SELECT COUNT(DISTINCT s.IDSTUDENT)
 FROM STUDENT s
 JOIN PROGRESS p ON s.IDSTUDENT = p.IDSTUDENT
 WHERE p.NOTE > ANY (
     SELECT NOTE FROM PROGRESS WHERE SUBJECT = 'СУБД'
 )
) AS students_with_note_greater_than_any_SUBD;
*/
