using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
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

        // GET: Documents/Create
        public ActionResult Upload(int? id)
        {
            using (var db = new ProgressTrackerEntities())
            {
                ViewBag.Id = id;
            }
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload([Bind(Include = "DocumentID,DocumentName,Path,Id,MilestoneID")] Document document, IEnumerable<HttpPostedFileBase> File, int id)
        {
            using (var db = new ProgressTrackerEntities())
            {
                
                    if (ModelState.IsValid)
                    {
                    
                        document.Path = "/UploadedFiles/" + File.First().FileName;
                        document.MilestoneID = id;
                         document.DocumentName = document.DocumentName;
                        db.Documents.Add(document);

                        db.SaveChanges();
                   
                    return RedirectToAction("Index");
                    }
                
                ViewBag.Id = new SelectList(db.Milestones, "Id", "Text", document.Id);
            }
            return View(document);
        }


        public ActionResult UploadFile(int? id)
        {
            using (var db = new ProgressTrackerEntities())
            {
                ViewBag.Id = id;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocumentID,DocumentName,FileExtension,ContentType")] Document document, IEnumerable<HttpPostedFileBase> file, int id)
        {

            var _path = "";

            Milestone milestone = new Milestone();
            using (var db = new ProgressTrackerEntities())
            {

                if (ModelState.IsValid)
                {

                    document.Id = id;
                    //  document.DocumentName=
                    // db.SaveChanges();

                    foreach (var doc in file)
                    {
                        document = new Document();
                        if (doc != null && doc.ContentLength > 0)
                        {
                            // string _FileName = Path.GetFileName(doc.FileName);
                            _path = Path.Combine(Server.MapPath("~/UploadedFiles"), document.DocumentName);

                            doc.SaveAs(_path);

                            // document.DocumentName = _FileName;
                            document.Path = _path;
                            document.Id = id;

                            db.Documents.Add(document);
                            db.SaveChanges();
                        }
                        ViewBag.Message = "File Uploaded Successfully!!";
                        return View();
                    }



                }

                ViewBag.Message = "File upload failed!!";
                return View();

            }
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

        public ActionResult ViewStudentMilestones(string id)
        {
            List<Milestone> data = new List<Milestone>();
            Student student = new Student();
            var studID = id;
            using (var db = new ProgressTrackerEntities())
            {
                var milestones = from rowM in db.Milestones
                                 where rowM.StudentNumber == studID
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
        public ActionResult AddMilestone()
        {
            ViewBag.MilestoneID = new SelectList(db.Students, "StudentNumber", "Course");
            return View();
        }

        // POST: Milestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMilestone([Bind(Include = "MilestoneID,Text,StartDate,Duration,ProjectNumber,Type,ParentId,Progress,Id,StudentNumber")] Milestone milestone)
        {
            var userID = User.Identity.GetUserId();
            using (var db = new ProgressTrackerEntities())
            {
                if (ModelState.IsValid)
                {
                   
                    milestone.StudentNumber = userID;
                    if(milestone.Progress==1)
                    {
                        milestone.Status = "Completed";
                        milestone.Completed = true;
                    }
                    else if(milestone.Progress>0 &&milestone.Progress<1)
                    {
                        milestone.Status = "In-Progress";
                        milestone.InProgress = true;
                    }
                    else
                    {
                        milestone.Status = "To-Do";
                        milestone.ToDo = true;
                    }
                    
                    
                    
                    db.Milestones.Add(milestone);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.MilestoneID = new SelectList(db.Students, "StudentNumber", "Course", milestone.MilestoneID);
            return View(milestone);
        }

        // GET: Milestones/Edit/5
        public ActionResult Edit(int? id)
        {
            Milestone milestone = new Milestone();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using(var db=new ProgressTrackerEntities())
            {
                milestone = db.Milestones.Find(id);

            }
            if (milestone == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MilestoneID = new SelectList(db.Students, "StudentNumber", "Course", milestone.MilestoneID);
            return View(milestone);
        }

        // POST: Milestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MilestoneID,Text,StartDate,Duration,ProjectNumber,Type,ParentId,Progress,Id,StudentNumber")] Milestone milestone)
        {
            var userID = User.Identity.GetUserId();
            using (var db = new ProgressTrackerEntities())
            {
                if (ModelState.IsValid)
                {
                    milestone.StudentNumber = userID;
                    db.Entry(milestone).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.MilestoneID = new SelectList(db.Students, "StudentNumber", "Course", milestone.MilestoneID);
            return View(milestone);
        }

        // GET: Milestones/Delete/5
        public ActionResult Delete(int? id)
        {
            var userID = User.Identity.GetUserId();
            using (var db = new ProgressTrackerEntities())
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
