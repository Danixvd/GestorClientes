# GestorClientes

Práctica Programada 1 – SC-701 Programación Avanzada

Sistema web para llevar el control de clientes y sus números de teléfono. Permite **listar**, **consultar el detalle**, **registrar**, **modificar** y **borrar** clientes; al borrar un cliente también se eliminan sus teléfonos.

## Integrantes

| Integrante | Responsabilidad |
|---|---|
| Erick Daniel Valverde Duran | Persona 1 – Arquitectura y base del proyecto |
| Oliver Roman Quiroz | Persona 2 – Listado y detalle de clientes |
| Fabricio Jose Quesada Araya | Persona 3 – Registrar y modificar clientes |
| Pablo Andres Castillo Zuniga | Persona 4 – Borrar, pruebas e integración |

## Repositorio

https://github.com/Danixvd/GestorClientes

## Tecnologías

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core con SQLite (Database First, por scaffold)
- AutoMapper
- Bootstrap y jQuery

## Arquitectura

La solución está separada en cuatro proyectos por capas:

| Proyecto | Responsabilidad | Depende de |
|---|---|---|
| `GestorClientes.Domain` | Entidades `Cliente` y `Telefono` y sus reglas de negocio (`Cliente.ReglasNegocio.cs`) | Nada |
| `GestorClientes.Infrastructure` | `ApplicationDbContext`, base de datos `clientes.db`, `IClienteRepository` y `ClienteRepository` | Domain |
| `GestorClientes.Application` | Servicios (`IClienteServicio`, `ClienteServicio`), DTOs, `Respuesta<T>` y perfil de AutoMapper (`MapeoClases`) | Domain, Infrastructure |
| `GestorClientes` (web) | Controllers, vistas Razor, `wwwroot` y configuración en `Program.cs` | Application, Infrastructure |

Flujo de una petición: **Vista → `ClienteController` → `IClienteServicio` → `IClienteRepository` → `ApplicationDbContext` → SQLite**.
Los controllers solo hablan con los servicios, nunca directamente con el repositorio ni con el `DbContext`.

### Base de datos

- **Cliente**: Id, Nombre, Apellido1, Apellido2, Correo, FechaCreacion.
- **Telefono**: Id, Numero, FKCliente, FechaCreacion.
- Relación 1 a N (un cliente tiene muchos teléfonos). La llave foránea tiene `ON DELETE CASCADE`, por lo que al borrar un cliente se borran sus teléfonos.

## Paquetes NuGet

| Paquete | Versión | Proyecto |
|---|---|---|
| AutoMapper | 16.2.0 | Application |
| Microsoft.EntityFrameworkCore.Sqlite | 10.0.12 | Infrastructure |
| Microsoft.EntityFrameworkCore.Tools | 10.0.12 | Infrastructure |
| Microsoft.EntityFrameworkCore.Design | 10.0.12 | Infrastructure, GestorClientes |

## Principios SOLID aplicados

- **S – Responsabilidad única:** cada clase tiene una sola tarea. El repositorio solo accede a datos, el servicio aplica las reglas de negocio y arma la respuesta, y el controller solo maneja la petición HTTP.
- **O – Abierto/cerrado:** las reglas de negocio están en métodos separados de la entidad (`ValidarReglaNegocio...`), así que se pueden agregar reglas nuevas sin modificar el repositorio.
- **I – Segregación de interfaces:** las interfaces son pequeñas y específicas: `IClienteRepository` para acceso a datos e `IClienteServicio` para la lógica de aplicación.
- **D – Inversión de dependencias:** las clases dependen de abstracciones y no de implementaciones. `ClienteServicio` recibe `IClienteRepository` y `ClienteController` recibe `IClienteServicio`; las implementaciones se registran en `Program.cs` con `AddScoped`.

## Patrones utilizados

- **MVC (Modelo–Vista–Controlador):** separa la interfaz (vistas Razor), el control del flujo (controllers) y los datos.
- **Repository:** `ClienteRepository` encapsula el acceso a datos con Entity Framework Core.
- **Inyección de dependencias:** el contenedor de ASP.NET Core crea y entrega el `DbContext`, los repositorios, los servicios y el `IMapper`.
- **DTO (Data Transfer Object):** `ClienteDto` y `TelefonoDto` evitan exponer las entidades del dominio a la vista.
- **Result (`Respuesta<T>`):** las operaciones de registrar, modificar y borrar devuelven una respuesta estándar con `EsCorrecto`, `Mensaje`, `Codigo` (400 regla de negocio, 500 error) y `Dato`, en lugar de usar excepciones para las reglas de negocio.
- **Mapper (AutoMapper):** convierte entre entidades y DTOs sin asignar propiedad por propiedad.

## Buenas prácticas

- Métodos asíncronos y `CancellationToken` propagado hasta Entity Framework Core.
- `AsNoTracking()` en las consultas de solo lectura.
- Sin valores nulos: si un cliente no existe, el repositorio devuelve un objeto vacío.
- Los repositorios verifican que el registro exista antes de actualizar o eliminar, y devuelven `bool` según `SaveChangesAsync() > 0`.
- Confirmación (modal) antes de eliminar un cliente.
- Validaciones en los DTOs (Data Annotations) y reglas de negocio en el Domain.
- Métodos documentados (propósito, parámetros y retorno).

## Cómo ejecutar

1. Clonar el repositorio y abrir `GestorClientes.slnx` en Visual Studio.
2. Dejar `GestorClientes` (proyecto web) como proyecto de inicio.
3. Presionar **F5**. La cadena de conexión es relativa (`../GestorClientes.Infrastructure/clientes.db`), así que no hay que cambiarla.
4. Entrar a **Clientes** en el menú.
