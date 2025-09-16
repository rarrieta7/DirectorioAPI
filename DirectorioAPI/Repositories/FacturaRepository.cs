using DirectorioAPI.Models;

namespace DirectorioAPI.Repositories;

public class FacturaRepository : IFacturaRepository
{
    private readonly List<Factura> _facturas = new();
    private int _nextId = 1;

    public IEnumerable<Factura> GetByPersonaId(int personaId) =>
        _facturas.Where(f => f.PersonaId == personaId);

    public void Add(Factura factura)
    {
        factura.Id = _nextId++;
        _facturas.Add(factura);
    }
}
