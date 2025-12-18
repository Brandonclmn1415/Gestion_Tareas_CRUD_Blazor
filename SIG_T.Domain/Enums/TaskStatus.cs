namespace SIG_T.Domain.Enums;

/// <summary>
/// Enumeration for task states
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Task is pending/not started
    /// </summary>
    Pendiente = 0,

    /// <summary>
    /// Task is currently in progress
    /// </summary>
    EnProgreso = 1,

    /// <summary>
    /// Task has been completed
    /// </summary>
    Completada = 2
}

/// <summary>
/// Helper extensions for TaskStatus enum
/// </summary>
public static class TaskStatusExtensions
{
    /// <summary>
    /// Gets the display name for the task status
    /// </summary>
    /// <param name="status">The task status</param>
    /// <returns>Display name</returns>
    public static string GetDisplayName(this TaskStatus status)
    {
        return status switch
        {
            TaskStatus.Pendiente => "Pendiente",
            TaskStatus.EnProgreso => "En Progreso",
            TaskStatus.Completada => "Completada",
            _ => "Desconocido"
        };
    }

    /// <summary>
    /// Gets all task status options for dropdowns
    /// </summary>
    /// <returns>List of status options</returns>
    public static IEnumerable<(int Value, string DisplayName)> GetAllStatuses()
    {
        return new[]
        {
            (0, "Pendiente"),
            (1, "En Progreso"),
            (2, "Completada")
        };
    }
}
