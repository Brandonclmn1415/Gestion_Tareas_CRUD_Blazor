CREATE INDEX IX_TareasGestion_UsuarioAsignado ON TareasGestion(IdUsuarioAsignado);
CREATE INDEX IX_TareasGestion_Estado ON TareasGestion(Estado);
CREATE INDEX IX_TareasGestion_FechaVencimiento ON TareasGestion(FechaVencimiento);
CREATE INDEX IX_Usuarios_Email ON Usuarios(Email);

