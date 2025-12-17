using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIG_T.Shared.Domain.Entities;

/// <summary>
/// User entity representing a system user
/// </summary>
[Table("Usuarios", Schema = "dbo")]
public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public bool Activo { get; set; } = true;

    // Navigation property for related tasks
    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();

    // Computed property for full name
    [NotMapped]
    public string NombreCompleto => $"{Nombre} {Apellido}";
}