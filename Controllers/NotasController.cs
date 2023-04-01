using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Proyecto_Notas.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Proyecto_Notas.Controllers
{
    public class NotasController : Controller
    {
        private readonly MerMariaJuliaContext _context;

        public NotasController(MerMariaJuliaContext context)
        {
            _context = context;
        }

        // GET: Notas
        public async Task<IActionResult> Index()
        {
            var merMariaJuliaContext = _context.Notas.Include(n => n.IdEstudianteNavigation).Include(n => n.IdMateriaNavigation).ToList();
            return View(merMariaJuliaContext);
        }

        // GET: Notas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notas == null)
            {
                return NotFound();
            }

            var nota = await _context.Notas
                .Include(n => n.IdEstudianteNavigation)
                .Include(n => n.IdMateriaNavigation)
                .FirstOrDefaultAsync(m => m.IdNota == id);
            if (nota == null)
            {
                return NotFound();
            }

            return View(nota);
        }

        // GET: Notas/Create
        public IActionResult Create()
        {

                ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes.Select(e => new {
                    e.IdEstudiante,
                    FullName = $"{e.Nombre} {e.Apellido}"
                }), "IdEstudiante", "FullName");
                ViewData["IdMateria"] = new SelectList(_context.Materias, "IdMateria", "NombreMateria");
                return View();
        }

        // POST: Notas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNota,IdEstudiante,IdMateria,Nota1")] Nota nota)
        {
            // Verificar si ya existe una nota para este estudiante y esta materia
            var existeNota = await _context.Notas.FirstOrDefaultAsync(n => n.IdEstudiante == nota.IdEstudiante && n.IdMateria == nota.IdMateria);

            if (existeNota != null)
            {
                ModelState.AddModelError(string.Empty, "Este estudiante ya tiene una nota para esta materia.");
                ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes.Select(e => new {
                    e.IdEstudiante,
                    FullName = $"{e.Nombre} {e.Apellido}"
                }), "IdEstudiante", "FullName");
                ViewBag.IdMateria = new SelectList(_context.Materias, "IdMateria", "NombreMateria", nota.IdMateria);
                return View(nota);
            }
            // Fin de Verificacion
            if (ModelState.IsValid)
            {
                _context.Add(nota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.IdEstudiante = new SelectList(_context.Estudiantes, "IdEstudiante", "Nombre");
            ViewBag.IdMateria = new SelectList(_context.Materias, "IdMateria", "NombreMateria");

            return View(nota);
        }

        // GET: Notas/Edit/5
        public async Task<IActionResult> EditAsync(int id)
        {

            var nota = _context.Notas.SingleOrDefault(n => n.IdNota == id);

            if (nota == null)
            {
                return NotFound();
            }

            var estudiantes = _context.Estudiantes.ToList();
            var materias = _context.Materias.ToList();

            ViewBag.IdEstudiante = new SelectList(_context.Estudiantes.Select(e => new
            {
                e.IdEstudiante,
                FullName = e.Nombre + " " + e.Apellido
            }), "IdEstudiante", "FullName", nota.IdEstudiante);
            ViewData["Materias"] = new SelectList(materias, "IdMateria", "NombreMateria", nota.IdMateria);

            return View(nota);
        }

        // POST: Notas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNota,IdEstudiante,IdMateria,Nota1")] Nota nota)
        {
            if (id != nota.IdNota)
            {
                return NotFound();
            }

            // Obtener la nota actual de la base de datos
            var notaActual = _context.Notas.AsNoTracking().SingleOrDefault(n => n.IdNota == id);

            // Verificar si el estudiante ya tiene una nota registrada para la materia
            var notaExistente = _context.Notas.AsNoTracking().SingleOrDefault(n => n.IdEstudiante == nota.IdEstudiante && n.IdMateria == nota.IdMateria);

            // Si se encuentra una nota y los IDs son diferentes, mostrar un mensaje de error
            if (notaExistente != null && notaExistente.IdNota != id)
            {
                ModelState.AddModelError("", "El estudiante ya tiene una nota registrada para esta materia.");

                ViewBag.IdEstudiante = new SelectList(_context.Estudiantes.Select(e => new
                {
                    e.IdEstudiante,
                    FullName = e.Nombre + " " + e.Apellido
                }), "IdEstudiante", "FullName", nota.IdEstudiante);
                ViewBag.IdMateria = new SelectList(_context.Materias, "IdMateria", "NombreMateria", nota.IdMateria);
                return View(nota);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaExists(nota.IdNota))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var estudiantes = _context.Estudiantes.ToList();
            var materias = _context.Materias.ToList();

            ViewBag.IdEstudiante = new SelectList(_context.Estudiantes.Select(e => new
            {
                e.IdEstudiante,
                FullName = e.Nombre + " " + e.Apellido
            }), "IdEstudiante", "FullName", nota.IdEstudiante);
            ViewData["Materias"] = new SelectList(materias, "IdMateria", "NombreMateria", nota.IdMateria);

            return View(nota);
        }

        // GET: Notas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notas == null)
            {
                return NotFound();
            }

            var nota = await _context.Notas
                .Include(n => n.IdEstudianteNavigation)
                .Include(n => n.IdMateriaNavigation)
                .FirstOrDefaultAsync(m => m.IdNota == id);
            if (nota == null)
            {
                return NotFound();
            }

            return View(nota);
        }

        // POST: Notas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notas == null)
            {
                return Problem("Entity set 'MerMariaJuliaContext.Notas'  is null.");
            }
            var nota = await _context.Notas.FindAsync(id);
            if (nota != null)
            {
                _context.Notas.Remove(nota);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotaExists(int id)
        {
            return (_context.Notas?.Any(e => e.IdNota == id)).GetValueOrDefault();
        }
    }
}
