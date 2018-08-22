using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgressTracker.Controllers
{
    public class GanttController : Controller
    {
        // GET: Gantt
        public ActionResult Index()
        {
            return View();
        }
    }
}