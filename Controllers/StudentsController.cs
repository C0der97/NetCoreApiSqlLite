using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ILogger<StudentsController> _logger;

    private ISqlLiteProvider _sqlLiteProvider {get; set; }

    public StudentsController(ILogger<StudentsController> logger,
    ISqlLiteProvider sqlLiteProvider)
    {
        _logger = logger;
        _sqlLiteProvider = sqlLiteProvider;
    }

    [HttpGet(Name = "GetAllStudents")]
    public IEnumerable<Student> GetAllStudents()
    {
        List<Student?> list = new SqlLiteConnector().GetAllStudents().ToList();
        return list;
    }


    [HttpGet("{id}")]
    public ActionResult<Student> GetStudentById(int id)
    {
        var student = new SqlLiteConnector().GetStudentById(id);

        if (student == null)
        {
            return NotFound("El estudiante no Existe");
        }

        return student;
    }

    [HttpPut(Name = "UpdateStudent")]
    public IActionResult UpdateStudent([FromBody] Student Student)
    {

        var studentExists = new SqlLiteConnector().GetStudentById(Student.Id);

        if(studentExists == null){
            return NotFound("El estudiante no Existe");
        }

        var existingStudent = new SqlLiteConnector().UpdateStudent(Student);

        if (existingStudent == false)
        {
            return NotFound();
        }
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteStudent(int id)
    {

        var studentExists = new SqlLiteConnector().GetStudentById(id);

        if(studentExists == null){
            return NotFound("El estudiante no Existe");
        }

        var student = new SqlLiteConnector().DeleteStudentById(id);

        if (student == false)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Student> CreateStudent([FromBody] Student student)
    {

        var studentExists = new SqlLiteConnector().GetStudentById(student.Id);

        if(studentExists != null){
            return Conflict("El estudiante ya existe.");
        }
        var response = new SqlLiteConnector().InsertStudent(student);


        if (response == false)
        {
            return NotFound();
        }
        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
    }

}
