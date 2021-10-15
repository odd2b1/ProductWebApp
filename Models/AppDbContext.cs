using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //product
            builder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products).OnDelete(DeleteBehavior.Restrict).HasForeignKey(x=>x.CategoryId);
            builder.Entity<Product>().Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
            builder.Entity<Product>().Property(x => x.DateStamp).HasDefaultValueSql("getdate()");

            //category
            builder.Entity<Category>().Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
            builder.Entity<Category>().Property(x => x.DateStamp).HasDefaultValueSql("getdate()");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<TestModel> TestModels { get; set; }

    }
}
