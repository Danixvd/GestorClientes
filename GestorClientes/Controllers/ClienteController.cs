using GestorClientes.Application.Dtos;
using GestorClientes.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestorClientes.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteServicio _clienteServicio;

        public ClienteController(IClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        public async Task<IActionResult> Index()
        {
            var clientes =
                await _clienteServicio.ObtenerClientesAsync();

            return View(clientes);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var cliente =
                await _clienteServicio.ObtenerClientePorIdAsync(id);

            // Si el cliente no existe se regresa al listado con un mensaje, en lugar de una página en blanco
            if (cliente == null)
            {
                TempData["Error"] = "El cliente no existe o ya fue eliminado.";
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // ---- Registrar cliente (Persona 3) ----

        [HttpGet]
        public IActionResult Registrar()
        {
            // Se arranca con una fila de teléfono vacía para que el formulario
            // ya muestre un campo listo para llenar.
            var modelo = new ClienteDto
            {
                Telefonos = new List<TelefonoDto> { new TelefonoDto() }
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(ClienteDto cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            var respuesta = await _clienteServicio.CrearClienteAsync(cliente);

            if (!respuesta.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, respuesta.Mensaje);
                return View(cliente);
            }

            TempData["Mensaje"] = "Cliente registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ---- Modificar cliente (Persona 3) ----

        [HttpGet]
        public async Task<IActionResult> Modificar(int id)
        {
            var cliente = await _clienteServicio.ObtenerClientePorIdAsync(id);

            if (cliente == null)
            {
                TempData["Error"] = "El cliente no existe o ya fue eliminado.";
                return RedirectToAction(nameof(Index));
            }

            if (cliente.Telefonos.Count == 0)
            {
                cliente.Telefonos.Add(new TelefonoDto());
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modificar(ClienteDto cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            var respuesta = await _clienteServicio.ActualizarClienteAsync(cliente);

            if (!respuesta.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, respuesta.Mensaje);
                return View(cliente);
            }

            TempData["Mensaje"] = "Cliente modificado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ---- Eliminar cliente (Persona 4) ----

        // Se llama por AJAX desde cliente.js después de confirmar en el modal,
        // y retorna la Respuesta en formato json para mostrar el resultado
        [HttpPost]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var respuesta = await _clienteServicio.EliminarClienteAsync(id);
            return Json(respuesta);
        }
    }
}
