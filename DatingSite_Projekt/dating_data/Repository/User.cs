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
    
    public partial class User
    {
        public User()
        {
            this.Descriptions = new HashSet<Description>();
            this.Friends = new HashSet<Friend>();
            this.Friends1 = new HashSet<Friend>();
            this.Messages = new HashSet<Message>();
            this.Messages1 = new HashSet<Message>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Searchable { get; set; }
    
        public virtual ICollection<Description> Descriptions { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Friend> Friends1 { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Message> Messages1 { get; set; }
    }
}
