public interface IStudentBL
{
    Student? GetStudentById(int id);
    IEnumerable<Student?> GetAllStudents();

    bool UpdateStudent(Student student);

    bool DeleteStudentById(int id);

    bool InsertStudent(Student student);
}