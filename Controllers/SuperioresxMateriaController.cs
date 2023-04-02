using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Notas.Models;

namespace Proyecto_Notas.Controllers
{
    public class SuperioresxMateriaController : Controller
    {
        private readonly MerMariaJuliaContext _context;

        public SuperioresxMateriaController(MerMariaJuliaContext context)
        {
            _context = context;
        }

        // GET: SuperioressxMateria
        public async Task<IActionResult> Index()
        {
            return View(await _context.SuperioresxMateria.ToListAsync());
        }
    }
}
