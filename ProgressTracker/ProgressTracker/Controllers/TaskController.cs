using Microsoft.AspNet.Identity;
using ProgressTracker.App_Start;
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
    [GanttAPIExceptionFilter]
    public class TaskController : ApiController
    {

       // GET api/Task
        public IEnumerable<TaskDto> Get()
        {
            List<Milestone> data = new List<Milestone>();
            var userID = User.Identity.GetUserId();

            using (var dc = new ProgressTrackerEntities())
            {
                var milestones = from rowM in dc.Milestones
                                 where rowM.StudentNumber == userID
                                 select rowM;
                data = milestones.OrderBy(t => t.SortOrder).ToList();

                return data.Select(t => (TaskDto)t);

                //return dc.Milestones
                //.ToList()
                //.Select(t => (TaskDto)t);
            }

        }

        //GET api/Task/5
       
        [System.Web.Http.HttpGet]
        public TaskDto Get(int id)
        {
            List<Milestone> data = new List<Milestone>();
            var userID = User.Identity.GetUserId();
            using (var dc = new ProgressTrackerEntities())
            {
                return (TaskDto)dc
               .Milestones
               .Find(id);
            }

        }

        //PUT api/Task/5
        [System.Web.Http.HttpPut]
        public IHttpActionResult EditTask(int id, TaskDto taskDto)
        {
            var userID = User.Identity.GetUserId();
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

      //  POST api/Task
       [System.Web.Http.HttpPost]
        public IHttpActionResult CreateTask(TaskDto taskDto)
        {
            var userID = User.Identity.GetUserId();
            Milestone task = new Milestone();
            using (var dc = new ProgressTrackerEntities())
            {

                var newTask = (Milestone)taskDto;
                newTask.SortOrder = dc.Milestones.Max(t => t.SortOrder) + 1;
                newTask.StudentNumber = userID;

                dc.Milestones.Add(newTask);

                dc.SaveChanges();

                return Ok(new
                {
                    tid = newTask.Id,


                    action = "inserted"
                });
            }

        }


        //private void _UpdateOrders(Milestone updatedTask, string orderTarget)
        //{
        //    using (var db = new ProgressTrackerEntities())
        //    {
        //        int adjacentTaskId;
        //        var nextSibling = false;

        //        var targetId = orderTarget;

        //        // adjacent task id is sent either as '{id}' or as 'next:{id}' depending 
        //        // on whether it's the next or the previous sibling
        //        if (targetId.StartsWith("next:"))
        //        {
        //            targetId = targetId.Replace("next:", "");
        //            nextSibling = true;
        //        }

        //        if (!int.TryParse(targetId, out adjacentTaskId))
        //        {
        //            return;
        //        }

        //        var adjacentTask = db.Milestones.Find(adjacentTaskId);
        //        var startOrder = adjacentTask.SortOrder;

        //        if (nextSibling)
        //            startOrder++;

        //        updatedTask.SortOrder = startOrder;

        //        var updateOrders = db.Milestones
        //         .Where(t => t.Id != updatedTask.Id)
        //         .Where(t => t.SortOrder >= startOrder)
        //         .OrderBy(t => t.SortOrder);

        //        var taskList = updateOrders.ToList();

        //        taskList.ForEach(t => t.SortOrder++);
        //    }
        //}

        // DELETE api/Task/5
        [System.Web.Http.HttpDelete]
        public IHttpActionResult DeleteTask(int id)
        {
            var userID = User.Identity.GetUserId();
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
            var userID = User.Identity.GetUserId();
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

