-- Vista para tareas con usuarios
CREATE VIEW vw_TareasConUsuarios
AS
SELECT 
    t.ID AS TareaID,
    t.Titulo,
    t.Descripcion,
    t.Estado,
    t.Prioridad,
    t.FechaCreacion,
    t.FechaVencimiento,
    u.ID AS UsuarioID,
    CONCAT(u.Nombre, ' ', u.Apellido) AS NombreCompleto,
    u.Email,
    DATEDIFF(DAY, GETDATE(), t.FechaVencimiento) AS DiasRestantes,
    CASE 
        WHEN t.Estado = 'Completada' THEN 'Completada'
        WHEN t.FechaVencimiento < GETDATE() THEN 'Vencida'
        WHEN DATEDIFF(DAY, GETDATE(), t.FechaVencimiento) <= 2 THEN 'Por Vencer'
        ELSE 'En Tiempo'
    END AS EstadoVencimiento
FROM Tareas t
INNER JOIN Usuarios u ON t.UsuarioID = u.ID
WHERE u.Activo = 1;
GO