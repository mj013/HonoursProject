using ProgressTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgressTracker.DTO
{
    public class LinkDto
    {
        public int id { get; set; }
        public string type { get; set; }
        public int source { get; set; }
        public int target { get; set; }

        public static explicit operator LinkDto(Link link)
        {
            return new LinkDto
            {
                id = link.LinkId,
                type = link.Type,
                source = (int)link.SourceTaskId,
                target =(int) link.TargetTaskId
            };
        }

        public static explicit operator Link(LinkDto link)
        {
            return new Link
            {
                LinkId = link.id,
                Type = link.type,
                SourceTaskId = link.source,
                TargetTaskId = link.target
            };
        }
    }
}