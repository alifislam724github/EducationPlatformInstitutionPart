//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EducationPlatform.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cours
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public Nullable<int> Price { get; set; }
        public string Duration { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> MentorId { get; set; }
        public string Photo { get; set; }
    
        public virtual Mentor Mentor { get; set; }
    }
}