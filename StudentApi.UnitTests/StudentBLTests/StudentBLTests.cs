using Microsoft.Data.Sqlite;
using Moq;
using StudentApi.Bussiness;

namespace StudentsApi.UnitTests.StudentBLTests
{
    public class StudentBLTests
    {

        private readonly Mock<ISqlLiteProvider> _providerMock;

        private readonly SqliteConnection _connectionLite;

        private readonly StudentBL _studentBL;

        public StudentBLTests()
        {
            // Arrange
            _providerMock = new Mock<ISqlLiteProvider>();
            _connectionLite = new SqliteConnection("Data Source=StudentsLite.db");
            DeleteTableStudents();
            _providerMock.Setup(provider => provider.GetConnection()).Returns(_connectionLite);
            _studentBL = new StudentBL(_providerMock.Object);
        }


        [Fact]
        public void GetStudentById_ReturnsCorrectStudent()
        {
            InsertStudent();

            var result = _studentBL.GetStudentById(1);

            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]

        public void InsertStudent_ReturnSucessOperation()
        {
            bool wasInsertedCorrectly = _studentBL.InsertStudent(new Student
            {
                FirstName = "Evelin",
                LastName = "Chan",
                Degree = "Art",
                Identifier = "ARS123",
                Age = 45
            });
            Assert.True(wasInsertedCorrectly);
        }

        [Fact]

        public void DeleteStudentById_ReturnSucessOperation()
        {
            InsertStudent();

            bool result = _studentBL.DeleteStudentById(1);
            Assert.True(result);
        }


        [Fact]

        public void GetAllStudent_ReturnSucessOperation()
        {
            InsertStudent();
            InsertStudent();
            IEnumerable<Student?> result = _studentBL.GetAllStudents();

            Assert.NotNull(result);
            Assert.True(result.Count() > 1);
        }

        [Fact]

        public void UpdateStudent_ReturnSucessOperation()
        {
            InsertStudent();

            Student? result = _studentBL.GetStudentById(1);
            bool resultUpdate = false;


            if (result != null)
            {
                result.LastName = "Miranda";
                resultUpdate = _studentBL.UpdateStudent(result);

            }
            Assert.True(resultUpdate);
            Assert.Equal("Miranda", result?.LastName);

        }

        public bool InsertStudent()
        {
          return  _studentBL.InsertStudent(new Student
            {
                FirstName = "John",
                LastName = "Doe",
                Degree = "Computer Science",
                Identifier = "CS123",
                Age = 25
            });
        }

        internal void DeleteTableStudents()
        {
            using SqliteCommand command = _connectionLite.CreateCommand();
            command.CommandText = "DROP TABLE IF EXISTS Students;";
            _connectionLite.Open();
            command.ExecuteNonQuery();
            _connectionLite.Close();
        }

    }
}
