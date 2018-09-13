using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProgressTracker.Models;

namespace ProgressTracker.Controllers
{
    public class StudentsController : Controller
    {
        private ProgressTrackerEntities db = new ProgressTrackerEntities();
        public ActionResult Choose()
        {
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email");
                return View();
            }
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Choose([Bind(Include = "StudentNumber,Course,StudyYear,ProjectName")] Student student)
        {
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.UserID);
                return View(student);
            }
        }

        //GET: ProjectSupervisors/Edit/5
        public ActionResult Select(string id, string StudentNumber)
        {
            string studID = StudentNumber;
            using (var db = new ProgressTrackerEntities())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProjectSupervisor supervisor = db.ProjectSupervisors.Find(id);
                Student student = db.Students.Find(id);
                if (supervisor == null)
                {
                    return HttpNotFound();
                }
                string selectedSup = supervisor.UserID;




                Allocation allocation = new Allocation();
                allocation.StaffNumber = id;
                allocation.StudentNumber = studID;
                db.Allocations.Add(allocation);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

        }




        public ActionResult ChooseSupervisors(string Id)
        {

            ViewBag.studentId = Id;
            var SelectedList = new List<SelectListItem>();

            ProjectSupervisor projectSupervisor = new ProjectSupervisor();
            List<AspNetUser> data = new List<AspNetUser>();
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {

                var supervisors = from rowS in db.AspNetUsers
                                  join rowSupervisor in db.ProjectSupervisors on rowS.Id equals rowSupervisor.UserID
                                  where rowS.Id == rowSupervisor.UserID
                                  //select new AspNetUser { Id = rowSupervisor.UserID };
                                  select rowS;


                data = supervisors.ToList();
                ViewBag.Students = data;

                return View(data);
            }


        }



        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.AspNetUser);
            return View(students.ToList());
        }
        public ActionResult SelectStudent()
        {
            List<AspNetUser> students = new List<AspNetUser>();

            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                var allstuds = from rowS in dc.AspNetUsers
                               join rowStudents in dc.Students on rowS.Id equals rowStudents.UserID
                               where rowS.Id == rowStudents.UserID
                               select rowS;
                students = allstuds.ToList();
                ViewBag.students = students;
                return View(allstuds.ToList());
            }
        }

        public ActionResult ViewAMySupervisor(string id)
        {
            List<AspNetUser> supervisor = new List<AspNetUser>();
            var studId = id;
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                var sups = from rowS in dc.AspNetUsers
                           join rowR in dc.Allocations on rowS.Id equals rowR.StaffNumber
                           where rowR.StudentNumber == studId
                           select rowS;
                supervisor = sups.ToList();
            }
            return View(supervisor);
        }

        public ActionResult ViewMySupervisor(string id)
        {
            List<AspNetUser> supervisor = new List<AspNetUser>();
            var studId = User.Identity.GetUserId();
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                var sups = from rowS in dc.AspNetUsers
                           join rowR in dc.Allocations on rowS.Id equals rowR.StaffNumber
                           where rowR.StudentNumber == studId
                           select rowS;
                supervisor = sups.ToList();
            }
            return View(supervisor);
        }




        public ActionResult ViewAllStudents()
        {
            List<AspNetUser> students = new List<AspNetUser>();
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                var allstuds = from rowS in dc.AspNetUsers
                               join rowStudents in dc.Students on rowS.Id equals rowStudents.UserID
                               where rowS.Id == rowStudents.UserID
                               select rowS;
                students = allstuds.ToList();

                return View(students);
            }
        }


        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email");
                return View();
            }
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentNumber,Course,StudyYear,ProjectName")] Student student)
        {
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.UserID);
                return View(student);
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.UserID);
                return View(student);
            }
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentNumber,Course,StudyYear,ProjectName")] Student student)
        {
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.UserID);
                return View(student);
            }
        }




        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
