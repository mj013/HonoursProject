using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgressTracker.Models
{
    public class SelectStudentModel
    {

            public string Name { get; set; }
            public string ProjectName { get; set; }

        public IEnumerable<SelectStudentModel> students { get; set; }
        
    }
}