-- Leer usuario por ID
CREATE PROCEDURE sp_Usuario_ObtenerPorId
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        IdUsuario,
        Nombre,
        Email,
        NombreUsuario
    FROM Usuarios
    WHERE IdUsuario = @IdUsuario
END
GO