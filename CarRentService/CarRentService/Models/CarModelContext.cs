using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentService.Models
{
    public class CarModelContext : DbContext
    {
        public CarModelContext(DbContextOptions<CarModelContext> options) : base(options)
        {

        }

        
        public DbSet<CarModel> Car { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarModel>().HasKey(c => c.Id);
            
            
        }

    }
    
}
