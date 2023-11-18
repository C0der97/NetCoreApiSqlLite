using Microsoft.Data.Sqlite;

namespace StudentApi.Bussiness
{
    public class StudentBL : IStudentBL
    {
        private readonly ISqlLiteProvider _sqlLiteProvider;

        private SqliteConnection connectionLite { get; set; }

        private readonly ILogger<StudentBL>? _logger;

        public StudentBL(ISqlLiteProvider sqlLiteProvider,
            ILogger<StudentBL> logger)
        {
            _logger = logger;
            _sqlLiteProvider = sqlLiteProvider;
            connectionLite = _sqlLiteProvider.GetConnection();
            CreateTable();
        }

        public StudentBL(ISqlLiteProvider sqlLiteProvider)
        {
            _sqlLiteProvider = sqlLiteProvider;
            connectionLite = _sqlLiteProvider.GetConnection();
            CreateTable();
        }


        public void CreateTable()
        {
            try
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
            catch (SqliteException sqlException)
            {
                _logger?.LogError(sqlException, "CreateTable: " + sqlException.Message);
            }
        }

        public Student? GetStudentById(int id)
        {
            try
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
            }
            catch (SqliteException sqlException)
            {
                _logger?.LogError(sqlException, "GetStudentById: " + sqlException.Message);
                return null;
            }
            return null;
        }

        public IEnumerable<Student?> GetAllStudents()
        {
            List<Student> students = [];
            try
            {

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
            }
            catch (SqliteException sqlException)
            {
                _logger?.LogError(sqlException, "GetAllStudents: " + sqlException.Message);
                return students;
            }
            return students;
        }

        public bool UpdateStudent(Student student)
        {
            try
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
            }
            catch (SqliteException sqlException)
            {
                _logger?.LogError(sqlException, "UpdateStudent: " + sqlException.Message);
                return false;
            }
            return true;
        }

        public bool DeleteStudentById(int id)
        {
            try
            {
                using SqliteCommand command = connectionLite.CreateCommand();
                command.CommandText = "DELETE FROM Students WHERE Id = $Id;";
                command.Parameters.AddWithValue("$Id", id);
                connectionLite.Open();
                command.ExecuteNonQuery();
                connectionLite.Close();
            }
            catch (SqliteException sqlException)
            {
                _logger?.LogError(sqlException, "DeleteStudentById: " + sqlException.Message);
                return false;
            }
            return true;
        }

        public bool InsertStudent(Student student)
        {
            try
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
            }
            catch (SqliteException sqlException)
            {
                _logger?.LogError(sqlException, "InsertStudent: " + sqlException.Message);
                return false;
            }

            return true;
        }


    }
}