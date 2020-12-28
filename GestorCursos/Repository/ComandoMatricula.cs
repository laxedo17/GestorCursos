using Dapper;

using GestorCursos.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCursos.Repository
{
    class ComandoMatricula
    {
        private string _stringConexion;

        public ComandoMatricula(string stringConexion)
        {
            _stringConexion = stringConexion;
        }

        public IList<ModeloMatricula> ObtenerLista()
        {
            List<ModeloMatricula> matriculas = new List<ModeloMatricula>();

            var sql = "Matriculas_ObtenerLista";

            using (SqlConnection conexion=new SqlConnection(_stringConexion))
            {
                matriculas = conexion.Query<ModeloMatricula>(sql).ToList();
            }

            foreach (var matricula in matriculas)
            {
                matricula.SeHaRegistrado = true;//axudanos a diferenciar entre estudiantes que se rexistraron e estan gardados e quenes entran novos
            }

            return matriculas;
        }

        public void ActualizarInsertar(ModeloMatricula modeloMatricula)
        {
            var sql = "Matriculas_ActualizarInsertar";
            var idUsuario = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString(); //colle o nome de usuario de Windows

            var tablaDeDatos = new DataTable();
            tablaDeDatos.Columns.Add("IdMatricula", typeof(int));
            tablaDeDatos.Columns.Add("IdEstudiante", typeof(int));
            tablaDeDatos.Columns.Add("IdCurso", typeof(int));
            tablaDeDatos.Rows.Add(modeloMatricula.IdMatricula, modeloMatricula.IdEstudiante, modeloMatricula.IdCurso);
            
            using(SqlConnection conexion=new SqlConnection(_stringConexion))
            {
                conexion.Execute(sql, new { @TipoDeMatricula = tablaDeDatos.AsTableValuedParameter("TipoDeMatricula"), @IdUsuario = idUsuario }, commandType: CommandType.StoredProcedure);
            }

        }
    }
}
