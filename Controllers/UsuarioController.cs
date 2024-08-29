using GestionUsuarios.Data;
using GestionUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace GestionUsuarios.Controllers;

[Route("[controller]")]
public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly ApplicationDBContext _context;

    public UsuarioController(ILogger<UsuarioController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return View(usuarios);
    }


    [Route("Buscar")]
    public async Task<IActionResult> Buscar(string palabra)
    {
        if (string.IsNullOrWhiteSpace(palabra))
        {
            return RedirectToAction(nameof(Index));
        }
        var usuarios = await _context.Usuarios
                                .Where(u => u.Nombre.Contains(palabra) ||
                                            u.Apellido.Contains(palabra) ||
                                            u.FechaNacimiento == DateOnly.Parse(palabra))
                                .ToListAsync();
        return View("Index", usuarios);
    }


    [Route("Crear")]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            _logger.LogError("Error saving");
            return View(usuario);
        }
    }


    [Route("SolicitudEditar")]
    public async Task<IActionResult> SolicitudEditar()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return View(usuarios);
    }

    [Route("Editar/{id}")]
    public async Task<IActionResult> Editar(int id)
    {
        var usuarioEncontrado = await _context.Usuarios.FindAsync(id);
        return View(usuarioEncontrado);
    }

    [HttpPost("Editar/{id}")]
    public async Task<IActionResult> Editar(int id, Usuario usuario)
    {
        var result = VerificarExistencia(id);
        if (result == false)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            _logger.LogError("El modelo no es valido");
            return View(usuario);
        }
    }

    [Route("SolicitudEliminar")]
    public async Task<IActionResult> SolicitudEliminar()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return View(usuarios);
    }

    [Route("Eliminar/{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var UsuarioEncontrado = await _context.Usuarios.FindAsync(id);
        if (UsuarioEncontrado == null)
        {
            return NotFound();
        }
        return View(UsuarioEncontrado);
    }

    [HttpPost("Eliminar/{id}")]
    public async Task<IActionResult> EliminacionConfirmada(int id)
    {
        var result = VerificarExistencia(id);
        if (result == false)
        {
            return NotFound();
        }

        var usuarioEncontrado = await _context.Usuarios.FindAsync(id);
        if (usuarioEncontrado == null)
        {
            return NotFound();
        }
        _context.Usuarios.Remove(usuarioEncontrado);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VerificarExistencia(int id)
    {
        var checkearUsuario = _context.Usuarios.Any(u => u.Id == id);
        if (checkearUsuario == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}