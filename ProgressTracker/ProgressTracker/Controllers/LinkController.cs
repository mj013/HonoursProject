using ProgressTracker.DTO;
using ProgressTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProgressTracker.Controllers
{
    public class LinkController : ApiController
    {

        // GET api/Link
        [System.Web.Http.HttpGet]
        public IEnumerable<LinkDto> Get()
        {
            using (var dc = new ProgressTrackerEntities())
            {
                return dc
                .Links
                .ToList()
                .Select(l => (LinkDto)l);
            }

        }

        // GET api/Link/5
        [System.Web.Http.HttpGet]
        public LinkDto Get(int id)
        {
            using (var dc = new ProgressTrackerEntities())
            {
                return (LinkDto)dc
               .Links
               .Find(id);
            }

        }

        // POST api/Link
        [System.Web.Http.HttpPost]
        public IHttpActionResult CreateLink(LinkDto linkDto)
        {
            using (var dc = new ProgressTrackerEntities())
            {

                var newLink = (Link)linkDto;
                dc.Links.Add(newLink);
                dc.SaveChanges();

                return Ok(new
                {
                    tid = newLink.LinkId,
                    action = "inserted"
                });
            }

        }

        // PUT api/Link/5
        [System.Web.Http.HttpPut]
        public IHttpActionResult EditLink(int id, LinkDto linkDto)
        {
            using (var dc = new ProgressTrackerEntities())
            {
                var clientLink = (Link)linkDto;
                clientLink.LinkId = id;

                dc.Entry(clientLink).State = EntityState.Modified;
                dc.SaveChanges();

                return Ok(new
                {
                    action = "updated"
                });
            }

        }

        // DELETE api/Link/5
        [System.Web.Http.HttpDelete]
        public IHttpActionResult DeleteLink(int id)
        {
            using (var dc = new ProgressTrackerEntities())
            {
                var link = dc.Links.Find(id);
                if (link != null)
                {
                    dc.Links.Remove(link);
                    dc.SaveChanges();
                }
                return Ok(new
                {
                    action = "deleted"
                });
            }
               
        }

        protected override void Dispose(bool disposing)
        {
            using (var dc = new ProgressTrackerEntities())
            {
                if (disposing)
                {
                    dc.Dispose();
                }
                base.Dispose(disposing);
            }

              
        }

    }
}
