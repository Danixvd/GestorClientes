using GestorClientes.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorClientes.Application.Services
{
    public interface IClienteServicio
    {
        Task<List<ClienteDto>> ObtenerClientesAsync(
           CancellationToken cancellationToken = default);

        Task<ClienteDto?> ObtenerClientePorIdAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}