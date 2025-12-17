-- Listar tareas próximas a vencer
CREATE PROCEDURE sp_Tarea_ProximasVencer
    @Dias INT = 7,
    @IdUsuarioAsignado INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.IdTarea,
        t.Titulo,
        t.Descripcion,
        t.IdUsuarioAsignado,
        ua.Nombre AS UsuarioAsignado,
        t.FechaCreacion,
        t.FechaVencimiento,
        t.Estado,
        t.Prioridad,
        DATEDIFF(DAY, GETDATE(), t.FechaVencimiento) AS DiasRestantes
    FROM TareasGestion t
    INNER JOIN Usuarios ua ON t.IdUsuarioAsignado = ua.IdUsuario
    WHERE t.Estado NOT IN ('Completada', 'Cancelada')
    AND t.FechaVencimiento BETWEEN GETDATE() AND DATEADD(DAY, @Dias, GETDATE())
    AND (@IdUsuarioAsignado IS NULL OR t.IdUsuarioAsignado = @IdUsuarioAsignado)
    ORDER BY t.FechaVencimiento ASC;
END
GO
