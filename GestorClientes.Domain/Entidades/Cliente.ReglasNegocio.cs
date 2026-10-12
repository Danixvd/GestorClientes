using System.Linq;
using System.Text.RegularExpressions;

namespace GestorClientes.Domain;

/// <summary>
/// Reglas de negocio de Cliente, en un archivo aparte del generado por scaffolding
/// (Cliente.cs) para no perderlas si se vuelve a correr el comando de scaffold.
/// </summary>
public partial class Cliente
{
    private const string PatronTelefono = @"^[0-9+\-\s()]{7,20}$";

    /// <summary>
    /// Un cliente debe tener registrado al menos un número de teléfono.
    /// </summary>
    /// <returns>True si tiene uno o más teléfonos.</returns>
    public bool ValidarReglaNegocioTieneAlMenosUnTelefono()
    {
        return Telefonos is not null && Telefonos.Count > 0;
    }

    /// <summary>
    /// Todos los teléfonos del cliente deben tener un formato válido
    /// (dígitos, espacios, guiones, paréntesis y '+', entre 7 y 20 caracteres).
    /// </summary>
    /// <returns>True si todos los números cumplen el formato esperado.</returns>
    public bool ValidarReglaNegocioTelefonosConFormatoValido()
    {
        if (Telefonos is null)
        {
            return false;
        }

        return Telefonos.All(t =>
            !string.IsNullOrWhiteSpace(t.Numero) && Regex.IsMatch(t.Numero, PatronTelefono));
    }

    /// <summary>
    /// Agrupa todas las reglas de negocio del cliente. Si alguna falla,
    /// el cliente no se considera válido para persistir.
    /// </summary>
    /// <returns>True si se cumplen todas las reglas de negocio.</returns>
    public bool ValidarReglasDeNegocio()
    {
        return ValidarReglaNegocioTieneAlMenosUnTelefono()
            && ValidarReglaNegocioTelefonosConFormatoValido();
    }
}
