-- Crear tarea
CREATE PROCEDURE sp_Tarea_Crear
    @Titulo NVARCHAR(200),
    @Descripcion NVARCHAR(MAX),
    @IdUsuarioAsignado INT,
    @FechaVencimiento DATETIME = NULL,
    @Estado NVARCHAR(50) = 'Pendiente',
    @Prioridad NVARCHAR(20) = 'Media',
    @IdUsuarioCreacion INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        INSERT INTO TareasGestion (
            Titulo,
            Descripcion,
            IdUsuarioAsignado,
            FechaVencimiento,
            Estado,
            Prioridad,
            IdUsuarioCreacion
        )
        VALUES (
            @Titulo,
            @Descripcion,
            @IdUsuarioAsignado,
            @FechaVencimiento,
            @Estado,
            @Prioridad,
            @IdUsuarioCreacion
        );
        
        SELECT SCOPE_IDENTITY() AS IdTarea;
        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN ERROR_NUMBER();
    END CATCH
END
GO