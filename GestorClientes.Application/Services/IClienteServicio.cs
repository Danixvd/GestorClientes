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

        /// <summary>
        /// Registra un nuevo cliente junto con sus teléfonos, validando las
        /// reglas de negocio antes de persistir.
        /// </summary>
        /// <param name="cliente">Datos del cliente a registrar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Respuesta estandarizada con el cliente creado si todo salió bien.</returns>
        Task<Respuesta<ClienteDto>> CrearClienteAsync(
            ClienteDto cliente,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza los datos de un cliente existente y reemplaza sus teléfonos,
        /// validando las reglas de negocio antes de persistir.
        /// </summary>
        /// <param name="cliente">Datos nuevos del cliente (incluye su Id).</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Respuesta estandarizada con el cliente actualizado si todo salió bien.</returns>
        Task<Respuesta<ClienteDto>> ActualizarClienteAsync(
            ClienteDto cliente,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina un cliente; sus teléfonos se borran por la regla ON DELETE CASCADE.
        /// </summary>
        /// <param name="id">Identificador del cliente a eliminar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Respuesta estandarizada indicando si se eliminó el cliente.</returns>
        Task<Respuesta<ClienteDto>> EliminarClienteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
