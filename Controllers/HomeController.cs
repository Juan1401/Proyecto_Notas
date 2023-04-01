using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Proyecto_Notas.Models;
using System.Diagnostics;
using Proyecto_Notas.DTOs;


namespace Proyecto_Notas.Controllers
{
    public class HomeController : Controller
    {
        private readonly MerMariaJuliaContext _context;


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MerMariaJuliaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            var query = (from n in _context.Notas
                                                          join e in _context.Estudiantes on n.IdEstudiante equals e.IdEstudiante
                                                          join m in _context.Materias on n.IdMateria equals m.IdMateria
                                                          where Convert.ToDouble(n.Nota1) >= 3.5
                                                          group new { e, m, n } by new { e.IdEstudiante, e.Nombre, e.Apellido, e.Celular, m.NombreMateria, n.Nota1 } into g
                                                          select new
                                                          {
                                                              g.Key.IdEstudiante,
                                                              Nombres = g.Key.Nombre,
                                                              APELLIDO = g.Key.Apellido,
                                                              g.Key.Celular,
                                                              Materia = g.Key.NombreMateria,
                                                              Nota = g.Key.Nota1,
                                                              Estado_Materia = Convert.ToDouble(g.Key.Nota1) > 3.5 ? "ASIGNATURA APROBADA" : "ASIGNATURA REPROBADA"
                                                          });
            List<ResultadoConsulta> resultados = new List<ResultadoConsulta>();

            foreach (var item in query)
            {
                ResultadoConsulta resultadoConsulta = new ResultadoConsulta
                {
                    IdEstudiante = item.IdEstudiante,
                    Nombre = item.Nombres,
                    Apellido = item.APELLIDO,
                    Celular = item.Celular,
                    NombreMateria = item.Materia,
                    NotaMaxima = item.Nota,
                    EstadoMateria = item.Estado_Materia
                };
                resultados.Add(resultadoConsulta);
            }
            return View(resultados);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}