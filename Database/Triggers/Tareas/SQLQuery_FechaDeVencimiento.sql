-- Trigger para validar fecha de vencimiento
CREATE TRIGGER trg_TareasGestion_ValidarFecha
ON TareasGestion
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validar que la fecha de vencimiento no sea anterior a la fecha actual
        IF EXISTS (
            SELECT 1 
            FROM inserted 
            WHERE FechaVencimiento < CAST(GETDATE() AS DATE)
        )
        BEGIN
            RAISERROR('La fecha de vencimiento no puede ser anterior a la fecha actual', 16, 1);
            RETURN;
        END
        
        -- Si es UPDATE, procesar
        IF EXISTS (SELECT * FROM deleted)
        BEGIN
            UPDATE t
            SET 
                t.Titulo = i.Titulo,
                t.Descripcion = i.Descripcion,
                t.IdUsuarioAsignado = i.IdUsuarioAsignado,
                t.FechaVencimiento = i.FechaVencimiento,
                t.Estado = i.Estado,
                t.Prioridad = i.Prioridad
            FROM TareasGestion t
            INNER JOIN inserted i ON t.IdTarea = i.IdTarea;
        END
        ELSE -- Es INSERT
        BEGIN
            INSERT INTO TareasGestion (
                Titulo, Descripcion, IdUsuarioAsignado, 
                FechaVencimiento, Estado, Prioridad, IdUsuarioCreacion
            )
            SELECT 
                Titulo, Descripcion, IdUsuarioAsignado,
                FechaVencimiento, 
                ISNULL(Estado, 'Pendiente'),
                ISNULL(Prioridad, 'Media'),
                IdUsuarioCreacion
            FROM inserted;
        END
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO