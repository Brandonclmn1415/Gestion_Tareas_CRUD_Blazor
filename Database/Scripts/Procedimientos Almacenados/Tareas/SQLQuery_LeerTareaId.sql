-- Leer tarea por ID
CREATE PROCEDURE sp_Tarea_ObtenerPorId
    @IdTarea INT
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
        t.IdUsuarioCreacion,
        uc.Nombre AS UsuarioCreacion
    FROM TareasGestion t
    INNER JOIN Usuarios ua ON t.IdUsuarioAsignado = ua.IdUsuario
    INNER JOIN Usuarios uc ON t.IdUsuarioCreacion = uc.IdUsuario
    WHERE t.IdTarea = @IdTarea;
END
GO