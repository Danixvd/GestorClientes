using GestorClientes.Domain;
using Microsoft.EntityFrameworkCore;

namespace GestorClientes.Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
        });

        modelBuilder.Entity<Telefono>(entity =>
        {
            entity.ToTable("Telefono");

            entity.HasIndex(e => e.Fkcliente, "idx_telefono_fkcliente");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
            entity.Property(e => e.Fkcliente).HasColumnName("FKCliente");

            entity.HasOne(d => d.FkclienteNavigation).WithMany(p => p.Telefonos).HasForeignKey(d => d.Fkcliente);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
