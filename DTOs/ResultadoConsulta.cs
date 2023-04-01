namespace Proyecto_Notas.DTOs
{
    public class ResultadoConsulta
    {
        public int IdEstudiante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Celular { get; set; }
        public string NombreMateria { get; set; }
        public decimal NotaMaxima { get; set; }
        public string EstadoMateria { get; set; }
    }
}
