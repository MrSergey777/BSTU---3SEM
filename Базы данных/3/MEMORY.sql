use master
go
create database Курсы_повышения
on primary
( NAME = N'Курсы_mdf',
  FILENAME = N'C:\Users\ASUS\Desktop\Лабы\Базы данных\Курсы_mdf.mdf',
  SIZE = 5MB,
  MAXSIZE = UNLIMITED,
  FILEGROWTH = 1MB
)
LOG ON
(
  NAME = N'Курсы_log',
  FILENAME = N'C:\Users\ASUS\Desktop\Лабы\Базы данных\Курсы_log.ldf',
  SIZE = 10MB,
  MAXSIZE = 30MB,
  FILEGROWTH = 10%
);
GO
