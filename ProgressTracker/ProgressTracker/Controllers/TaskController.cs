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
            Completed completed = new Completed();
            To_Do to_Do = new To_Do();
            using (var dc = new ProgressTrackerEntities())
            {
                
                var updatedTask = (Milestone)taskDto;
                updatedTask.Id = id;
                if(taskDto.progress==1)
                {
                    updatedTask.Status = "Completed";
                    completed.Progress= updatedTask.Progress;
                    completed.StudentNumber = updatedTask.StudentNumber;
                    dc.Completeds.Add(completed);
                    dc.SaveChanges();



                }
                else if(taskDto.progress==0)
                {
                    updatedTask.Status = "To-Do";
                    to_Do.StudentNumber = updatedTask.StudentNumber;
                    to_Do.Progress = updatedTask.Progress;
                    
                    dc.To_Do.Add(to_Do);
                    dc.SaveChanges();

                }
                else
                {
                    updatedTask.Status = "In-Progress";
                }
                

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
            Completed completed = new Completed();
            To_Do to_Do = new To_Do();
            var userID = User.Identity.GetUserId();
            Milestone task = new Milestone();
            using (var dc = new ProgressTrackerEntities())
            {

                var newTask = (Milestone)taskDto;
                newTask.SortOrder = dc.Milestones.Max(t => t.SortOrder) + 1;
                newTask.StudentNumber = userID;
                if (newTask.Progress == 1)
                {
                    newTask.Status = "Completed";
                    newTask.Completed = true;
                    completed.Progress = newTask.Progress;
                    completed.StudentNumber = newTask.StudentNumber;
                    dc.Completeds.Add(completed);
                    dc.SaveChanges();
                }
                if (newTask.Progress > 0 && newTask.Progress < 1)
                {
                    newTask.Status = "In-Progress";
                    newTask.InProgress = true;
                }
                if (newTask.Progress == 0)
                {
                    newTask.Status = "To-Do";
                    newTask.ToDo = true;
                    to_Do.StudentNumber = newTask.StudentNumber;
                    to_Do.Progress = newTask.Progress;
                    dc.To_Do.Add(to_Do);
                    dc.SaveChanges();

                }





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

