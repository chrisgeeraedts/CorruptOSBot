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
    
    public partial class hunt_bossdrops
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hunt_bossdrops()
        {
            this.hunt_team_drops = new HashSet<hunt_team_drops>();
        }
    
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int BasePointValue { get; set; }
        public int BossId { get; set; }
    
        public virtual hunt_bosses hunt_bosses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hunt_team_drops> hunt_team_drops { get; set; }
    }
}
