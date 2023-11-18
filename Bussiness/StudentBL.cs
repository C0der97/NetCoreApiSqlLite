using Microsoft.Data.Sqlite;


public class StudentBL : IStudentBL
{
    private readonly ISqlLiteProvider _sqlLiteProvider;

    private SqliteConnection connectionLite { get; set; }

    public StudentBL(ISqlLiteProvider sqlLiteProvider)
    {
        _sqlLiteProvider = sqlLiteProvider;
        connectionLite = _sqlLiteProvider.GetConnection();
        CreateTable();
    }

    public void CreateTable()
    {
        using SqliteCommand command = connectionLite.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS Students (" +
   " Id INTEGER PRIMARY KEY AUTOINCREMENT," +
    "Identifier Text," +
    "LastName TEXT," +
    "FirstName TEXT," +
    "Degree TEXT," +
    "Age INTEGER" +
");";

        connectionLite.Open();
        command.ExecuteNonQuery();
        connectionLite.Close();
    }

    public Student? GetStudentById(int id)
    {
        using var command = connectionLite.CreateCommand();
        command.CommandText = "SELECT * FROM Students WHERE Id = $Id;";
        command.Parameters.AddWithValue("$Id", id);

        connectionLite.Open();
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            Student student = new Student
            {
                Id = reader.GetInt32(0),
                Identifier = reader.GetString(1),
                LastName = reader.GetString(2),
                FirstName = reader.GetString(3),
                Degree = reader.GetString(4),
                Age = reader.GetInt32(5),
            };
            connectionLite.Close();
            return student;
        }
        connectionLite.Close();
        return null;
    }

    public IEnumerable<Student?> GetAllStudents()
    {
        List<Student> students = new List<Student>();
        using SqliteCommand command = connectionLite.CreateCommand();
        command.CommandText = "SELECT * FROM Students";

        connectionLite.Open();
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Student student = new Student
            {
                Id = reader.GetInt32(0),
                Identifier = reader.GetString(1),
                LastName = reader.GetString(2),
                FirstName = reader.GetString(3),
                Degree = reader.GetString(4),
                Age = reader.GetInt32(5),
            };
            students.Add(student);
        }
        connectionLite.Close();
        return students;
    }

    public bool UpdateStudent(Student student)
    {
        using SqliteCommand command = connectionLite.CreateCommand();
        command.CommandText = "UPDATE Students SET FirstName = $FirstName, LastName = $LastName," +
        "Degree = $Degree,Identifier = $Identifier,Age = $Age WHERE Id = $Id;";
        command.Parameters.AddWithValue("$FirstName", student.FirstName);
        command.Parameters.AddWithValue("$LastName", student.LastName);
        command.Parameters.AddWithValue("$Degree", student.Degree);
        command.Parameters.AddWithValue("$Id", student.Id);
        command.Parameters.AddWithValue("$Identifier", student.Identifier);
        command.Parameters.AddWithValue("$Age", student.Age);
        connectionLite.Open();
        command.ExecuteNonQuery();
        connectionLite.Close();
        return true;
    }

    public bool DeleteStudentById(int id)
    {
        using SqliteCommand command = connectionLite.CreateCommand();
        command.CommandText = "DELETE FROM Students WHERE Id = $Id;";
        command.Parameters.AddWithValue("$Id", id);
        connectionLite.Open();
        command.ExecuteNonQuery();
        connectionLite.Close();
        return true;
    }

    public bool InsertStudent(Student student)
    {
        using var command = connectionLite.CreateCommand();
        command.CommandText = "INSERT INTO Students (FirstName,LastName,Degree,Identifier,Age)" +
        " VALUES ($FirstName, $LastName,$Degree,$Identifier,$Age);";
        command.Parameters.AddWithValue("$FirstName", student.FirstName);
        command.Parameters.AddWithValue("$LastName", student.LastName);
        command.Parameters.AddWithValue("$Degree", student.Degree);
        command.Parameters.AddWithValue("$Identifier", student.Identifier);
        command.Parameters.AddWithValue("$Age", student.Age);

        connectionLite.Open();
        command.ExecuteNonQuery();
        connectionLite.Close();
        return true;
    }


}