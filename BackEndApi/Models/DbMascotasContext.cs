using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackEndApi.Models;

public partial class DbMascotasContext : DbContext
{
    public DbMascotasContext()
    {
    }

    public DbMascotasContext(DbContextOptions<DbMascotasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Mascota> Mascota { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
    
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.IdMascota);

            entity.Property(e => e.IdMascota).HasColumnName("id_mascota");
            entity.Property(e => e.Descripcion)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
