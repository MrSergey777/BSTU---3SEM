USE Курсы_повышения_квалификации
ALTER DATABASE Курсы_повышения_квалификации 
  ADD FILEGROUP FG1;
CREATE TABLE TO_GROP
(
	namee char(50) primary key
) ON FG1;
