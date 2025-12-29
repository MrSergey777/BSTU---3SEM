
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
        this.Name = "None";
        this.Age = 0;
    }

    public Person(Person other)
    {
        this.Name = other.Name;
        this.Age = other.Age;
    }

    public Person(string n = "None", int a = 0)
    {
        this.Name = n;
        this.Age = a;
    }

    public void SetName(string n) => this.Name = n;

    public void SetAge(int a) => this.Age = a;

    public void PrintInfo() => Console.WriteLine($"{this.Name}, Age: {this.Age}");
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
        this.Id = globalId;
        this.Salary = 0;
        this.Requirements = "None";
        this.JobTitle = "None";
        this.Open = false;
    }

    public JobVacancy(JobVacancy other)
    {
        globalId++;
        this.Id = globalId;
        this.Salary = other.Salary;
        this.Requirements = other.Requirements;
        this.JobTitle = other.JobTitle;
        this.Open = other.Open;
        this.Worker = other.Worker;
    }

    public JobVacancy(string jt = "None", int s = 0, string r = "None", bool o = false)
    {
        globalId++;
        this.Id = globalId;
        this.JobTitle = jt;
        this.Salary = s;
        this.Requirements = r;
        this.Open = o;
    }

    public void SetWorker(Person w) => this.Worker = w;

    public void SetSalary(int s) => this.Salary = s;

    public void SetRequirements(string r) => this.Requirements = r;

    public void SetPosition(string jt) => this.JobTitle = jt;

    public void SetOpen(bool o) => this.Open = o;

    public void PrintInfo()
    {
        Console.WriteLine(
            $"[{this.Id}] {this.JobTitle}, Salary: {this.Salary}, Open: {this.Open}, Worker: {this.Worker?.Name ?? "vacant"}"
        );
    }
}


public class Department
{
    private static int globalId = 0;
    public int Id { get; private set; }
    public string Name { get; private set; }

    public Department()
    {
        globalId++;
        this.Id = globalId;
        this.Name = "None";
    }

    public Department(Department other)
    {
        globalId++;
        this.Id = globalId;
        this.Name = other.Name;
    }

    public Department(string name = "None")
    {
        globalId++;
        this.Id = globalId;
        this.Name = name;
    }

    public void SetName(string name) => this.Name = name;

    public void PrintInfo() => Console.WriteLine($"[{this.Id}] Department: {this.Name}");
}

public class Organization
{
    private static int globalId = 0;
    public int Id { get; private set; }
    public string Name { get; protected set; }
    public string ShortName { get; protected set; }
    public string Address { get; protected set; }
    public DateTime TimeStamp { get; protected set; }

    public Organization()
    {
        globalId++;
        this.Id = globalId;
        this.Name = "None";
        this.ShortName = "None";
        this.Address = "None";
        this.TimeStamp = DateTime.Now;
    }

    public Organization(Organization other)
    {
        globalId++;
        this.Id = globalId;
        this.Name = other.Name;
        this.ShortName = other.ShortName;
        this.Address = other.Address;
        this.TimeStamp = DateTime.Now;
    }

    public Organization(
        string name = "None",
        string shortName = "None",
        string address = "None",
        DateTime? dt = null
    )
    {
        globalId++;
        this.Id = globalId;
        this.Name = name;
        this.ShortName = shortName;
        this.Address = address;
        this.TimeStamp = dt ?? DateTime.Now;
    }

    public void SetName(string n) => this.Name = n;

    public void SetAddress(string a) => this.Address = a;

    public void PrintInfo() =>
        Console.WriteLine(
            $"[{this.Id}] {this.Name} ({this.ShortName}) — {this.Address}, {this.TimeStamp}"
        );
}

public class Faculty
{
    private static int globalId = 0;
    public int Id { get; private set; }
    public string Name { get; private set; }

    protected List<Department> Departments = new();

    public Faculty()
    {
        globalId++;
        this.Id = globalId;
        this.Name = "None";
    }

    public Faculty(string name)
    {
        globalId++;
        this.Id = globalId;
        this.Name = name;
    }

    public Faculty(Faculty other)
    {
        globalId++;
        this.Id = globalId;
        this.Name = other.Name;
        this.Departments = new List<Department>();
        foreach (var d in other.Departments)
            this.Departments.Add(new Department(d));
    }

    public void AddDepartment(Department d) => this.Departments.Add(d);

    public void DelDepartment(int id) => this.Departments.Find(x => x.Id == id)?.SetName(null); // пример использования ?.

    public void UpdDepartment(int id, string newName) =>
        this.Departments.Find(x => x.Id == id)?.SetName(newName);

    public bool VerDepartment(int id) => this.Departments.Exists(x => x.Id == id);

    public List<Department> GetDepartments() => this.Departments;

    public void PrintInfo()
    {
        Console.WriteLine(
            $"Faculty [{this.Id}]: {this.Name}, Departments: {this.Departments.Count}"
        );
        foreach (var d in this.Departments)
            d.PrintInfo();
    }
}

public class University
{
    private static int globalId = 0;
    public int Id { get; private set; }
    public string Name { get; private set; }

    protected List<Faculty> Faculties = new();
    protected List<JobVacancy> JobVacancies = new();

    public University()
    {
        globalId++;
        this.Id = globalId;
        this.Name = "None";
    }

    public University(string name)
    {
        globalId++;
        this.Id = globalId;
        this.Name = name;
    }

    public University(University other)
    {
        globalId++;
        this.Id = globalId;
        this.Name = other.Name;

        this.Faculties = new List<Faculty>();
        foreach (var f in other.Faculties)
            this.Faculties.Add(new Faculty(f));

        this.JobVacancies = new List<JobVacancy>();
        foreach (var jv in other.JobVacancies)
            this.JobVacancies.Add(new JobVacancy(jv));
    }

    public void AddFaculty(Faculty f) => this.Faculties.Add(f);

    public void DelFaculty(int id) => this.Faculties.Find(x => x.Id == id)?.PrintInfo();

    public Faculty GetFacultyById(int id) => this.Faculties.Find(x => x.Id == id);

    public List<Faculty> GetFaculties() => this.Faculties;

    public void AddJobVacancy(JobVacancy jv) => this.JobVacancies.Add(jv);

    public void DelJobVacancy(int id) => this.JobVacancies.Find(x => x.Id == id)?.PrintInfo();

    public void OpenJobVacancy(int id) => this.JobVacancies.Find(x => x.Id == id)?.SetOpen(true);

    public void CloseJobVacancy(int id) => this.JobVacancies.Find(x => x.Id == id)?.SetOpen(false);

    public void Recruit(int jvId, Person w) =>
        this.JobVacancies.Find(x => x.Id == jvId)?.SetWorker(w);

    public void PrintInfo()
    {
        Console.WriteLine($"University [{this.Id}]: {this.Name}");
        Console.WriteLine(
            $"Faculties: {this.Faculties.Count}, Job Vacancies: {this.JobVacancies.Count}"
        );
        foreach (var f in this.Faculties)
            f.PrintInfo();
        foreach (var jv in this.JobVacancies)
            jv.PrintInfo();
    }
}

public class Program
{
    public static void Main()
    {
        Person alice = new Person("Alice", 25);
        Person bob = new Person("Bob", 30);

        Department d1 = new Department("Mathematics");
        Department d2 = new Department("Physics");

        Faculty f1 = new Faculty("Science");
        f1.AddDepartment(d1);
        f1.AddDepartment(d2);

        JobVacancy jv1 = new JobVacancy("Professor", 5000, "PhD Required");
        JobVacancy jv2 = new JobVacancy("Lecturer", 3000, "Master Degree");

        University uni = new University("Tech University");
        uni.AddFaculty(f1);
        uni.AddJobVacancy(jv1);
        uni.AddJobVacancy(jv2);

        uni.Recruit(jv1.Id, alice);
        uni.Recruit(jv2.Id, bob);

        uni.PrintInfo();

        // Удаляем по Id с ?.
        f1.DelDepartment(d1.Id);
        uni.DelJobVacancy(jv2.Id);

        Console.WriteLine("\nПосле удаления:");
        uni.PrintInfo();
    }
}
