using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIAPOracleEF.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPOracleEF.Database;

public class AppDbContext : DbContext
{
    private readonly string _connString;

    public AppDbContext(string connString)
    {
        _connString = connString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseOracle(_connString);
    }

    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Materia> Materias => Set<Materia>();

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Aluno>(e =>
    //    {
    //        e.ToTable("ALUNOS");
    //        e.HasKey(x => x.Id);
    //        e.Property(x => x.Id).ValueGeneratedOnAdd();
    //        e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(255).IsRequired();
    //        e.Property(x => x.Matricula).HasColumnName("MATRICULA").HasMaxLength(255).IsRequired();
    //    });

    //    modelBuilder.Entity<Materia>(e =>
    //    {
    //        e.ToTable("MATERIAS");
    //        e.HasKey(x => x.Id);
    //        e.Property(x => x.Id).ValueGeneratedOnAdd();
    //        e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(255).IsRequired();
    //        e.Property(x => x.Codigo).HasColumnName("CODIGO").HasMaxLength(50).IsRequired();
    //    });
    //}
}
