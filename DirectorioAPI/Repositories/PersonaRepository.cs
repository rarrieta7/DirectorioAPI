using DirectorioAPI.Models;

namespace DirectorioAPI.Repositories;

public class PersonaRepository : IPersonaRepository
{
    private readonly List<Persona> _personas = new();
    private int _nextId = 1;

    public IEnumerable<Persona> GetAll() => _personas;

    public Persona? GetById(int id) => _personas.FirstOrDefault(p => p.Id == id);

    public void Add(Persona persona)
    {
        persona.Id = _nextId++;
        _personas.Add(persona);
    }

    public void Delete(int id)
    {
        var persona = GetById(id);
        if (persona != null)
        {
            _personas.Remove(persona);
        }
    }
}
