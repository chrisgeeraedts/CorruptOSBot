//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CorruptOSBot.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChatLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public System.DateTime Datetime { get; set; }
        public string Severity { get; set; }
        public string Channel { get; set; }
        public Nullable<long> PostId { get; set; }
        public Nullable<long> ChannelId { get; set; }
        public Nullable<long> AuthorId { get; set; }
    }
}
