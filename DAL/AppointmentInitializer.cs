using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PurdueUniversity.Models;

namespace PurdueUniversity.DAL
{
    public class AppointmentInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AppointmentContext>
    {
        protected override void Seed(AppointmentContext context)
        {
            var students = new List<Student>
            {
                new Student {UserName="student1", Password=User.CalculateHash("student1"), FirstName="Student1"},
                new Student {UserName="student2", Password=User.CalculateHash("student2"), FirstName="Student2"},
                new Student {UserName="student3", Password=User.CalculateHash("student3"), FirstName="Student3"},
                new Student {UserName="student4", Password=User.CalculateHash("student4"), FirstName="Student4"},
                new Student {UserName="student5", Password=User.CalculateHash("student5"), FirstName="Student5"}
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            var counselors = new List<Counselor>
            {
                new Counselor {UserName="counselor1", Password=User.CalculateHash("counselor1"), FirstName="Counselor1"},
                new Counselor {UserName="counselor2", Password=User.CalculateHash("counselor2"), FirstName="Counselor2"},
                new Counselor {UserName="counselor3", Password=User.CalculateHash("counselor3"), FirstName="Counselor3"}
            };

            counselors.ForEach(c => context.Counselors.Add(c));
            context.SaveChanges();

            /*        var admins = new List<Admin>
                    {
                        new Admin {FirstName="admin1", LastName="ln1", UserName="admin1" },
                        new Admin {FirstName="admin2", LastName="ln1", UserName="admin2" }
                    };*/

          var appointments = new List<Appointment>
            {
                new Appointment {StudentID=null, CounselorID =null}
            };

            appointments.ForEach(a => context.Appointments.Add(a));
            context.SaveChanges();

            

        }
    }
}