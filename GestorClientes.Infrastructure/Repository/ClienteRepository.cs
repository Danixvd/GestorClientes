using GestorClientes.Domain;
using GestorClientes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestorClientes.Infrastructure.Repository;

/// <summary>
/// Implementación del acceso a datos de Cliente usando Entity Framework Core.
/// </summary>
public class ClienteRepository : IClienteRepository
{
    private readonly ApplicationDbContext _context;

    // Inyección de dependencias: el contexto lo entrega ASP.NET Core
    public ClienteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los clientes junto con sus teléfonos.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la consulta.</param>
    /// <returns>Lista de clientes (vacía si no hay registros).</returns>
    public async Task<List<Cliente>> GetAllClientesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .Include(c => c.Telefonos)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtiene un cliente por su id, con sus teléfonos.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la consulta.</param>
    /// <returns>El cliente, o un objeto vacío si no existe.</returns>
    public async Task<Cliente> GetClienteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes
            .AsNoTracking()
            .Include(c => c.Telefonos)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return cliente ?? new Cliente();
    }

    /// <summary>
    /// Guarda un cliente nuevo junto con sus teléfonos.
    /// </summary>
    /// <param name="cliente">Cliente a guardar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>True si se guardó correctamente.</returns>
    public async Task<bool> CreateClienteAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// Actualiza los datos de un cliente y reemplaza sus teléfonos.
    /// </summary>
    /// <param name="cliente">Cliente con los datos nuevos.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>True si se actualizó; false si el cliente no existe.</returns>
    public async Task<bool> UpdateClienteAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        var clienteExistente = await _context.Clientes
            .Include(c => c.Telefonos)
            .FirstOrDefaultAsync(c => c.Id == cliente.Id, cancellationToken);

        if (clienteExistente is null)
        {
            return false;
        }

        clienteExistente.Nombre = cliente.Nombre;
        clienteExistente.Apellido1 = cliente.Apellido1;
        clienteExistente.Apellido2 = cliente.Apellido2;
        clienteExistente.Correo = cliente.Correo;

        // Se eliminan los teléfonos anteriores y se agregan los nuevos como registros limpios
        _context.Telefonos.RemoveRange(clienteExistente.Telefonos);

        foreach (var telefono in cliente.Telefonos)
        {
            clienteExistente.Telefonos.Add(new Telefono { Numero = telefono.Numero });
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    /// <summary>
    /// Elimina un cliente; sus teléfonos se borran por la regla ON DELETE CASCADE.
    /// </summary>
    /// <param name="id">Identificador del cliente a eliminar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>True si se eliminó; false si no existe.</returns>
    public async Task<bool> DeleteClienteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (cliente is null)
        {
            return false;
        }

        _context.Clientes.Remove(cliente);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}