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
    
    public partial class Institution
    {
        public Institution()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string WebsiteLink { get; set; }
        public string IsValid { get; set; }
        public byte[] Photo { get; set; }
    
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
