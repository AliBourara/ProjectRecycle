#pragma warning disable CS8618
using ProjectRecycle.Models;
using ProjectRecycle.Utility;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjectRecycle.Models
{
    public class Mission
    {
        [Key]
        public int MissionId { get; set; }
        public int WasteId { get; set; }
        
        public int UserId { get; set; }
        

        public StaticData.MissionStatus Status { get; set; } = StaticData.MissionStatus.Pending;
        public string Message { get; set; }
        // Created At
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Updated At
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //Navigation Properties
        public Waste? Waste { get; set; }
        public AppUser? Consultant { get; set; }

    }
}
