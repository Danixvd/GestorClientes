using System;
using System.Collections.Generic;
using System.Text;

namespace GestorClientes.Application.Dtos
{
  public  class ClienteDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido1 { get; set; } = string.Empty;

        public string Apellido2 { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public List<TelefonoDto> Telefonos { get; set; } = new();
    }
}
