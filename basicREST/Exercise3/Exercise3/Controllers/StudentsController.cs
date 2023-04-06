using Exercise3.Models;
using Exercise3.Models.DTOs;
using Exercise3.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exercise3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsRepository _studentsRepository;
        public StudentsController(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            { 
                var students = _studentsRepository.GetStudents();
                if (students is null)
                {
                    return NotFound();
                }
                
                return Ok(students);
            }
            catch (Exception e)
            {
                return Problem();
            }
        }

        [HttpGet("{index}")]
        public async Task<IActionResult> Get(string index)
        {
            try
            {
                var student = _studentsRepository.GetStudents()
                    .FirstOrDefault(s => s.IndexNumber == index);
                if (student is null)
                {
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception e)
            {
                return Problem();
            }
        }

        [HttpPut("{index}")]
        public async Task<IActionResult> Put(string index, StudentPUT newStudentData)
        {
            try
            { 
                var student = _studentsRepository.GetStudents().Where(e => e.IndexNumber == index).FirstOrDefault();
            if (student is null)
            {
                return NotFound();
            }

            _studentsRepository.UpdateStudent(
                student,
                new Models.Student()
                {
                    FirstName = newStudentData.FirstName,
                    LastName = newStudentData.LastName,
                    BirthDate = newStudentData.BirthDate,
                    StudyName = newStudentData.StudyName,
                    StudyMode = newStudentData.StudyMode,
                    Email = newStudentData.Email,
                    FathersName = newStudentData.FathersName,
                    MothersName = newStudentData.MothersName
                }
                );
            return Ok();
            }
            catch (Exception e)
            {
                return Problem();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(StudentPOST newStudent)
        {
            try
            {
                
                var student = new Student
                {
                    FirstName = newStudent.FirstName,
                    LastName = newStudent.LastName,
                    IndexNumber = newStudent.IndexNumber,
                    BirthDate = newStudent.BirthDate,
                    StudyName = newStudent.StudyName,
                    StudyMode = newStudent.StudyMode,
                    Email = newStudent.Email,
                    FathersName = newStudent.FathersName,
                    MothersName = newStudent.MothersName
                };
                
                var existingStudent = _studentsRepository.GetStudents().Where(e => e.IndexNumber == student.IndexNumber).FirstOrDefault();
                if (existingStudent != null)
                {
                    return Conflict();
                }

                if (student.FirstName == null || student.LastName == null || student.IndexNumber == null
                    || student.BirthDate == null || student.StudyName == null || student.StudyMode == null
                    || student.Email == null || student.FathersName == null || student.MothersName == null)
                {
                    return Conflict();
                }

                _studentsRepository.AddStudent(student);

                return CreatedAtAction(nameof(Get), new { index = student.IndexNumber }, student);
            }
            catch (Exception e)
            {
                return Problem();
            }
        }

        [HttpDelete("{index}")]
        public async Task<IActionResult> Delete(string index)
        {
            try
            {
                var student = _studentsRepository.GetStudents().FirstOrDefault(s => s.IndexNumber == index);
                if (student == null)
                {
                    return NotFound($"Student with index {index} not found");
                }

                await _studentsRepository.DeleteStudent(student);

                return Ok($"Student with index {index} has been deleted");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
