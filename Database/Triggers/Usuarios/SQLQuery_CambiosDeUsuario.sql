-- Trigger para auditoría de cambios en Usuarios
CREATE TRIGGER trg_Usuarios_Auditoria
ON Usuarios
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Tabla temporal para almacenar cambios
    DECLARE @Cambios TABLE (
        TipoOperacion VARCHAR(10),
        IdUsuario INT,
        FechaOperacion DATETIME DEFAULT GETDATE(),
        UsuarioEjecutor NVARCHAR(100),
        Detalles NVARCHAR(MAX)
    );
    
    -- Determinar tipo de operación y capturar datos
    DECLARE @TipoOperacion VARCHAR(10);
    
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @TipoOperacion = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted)
        SET @TipoOperacion = 'INSERT';
    ELSE
        SET @TipoOperacion = 'DELETE';
    
    -- Capturar información del usuario ejecutor
    DECLARE @UsuarioEjecutor NVARCHAR(100) = SYSTEM_USER;
    
    -- Procesar según tipo de operación
    IF @TipoOperacion = 'INSERT'
    BEGIN
        INSERT INTO @Cambios (TipoOperacion, IdUsuario, UsuarioEjecutor, Detalles)
        SELECT 
            'INSERT',
            i.IdUsuario,
            @UsuarioEjecutor,
            JSON_QUERY((
                SELECT 
                    'Nuevo registro' AS Accion,
                    i.Nombre,
                    i.Email
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            ))
        FROM inserted i;
    END
    ELSE IF @TipoOperacion = 'UPDATE'
    BEGIN
        INSERT INTO @Cambios (TipoOperacion, IdUsuario, UsuarioEjecutor, Detalles)
        SELECT 
            'UPDATE',
            i.IdUsuario,
            @UsuarioEjecutor,
            JSON_QUERY((
                SELECT 
                    'Registro actualizado' AS Accion,
                    CASE WHEN i.Nombre <> d.Nombre THEN CONCAT('Nombre: ', d.Nombre, ' ? ', i.Nombre) ELSE NULL END AS CambioNombre,
                    CASE WHEN i.Email <> d.Email THEN CONCAT('Email: ', d.Email, ' ? ', i.Email) ELSE NULL END AS CambioEmail
                FOR JSON PATH
            ))
        FROM inserted i
        INNER JOIN deleted d ON i.IdUsuario = d.IdUsuario
        WHERE i.Nombre <> d.Nombre OR i.Email <> d.Email;
    END
    ELSE IF @TipoOperacion = 'DELETE'
    BEGIN
        INSERT INTO @Cambios (TipoOperacion, IdUsuario, UsuarioEjecutor, Detalles)
        SELECT 
            'DELETE',
            d.IdUsuario,
            @UsuarioEjecutor,
            JSON_QUERY((
                SELECT 
                    'Registro eliminado' AS Accion,
                    d.Nombre,
                    d.Email
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            ))
        FROM deleted d;
    END
    
    -- Insertar en tabla de auditoría si hay cambios
    IF EXISTS (SELECT 1 FROM @Cambios)
    BEGIN
        -- Crear tabla de auditoría de usuarios si no existe
        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditoriaUsuarios]') AND type in (N'U'))
        BEGIN
            CREATE TABLE AuditoriaUsuarios (
                IdAuditoria INT IDENTITY(1,1) PRIMARY KEY,
                TipoOperacion VARCHAR(10),
                IdUsuario INT,
                FechaOperacion DATETIME DEFAULT GETDATE(),
                UsuarioEjecutor NVARCHAR(100),
                Detalles NVARCHAR(MAX),
                IpAddress NVARCHAR(50) DEFAULT NULL
            );
            
            CREATE INDEX IX_AuditoriaUsuarios_Fecha ON AuditoriaUsuarios(FechaOperacion);
            CREATE INDEX IX_AuditoriaUsuarios_Usuario ON AuditoriaUsuarios(IdUsuario);
        END
        
        -- Insertar los cambios
        INSERT INTO AuditoriaUsuarios (TipoOperacion, IdUsuario, UsuarioEjecutor, Detalles)
        SELECT TipoOperacion, IdUsuario, UsuarioEjecutor, Detalles
        FROM @Cambios;
    END
END
GO