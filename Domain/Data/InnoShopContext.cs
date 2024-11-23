using System;
using System.Collections.Generic;
using InnoShop.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Identity;

namespace InnoShop.Domain.Data;

public partial class InnoShopContext : IdentityDbContext<User,IdentityRole<int>,int>
{
    public InnoShopContext()
    {
    }

    public InnoShopContext(DbContextOptions<InnoShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Locality> Localities { get; set; }

    public virtual DbSet<ProdAttrib> ProdAttribs { get; set; }

    public virtual DbSet<ProdType> ProdTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    {
        string getStringFrom = "newDBConnection";
        string connectionString = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString(getStringFrom);
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Locality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Localiti__3214EC07601BC24A");
            entity.HasIndex(e => e.Name, "UQ__Localiti__5E55825B2B853D4G").IsUnique();
            entity.Property(e => e.Name).HasMaxLength(40);
        });

        modelBuilder.Entity<ProdAttrib>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProdAttr__3214EC07905CC3BF");

            entity.Property(e => e.Name).HasMaxLength(40);

            entity.HasOne(d => d.Prod).WithMany(p => p.ProdAttribs)
                .HasForeignKey(d => d.ProdId)
                .HasConstraintName("FK__ProdAttri__ProdI__0D0FEE32");
        });

        modelBuilder.Entity<ProdType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProdType__3214EC070A05AE8A");
            entity.HasIndex(e => e.Name, "UQ__ProdType__5E55825B2B853D4K").IsUnique();
            entity.Property(e => e.Name).HasMaxLength(40);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC074E540A36");

            entity.Property(e => e.Cost).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(40);

            entity.HasOne(d => d.Buyer).WithMany(p => p.ProductsBuyer)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK_Users_Buyers");

            entity.HasOne(d => d.ProdType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProdTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Products__ProdTy__084B3915");

            entity.HasOne(d => d.User).WithMany(p => p.ProductsUser)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Products_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC072D6A52E8");

            entity.HasIndex(e => e.UserName, "UQ__Users__5E55825B2B853D4C").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Locality).WithMany(p => p.Users)
                .HasForeignKey(d => d.LocalityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Users__LocalityI__056ECC6A");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__UserTypeI__047AA831");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserType__3214EC0709A91F01");

            entity.Property(e => e.Name).HasMaxLength(40);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
