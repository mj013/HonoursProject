//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProgressTracker.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
    
        public virtual Milestone Milestone { get; set; }
    }
}
