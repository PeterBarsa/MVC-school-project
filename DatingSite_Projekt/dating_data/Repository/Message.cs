//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dating_data.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Messages { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
