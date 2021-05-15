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
    
    public partial class DiscordUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DiscordUser()
        {
            this.RunescapeAccounts = new HashSet<RunescapeAccount>();
            this.hunt_team_members = new HashSet<hunt_team_members>();
            this.PointMutations = new HashSet<PointMutation>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public Nullable<long> DiscordId { get; set; }
        public Nullable<System.DateTime> OriginallyJoinedAt { get; set; }
        public bool BlacklistedForPromotion { get; set; }
        public Nullable<System.DateTime> LeavingDate { get; set; }
        public int CorruptPoints { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RunescapeAccount> RunescapeAccounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hunt_team_members> hunt_team_members { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PointMutation> PointMutations { get; set; }
    }
}
