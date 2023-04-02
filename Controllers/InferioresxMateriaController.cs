using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Notas.Models;

namespace Proyecto_Notas.Controllers
{
    public class InferioresxMateriaController : Controller
    {
        private readonly MerMariaJuliaContext _context;

        public InferioresxMateriaController(MerMariaJuliaContext context)
        {
            _context = context;
        }

        // GET: InferioresxMateria
        public async Task<IActionResult> Index()
        {
            return View(await _context.InferioresxMateria.ToListAsync());
        }
    }
}
