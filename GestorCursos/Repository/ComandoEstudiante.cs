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
    class ComandoEstudiante
    {
        private string _stringConexion;

        public ComandoEstudiante(string stringConexion)
        {
            _stringConexion = stringConexion;
        }

        public IList<ModeloEstudiante> ObtenerLista()
        {
            List<ModeloEstudiante> estudiantes = new List<ModeloEstudiante>();

            var sql = "Estudiante_ObtenerLista";

            using (SqlConnection conexion = new SqlConnection(_stringConexion))
            {
                estudiantes = conexion.Query<ModeloEstudiante>(sql).ToList();
            }

            return estudiantes;
        }
    }
}
