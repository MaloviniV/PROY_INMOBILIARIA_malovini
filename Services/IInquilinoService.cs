using PROY_INMOBILIARIA_malovini.Models;

namespace PROY_INMOBILIARIA_malovini.Services;
public interface IInquilinoService : IServiceBase<InquilinoModel>
{
	Task<(PersonaModel? persona, InquilinoModel? inquilino)> BuscarPorDni(string dni);
}