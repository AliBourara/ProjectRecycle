using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectRecycle.Models
{
    public class MyContext : IdentityDbContext<IdentityUser>
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Waste> Wastes { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

    }

}
