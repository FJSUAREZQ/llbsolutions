# llbsolutions
llbsolutions proj

# Sistema de Carrito de Compras – Blazor Web app (.NET 8)

Aplicación web desarrollada en **Blazor Web app** con un enfoque arquitectónico limpio usando **MVVM** y manejo de estado desacoplado. 
Incorpora persistencia de sesión segura con `ProtectedLocalStorage`, sistema de fidelización de clientes con puntos canjeables, 
y un carrito de compras inteligente completamente gestionado por servicios.

---

## Funcionalidades principales

- Arquitectura MVVM desacoplada con `CommunityToolkit.Mvvm`
- Gestión de carrito en memoria con subtotales, descuentos y puntos ganados
- Sesiones autenticadas con almacenamiento cifrado (`ProtectedLocalStorage`)
- Canje de puntos para aplicar descuentos dinámicos
- Confirmación de compra con comando para descarga de factura (pendiente PDF)
- Feedback visual ante errores y operaciones exitosas
- UI responsiva con Bootstrap 5 + Bootstrap Icons

---

## Estructura del proyecto
Clean Architecture:
 - FrontEnd (Blazor Web App) - MVVM
 - Application
 - Domain
 - Infrastructure
 - Shared

## Tecnologías usadas

- [.NET 8](https://dotnet.microsoft.com/)
- Blazor Server (Componentes interactivos con renderizado dinámico)
- Entity Framework Core + SQLite
- ProtectedBrowserStorage para persistencia de sesión
- Bootstrap 5
- CommunityToolkit.Mvvm (RelayCommand, ViewModelBase)
- Bootstrap Icons
- QuestPDF

---

## Cómo ejecutar el proyecto

- Clona el repositorio:
	git clone https://github.com/tu-usuario/tu-repo.git
	cd tu-repo
- Restaura paquetes y compila:
	dotnet restore
	dotnet build
- Establece el proyecto Blazor como proyecto de inicio
	Si usas Visual Studio:
		- Haz clic derecho sobre el proyecto BlazorApp (o como se llame tu .csproj)
		- Selecciona "Establecer como proyecto de inicio"
	- O desde terminal:
		dotnet run --project BlazorApp/BlazorApp.csproj
- Accede a la aplicación: Abre tu navegador en https://localhost:5121 o la URL que indique la consola.


 Seguridad de sesión
- El estado de autenticación se guarda cifrado en el navegador usando ProtectedLocalStorage.
- Si el usuario no está autenticado o su sesión caduca, se redirige automáticamente a la vista /Login.

 Consideraciones
- El archivo .gitignore está configurado para permitir subir la base de datos SQLite y las migraciones.
- El proyecto sigue una estructura modular y escalable: cada vista tiene su propio ViewModel e inyección limpia de dependencias.
- El sistema está diseñado para ser extensible: puedes conectar una API externa, exportar facturas como PDF o implementar Identity Server sin romper la arquitectura actual.

 Siguiente etapa sugerida
- Dashboard de historial de compras por usuario
- Vinculación con sistema de recompensas o cupones
- Autenticación externa (Google, Microsoft, etc.)

