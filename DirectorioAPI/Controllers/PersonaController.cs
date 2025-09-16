using DirectorioAPI.Models;
using DirectorioAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DirectorioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonasController : ControllerBase
{
    private readonly IPersonaRepository _personaRepo;

    // Inyección de dependencias Program.cs
    public PersonasController(IPersonaRepository personaRepo)
    {
        _personaRepo = personaRepo;
    }

    // GET /api/personas
    [HttpGet]
    public IActionResult GetAll() => Ok(_personaRepo.GetAll());

    // GET /api/personas/1
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var persona = _personaRepo.GetById(id);
        return persona == null ? NotFound() : Ok(persona);
    }

    // POST /api/personas
    [HttpPost]
    public IActionResult Create([FromBody] Persona persona)
    {
        if (string.IsNullOrWhiteSpace(persona.Nombre) ||
            string.IsNullOrWhiteSpace(persona.ApellidoPaterno) ||
            string.IsNullOrWhiteSpace(persona.Identificacion))
        {
            return BadRequest("Faltan datos obligatorios");
        }

        _personaRepo.Add(persona);
        return CreatedAtAction(nameof(GetById), new { id = persona.Id }, persona);
    }

    // DELETE /api/personas/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _personaRepo.Delete(id);
        return NoContent();
    }
}
