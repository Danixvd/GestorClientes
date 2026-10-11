using AutoMapper;
using GestorClientes.Application.Dtos;
using GestorClientes.Domain;

namespace GestorClientes.Application.Mapping;

/// <summary>
/// Perfil de AutoMapper. Aquí se declaran las conversiones entre entidades y DTOs.
/// </summary>
public class MapeoClases : Profile
{
    public MapeoClases()
    {
        CreateMap<Cliente, ClienteDto>().ReverseMap();

        CreateMap<Telefono, TelefonoDto>().ReverseMap();
    }
}