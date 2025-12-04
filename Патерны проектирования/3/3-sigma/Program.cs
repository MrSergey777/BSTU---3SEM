using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
public class JobVacancy
{
    public List<string> JobTitle = new List<string>();
    protected List<JobVacancy> JobVacancies = new List<JobVacancy>();
}
public class Person : JobVacancy
{
    string name;
}
public class Employee
{
    JobVacancy job = new JobVacancy();
    Person per = new Person();
    public void addjob(JobVacancy job) { this.job = job; }
    public void addPerson(Person p) { this.per = p; }
}
public class Organisation : Person
{
    public int Id { get; private set; }
    public string name { get; protected set; }
    public Type shortname {  get; protected set; } 
    public string address { get; protected set; }
    public DateTime timeStamp { get; protected set; }
    public Organisation() { this.Id = 12; timeStamp = DateTime.Now; }
    public Organisation(Organisation other)
    {
        this.name  = other.name;    
        this.Id = other.Id; 
        this.shortname = other.shortname;
        this.address = other.address;   
        this.timeStamp = other.timeStamp;   
    }
    public Organisation(string name, Type shortname, string address)
    {
        this.name = name;
        this.address = address;
        this.shortname = shortname;
        this.timeStamp = DateTime.Now;
    }
    public void PrintInfo()
    {
        Console.WriteLine("Id" +  this.Id);
        Console.WriteLine("Имя" + this.name);
        Console.WriteLine("Name" + this.shortname);
        Console.WriteLine("Адрес" + this.address);
        Console.WriteLine("Время" + this.timeStamp);
    }
}
public class University : Organisation
{
    protected List <Faculty> faculties = new List<Faculty>();
    University(University other) {
        this.name = other.name;
        this.shortname = other.shortname;
        this.address = other.address;
        this.timeStamp = other.timeStamp;
    }
    University(string name, Type shortname, string address) {
        this.name = name;
        this.address = address;
        this.shortname = shortname;
        this.timeStamp = DateTime.Now;
    }
    public int addFaculty(Faculty other) { faculties.Add(other); return faculties.IndexOf(other); }
    public  bool delFaculty(int index) { faculties.RemoveAt(index); return true;}
    public bool updFaculty(Faculty other) { int index = faculties.IndexOf(other); faculties[index] = other;  return true; }
    private bool verFaculty(int index) { return faculties[index] != null; }
    public List<Faculty> GetFaculties() { return faculties; }
    public void PrintInfo()
    {
        Console.WriteLine(this.faculties);
        Console.WriteLine("Имя" + this.name);
        Console.WriteLine("Name" + this.shortname);
        Console.WriteLine("Адрес" + this.address);
        Console.WriteLine("Время" + this.timeStamp);
    }
    public List<JobVacancy> getJobVacancies() { return JobVacancies; }
    public int addJobTitle(string JobTitle) { this.JobTitle.Add(JobTitle); return JobTitle.IndexOf(JobTitle); }
    public bool delJobTitle(int index) { JobTitle.RemoveAt(index); return true;}
    public Employee recruit(JobVacancy n1, Person n2) { Employee em = new Employee(); em.addjob(n1); em.addPerson(n2); return em; }
    public void dismiss(int i, string reason) { }
}
public class Faculty
{

}