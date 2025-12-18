-- SP para actualizar una tarea existente
CREATE PROCEDURE sp_ActualizarTarea
    @TareaID INT,
    @Titulo NVARCHAR(200) = NULL,
    @Descripcion NVARCHAR(MAX) = NULL,
    @Estado NVARCHAR(50) = NULL,
    @Prioridad INT = NULL,
    @FechaVencimiento DATETIME = NULL,
    @UsuarioID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validar que la tarea existe
        IF NOT EXISTS (SELECT 1 FROM Tareas WHERE ID = @TareaID)
        BEGIN
            RAISERROR('La tarea especificada no existe.', 16, 1);
            RETURN;
        END
        
        -- Validar usuario si se proporciona
        IF @UsuarioID IS NOT NULL 
           AND NOT EXISTS (SELECT 1 FROM Usuarios WHERE ID = @UsuarioID AND Activo = 1)
        BEGIN
            RAISERROR('El usuario especificado no existe o está inactivo.', 16, 1);
            RETURN;
        END
        
        -- Actualizar solo los campos proporcionados
        UPDATE Tareas
        SET 
            Titulo = ISNULL(@Titulo, Titulo),
            Descripcion = ISNULL(@Descripcion, Descripcion),
            Estado = ISNULL(@Estado, Estado),
            Prioridad = ISNULL(@Prioridad, Prioridad),
            FechaVencimiento = ISNULL(@FechaVencimiento, FechaVencimiento),
            UsuarioID = ISNULL(@UsuarioID, UsuarioID)
        WHERE ID = @TareaID;
        
        -- Registrar la actividad de actualización
        INSERT INTO Registro_Actividad (TareaID, UsuarioID, Accion, Descripcion)
        VALUES (
            @TareaID,
            ISNULL(@UsuarioID, (SELECT UsuarioID FROM Tareas WHERE ID = @TareaID)),
            'ACTUALIZACION',
            'Tarea actualizada mediante procedimiento almacenado'
        );
        
        SELECT 'Tarea actualizada exitosamente' AS Mensaje;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO