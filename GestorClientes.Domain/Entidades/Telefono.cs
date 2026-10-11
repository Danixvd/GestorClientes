using System;
using System.Collections.Generic;

namespace GestorClientes.Domain;

public partial class Telefono
{
    public int Id { get; set; }

    public string Numero { get; set; } = null!;

    public int Fkcliente { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Cliente FkclienteNavigation { get; set; } = null!;
}
