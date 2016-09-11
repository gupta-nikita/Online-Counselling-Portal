using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PurdueUniversity.Models;
using PurdueUniversity.DAL;
using System.Net;

namespace PurdueUniversity.Controllers
{
    public class HomeController : Controller
    {
        private AppointmentContext db = new AppointmentContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "UserName,Password")] User user)
        {
            User loggedInUser = null;
            if (ModelState.IsValid) {

                user.Password = PurdueUniversity.Models.User.CalculateHash(user.Password);
                if (user.UserName == "admin" && user.Password == PurdueUniversity.Models.User.CalculateHash("admin"))
                {
                    return RedirectToAction("Admin");
                }

             
                loggedInUser = db.Students.Where(s => s.UserName == user.UserName).FirstOrDefault();
                if (loggedInUser == null || user.Password != loggedInUser.Password)
                {
                    loggedInUser = db.Counselors.Where(c => c.UserName == user.UserName).FirstOrDefault();
                    if (loggedInUser == null || user.Password != loggedInUser.Password)
                    {
                        // return HttpNotFound();
                        ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                    }
                    else
                    {
                        Session["userId"] = loggedInUser.ID;
                        return RedirectToAction("Counselor");
                    }
                }
                else
                {
                    Session["userId"] = loggedInUser.ID;
                    return RedirectToAction("Student");
                }
            }
            
            return View(loggedInUser);
        }

        public ActionResult Student()
        {
            Appointment appointment = new Appointment();
            return View(appointment);   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Student([Bind(Include = "Date,Time,Text")] Appointment apt)
        {
            if (ModelState.IsValid)
            {
                var studentId = Session["userId"];
                apt.StudentID = (int)studentId;
                apt.CounselorID = (int?)null;
                db.Appointments.Add(apt);
                db.SaveChanges();
            }

            return View();
        }

        public ActionResult counselor()
        {
            
            int counselorId = (int)Session["userId"];

            Counselor c = db.Counselors.Find(counselorId);
            HashSet<Appointment> counselorAppointment = (HashSet<Appointment>)c.Appointments;

            List<Appointment> appointments = db.Appointments.Where(a => a.Alloted == false).ToList();
    
             foreach( var item in appointments)
            {
                counselorAppointment.Add(item);
            }
            
            return View(counselorAppointment.ToList());
        }

        public ActionResult Update(int? appointmentId)
        {
            if (appointmentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var counselorId = Session["userId"];
            Appointment apt = db.Appointments.Find(appointmentId);

            db.Appointments.Attach(apt);
            apt.Alloted = true;
            apt.CounselorID = (int?)counselorId;
            db.SaveChanges();
            return RedirectToAction("Counselor", "Home");
        }

        public ActionResult Admin()
        {
            List<Appointment> appointments = db.Appointments.ToList();
            return View(appointments);
        }

        [HttpPost]
        public ActionResult Admin(string tbAuto)
        {
            Student student = db.Students.Where(s => s.FirstName.Equals(tbAuto)).FirstOrDefault();
            List<Appointment> appointments = db.Appointments.Where(a => a.StudentID == student.ID).ToList();
            return View(appointments);
        }
            
        public JsonResult AutoCompleteSuggestions(string searchString)
        {
            var suggestions = db.Students.ToList();
            var namelist = suggestions.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower())).ToList();
            List<string> listOfNames = new List<string>();
            foreach (Student s in namelist) {
                listOfNames.Add(s.FirstName);
            }
            return Json(listOfNames, JsonRequestBehavior.AllowGet);
        }

    }
}