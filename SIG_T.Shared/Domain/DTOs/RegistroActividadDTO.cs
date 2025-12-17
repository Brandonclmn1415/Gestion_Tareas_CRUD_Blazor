namespace SIG_T.Shared.Domain.DTOs;

/// <summary>
/// Data Transfer Object for Activity Log entries
/// </summary>
public class RegistroActividadDto
{
    public int Id { get; set; }
    public int TareaId { get; set; }
    public string Accion { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public string Usuario { get; set; } = string.Empty;
}