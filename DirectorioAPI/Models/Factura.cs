namespace DirectorioAPI.Models;

public class Factura
{
    public int Id { get; set; }
    public string Numero { get; set; } = null!;
    public decimal Monto { get; set; }
    public int PersonaId { get; set; }
    public Persona? Persona { get; set; }
}
