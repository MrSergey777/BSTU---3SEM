USE Курсы_повышения_квалификации
CREATE TABLE Преподаватели
( Код_преподавателя int primary key,
  Фамилия varchar(50),
  Имя varchar(50) not null,
  Отчество varchar(50),
  Телефон varchar(50) not null,
  Стаж int,
);
CREATE TABLE Группы
( Номер_группы int primary key,
  Специальность varchar(50),
  Отделение varchar(50),
  Количество_студентов int,
);
CREATE TABLE Курсы
(  Код_курса int primary key,
   Предмет varchar(50),
   Тип_занятия varchar(50),
   Количество_часов int,
   Оплата float,
);
CREATE TABLE Занятия
( Номер_группы int,
  Код_преподавателя int,
  Код_курса int,
  id_занятия int primary key,
  CONSTRAINT FK_Занятия_Группы FOREIGN KEY (Номер_группы) REFERENCES Группы(Номер_группы),
  CONSTRAINT FK_Занятия_Преподаватели FOREIGN KEY (Код_преподавателя) REFERENCES Преподаватели(Код_преподавателя),
  CONSTRAINT FK_Занятия_Курсы FOREIGN KEY (Код_курса) REFERENCES Курсы(Код_курса),
);
