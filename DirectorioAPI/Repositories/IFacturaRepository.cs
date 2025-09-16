using DirectorioAPI.Models;

namespace DirectorioAPI.Repositories;

public interface IFacturaRepository
{
    IEnumerable<Factura> GetByPersonaId(int personaId);
    void Add(Factura factura);
}
