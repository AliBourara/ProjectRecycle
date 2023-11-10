#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjectRecycle.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public int WasteId { get; set; }
        public Waste? Waste { get; set; }
        
        public double StartPrice { get; set; }
        public DateTime EndDate { get; set; }

        // Created Ats
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Updated At
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public List<Bid> Bids { get; set; } = new();  

    }
}
