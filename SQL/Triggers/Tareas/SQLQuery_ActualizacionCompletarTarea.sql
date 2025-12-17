-- Trigger para actualización automática al completar tarea
CREATE TRIGGER trg_TareasGestion_AlCompletar
ON TareasGestion
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Si una tarea cambia a estado "Completada"
    IF UPDATE(Estado)
    BEGIN
        
        -- Registrar en historial
        INSERT INTO TareasGestion(IdTarea, Estado, IdUsuarioAsignado)
        SELECT 
            i.IdTarea,
            'Tarea completada',
            i.IdUsuarioAsignado
        FROM inserted i
        INNER JOIN deleted d ON i.IdTarea = d.IdTarea
        WHERE i.Estado = 'Completada' 
        AND d.Estado <> 'Completada';
    END
END
GO