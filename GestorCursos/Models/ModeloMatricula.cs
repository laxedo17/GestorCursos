using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCursos.Models
{
    class ModeloMatricula
    {
        public int IdMatricula { get; set; }
        public int IdEstudiante { get; set; }
        public int IdCurso { get; set; }
        public bool SeHaRegistrado { get; set; } //este atributo non esta na base de datos pero sera valioso no programa
    }
}
