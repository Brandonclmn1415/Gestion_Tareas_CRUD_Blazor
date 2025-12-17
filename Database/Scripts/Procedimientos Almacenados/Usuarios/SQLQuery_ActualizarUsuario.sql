-- Actualizar usuario
CREATE PROCEDURE sp_Usuario_Actualizar
    @IdUsuario INT,
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Verificar si el nuevo email ya existe en otro usuario
        IF EXISTS (SELECT 1 FROM Usuarios WHERE Email = @Email AND IdUsuario != @IdUsuario)
        BEGIN
            RETURN -1; -- Email duplicado
        END
        
        UPDATE Usuarios
        SET 
            Nombre = @Nombre,
            Email = @Email,
            ContraseñaUsuario = ISNULL(@PasswordHash, ContraseñaUsuario)
        WHERE IdUsuario = @IdUsuario;
        
        RETURN 1; -- Éxito
    END TRY
    BEGIN CATCH
        RETURN ERROR_NUMBER();
    END CATCH
END
GO