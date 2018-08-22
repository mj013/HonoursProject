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
    public class MilestonesController : Controller
    {
        private ProgressTrackerEntities db = new ProgressTrackerEntities();

        // GET: Milestones
        public ActionResult Index()
        {
            var milestones = db.Milestones.Include(m => m.Student);
            return View(milestones.ToList());
        }

        public ActionResult ViewMilestones()
        {
            List<Milestone> data = new List<Milestone>();
            var userID = User.Identity.GetUserId();
            using (var db = new ProgressTrackerEntities())
            {
                var milestones = from rowM in db.Milestones
                                 where rowM.StudentNumber == userID
                                 select rowM;
                data = milestones.ToList();
            }
            return View(data);

        }

        // GET: Milestones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = db.Milestones.Find(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // GET: Milestones/Create
        public ActionResult Create()
        {
            ViewBag.MilestoneID = new SelectList(db.Students, "StudentNumber", "Course");
            return View();
        }

        // POST: Milestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MilestoneID,Text,StartDate,Duration,ProjectNumber,Type,ParentId,Progress,Id,StudentNumber")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                db.Milestones.Add(milestone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MilestoneID = new SelectList(db.Students, "StudentNumber", "Course", milestone.MilestoneID);
            return View(milestone);
        }

        // GET: Milestones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = db.Milestones.Find(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            ViewBag.MilestoneID = new SelectList(db.Students, "StudentNumber", "Course", milestone.MilestoneID);
            return View(milestone);
        }

        // POST: Milestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MilestoneID,Text,StartDate,Duration,ProjectNumber,Type,ParentId,Progress,Id,StudentNumber")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(milestone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MilestoneID = new SelectList(db.Students, "StudentNumber", "Course", milestone.MilestoneID);
            return View(milestone);
        }

        // GET: Milestones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = db.Milestones.Find(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // POST: Milestones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Milestone milestone = db.Milestones.Find(id);
            db.Milestones.Remove(milestone);
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
