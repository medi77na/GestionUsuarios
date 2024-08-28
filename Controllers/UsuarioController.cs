using GestionUsuarios.Data;
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

}