﻿using DewaEShop.Domain.Cart;
using DewaEShop.Domain.CartItem;
using DewaEShop.Domain.Product;
using DewaEShop.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DewaEShop.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User>, IUnitOfWork
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().HasData(
                new Product("iPhone 15 Pro", "Apple's latest flagship smartphone with a ProMotion display and improved cameras", 999.99m),
                new Product("Dell XPS 15", "Dell's high-performance laptop with a 4K InfinityEdge display", 1899.99m),
                new Product("Sony WH-1000XM4", "Sony's top-of-the-line wireless noise-canceling headphones", 349.99m)
                );
            modelBuilder.Entity<Cart>().HasKey(p => p.Id);

            // Fixing error : "The entity type 'CartItem' requires a primary key to be defined. If you intended to use a keyless entity type, call 'HasNoKey' in 'OnModelCreating'"
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<User>().Property(u => u.Initials).HasMaxLength(5);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => p.UserId);
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("codewithDewz");
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}