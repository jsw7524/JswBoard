//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyCard3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Friend
    {
        public int Id { get; set; }
        public string LastMessage { get; set; }
    
        public virtual Person PersonA { get; set; }
        public virtual Person PersonB { get; set; }
    }
}
