-- Listar tareas con filtros
CREATE PROCEDURE sp_Tarea_Listar
    @IdUsuarioAsignado INT = NULL,
    @Estado NVARCHAR(50) = NULL,
    @Prioridad NVARCHAR(20) = NULL,
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL,
    @Pagina INT = 1,
    @RegistrosPorPagina INT = 10,
    @TotalRegistros INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Calcular total con filtros
    SELECT @TotalRegistros = COUNT(*)
    FROM TareasGestion t
    INNER JOIN Usuarios ua ON t.IdUsuarioAsignado = ua.IdUsuario
    INNER JOIN Usuarios uc ON t.IdUsuarioCreacion = uc.IdUsuario
    WHERE (@IdUsuarioAsignado IS NULL OR t.IdUsuarioAsignado = @IdUsuarioAsignado)
    AND (@Estado IS NULL OR t.Estado = @Estado)
    AND (@Prioridad IS NULL OR t.Prioridad = @Prioridad)
    AND (@FechaDesde IS NULL OR t.FechaCreacion >= @FechaDesde)
    AND (@FechaHasta IS NULL OR t.FechaCreacion <= @FechaHasta);
    
    -- Obtener registros paginados
    SELECT 
        t.IdTarea,
        t.Titulo,
        LEFT(t.Descripcion, 100) AS DescripcionCorta,
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
    WHERE (@IdUsuarioAsignado IS NULL OR t.IdUsuarioAsignado = @IdUsuarioAsignado)
    AND (@Estado IS NULL OR t.Estado = @Estado)
    AND (@Prioridad IS NULL OR t.Prioridad = @Prioridad)
    AND (@FechaDesde IS NULL OR t.FechaCreacion >= @FechaDesde)
    AND (@FechaHasta IS NULL OR t.FechaCreacion <= @FechaHasta)
    ORDER BY 
        CASE WHEN t.Prioridad = 'Alta' THEN 1
             WHEN t.Prioridad = 'Media' THEN 2
             ELSE 3 END,
        t.FechaVencimiento ASC,
        t.FechaCreacion DESC
    OFFSET (@Pagina - 1) * @RegistrosPorPagina ROWS
    FETCH NEXT @RegistrosPorPagina ROWS ONLY;
END
GO
