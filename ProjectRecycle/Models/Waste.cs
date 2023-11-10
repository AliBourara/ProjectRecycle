#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectRecycle.Utility;
namespace ProjectRecycle.Models
{
    public class Waste
    {
        [Key]
        public int WasteId { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public string Description { get; set; }

        public string AnalyseDocument { get; set; }
        public DateTime AvailableDate { get; set; }
        public DateTime EndingAvailability { get; set; }

        public StaticData.WasteType Type { get; set; }
        // Created At
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Updated At
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        //Navigations properties
        public Mission? Validation { get; set; }
        public List<Offer> Offers { get; set; } = new();
        public Company? Owner { get; set; }


    }
}
