using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Proyecto_Notas.DTOs;

namespace Proyecto_Notas.Models;

public partial class MerMariaJuliaContext : DbContext
{
    public MerMariaJuliaContext()
    {
    }

    public MerMariaJuliaContext(DbContextOptions<MerMariaJuliaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Materia> Materias { get; set; }

    public virtual DbSet<Nota> Notas { get; set; }

    public virtual DbSet<InferioresxMateria> InferioresxMateria { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        if (!optionsBuilder.IsConfigured)
        { 
#warning to protect potentially sensitive information in your connection string, you should move it out of source code. you can avoid scaffolding the connection string by using the name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. for more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?linkid=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-FRPPVO0\\SQLEXPRESS; database=MER_Maria_Julia; integrated security=true;TrustServerCertificate=true;");

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.IdEstudiante);

            entity.Property(e => e.IdEstudiante)
                .ValueGeneratedNever()
                .HasColumnName("id_estudiante");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("celular");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.IdMateria);

            entity.Property(e => e.IdMateria)
                .ValueGeneratedNever()
                .HasColumnName("id_materia");
            entity.Property(e => e.NombreMateria)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_materia");
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.HasKey(e => e.IdNota);

            entity.Property(e => e.IdNota)
                .ValueGeneratedNever()
                .HasColumnName("id_nota");
            entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");
            entity.Property(e => e.IdMateria).HasColumnName("id_materia");
            entity.Property(e => e.Nota1)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("nota");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("FK_Estudiantes_Notas");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdMateria)
                .HasConstraintName("FK_Materias_Notas");
        });

        modelBuilder.Entity<InferioresxMateria>(entity =>
        {
            entity.HasNoKey();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}