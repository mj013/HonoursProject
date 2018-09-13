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
    public class ProjectSupervisorsController : Controller
    {
        private ProgressTrackerEntities db = new ProgressTrackerEntities();

        // GET: ProjectSupervisors
        public ActionResult Index()
        {
            var projectSupervisors = db.ProjectSupervisors.Include(p => p.AspNetUser);
            return View(projectSupervisors.ToList());
        }

        public ActionResult ViewMyStudents()
        {
            var staffId= User.Identity.GetUserId();
            List<AspNetUser> students = new List<AspNetUser>();
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                var mystudents = from rowS in db.AspNetUsers
                                 join rowStudent in db.Students on rowS.Id equals rowStudent.UserID
                                 join rowAll in db.Allocations on rowStudent.UserID equals rowAll.StudentNumber
                                 where rowAll.StaffNumber == staffId
                                 select rowS;
                students = mystudents.ToList();


            }
            return View(students);

        }

        public ActionResult ViewAllSupervisors()
        {
            
            List<AspNetUser> supervisors = new List<AspNetUser>();
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                var allsupervisors = from rowS in db.AspNetUsers
                                 join rowSupervisor in db.ProjectSupervisors on rowS.Id equals rowSupervisor.UserID
                                
                                 where rowS.Id == rowSupervisor.UserID
                                 select rowS;
                supervisors = allsupervisors.ToList();
                return View(supervisors);


            }

        }
        public ActionResult viewAll()
        {
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                List<Object> mymodal = new List<object>();
                mymodal.Add(db.Students.ToList());
                mymodal.Add(db.ProjectSupervisors.ToList());
                return View(mymodal);
            }

        }





        // GET: ProjectSupervisors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectSupervisor projectSupervisor = db.ProjectSupervisors.Find(id);
            if (projectSupervisor == null)
            {
                return HttpNotFound();
            }
            return View(projectSupervisor);
        }



        // GET: ProjectSupervisors/Create
        public ActionResult Create()
        {
            ViewBag.StaffNumber = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: ProjectSupervisors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffNumber,calId")] ProjectSupervisor projectSupervisor)
        {
            if (ModelState.IsValid)
            {
                db.ProjectSupervisors.Add(projectSupervisor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StaffNumber = new SelectList(db.AspNetUsers, "Id", "Email", projectSupervisor.UserID);
            return View(projectSupervisor);
        }

        // GET: ProjectSupervisors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectSupervisor projectSupervisor = db.ProjectSupervisors.Find(id);
            if (projectSupervisor == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffNumber = new SelectList(db.AspNetUsers, "Id", "Email", projectSupervisor.UserID);
            return View(projectSupervisor);
        }

        // POST: ProjectSupervisors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffNumber,calId")] ProjectSupervisor projectSupervisor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectSupervisor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffNumber = new SelectList(db.AspNetUsers, "Id", "Email", projectSupervisor.UserID);
            return View(projectSupervisor);
        }




        // GET: ProjectSupervisors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectSupervisor projectSupervisor = db.ProjectSupervisors.Find(id);
            if (projectSupervisor == null)
            {
                return HttpNotFound();
            }
            return View(projectSupervisor);
        }

        // POST: ProjectSupervisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ProjectSupervisor projectSupervisor = db.ProjectSupervisors.Find(id);
            db.ProjectSupervisors.Remove(projectSupervisor);
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
