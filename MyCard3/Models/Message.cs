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
    
    public partial class Message
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Message()
        {
            this.MessageContent = "";
        }
    
        public int Id { get; set; }
        public int SendPersonId { get; set; }
        public string MessageContent { get; set; }
        public System.DateTime Time { get; set; }
    
        public virtual Person SendPerson { get; set; }
        public virtual Person ReceivePerson { get; set; }
    }
}
