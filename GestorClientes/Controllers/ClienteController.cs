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
    }
}