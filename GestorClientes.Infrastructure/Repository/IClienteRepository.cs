using GestorClientes.Domain;

namespace GestorClientes.Infrastructure.Repository;

/// <summary>
/// Contrato de acceso a datos para la entidad Cliente y sus teléfonos.
/// </summary>
public interface IClienteRepository
{
    Task<List<Cliente>> GetAllClientesAsync(CancellationToken cancellationToken = default);
    Task<Cliente> GetClienteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CreateClienteAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task<bool> UpdateClienteAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task<bool> DeleteClienteAsync(int id, CancellationToken cancellationToken = default);
}