using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GestorClientes.Application.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string Apellido1 { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string Apellido2 { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
        public string Correo { get; set; } = string.Empty;

        public List<TelefonoDto> Telefonos { get; set; } = new();
    }
}
