#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjectRecycle.Models
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }
        public int OfferId { get; set; }
        
        public int CompanyId { get; set; }

        public double Price { get; set; }
        // Created At
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Updated At
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //Navigation Properties
        public Company? Bidder { get; set; }
        public Offer? Offer { get; set; }
    }
}

