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

            if (cliente == null)
                return NotFound();

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
                return NotFound();

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
    }
}
