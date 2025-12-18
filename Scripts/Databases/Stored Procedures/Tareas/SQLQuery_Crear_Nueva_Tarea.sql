-- SP Crear una nueva tarea
CREATE PROCEDURE sp_CrearTarea
    @Titulo NVARCHAR(200),
    @Descripcion NVARCHAR(MAX) = NULL,
    @Estado NVARCHAR(50) = 'Pendiente',
    @Prioridad INT = 3,
    @FechaVencimiento DATETIME = NULL,
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validar que el usuario existe
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE ID = @UsuarioID AND Activo = 1)
        BEGIN
            RAISERROR('El usuario especificado no existe o está inactivo.', 16, 1);
            RETURN;
        END
        
        -- Insertar la tarea
        INSERT INTO Tareas (
            Titulo, 
            Descripcion, 
            Estado, 
            Prioridad, 
            FechaVencimiento, 
            UsuarioID,
            FechaCreacion
        )
        VALUES (
            @Titulo,
            @Descripcion,
            @Estado,
            @Prioridad,
            @FechaVencimiento,
            @UsuarioID,
            GETDATE()
        );
        
        -- Devolver el ID de la tarea creada
        SELECT SCOPE_IDENTITY() AS TareaID, 
               'Tarea creada exitosamente' AS Mensaje;
        
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