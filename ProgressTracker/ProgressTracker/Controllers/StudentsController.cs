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

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.AspNetUser);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
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

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentNumber,Course,StudyYear,ProjectName")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.StudentNumber);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.StudentNumber);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentNumber,Course,StudyYear,ProjectName")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.StudentNumber);
            return View(student);
        }
        [Authorize(Roles = "Student")]
        // GET: Students/Edit/5
        public ActionResult AddProject()
        {
            string id = User.Identity.GetUserId();
            using (var db = new ProgressTrackerEntities())
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
                ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.StudentNumber);
                return View(student);
            }
               
        }

        [Authorize(Roles = "Student")]

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProject([Bind(Include = "StudentNumber,Course,StudyYear,ProjectName")] Student student)
        {
            string id = User.Identity.GetUserId();
            
            using (var db = new ProgressTrackerEntities())
            {
                
                var currentUser = db.Students.Find(student.StudentNumber);
                if (ModelState.IsValid)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(student).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Email", student.StudentNumber);
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
