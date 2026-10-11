using System;
using System.Collections.Generic;
using System.Text;

namespace GestorClientes.Application.Dtos
{
    public class TelefonoDto
    {
        public int Id { get; set; }

        // Nullable a propósito: el formulario permite filas de teléfono vacías
        // (se agregan/quitan dinámicamente), y se descartan en el servicio antes
        // de validar. Si fuera "string" a secas, ASP.NET Core le pone un
        // [Required] implícito por el Nullable del proyecto y rompe ese flujo.
        public string? Numero { get; set; } = string.Empty;
    }
}
