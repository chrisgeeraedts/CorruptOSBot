//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CorruptOSBot.TheHunt.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class hunt_team_members
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int DiscordUserId { get; set; }
    
        public virtual DiscordUser DiscordUser { get; set; }
        public virtual hunt_teams hunt_teams { get; set; }
    }
}