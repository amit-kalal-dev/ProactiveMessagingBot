using BotWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotWebApi.Data
{
    public class MockStudentRepo : IStudentRepo
    {
        public void CreateStudent(Student std)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            var students = new List<Student>
            {
             new Student { Id = 0, Name = "Ron" },
             new Student { Id = 1, Name = "Sam" },
             new Student { Id = 2, Name = "Dev" },
            };
        return students;
        }

        

        public Student GetStudentById(int id)
        {
            return new Student { Id = 0, Name = "Ron" };
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
