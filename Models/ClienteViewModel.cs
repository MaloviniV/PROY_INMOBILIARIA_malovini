namespace PROY_INMOBILIARIA_malovini.Models;

public class ClienteViewModel
{
  public int Id { get; set; }
  public string Dni { get; set; }
  public string Nombre { get; set; }
  public string Apellido { get; set; }
  public bool EsPropietario { get; set; }
  public bool EsInquilino { get; set; }
}
