//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCExample.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DietList
    {
        public int DietListID { get; set; }
        public int GivenFromId { get; set; }
        public int GiveToId { get; set; }
        public string DietListContent { get; set; }
        public Nullable<System.DateTime> DietListDate { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<int> MinAge { get; set; }
        public Nullable<int> MaxAge { get; set; }
        public Nullable<double> MaxBMI { get; set; }
        public Nullable<double> MinBMI { get; set; }
        public string InsulingUsage { get; set; }
    
        public virtual UserDetail UserDetail { get; set; }
        public virtual UserDetail UserDetail1 { get; set; }
    }
}
