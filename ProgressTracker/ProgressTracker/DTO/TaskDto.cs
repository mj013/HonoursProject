using ProgressTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgressTracker.DTO
{
    public class TaskDto
    {
        public int id { get; set; }
        public string userid { get; set; }
        public string text { get; set; }
        public string start_date { get; set; }
        public int duration { get; set; }
        public float progress { get; set; }
        public int? parent { get; set; }
        public string type { get; set; }
        public bool open
        {
            get { return true; }
            set { }
        }
        public string target { get; set; }

        public static explicit operator TaskDto(Milestone task)
        {
            
            return new TaskDto
            {
                id = task.Id,
                text = task.Text,
                start_date = task.StartDate.ToString("yyyy-MM-dd HH:mm"),
                duration = (int)task.Duration,
                parent = task.ParentId,
                type = task.Type,
                userid=task.StudentNumber,
                progress = (float)task.Progress
            };
        }

        public static explicit operator Milestone(TaskDto task)
        {
            return new Milestone
            {
                Id = task.id,
                Text = task.text,
                StartDate = DateTime.Parse(task.start_date, System.Globalization.CultureInfo.InvariantCulture),
                Duration = task.duration,
                ParentId = task.parent,
                StudentNumber=task.userid,
                Type = task.type,
                Progress = task.progress
            };
        }
    }
}