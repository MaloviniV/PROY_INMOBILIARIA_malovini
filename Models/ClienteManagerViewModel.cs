using PROY_INMOBILIARIA_malovini.Models;

public class ClienteManagerViewModel
{
  public string Modo { get; set; } = "crear";
  public PersonaModel Persona { get; set; } = new PersonaModel();
  public PropietarioModel? Propietario { get; set; }
  public InquilinoModel? Inquilino { get; set; }
  public IList<InmuebleModel> InmueblesPropietario { get; set; } = new List<InmuebleModel>();
  public IList<ReservaModel> AlquileresInquilino { get; set; } = new List<ReservaModel>();
  public string? SeccionGuardada { get; set; }
}