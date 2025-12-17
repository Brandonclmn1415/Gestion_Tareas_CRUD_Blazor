-- Trigger para auditoría y registro automático de cambios
CREATE TRIGGER trg_TareasGestion_Auditoria
ON TareasGestion
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Registrar en HistorialTareas para cambios
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        -- Es una actualización
        INSERT INTO TareasGestion (IdTarea, Estado, Prioridad)
        SELECT 
            i.IdTarea,
            CONCAT(
                'Tarea actualizada - ',
                'Estado: ', d.Estado, ' ? ', i.Estado, ' | ',
                'Asignado: Usuario ', d.IdUsuarioAsignado, ' ? ', i.IdUsuarioAsignado, ' | ',
                'Prioridad: ', d.Prioridad, ' ? ', i.Prioridad,
                CASE WHEN i.FechaVencimiento <> d.FechaVencimiento 
                     THEN CONCAT(' | Fecha: ', FORMAT(d.FechaVencimiento, 'dd/MM/yyyy'), ' ? ', FORMAT(i.FechaVencimiento, 'dd/MM/yyyy'))
                     ELSE '' END
            ),
            -- Usuario que realiza el cambio (podría venir de una variable de sesión)
            ISNULL(CAST(CONTEXT_INFO() AS INT), i.IdUsuarioCreacion)
        FROM inserted i
        INNER JOIN deleted d ON i.IdTarea = d.IdTarea
        WHERE i.Estado <> d.Estado 
           OR i.IdUsuarioAsignado <> d.IdUsuarioAsignado
           OR i.Prioridad <> d.Prioridad
           OR i.FechaVencimiento <> d.FechaVencimiento;
    END
    ELSE IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Es una inserción
        INSERT INTO TareasGestion(IdTarea, Estado, Prioridad)
        SELECT 
            IdTarea,
            CONCAT('Tarea creada - ', Titulo, ' (', Estado, ')'),
            IdUsuarioCreacion
        FROM inserted;
    END
    
    -- Notificar tareas urgentes (Prioridad Alta con fecha próxima)
    DECLARE @Hoy DATE = GETDATE();
    
    IF EXISTS (
        SELECT 1 
        FROM inserted i 
        WHERE i.Prioridad = 'Alta' 
        AND i.FechaVencimiento <= DATEADD(DAY, 2, @Hoy)
        AND i.Estado NOT IN ('Completada', 'Cancelada')
    )
    BEGIN
        -- Podrías implementar notificación por email o mensaje aquí
        PRINT 'ATENCIÓN: Se ha creado/actualizado una tarea URGENTE con vencimiento próximo.';
    END
END
GO