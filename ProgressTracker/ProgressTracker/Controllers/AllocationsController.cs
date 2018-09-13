using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProgressTracker.Models;

namespace ProgressTracker.Controllers
{
    public class AllocationsController : Controller
    {
        private ProgressTrackerEntities db = new ProgressTrackerEntities();

        // GET: Allocations
        public ActionResult Index()
        {
            var allocations = db.Allocations.Include(a => a.ProjectSupervisor).Include(a => a.Student);
            return View(allocations.ToList());
        }

        // GET: Allocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Allocation allocation = db.Allocations.Find(id);
            if (allocation == null)
            {
                return HttpNotFound();
            }
            return View(allocation);
        }

        // GET: Allocations/Create
        public ActionResult Create()
        {
            ViewBag.StaffNumber = new SelectList(db.AspNetUsers, "Id", "Id");
            ViewBag.StudentNumber = new SelectList(db.AspNetUsers, "Id", "Id");

         
            return View();
        }

        // POST: Allocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentNumber,StaffNumber,Id")] Allocation allocation)
        {
            if (ModelState.IsValid)
            {
                allocation.StaffNumber = allocation.StaffNumber;
                allocation.StudentNumber = allocation.StudentNumber;
                db.Allocations.Add(allocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StaffNumber = new SelectList(db.ProjectSupervisors, "UserID", "UserID", allocation.StaffNumber);
            ViewBag.StudentNumber = new SelectList(db.Students, "UserID", "Course", allocation.StudentNumber);
            return View(allocation);
        }

        // GET: Allocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Allocation allocation = db.Allocations.Find(id);
            if (allocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffNumber = new SelectList(db.ProjectSupervisors, "UserID", "UserID", allocation.StaffNumber);
            ViewBag.StudentNumber = new SelectList(db.Students, "UserID", "Course", allocation.StudentNumber);
            return View(allocation);
        }

        // POST: Allocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentNumber,StaffNumber,Id")] Allocation allocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(allocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffNumber = new SelectList(db.ProjectSupervisors, "UserID", "UserID", allocation.StaffNumber);
            ViewBag.StudentNumber = new SelectList(db.Students, "UserID", "Course", allocation.StudentNumber);
            return View(allocation);
        }

        // GET: Allocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Allocation allocation = db.Allocations.Find(id);
            if (allocation == null)
            {
                return HttpNotFound();
            }
            return View(allocation);
        }

        // POST: Allocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Allocation allocation = db.Allocations.Find(id);
            db.Allocations.Remove(allocation);
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
