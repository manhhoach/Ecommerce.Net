﻿using EcommerceWebRazor_Temp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWebRazor_Temp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category[]
            {
                new Category()
                {
                    Id =1, Name = "Action", DisplayOrder =1
                },
                new Category()
                {
                    Id =2,  Name = "SciFi", DisplayOrder =2
                },
                new Category()
                {
                   Id =3,   Name = "History", DisplayOrder =3
                },
                new Category()
                {
                    Id =4,  Name = "Comedy", DisplayOrder =4
                },
            });
            //  base.OnModelCreating(modelBuilder);
        }
    }
}
