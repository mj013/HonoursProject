using Microsoft.AspNet.Identity;
using ProgressTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgressTracker.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            List<Event> data = new List<Event>();
            var userID = User.Identity.GetUserId();
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                //var allEvents = from rowE in dc.Events
                //                where rowE.UserId == userID
                //                select rowE;
                data = dc.Events.ToList();

                // var events = dc.Events.ToList();
                return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //  return  Json (data, JsonRequestBehavior.AllowGet );
            }
        }
        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            var userID = User.Identity.GetUserId();
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.StartTime = e.StartTime;
                        v.EndTime =e.EndTime;
                        v.Description = e.Description;
                        v.isFullDay = e.isFullDay;
                        v.ThemeColor = e.ThemeColor;
                        
                       // userID = e.UserId;
                    }
                }
                else
                {
                    e.UserId = userID;
                    dc.Events.Add(e);
                }
                dc.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            var userID = User.Identity.GetUserId();
            using (ProgressTrackerEntities dc = new ProgressTrackerEntities())
            {
                var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}