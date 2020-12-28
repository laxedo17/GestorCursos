using Dapper;

using GestorCursos.Models;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCursos.Repository
{
    class ComandoCurso
    {
        private string _stringConexion;

        public ComandoCurso(string stringConexion)
        {
            _stringConexion = stringConexion;
        }

        public IList<ModeloCurso> ObtenerLista()
        {
            List<ModeloCurso> cursos = new List<ModeloCurso>();

            var sql = "Curso_ObtenerLista";

            using (SqlConnection conexion = new SqlConnection(_stringConexion))
            {
                cursos = conexion.Query<ModeloCurso>(sql).ToList();
            }

            return cursos;
        }
    }
}
