-- Cambiar estado de tarea
CREATE PROCEDURE sp_Tarea_CambiarEstado
    @IdTarea INT,
    @Estado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        UPDATE TareasGestion
        SET Estado = @Estado
        WHERE IdTarea = @IdTarea;
        
        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN ERROR_NUMBER();
    END CATCH
END
GO
