using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ILogger<StudentsController> _logger;

    private IStudentBL _studentBL {get; set; }

    public StudentsController(ILogger<StudentsController> logger,
    IStudentBL studentBL)
    {
        _logger = logger;
        _studentBL = studentBL;
    }

    [HttpGet(Name = "GetAllStudents")]
    public IEnumerable<Student> GetAllStudents()
    {
        List<Student?> list = _studentBL.GetAllStudents().ToList();
        return list;
    }


    [HttpGet("{id}")]
    public ActionResult<Student> GetStudentById(int id)
    {
        var student = _studentBL.GetStudentById(id);

        if (student == null)
        {
            return NotFound("El estudiante no Existe");
        }

        return student;
    }

    [HttpPut(Name = "UpdateStudent")]
    public IActionResult UpdateStudent([FromBody] Student Student)
    {

        var studentExists = _studentBL.GetStudentById(Student.Id);

        if(studentExists == null){
            return NotFound("El estudiante no Existe");
        }

        var existingStudent = _studentBL.UpdateStudent(Student);

        if (existingStudent == false)
        {
            return NotFound();
        }
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteStudent(int id)
    {

        var studentExists = _studentBL.GetStudentById(id);

        if(studentExists == null){
            return NotFound("El estudiante no Existe");
        }

        var student = _studentBL.DeleteStudentById(id);

        if (student == false)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Student> CreateStudent([FromBody] Student student)
    {

        var studentExists = _studentBL.GetStudentById(student.Id);

        if(studentExists != null){
            return Conflict("El estudiante ya existe.");
        }
        var response = _studentBL.InsertStudent(student);


        if (response == false)
        {
            return NotFound();
        }
        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
    }

}
