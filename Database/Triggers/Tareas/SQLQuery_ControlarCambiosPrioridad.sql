-- Trigger para controlar cambios de prioridad
CREATE TRIGGER trg_TareasGestion_ControlPrioridad
ON TareasGestion
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Limitar cambios de prioridad a "Alta" (solo administradores)
    DECLARE @UsuarioActual INT = CAST(CONTEXT_INFO() AS INT);
    
    IF UPDATE(Prioridad)
    BEGIN
        -- Verificar si usuario tiene permisos para cambiar a prioridad Alta
        IF EXISTS (
            SELECT 1 
            FROM inserted i
            INNER JOIN deleted d ON i.IdTarea = d.IdTarea
            WHERE i.Prioridad = 'Alta' 
            AND d.Prioridad <> 'Alta'
            AND NOT EXISTS (
                SELECT 1 
                FROM Usuarios u 
                WHERE u.IdUsuario = @UsuarioActual 
                AND u.Email LIKE '%admin%' -- Ejemplo simple de validación
            )
        )
        BEGIN
            RAISERROR('Solo administradores pueden asignar prioridad Alta', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
    END
END
GO