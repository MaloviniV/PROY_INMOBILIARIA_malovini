using PROY_INMOBILIARIA_malovini.Models;
namespace PROY_INMOBILIARIA_malovini.Repositories;

public interface IPersonaRepository : IRepositorioBase<PersonaModel>
{
  Task<PersonaModel?> ObtenerPorMail(string mail);
  Task<PersonaModel?> ObtenerPorDni(string dni);
}