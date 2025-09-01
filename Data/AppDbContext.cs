﻿using MEC.Models;
using Microsoft.EntityFrameworkCore;

namespace MEC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.CPF)
                .IsUnique();
        }
    }
}
