//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CorruptOSBot.CorruptPoints
{
    using System;
    using System.Collections.Generic;
    
    public partial class PointMutation
    {
        public int Id { get; set; }
        public int PointChange { get; set; }
        public System.DateTime DateTime { get; set; }
        public Nullable<int> TargetPlayerId { get; set; }
        public Nullable<int> PointStoreItemId { get; set; }
    
        public virtual DiscordUser DiscordUser { get; set; }
        public virtual PointStore PointStore { get; set; }
    }
}