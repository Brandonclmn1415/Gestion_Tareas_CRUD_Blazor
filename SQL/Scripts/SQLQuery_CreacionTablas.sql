CREATE TABLE Usuarios(
    IdUsuario int IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    NombreUsuario NVARCHAR(100) UNIQUE NOT NULL,
    ContraseñaUsuario NVARCHAR(100) NOT NULL
);

CREATE TABLE TareasGestion (
    IdTarea INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    IdUsuarioAsignado INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaVencimiento DATETIME,
    Estado NVARCHAR(50) DEFAULT 'Pendiente',
    Prioridad NVARCHAR(20) DEFAULT 'Media',
    IdUsuarioCreacion INT NOT NULL,
    FOREIGN KEY (IdUsuarioAsignado) REFERENCES Usuarios(IdUsuario),
    FOREIGN KEY (IdUsuarioCreacion) REFERENCES Usuarios(IdUsuario)
);

