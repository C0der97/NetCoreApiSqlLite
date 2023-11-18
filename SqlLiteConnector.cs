using Microsoft.Data.Sqlite;

public class SqlLiteConnector
{

private SqliteConnection connectionLite { get; set; }

public SqlLiteConnector()
{
    //Connect();
}


public void Connect(){
        connectionLite = new SqliteConnection("Data Source=StudentsLite.db");
        connectionLite.Open();
        //connectionLite.Close();

            var command = connectionLite.CreateCommand();
            command.CommandText =
            @"
            SELECT StudentId, LastName, FirstName, Degree
            FROM Student;
                    ";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var name = reader.GetString(0);

            Console.WriteLine($"Hello, {name}!");
        }
    }

public Student? GetStudentById(int id){
       connectionLite = new SqliteConnection("Data Source=StudentsLite.db");
       using var command = connectionLite.CreateCommand();
        command.CommandText = "SELECT * FROM Student WHERE StudentId = $Id;";
        command.Parameters.AddWithValue("$Id", id);

        connectionLite.Open();
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var student = new Student
            {
                Id = reader.GetInt32(0),
                LastName = reader.GetString(1),
                FirstName = reader.GetString(2),
                Degree = reader.GetString(3)
            };
            connectionLite.Close();
            return student;
        }
        connectionLite.Close();
        return null;
}

public IEnumerable<Student?> GetAllStudents(){

var students = new List<Student>();

       connectionLite = new SqliteConnection("Data Source=StudentsLite.db");
       using var command = connectionLite.CreateCommand();
        command.CommandText = "SELECT * FROM Student";

        connectionLite.Open();
        using var reader = command.ExecuteReader();
     while (reader.Read())
        {
            var student = new Student
            {
                Id = reader.GetInt32(0),
                LastName = reader.GetString(1),
                FirstName = reader.GetString(2),
                Degree = reader.GetString(3)
            };
            students.Add(student);
        }
        connectionLite.Close();
        return students;
}


    public bool UpdateStudent(Student student)
    {
       connectionLite = new SqliteConnection("Data Source=StudentsLite.db");
        using var command = connectionLite.CreateCommand();
        command.CommandText = "UPDATE Student SET FirstName = $FirstName, LastName = $LastName,Degree = $Degree WHERE StudentId = $Id;";
        command.Parameters.AddWithValue("$FirstName", student.FirstName);
        command.Parameters.AddWithValue("$LastName", student.LastName);
        command.Parameters.AddWithValue("$Degree", student.Degree);
        command.Parameters.AddWithValue("$Id", student.Id);


        connectionLite.Open();
        command.ExecuteNonQuery();
        connectionLite.Close();
        return true;
    }

   public bool DeleteStudentById(int id)
    {
       connectionLite = new SqliteConnection("Data Source=StudentsLite.db");
        using var command = connectionLite.CreateCommand();
        command.CommandText = "DELETE FROM Student WHERE StudentId = $Id;";
        command.Parameters.AddWithValue("$Id", id);
        connectionLite.Open();
        command.ExecuteNonQuery();
        connectionLite.Close();
        return true;
    }

    public bool InsertStudent(Student student)
    {
       connectionLite = new SqliteConnection("Data Source=StudentsLite.db");
        using var command = connectionLite.CreateCommand();
        command.CommandText = "INSERT INTO Student (StudentId, FirstName,LastName,Degree) VALUES ($Id,$FirstName, $LastName,$Degree);";
        command.Parameters.AddWithValue("$FirstName", student.FirstName);
        command.Parameters.AddWithValue("$LastName", student.LastName);
        command.Parameters.AddWithValue("$Degree", student.Degree);
        command.Parameters.AddWithValue("$Id", student.Id);

        connectionLite.Open();
        command.ExecuteNonQuery();
        connectionLite.Close();
        return true;
    }

}