using SIG_T.Shared.Models;
using SIG_T.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SIG_T.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; } = null!;
    public DbSet<RegistroDeActividad> RegistroDeActividad { get; set; } = null!;

    // Shared domain entities from SIG_T.Shared
    public DbSet<Tarea> Tareas { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.DueDate);
        });

        modelBuilder.Entity<RegistroDeActividad>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TareaId).IsRequired();
            entity.Property(e => e.Accion).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Usuario).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.Estado).IsRequired();
            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.Tareas)
                  .HasForeignKey(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
        });
    }
}
