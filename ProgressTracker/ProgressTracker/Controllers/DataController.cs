using ProgressTracker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProgressTracker.Controllers
{
    public class DataController : ApiController
    {
        // GET api/
        [System.Web.Http.HttpGet]
        public GanttDto Get()
        {
            return new GanttDto
            {
                data = new TaskController().Get(),
                links = new LinkController().Get()
            };
        }
    }
}
