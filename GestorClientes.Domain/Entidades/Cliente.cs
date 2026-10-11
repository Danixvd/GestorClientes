using System;
using System.Collections.Generic;

namespace GestorClientes.Domain;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();
}
