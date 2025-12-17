-- Listar todos los usuarios
CREATE PROCEDURE sp_Usuario_Listar
    @Pagina INT = 1,
    @RegistrosPorPagina INT = 10,
    @TotalRegistros INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Calcular total de registros
    SELECT @TotalRegistros = COUNT(*)
    FROM Usuarios
    
    -- Obtener registros paginados
    SELECT 
        IdUsuario,
        Nombre,
        Email
    FROM Usuarios
    ORDER BY Nombre
    OFFSET (@Pagina - 1) * @RegistrosPorPagina ROWS
    FETCH NEXT @RegistrosPorPagina ROWS ONLY;
END
GO