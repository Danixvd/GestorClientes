namespace GestorClientes.Application.Dtos;

/// <summary>
/// Patrón Result: estandariza lo que devuelven los servicios, con el dato,
/// un mensaje y un código de estado.
/// </summary>
/// <typeparam name="T">Tipo del dato que se devuelve.</typeparam>
public class Respuesta<T>
{
    public bool EsCorrecto { get; set; }

    public string Mensaje { get; set; } = string.Empty;

    public int Codigo { get; set; }

    public T? Dato { get; set; }

    // Por defecto la respuesta es exitosa; el servicio la cambia si algo falla
    public Respuesta()
    {
        EsCorrecto = true;
        Mensaje = "Operacion realizada correctamente";
        Codigo = 200;
    }
}