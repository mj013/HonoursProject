using ProgressTracker.DTO;
using ProgressTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace ProgressTracker.Controllers
{
    public class TaskController : ApiController
    {
        // GET api/Task
        public IEnumerable<TaskDto> Get()
        {
            using (var dc = new ProgressTrackerEntities())
            {
                return dc.Milestones
                .ToList()
                .Select(t => (TaskDto)t);
            }

        }

        // GET api/Task/5
        [System.Web.Http.HttpGet]
        public TaskDto Get(int id)
        {
            using (var dc = new ProgressTrackerEntities())
            {
                return (TaskDto)dc
               .Milestones
               .Find(id);
            }

        }

        // PUT api/Task/5
        [System.Web.Http.HttpPut]
        public IHttpActionResult EditTask(int id, TaskDto taskDto)
        {
            using (var dc = new ProgressTrackerEntities())
            {
                var updatedTask = (Milestone)taskDto;
                updatedTask.Id = id;
                dc.Entry(updatedTask).State = EntityState.Modified;
                dc.SaveChanges();

                return Ok(new
                {
                    action = "updated"
                });
            }
        }

        // POST api/Task
        [System.Web.Http.HttpPost]
        public IHttpActionResult CreateTask(TaskDto taskDto)
        {
            using (var dc = new ProgressTrackerEntities())
            {
                var newTask = (Milestone)taskDto;

                dc.Milestones.Add(newTask);
                dc.SaveChanges();

                return Ok(new
                {
                    tid = newTask.Id,
                    action = "inserted"
                });
            }

        }

        // DELETE api/Task/5
        [System.Web.Http.HttpDelete]
        public IHttpActionResult DeleteTask(int id)
        {
            using (var dc = new ProgressTrackerEntities())
            {
                var task = dc.Milestones.Find(id);
                if (task != null)
                {
                    dc.Milestones.Remove(task);
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

