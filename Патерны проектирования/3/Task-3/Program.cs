interface IStaff
{
    List<JobVacancy> GetJobVacancies();
    List<Person> GetEmployees();
    List<string> GetJobTitles();
    int AddJobTitle();
    string PrintJobVacancies();
    bool DelJobTitle();
    void OpenJobVacancy(JobVacancy jv);
    bool CloseJobVacancy(int indexJV);
    void Recruit(int indexJV, Person w);
    void Dismiss(int indexJV, string reason);
}

public class Person
{
    public string Name { get; private set; }
    public int Age { get; private set; }

    public Person()
    {
        Name = "None";
        Age = 0;
    }

    public Person(Person other)
    {
        Name = other.Name;
        Age = other.Age;
    }

    public Person(string n = "None", int a = 0)
    {
        Name = n;
        Age = a;
    }

    public void SetName(string n) => Name = n;

    public void SetAge(int a) => Age = a;

    public void PrintInfo() => Console.WriteLine($"{Name}, Age: {Age}");
}

public class JobVacancy
{
    private static int globalId = 0;
    public int Id { get; private set; }
    public Person Worker { get; private set; }
    public int Salary { get; private set; }
    public string Requirements { get; private set; }
    public string JobTitle { get; private set; }
    public bool Open { get; private set; }

    public JobVacancy()
    {
        globalId++;
        Id = globalId;
        Salary = 0;
        Requirements = "None";
        JobTitle = "None";
        Open = false;
    }

    public JobVacancy(JobVacancy other)
    {
        globalId++;
        Id = globalId;
        Salary = other.Salary;
        Requirements = other.Requirements;
        JobTitle = other.JobTitle;
        Open = other.Open;
        Worker = other.Worker;
    }

    public JobVacancy(string jt = "None", int s = 0, string r = "None", bool o = false)
    {
        globalId++;
        Id = globalId;
        JobTitle = jt;
        Salary = s;
        Requirements = r;
        Open = o;
    }

    public void SetWorker(Person w) => Worker = w;

    public void SetSalary(int s) => Salary = s;

    public void SetRequirements(string r) => Requirements = r;

    public void SetPosition(string jt) => JobTitle = jt;

    public void SetOpen(bool o) => Open = o;

    public void PrintInfo() =>
        Console.WriteLine(
            $"[{Id}] {JobTitle}, Salary: {Salary}, Open: {Open}, Worker: {Worker?.Name ?? "vacant"}"
        );
}

public class Department
{
    private static int globalId = 0;
    public int Id { get; private set; }
    public string Name { get; private set; }

    public Department()
    {
        globalId++;
        Id = globalId;
        Name = "None";
    }

    public Department(Department other)
    {
        globalId++;
        Id = globalId;
        Name = other.Name;
    }

    public Department(string name = "None")
    {
        globalId++;
        Id = globalId;
        Name = name;
    }

    public void SetName(string name) => Name = name;

    public void PrintInfo() => Console.WriteLine($"[{Id}] Department: {Name}");
}

public class Organization : IStaff
{
    private static int globalId = 0;
    public int Id { get; private set; }
    public string Name { get; protected set; }
    public string ShortName { get; protected set; }
    public string Address { get; protected set; }
    public DateTime TimeStamp { get; protected set; }

    protected List<JobVacancy> JobVacancies = new List<JobVacancy>();

    public Organization()
    {
        globalId++;
        Id = globalId;
        Name = "None";
        ShortName = "None";
        Address = "None";
        TimeStamp = DateTime.Now;
    }

    public Organization(
        string name,
        string shortName = "None",
        string address = "None",
        DateTime? dt = null
    )
    {
        globalId++;
        Id = globalId;
        Name = name;
        ShortName = shortName;
        Address = address;
        TimeStamp = dt ?? DateTime.Now;
    }

    public Organization(Organization other)
    {
        globalId++;
        Id = globalId;
        Name = other.Name;
        ShortName = other.ShortName;
        Address = other.Address;
        TimeStamp = DateTime.Now;

        JobVacancies = new List<JobVacancy>();
        foreach (var jv in other.JobVacancies)
            JobVacancies.Add(new JobVacancy(jv));
    }

    public void SetName(string n) => Name = n;

    public void SetAddress(string a) => Address = a;

    public virtual void PrintInfo()
     {
        Console.WriteLine("Organization Method");
         Console.WriteLine($"[{Id}] {Name} ({ShortName}) — {Address}, {TimeStamp}");
     }
  
    /*public void PrintInfo()
    {
        Console.WriteLine("Organization Method");
        Console.WriteLine($"[{Id}] {Name} ({ShortName}) — {Address}, {TimeStamp}");
    }
   */
    public List<JobVacancy> GetJobVacancies() => JobVacancies;

    public List<Person> GetEmployees() =>
        JobVacancies.Where(jv => jv.Worker != null).Select(jv => jv.Worker).ToList();

    public List<string> GetJobTitles() => JobVacancies.Select(jv => jv.JobTitle).ToList();

    public int AddJobTitle()
    {
        var jv = new JobVacancy();
        JobVacancies.Add(jv);
        return jv.Id;
    }

    public void AddJobVacancy(JobVacancy jv)
    {
        JobVacancies.Add(jv);
    }

    public bool DelJobVacancy(int id)
    {
        var jv = JobVacancies.Find(x => x.Id == id);
        if (jv == null)
            return false;
        JobVacancies.Remove(jv);
        return true;
    }

    public string PrintJobVacancies()
    {
        string result = "";
        foreach (var jv in JobVacancies)
            result +=
                $"[{jv.Id}] {jv.JobTitle}, Salary: {jv.Salary}, Open: {jv.Open}, Worker: {jv.Worker?.Name ?? "vacant"}\n";
        return result;
    }

    public bool DelJobTitle()
    {
        if (JobVacancies.Count == 0)
            return false;
        JobVacancies.RemoveAt(JobVacancies.Count - 1);
        return true;
    }

    public void OpenJobVacancy(JobVacancy jv) => jv.SetOpen(true);

    public bool CloseJobVacancy(int indexJV)
    {
        var jv = JobVacancies.Find(x => x.Id == indexJV);
        if (jv == null)
            return false;
        jv.SetOpen(false);
        return true;
    }

    public void Recruit(int indexJV, Person w)
    {
        var jv = JobVacancies.Find(x => x.Id == indexJV);
        if (jv != null)
            jv.SetWorker(w);
    }

    public void Dismiss(int indexJV, string reason)
    {
        var jv = JobVacancies.Find(x => x.Id == indexJV);
        if (jv != null)
            jv.SetWorker(null);
    }
}

public class Faculty : Organization
{
    protected List<Department> Departments = new List<Department>();

    public Faculty()
        : base() { }

    public Faculty(string name)
        : base(name) { }

    public Faculty(Faculty other)
        : base(other)
    {
        foreach (var d in other.Departments)
            Departments.Add(new Department(d));
    }

    public void AddDepartment(Department d) => Departments.Add(d);

    public void DelDepartment(int id) => Departments.Find(x => x.Id == id)?.SetName(null);

    public void UpdDepartment(int id, string newName) =>
        Departments.Find(x => x.Id == id)?.SetName(newName);

    public bool VerDepartment(int id) => Departments.Exists(x => x.Id == id);

    public List<Department> GetDepartments() => Departments;

    // public override void PrintInfo()
    // {
    //     Console.WriteLine($"Departments: {Departments.Count}");
    //     foreach (var d in Departments)
    //         d.PrintInfo();
    // }

    public new void PrintInfo()
    {
        Console.WriteLine($"Departments: {Departments.Count}");
        foreach (var d in Departments)
            d.PrintInfo();
    }
    // создать класс производный университет
    // создать ссылку на класс организация
}

public class University : Organization
{
    protected List<Faculty> Faculties = new List<Faculty>();

    public University()
        : base() { }

    public University(string name)
        : base(name) { }

    public University(University other)
        : base(other)
    {
        foreach (var f in other.Faculties)
            Faculties.Add(new Faculty(f));
    }

    public void AddFaculty(Faculty f) => Faculties.Add(f);

    public void DelFaculty(int id) => Faculties.Find(x => x.Id == id)?.PrintInfo();

    public Faculty GetFacultyById(int id) => Faculties.Find(x => x.Id == id);

    public List<Faculty> GetFaculties() => Faculties;

    public override void PrintInfo()
     {
        Console.WriteLine("University Method");
         Console.WriteLine($"Faculties: {Faculties.Count}");
         foreach (var f in Faculties)
             f.PrintInfo();
     }

    /*public new void PrintInfo()
    {
        Console.WriteLine("University Method");
        Console.WriteLine($"Faculties: {Faculties.Count}");
        foreach (var f in Faculties)
            f.PrintInfo();
    }*/

    public class Program
    {
        public static void Main()
        {
            Organization organization = new Organization();
            organization.PrintInfo();

            University university = new University();
            university.PrintInfo();

               Organization xxx = (Organization) university;
            xxx.PrintInfo();

           
            // Console.WriteLine("=== ДЕМОНСТРАЦИЯ РАБОТЫ СИСТЕМЫ ===\n");

            // // 1. Создаем людей
            // Console.WriteLine("1. СОЗДАЕМ ЛЮДЕЙ:");
            // Person alice = new Person("Alice", 25);
            // Person bob = new Person("Bob", 30);
            // alice.PrintInfo();
            // bob.PrintInfo();
            // Console.WriteLine();

            // // 2. Создаем отделы
            // Console.WriteLine("2. СОЗДАЕМ ОТДЕЛЫ:");
            // Department d1 = new Department("Mathematics");
            // Department d2 = new Department("Physics");
            // d1.PrintInfo();
            // d2.PrintInfo();
            // Console.WriteLine();

            // // 3. Создаем факультет и добавляем отделы
            // Console.WriteLine("3. СОЗДАЕМ ФАКУЛЬТЕТ:");
            // Faculty f1 = new Faculty("Science");
            // f1.AddDepartment(d1);
            // f1.AddDepartment(d2);
            // f1.PrintInfo();
            // Console.WriteLine();

            // // 4. Создаем вакансии
            // Console.WriteLine("4. СОЗДАЕМ ВАКАНСИИ:");
            // JobVacancy jv1 = new JobVacancy("Professor", 5000, "PhD Required");
            // JobVacancy jv2 = new JobVacancy("Lecturer", 3000, "Master Degree");
            // jv1.SetOpen(true);
            // jv2.SetOpen(true);
            // jv1.PrintInfo();
            // jv2.PrintInfo();
            // Console.WriteLine();

            // 5. Создаем университет
            Console.WriteLine("5. СОЗДАЕМ УНИВЕРСИТЕТ:");
            University uni = new University("Tech University");
            Organization org1 = (Organization)uni;
            org1.PrintInfo();

            Organization org2 = new University("Econom University");
            org2.PrintInfo();

            // uni.AddFaculty(f1);
            // uni.AddJobVacancy(jv1);
            // uni.AddJobVacancy(jv2);
            // uni.PrintInfo();
            // Console.WriteLine();

            // // 6. Нанимаем сотрудников
            // Console.WriteLine("6. НАНИМАЕМ СОТРУДНИКОВ:");
            // Console.WriteLine("Нанимаем Alice на должность Professor...");
            // uni.Recruit(jv1.Id, alice);
            // Console.WriteLine("Нанимаем Bob на должность Lecturer...");
            // uni.Recruit(jv2.Id, bob);

            // Console.WriteLine("\nСостояние после найма:");
            // uni.PrintInfo();
            // Console.WriteLine();

            // // 7. Показываем всех сотрудников
            // Console.WriteLine("7. ВСЕ СОТРУДНИКИ УНИВЕРСИТЕТА:");
            // var employees = uni.GetEmployees();
            // foreach (var emp in employees)
            // {
            //     emp.PrintInfo();
            // }
            // Console.WriteLine();

            // // 8. Показываем все вакансии
            // Console.WriteLine("8. ВСЕ ВАКАНСИИ:");
            // Console.WriteLine(uni.PrintJobVacancies());

            // // 9. Удаляем отдел и вакансию
            // Console.WriteLine("9. УДАЛЯЕМ ОТДЕЛ И ВАКАНСИЮ:");
            // Console.WriteLine("Удаляем отдел Mathematics...");
            // f1.DelDepartment(d1.Id);
            // Console.WriteLine("Удаляем вакансию Lecturer...");
            // uni.DelJobVacancy(jv2.Id);

            // Console.WriteLine("\n10. РЕЗУЛЬТАТ ПОСЛЕ УДАЛЕНИЯ:");
            // uni.PrintInfo();

            // // 11. Финальная информация
            // Console.WriteLine("\n11. ФИНАЛЬНАЯ СТАТИСТИКА:");
            // Console.WriteLine($"Всего факультетов: {uni.GetFaculties().Count}");
            // Console.WriteLine($"Всего вакансий: {uni.GetJobVacancies().Count}");
            // Console.WriteLine($"Всего сотрудников: {uni.GetEmployees().Count}");

            // var faculties = uni.GetFaculties();
            // foreach (var faculty in faculties)
            // {
            //     Console.WriteLine(
            //         $"Факультет '{faculty.Name}': {faculty.GetDepartments().Count} отделов"
            //     );
            // }
        }
    }
}
