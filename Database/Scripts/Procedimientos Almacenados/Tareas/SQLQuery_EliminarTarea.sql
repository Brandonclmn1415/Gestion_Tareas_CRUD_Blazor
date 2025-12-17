-- Eliminar tarea
CREATE PROCEDURE sp_Tarea_Eliminar
    @IdTarea INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        DELETE FROM TareasGestion
        WHERE IdTarea = @IdTarea;
        
        RETURN 1;
    END TRY
    BEGIN CATCH
        RETURN ERROR_NUMBER();
    END CATCH
END
GO