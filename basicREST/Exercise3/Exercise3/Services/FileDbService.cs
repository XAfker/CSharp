using Exercise3.Models;
using Exercise3.Repositories;

namespace Exercise3.Services
{
    public interface IFileDbService
    {
        public IEnumerable<Student> Students { get; set; }
        Task SaveChanges();
    }

    public class FileDbService : IFileDbService
    {
        private readonly string _pathToFileDatabase;
        public IEnumerable<Student> Students { get; set; } = new List<Student>();
        public FileDbService(IConfiguration configuration)
        {
            _pathToFileDatabase = configuration.GetConnectionString("Default") ?? throw new ArgumentNullException(nameof(configuration));
            Initialize();
        }

        private void Initialize()
        {
            if (!File.Exists(_pathToFileDatabase))
            {
                return;
            }
            var lines = File.ReadLines(_pathToFileDatabase);

            var students = new List<Student>();

            foreach (var line in lines)
            {
                var values = line.Split(',');

                var student = new Student
                {
                    IndexNumber = values[0],
                    FirstName = values[1],
                    LastName = values[2],
                    BirthDate = values[3],
                    StudyName = values[4],
                    StudyMode = values[5],
                    Email = values[6],
                    FathersName = values[7],
                    MothersName = values[8]
                };

                students.Add(student);
            }

            Students = students;
        }

        public async Task SaveChanges()
        {
            try
            {
                var studentLines = Students.Select(s => $"{s.IndexNumber},{s.FirstName},{s.LastName},{s.BirthDate},{s.StudyName},{s.StudyMode},{s.Email},{s.FathersName},{s.MothersName}");
                await File.WriteAllLinesAsync(_pathToFileDatabase, studentLines);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while saving changes to the database.", e);
            }
        }

    }
}
