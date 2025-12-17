-- Crear usuario (Register)
CREATE PROCEDURE sp_Usuario_Crear
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Contraseña NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Verificar si el email ya existe
        IF EXISTS (SELECT 1 FROM Usuarios WHERE Email = @Email)
        BEGIN
            RETURN -1; -- Código de error para email duplicado
        END
        
        INSERT INTO Usuarios (Nombre, Email, ContraseñaUsuario)
        VALUES (@Nombre, @Email, @Contraseña);
        
        SELECT SCOPE_IDENTITY() AS IdUsuario;
        RETURN 1; -- Éxito
    END TRY
    BEGIN CATCH
        RETURN ERROR_NUMBER();
    END CATCH
END
GO
