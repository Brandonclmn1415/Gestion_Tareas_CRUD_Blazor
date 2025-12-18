using System.ComponentModel.DataAnnotations;

namespace SIG_T.Domain.DTO.Tarea;

/// <summary>
/// DTO for creating a new task
/// </summary>
public class TareaCreateDTO
{
    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio")]
    [Range(0, 2, ErrorMessage = "El estado debe ser 0 (Pendiente), 1 (En Progreso) o 2 (Completada)")]
    public int Estado { get; set; } = 0;

    public DateTime? FechaVencimiento { get; set; }

    [Required(ErrorMessage = "El usuario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un usuario válido")]
    public int UsuarioId { get; set; }
}

/// <summary>
/// DTO for updating an existing task
/// </summary>
public class TareaUpdateDTO
{
    [Required(ErrorMessage = "El ID es obligatorio")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio")]
    [Range(0, 2, ErrorMessage = "El estado debe ser 0 (Pendiente), 1 (En Progreso) o 2 (Completada)")]
    public int Estado { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    [Required(ErrorMessage = "El usuario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un usuario válido")]
    public int UsuarioId { get; set; }
}

/// <summary>
/// DTO for task response (includes user information)
/// </summary>
public class TareaResponseDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int Estado { get; set; }
    public string EstadoDescripcion { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public DateTime? FechaCompletada { get; set; }
    public int UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string EmailUsuario { get; set; } = string.Empty;
    public bool EstaVencida { get; set; }
    public int DiasTranscurridos { get; set; }
}

/// <summary>
/// DTO for task list response with filtering options
/// </summary>
public class TareaListDTO
{
    public IEnumerable<TareaResponseDTO> Tareas { get; set; } = new List<TareaResponseDTO>();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int TamanoPagina { get; set; }
    public int TotalPaginas => (int)Math.Ceiling((double)Total / TamanoPagina);
}

/// <summary>
/// DTO for task statistics
/// </summary>
public class TareaEstadisticasDTO
{
    public int TotalTareas { get; set; }
    public int TareasPendientes { get; set; }
    public int TareasEnProgreso { get; set; }
    public int TareasCompletadas { get; set; }
    public int TareasVencidas { get; set; }
    public double PorcentajeCompletado { get; set; }
    public double PromedioDiasCompletar { get; set; }
}