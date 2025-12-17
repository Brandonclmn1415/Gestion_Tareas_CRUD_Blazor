INSERT INTO Usuarios (Nombre,Email,NombreUsuario,ContraseñaUsuario) 
VALUES 
('Administrador','adminitrador@gmail.com','Admin','admin123'),
('Brandon','brandon@gmail.com','BrandonVega','brandon123'),
('Steven','stiven@gmail.com','StivenHer','stiven123');

INSERT INTO TareasGestion (Titulo, Descripcion, IdUsuarioAsignado, IdUsuarioCreacion, FechaVencimiento, Prioridad)
VALUES
('Revisar Documentacion','Revisar los documentos del proyecto',2,1,DATEADD(DAY, 7, GETDATE()),'Media'),
('Actualizar Base De Datos','Migrar datos a la nueva version',2,1,DATEADD(DAY, 3, GETDATE()),'Alta');