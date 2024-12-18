using System;
using System.Collections.Generic;

// Абстрактний клас користувача
abstract class User
{
    public string Name { get; }
    public string Email { get; }

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public abstract void Login();

    // Фінальний метод реєстрації
    public void Register()
    {
        Console.WriteLine($"Користувач {Name} зареєстрований");
    }
}

// Інтерфейс курсу
interface ICourse
{
    void AddMaterial(string material);
    void RemoveMaterial(string material);
}

// Клас онлайн-курсу
class OnlineCourse : ICourse
{
    private string courseName;
    private List<string> materials;
    private List<Student> enrolledStudents;

    public OnlineCourse(string courseName)
    {
        this.courseName = courseName;
        materials = new List<string>();
        enrolledStudents = new List<Student>();
    }

    public void AddMaterial(string material)
    {
        materials.Add(material);
        Console.WriteLine($"Додано матеріал: {material} до курсу {courseName}");
    }

    // Перевантаження методу
    public void AddMaterial(string material, bool isImportant)
    {
        AddMaterial(material);
        if (isImportant)
        {
            Console.WriteLine("Позначено як важливий матеріал");
        }
    }

    public void RemoveMaterial(string material)
    {
        materials.Remove(material);
        Console.WriteLine($"Видалено матеріал: {material} з курсу {courseName}");
    }

    public void EnrollStudent(Student student)
    {
        enrolledStudents.Add(student);
        Console.WriteLine($"{student.Name} записаний на курс {courseName}");
    }
}

// Клас студента
class Student : User
{
    private List<OnlineCourse> courses;
    private List<Assignment> assignments;

    public Student(string name, string email) : base(name, email)
    {
        courses = new List<OnlineCourse>();
        assignments = new List<Assignment>();
    }

    public override void Login()
    {
        Console.WriteLine($"Студент {Name} увійшов у систему");
    }

    public void EnrollCourse(OnlineCourse course)
    {
        courses.Add(course);
        course.EnrollStudent(this);
    }

    public void SubmitAssignment(Assignment assignment)
    {
        assignments.Add(assignment);
        assignment.Submit(this);
    }
}

// Клас викладача
class Teacher : User
{
    private List<OnlineCourse> teachingCourses;

    public Teacher(string name, string email) : base(name, email)
    {
        teachingCourses = new List<OnlineCourse>();
    }

    public override void Login()
    {
        Console.WriteLine($"Викладач {Name} увійшов у систему");
    }

    public OnlineCourse CreateCourse(string courseName)
    {
        var course = new OnlineCourse(courseName);
        teachingCourses.Add(course);
        return course;
    }

    public void EvaluateAssignment(Assignment assignment, int grade)
    {
        assignment.Evaluate(grade);
    }
}

// Клас завдання
class Assignment
{
    public string Title { get; }
    private AssignmentStatus status;
    private Student student;
    private int grade;

    public Assignment(string title)
    {
        Title = title;
        status = AssignmentStatus.Created;
    }

    public void Submit(Student student)
    {
        this.student = student;
        status = AssignmentStatus.Submitted;
        Console.WriteLine($"Завдання \"{Title}\" подане студентом {student.Name}");
    }

    public void Evaluate(int grade)
    {
        this.grade = grade;
        status = grade >= 60 ? AssignmentStatus.Passed : AssignmentStatus.Failed;
        Console.WriteLine($"Завдання \"{Title}\" оцінене на {grade} балів");
    }

    public enum AssignmentStatus
    {
        Created, Submitted, Passed, Failed
    }
}

// Клас адміністратора
class Administrator : User
{
    public Administrator(string name, string email) : base(name, email) { }

    public override void Login()
    {
        Console.WriteLine($"Адміністратор {Name} увійшов у систему");
    }

    public void GenerateReport(OnlineCourse course)
    {
        Console.WriteLine($"Звіт по курсу: {course}");
    }
}

// Демонстраційний клас
class Program
{
    static void Main()
    {
        // Створення користувачів
        var teacher = new Teacher("Іван Петров", "ivan@school.com");
        var student = new Student("Марія Іванова", "maria@school.com");
        var admin = new Administrator("Олена Сидорова", "olena@school.com");

        // Створення курсу
        var javaCourse = teacher.CreateCourse("Основи Java");
        javaCourse.AddMaterial("Вступ до Java", true);
        javaCourse.AddMaterial("ООП в Java");

        // Реєстрація студента
        student.Register();
        student.EnrollCourse(javaCourse);

        // Створення та подання завдання
        var assignment = new Assignment("Перша програма на Java");
        student.SubmitAssignment(assignment);

        // Оцінювання завдання
        teacher.EvaluateAssignment(assignment, 85);

        // Логін користувачів
        teacher.Login();
        student.Login();
        admin.Login();
    }
}
