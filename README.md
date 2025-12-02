#  **README -- Configuración del Proyecto y Creación de la Base de Datos (.NET + EF Core + SQL Server)**

Este proyecto utiliza **ASP.NET Core 8**, **Entity Framework Core 8**,
**SQL Server** y un cliente **Blazor WebAssembly** para consumir la API.

------------------------------------------------------------------------

##  1. **Instalar los paquetes NuGet necesarios**

En el **proyecto del backend (API)** instala los siguientes paquetes,
todos en versión **8.0.0**:

    Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.0
    Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.0
    Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.0

------------------------------------------------------------------------

##  2. **Configurar la cadena de conexión**

En el archivo `appsettings.json`:

    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=GestionTareasDB;Trusted_Connection=True;TrustServerCertificate=True;"
      }
    }

Si es con usuario y contraseña:

    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=GestionTareasDB;User Id=sa;Password=tu_password;TrustServerCertificate=True;"
      }
    }

Registrar en `Program.cs`:

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

------------------------------------------------------------------------

##  3. **Crear Base de Datos**

    Add-Migration InitialCreate
    Update-Database

Para nuevos cambios:

    Add-Migration NombreDeLaMigracion
    Update-Database

------------------------------------------------------------------------

##  4. **Configurar URL del backend en Blazor WASM**

En `Program.cs` del cliente:

    var url = "https://localhost:7161/";
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });

Cambiar por la URL deseada:

    var url = "https://tu-servidor-o-railway.com/";

------------------------------------------------------------------------

##  5. **Estructura recomendada**

    /Backend
    /Frontend

------------------------------------------------------------------------

##  6. **Ejecución**

1.  Ejecutar la API\
2.  Ejecutar Blazor WASM\
3.  La comunicación será automática mediante la URL configurada

------------------------------------------------------------------------

## 7. **El programa no tiene tantos estilos CSS ya que me enfoque mas en la logica y funcionamiento del programa que en los estilos por el poco tiempo**

Muchas gracias por la oportunidad y quedo atento a cualquier novedad