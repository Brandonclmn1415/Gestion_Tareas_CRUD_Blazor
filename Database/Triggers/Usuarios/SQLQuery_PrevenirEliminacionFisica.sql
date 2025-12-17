-- Trigger para prevenir eliminación física de usuarios
CREATE TRIGGER trg_Usuarios_PrevenirDelete
ON Usuarios
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    RAISERROR('No se permite eliminar usuarios físicamente. Use la desactivación', 16, 1);
    RETURN;
END
GO