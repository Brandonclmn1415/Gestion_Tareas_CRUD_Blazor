-- Trigger para validar email único antes de INSERT/UPDATE
CREATE TRIGGER trg_Usuarios_ValidarEmail
ON Usuarios
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Verificar duplicados en INSERT
        IF EXISTS (
            SELECT 1 
            FROM inserted i
            WHERE EXISTS (
                SELECT 1 
                FROM Usuarios u 
                WHERE u.Email = i.Email 
                AND u.IdUsuario != ISNULL(i.IdUsuario, -1)
            )
        )
        BEGIN
            RAISERROR('El email ya está registrado en el sistema', 16, 1);
            RETURN;
        END
        
        -- Si es UPDATE, procesar actualización
        IF EXISTS (SELECT * FROM deleted)
        BEGIN
            UPDATE u
            SET 
                u.Nombre = i.Nombre,
                u.Email = i.Email,
                u.ContraseñaUsuario = i.ContraseñaUsuario
            FROM Usuarios u
            INNER JOIN inserted i ON u.IdUsuario = i.IdUsuario;
        END
        ELSE -- Es INSERT
        BEGIN
            INSERT INTO Usuarios (Nombre, Email, ContraseñaUsuario)
            SELECT Nombre, Email, ContraseñaUsuario
            FROM inserted;
        END
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO