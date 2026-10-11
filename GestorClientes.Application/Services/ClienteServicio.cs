using AutoMapper;
using GestorClientes.Application.Dtos;
using GestorClientes.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorClientes.Application.Services
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteServicio(
            IClienteRepository clienteRepository,
            IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<List<ClienteDto>> ObtenerClientesAsync(
            CancellationToken cancellationToken = default)
        {
            var clientes = await _clienteRepository
                .GetAllClientesAsync(cancellationToken);

            return _mapper.Map<List<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto?> ObtenerClientePorIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var cliente = await _clienteRepository
                .GetClienteByIdAsync(id, cancellationToken);

            if (cliente.Id == 0)
                return null;

            return _mapper.Map<ClienteDto>(cliente);
        }
    }
}