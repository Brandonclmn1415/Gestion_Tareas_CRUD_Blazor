-- Leer usuario por Email (para login)
CREATE PROCEDURE sp_Usuario_ObtenerPorEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        IdUsuario,
        Nombre,
        Email,
        ContraseñaUsuario
    FROM Usuarios
    WHERE Email = @Email
END
GO