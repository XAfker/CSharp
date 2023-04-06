using Exercise3.Models;
using Exercise3.Services;

namespace Exercise3.Repositories
{
    public interface IStudentsRepository
    {
        IEnumerable<Student> GetStudents();
        Task DeleteStudent (Student student);
        Task AddStudent(Student student);
        Task UpdateStudent(Student student, Student newData);
    }

    public class StudentsRepository : IStudentsRepository
    {

        private readonly IFileDbService _fileDbService;

        public StudentsRepository(IFileDbService fileDbService)
        {
            _fileDbService = fileDbService;
        }

        public IEnumerable<Student> GetStudents()
        {
            return _fileDbService.Students;
        }

        public async Task DeleteStudent(Student student)
        {
            try
            {
                var students = GetStudents().ToList();;
                var index = students.FindIndex(s => s.IndexNumber == student.IndexNumber);
                if (index == -1)
                {
                    throw new ArgumentException($"Student with Index {student.IndexNumber} does not exist in the database.");
                }
                students.RemoveAt(index);
                _fileDbService.Students = students;
                _fileDbService.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while deleting the student.", e);
            }
        }

        public async Task AddStudent(Student student)
        {
            try
            {
                var students = GetStudents().ToList();
                students.Add(student);
                _fileDbService.Students = students;
                _fileDbService.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while adding the student.", e);
            }
        }

        public async Task UpdateStudent(Student student, Student newData)
        {
            student.FirstName = newData.FirstName;
            student.LastName = newData.LastName;
            student.BirthDate = newData.BirthDate;
            student.StudyName = newData.StudyName;
            student.StudyMode = newData.StudyMode;
            student.Email = newData.Email;
            student.FathersName = newData.FathersName;
            student.MothersName = newData.MothersName;
            
        }
    }
}
