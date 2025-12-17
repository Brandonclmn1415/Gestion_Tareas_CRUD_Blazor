-- Actualizar tarea
CREATE PROCEDURE sp_Tarea_Actualizar
    @IdTarea INT,
    @Titulo NVARCHAR(200) = NULL,
    @Descripcion NVARCHAR(MAX) = NULL,
    @IdUsuarioAsignado INT = NULL,
    @FechaVencimiento DATETIME = NULL,
    @Estado NVARCHAR(50) = NULL,
    @Prioridad NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        UPDATE TareasGestion
        SET 
            Titulo = ISNULL(@Titulo, Titulo),
            Descripcion = ISNULL(@Descripcion, Descripcion),
            IdUsuarioAsignado = ISNULL(@IdUsuarioAsignado, IdUsuarioAsignado),
            FechaVencimiento = ISNULL(@FechaVencimiento, FechaVencimiento),
            Estado = ISNULL(@Estado, Estado),
            Prioridad = ISNULL(@Prioridad, Prioridad)
        WHERE IdTarea = @IdTarea;
        
        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN ERROR_NUMBER();
    END CATCH
END
GO