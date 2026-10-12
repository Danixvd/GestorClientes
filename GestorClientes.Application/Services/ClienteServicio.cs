using AutoMapper;
using GestorClientes.Application.Dtos;
using GestorClientes.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Obtiene todos los clientes con sus teléfonos y los convierte a DTO.
        /// </summary>
        /// <param name="cancellationToken">Token para cancelar la consulta.</param>
        /// <returns>Lista de clientes (vacía si no hay registros).</returns>
        public async Task<List<ClienteDto>> ObtenerClientesAsync(
            CancellationToken cancellationToken = default)
        {
            var clientes = await _clienteRepository
                .GetAllClientesAsync(cancellationToken);

            return _mapper.Map<List<ClienteDto>>(clientes);
        }

        /// <summary>
        /// Obtiene un cliente por su id y lo convierte a DTO.
        /// El repositorio devuelve un cliente vacío (Id = 0) si no existe.
        /// </summary>
        /// <param name="id">Identificador del cliente.</param>
        /// <param name="cancellationToken">Token para cancelar la consulta.</param>
        /// <returns>El cliente, o null si no existe.</returns>
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

        /// <summary>
        /// Registra un nuevo cliente junto con sus teléfonos.
        /// Valida las reglas de negocio antes de guardar; si alguna falla,
        /// retorna una Respuesta con EsCorrecto = false y no toca la base de datos.
        /// </summary>
        public async Task<Respuesta<ClienteDto>> CrearClienteAsync(
            ClienteDto cliente,
            CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<ClienteDto>();

            // El formulario permite agregar/quitar filas de teléfono dinámicamente;
            // se descartan las filas que quedaron vacías antes de validar y mapear.
            cliente.Telefonos = cliente.Telefonos
                .Where(t => !string.IsNullOrWhiteSpace(t.Numero))
                .ToList();

            var clienteEntity = _mapper.Map<Domain.Cliente>(cliente);
            clienteEntity.FechaCreacion = DateTime.Now;

            if (!clienteEntity.ValidarReglaNegocioTieneAlMenosUnTelefono())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "El cliente debe tener al menos un número de teléfono.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!clienteEntity.ValidarReglaNegocioTelefonosConFormatoValido())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "Uno o más números de teléfono no tienen un formato válido.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!await _clienteRepository.CreateClienteAsync(clienteEntity, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo registrar el cliente.";
                respuesta.Codigo = 500;
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<ClienteDto>(clienteEntity);
            return respuesta;
        }

        /// <summary>
        /// Actualiza un cliente existente y reemplaza su lista de teléfonos
        /// por la que llega en el DTO. Valida las reglas de negocio antes de guardar.
        /// </summary>
        public async Task<Respuesta<ClienteDto>> ActualizarClienteAsync(
            ClienteDto cliente,
            CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<ClienteDto>();

            cliente.Telefonos = cliente.Telefonos
                .Where(t => !string.IsNullOrWhiteSpace(t.Numero))
                .ToList();

            var clienteEntity = _mapper.Map<Domain.Cliente>(cliente);

            if (!clienteEntity.ValidarReglaNegocioTieneAlMenosUnTelefono())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "El cliente debe tener al menos un número de teléfono.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!clienteEntity.ValidarReglaNegocioTelefonosConFormatoValido())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "Uno o más números de teléfono no tienen un formato válido.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!await _clienteRepository.UpdateClienteAsync(clienteEntity, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo actualizar el cliente (puede que ya no exista).";
                respuesta.Codigo = 500;
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<ClienteDto>(clienteEntity);
            return respuesta;
        }

        /// <summary>
        /// Elimina un cliente junto con sus teléfonos (ON DELETE CASCADE en la base de datos).
        /// Si el repositorio no logra eliminarlo, retorna una Respuesta con EsCorrecto = false.
        /// </summary>
        /// <param name="id">Identificador del cliente a eliminar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Respuesta estandarizada indicando si se eliminó el cliente.</returns>
        public async Task<Respuesta<ClienteDto>> EliminarClienteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<ClienteDto>();

            // El repositorio retorna false si el cliente no existe o si no se borró ningún registro
            if (!await _clienteRepository.DeleteClienteAsync(id, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo eliminar el cliente (puede que ya no exista).";
                respuesta.Codigo = 500;
                return respuesta;
            }

            respuesta.Mensaje = "Cliente eliminado correctamente.";
            return respuesta;
        }
    }
}
