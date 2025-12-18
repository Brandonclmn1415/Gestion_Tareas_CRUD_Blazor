-- Registrar la creacion de una tarea
CREATE TRIGGER trg_RegistrarCreacionTarea
ON Tareas
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Registro_Actividad (TareaID, UsuarioID, Accion, Descripcion)
    SELECT 
        i.ID,
        i.UsuarioID,
        'CREACION',
        'Tarea creada: ' + i.Titulo
    FROM inserted i;
END;
GO