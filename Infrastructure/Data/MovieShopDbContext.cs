﻿using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MovieShopDbContext : DbContext
    {
        public MovieShopDbContext(DbContextOptions<MovieShopDbContext> options) : base(options)
        {

        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cast> Casts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // First method to handle many-to-many relationship (No unrelated unique column)
            modelBuilder.Entity<Movie>().HasMany(m => m.Genres).WithMany(g => g.Movies)
                .UsingEntity<Dictionary<string, object>>("MovieGenre",
                m => m.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                g => g.HasOne<Movie>().WithMany().HasForeignKey("MovieId"));

            modelBuilder.Entity<User>().HasMany(u => u.Roles).WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>("UserRole",
                u => u.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                r => r.HasOne<User>().WithMany().HasForeignKey("UserId"));

            modelBuilder.Entity<Movie>(ConfigureMovie);
            modelBuilder.Entity<Trailer>(ConfigureTrailer);
            modelBuilder.Entity<Cast>(ConfigureCast);
            // Second method to handle many-to-many relationship (With unique columns; Need to create the entity)
            modelBuilder.Entity<MovieCast>(ConfigureMovieCast);
            modelBuilder.Entity<Crew>(ConfigureCrew);
            modelBuilder.Entity<MovieCrew>(ConfigureMovieCrew);
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<Role>(ConfigureRole);
            modelBuilder.Entity<Favorite>(ConfigureFavorite);
            modelBuilder.Entity<Review>(ConfigureReview);
            modelBuilder.Entity<Purchase>(ConfigurePurchase);


        }

        private void ConfigurePurchase(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Purchase");
            builder.HasKey(p => new { p.Id });
            builder.Property(p => p.UserId);
            builder.Property(p => p.PurchaseNumber);
            builder.Property(p => p.TotalPrice).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PurchaseDateTime).HasMaxLength(7);
            builder.Property(p => p.MovieId);
            builder.HasOne(p => p.Movie).WithMany(p => p.Purchases).HasForeignKey(p => p.MovieId);
            builder.HasOne(p => p.User).WithMany(p => p.Purchases).HasForeignKey(p => p.UserId);
        }

        private void ConfigureReview(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Review");
            builder.HasKey(r => new { r.MovieId, r.UserId });
            builder.Property(r => r.Rating).HasColumnType("decimal(3, 2)");
            builder.Property(r => r.ReviewText).HasMaxLength(4096);
            builder.HasOne(r => r.Movie).WithMany(r => r.Reviews).HasForeignKey(r => r.MovieId);
            builder.HasOne(r => r.User).WithMany(r => r.Reviews).HasForeignKey(r => r.UserId);
        }

        private void ConfigureFavorite(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorite");
            builder.HasKey(f => new { f.Id });
            builder.Property(f => f.MovieId);
            builder.Property(f => f.UserId);
            builder.HasOne(f => f.Movie).WithMany(f => f.Favorites).HasForeignKey(f => f.MovieId);
            builder.HasOne(f => f.User).WithMany(f => f.Favorites).HasForeignKey(f => f.UserId);
        }

        private void ConfigureRole(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(ro => new { ro.Id });
            builder.Property(ro => ro.Name).HasMaxLength(20);
        }

        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName).HasMaxLength(128);
            builder.Property(u => u.LastName).HasMaxLength(128);
            builder.Property(u => u.DateOfBirth).HasMaxLength(7);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.HashedPassword).HasMaxLength(1024);
            builder.Property(u => u.Salt).HasMaxLength(1024);
            builder.Property(u => u.PhoneNumber).HasMaxLength(16);
            builder.Property(u => u.TwoFactorEnabled);
            builder.Property(u => u.LockoutEndDate).HasMaxLength(7);
            builder.Property(u => u.LastLoginDateTime).HasMaxLength(7);
            builder.Property(u => u.IsLocked);
            builder.Property(u => u.AccessFailedCount);
        }

        private void ConfigureMovieCrew(EntityTypeBuilder<MovieCrew> builder)
        {
            builder.ToTable("MovieCrew");
            builder.HasKey(mcr => new { mcr.CrewId, mcr.MovieId, mcr.Department, mcr.Job });
            builder.HasOne(mcr => mcr.Movie).WithMany(mcr => mcr.MovieCrews).HasForeignKey(mcr => mcr.MovieId);
            builder.HasOne(mcr => mcr.Crew).WithMany(mcr => mcr.MovieCrews).HasForeignKey(mcr => mcr.CrewId);
            builder.Property(mcr => mcr.Department).HasMaxLength(128);
            builder.Property(mcr => mcr.Job).HasMaxLength(128);
        }

        private void ConfigureCrew(EntityTypeBuilder<Crew> builder)
        {
            builder.ToTable("Crew");
            builder.HasKey(cr => cr.Id);
            builder.Property(cr => cr.Name).HasMaxLength(128);
            builder.Property(cr => cr.Gender).HasMaxLength(4096);
            builder.Property(cr => cr.TmdbUrl).HasMaxLength(4096);
            builder.Property(c => c.ProfilePath).HasMaxLength(2084);
        }

        private void ConfigureMovieCast(EntityTypeBuilder<MovieCast> builder)
        {
            builder.ToTable("MovieCast");
            builder.HasKey(mc => new { mc.CastId, mc.MovieId, mc.Character });
            builder.HasOne(mc => mc.Movie).WithMany(mc => mc.MovieCasts).HasForeignKey(mc => mc.MovieId);
            builder.HasOne(mc => mc.Cast).WithMany(mc => mc.MovieCasts).HasForeignKey(mc => mc.CastId);

        }

        private void ConfigureCast(EntityTypeBuilder<Cast> builder)
        {
            builder.ToTable("Cast");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(2084);
            builder.Property(c => c.ProfilePath).HasMaxLength(2084);
        }

        private void ConfigureTrailer(EntityTypeBuilder<Trailer> builder)
        {
            builder.ToTable("Trailer");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.TrailerUrl).HasMaxLength(2084);
            builder.Property(t => t.Name).HasMaxLength(2084);
        }

        private void ConfigureMovie(EntityTypeBuilder<Movie> builder)
        {
            // User Fluent API Rules

            builder.ToTable("Movie");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Title).HasMaxLength(256);
            builder.Ignore(m => m.Rating);
            builder.Property(m => m.Overview).HasMaxLength(4096);
            builder.Property(m => m.Tagline).HasMaxLength(512);
            builder.Property(m => m.ImdbUrl).HasMaxLength(2084);
            builder.Property(m => m.TmdbUrl).HasMaxLength(2084);
            builder.Property(m => m.PosterUrl).HasMaxLength(2084);
            builder.Property(m => m.BackdropUrl).HasMaxLength(2084);
            builder.Property(m => m.OriginalLanguage).HasMaxLength(64);
            builder.Property(m => m.Price).HasColumnType("decimal(5, 2)").HasDefaultValue(9.9m);
            builder.Property(m => m.Budget).HasColumnType("decimal(18, 4)").HasDefaultValue(9.9m);
            builder.Property(m => m.Revenue).HasColumnType("decimal(18, 4)").HasDefaultValue(9.9m);
            builder.Property(m => m.CreatedDate).HasDefaultValueSql("getdate()");
        }
    }
}
