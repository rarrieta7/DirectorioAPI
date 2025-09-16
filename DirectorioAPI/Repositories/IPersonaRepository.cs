using DirectorioAPI.Models;

namespace DirectorioAPI.Repositories;

public interface IPersonaRepository
{
    IEnumerable<Persona> GetAll();
    Persona? GetById(int id);
    void Add(Persona persona);
    void Delete(int id);
}
