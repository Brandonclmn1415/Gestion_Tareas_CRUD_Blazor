using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIG_T.Shared.Domain.Entities;

/// <summary>
/// Task entity representing a work task
/// </summary>
[Table("Tareas", Schema = "dbo")]
public class Tarea
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descripcion { get; set; }

    [Required]
    public int Estado { get; set; } = 0; // 0: Pendiente, 1: En Progreso, 2: Completada

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaVencimiento { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    public DateTime? FechaCompletada { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UsuarioId))]
    public virtual Usuario Usuario { get; set; } = null!;

    // Computed properties
    [NotMapped]
    public string EstadoDescripcion => Estado switch
    {
        0 => "Pendiente",
        1 => "En Progreso", 
        2 => "Completada",
        _ => "Desconocido"
    };

    [NotMapped]
    public bool EstaVencida => FechaVencimiento.HasValue && 
                              FechaVencimiento.Value < DateTime.UtcNow && 
                              Estado != 2;

    [NotMapped]
    public int DiasTranscurridos => (int)(DateTime.UtcNow - FechaCreacion).TotalDays;
}