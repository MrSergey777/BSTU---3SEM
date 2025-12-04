--1
USE UNIVER
SELECT
  F.FACULTY_NAME        AS FACULTY,
  PR.PROFESSION_NAME    AS PROFESSION,
  S.SUBJECT_NAME        AS SUBJECT,
  AVG(P.NOTE)           AS AVG_MARK,
  COUNT(*)              AS EXAM_COUNT
FROM STUDENT SS
JOIN [GROUP] G      ON SS.IDGROUP   = G.IDGROUP
JOIN PROFESSION PR  ON G.PROFESSION = PR.PROFESSION
JOIN FACULTY F     ON PR.FACULTY   = F.FACULTY
JOIN PROGRESS P    ON SS.IDSTUDENT = P.IDSTUDENT
JOIN [SUBJECT] S   ON P.SUBJECT    = S.SUBJECT
WHERE F.FACULTY = N'ЛХ'
GROUP BY ROLLUP (F.FACULTY_NAME, PR.PROFESSION_NAME, S.SUBJECT_NAME)
ORDER BY F.FACULTY_NAME, PR.PROFESSION_NAME, S.SUBJECT_NAME;
--2
USE UNIVER
SELECT
  F.FACULTY_NAME        AS FACULTY,
  PR.PROFESSION_NAME    AS PROFESSION,
  S.SUBJECT_NAME        AS SUBJECT,
  AVG(P.NOTE)           AS AVG_MARK,
  COUNT(*)              AS EXAM_COUNT
FROM STUDENT SS
JOIN [GROUP] G      ON SS.IDGROUP   = G.IDGROUP
JOIN PROFESSION PR  ON G.PROFESSION = PR.PROFESSION
JOIN FACULTY F     ON PR.FACULTY   = F.FACULTY
JOIN PROGRESS P    ON SS.IDSTUDENT = P.IDSTUDENT
JOIN [SUBJECT] S   ON P.SUBJECT    = S.SUBJECT
WHERE F.FACULTY = N'ЛХ'
GROUP BY CUBE (F.FACULTY_NAME, PR.PROFESSION_NAME, S.SUBJECT_NAME)
ORDER BY F.FACULTY_NAME, PR.PROFESSION_NAME, S.SUBJECT_NAME;
--3
USE UNIVER
SELECT subject_name, avg_mark, exam_count, students_count
FROM (
  -- запрос для ТОВ
  SELECT
    g.FACULTY, g.PROFESSION, s.SUBJECT_NAME,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*) AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  INNER JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  INNER JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  INNER JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ЛХ'
  GROUP BY g.FACULTY, g.PROFESSION, s.SUBJECT_NAME

  UNION

  -- запрос для ХТиТ
  SELECT
    g.FACULTY, g.PROFESSION, s.SUBJECT_NAME,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*) AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ХТиТ'
  GROUP BY g.FACULTY, g.PROFESSION, s.SUBJECT_NAME
) t
ORDER BY profession, subject_name;
--------3-2
USE UNIVER
SELECT subject_name, avg_mark, exam_count, students_count
FROM (
  -- запрос для ТОВ
  SELECT
    g.FACULTY, g.PROFESSION, s.SUBJECT_NAME,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*) AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  INNER JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  INNER JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  INNER JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ЛХ'
  GROUP BY g.FACULTY, g.PROFESSION, s.SUBJECT_NAME

  UNION ALL

  -- запрос для ХТиТ
  SELECT
    g.FACULTY, g.PROFESSION, s.SUBJECT_NAME,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*) AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ХТиТ'
  GROUP BY g.FACULTY, g.PROFESSION, s.SUBJECT_NAME
) t
ORDER BY profession, subject_name;
--4
USE UNIVER;
GO

-- Левая часть: объединение по ЛХ и ХТиТ (через UNION)
SELECT subject_name, avg_mark, exam_count, students_count
FROM (
  SELECT
    s.SUBJECT_NAME      AS subject_name,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*)            AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  INNER JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  INNER JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  INNER JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ЛХ'
  GROUP BY s.SUBJECT_NAME

  UNION

  SELECT
    s.SUBJECT_NAME,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*) AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  INNER JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  INNER JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  INNER JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ХТиТ'
  GROUP BY s.SUBJECT_NAME
) AS left_set

INTERSECT

-- Правая часть: тот же набор (пример: можно заменить логикой сравнения с другими фильтрами)
SELECT subject_name, avg_mark, exam_count, students_count
FROM (
  SELECT
    s.SUBJECT_NAME      AS subject_name,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*)            AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  INNER JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  INNER JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  INNER JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ЛХ'
  GROUP BY s.SUBJECT_NAME

  UNION ALL

  SELECT
    s.SUBJECT_NAME,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*) AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  INNER JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  INNER JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  INNER JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ХТиТ'
  GROUP BY s.SUBJECT_NAME
) AS right_set
ORDER BY subject_name, avg_mark;
--5
USE UNIVER;
GO

SELECT subject_name, avg_mark, exam_count, students_count
FROM (
  -- Объединённый набор: ЛХ + ХТиТ
  SELECT
    s.SUBJECT_NAME AS subject_name,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*) AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY IN ('ЛХ', 'ХТиТ')
  GROUP BY s.SUBJECT_NAME
) AS combined

EXCEPT

SELECT subject_name, avg_mark, exam_count, students_count
FROM (
  -- Только ХТиТ
  SELECT
    s.SUBJECT_NAME AS subject_name,
    ROUND(AVG(p.NOTE), 2) AS avg_mark,
    COUNT(*) AS exam_count,
    COUNT(DISTINCT p.IDSTUDENT) AS students_count
  FROM [GROUP] g
  JOIN STUDENT st ON st.IDGROUP = g.IDGROUP
  JOIN PROGRESS p ON p.IDSTUDENT = st.IDSTUDENT
  JOIN SUBJECT s ON s.SUBJECT = p.SUBJECT
  WHERE g.FACULTY = 'ХТиТ'
  GROUP BY s.SUBJECT_NAME
) AS htit_only

ORDER BY subject_name;


