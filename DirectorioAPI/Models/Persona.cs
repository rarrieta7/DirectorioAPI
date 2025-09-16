namespace DirectorioAPI.Models;

public class Persona
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string ApellidoPaterno { get; set; } = null!;
    public string? ApellidoMaterno { get; set; }
    public string Identificacion { get; set; } = null!;
    public List<Factura> Facturas { get; set; } = new();
}
