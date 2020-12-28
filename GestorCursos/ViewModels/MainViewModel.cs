using Caliburn.Micro;

using GestorCursos.Models;
using GestorCursos.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCursos.ViewModels
{
    class MainViewModel : Screen
    {
        private BindableCollection<ModeloMatricula> _matriculas = new BindableCollection<ModeloMatricula>();
        private BindableCollection<ModeloEstudiante> _estudiantes = new BindableCollection<ModeloEstudiante>();
        private BindableCollection<ModeloCurso> _cursos = new BindableCollection<ModeloCurso>();
        private readonly string _stringConexion = @"Data Source=localhost;Initial Catalog=InformeCurso;Integrated Security=True";
        private string _appStatus;
        private ModeloMatricula _matriculaSeleccionada;
        private ComandoMatricula _comandoMatricula; //o contrario que ComandoEstudiante e ComandoCurso este campo privado situamolo aqui porque queremos usalo despois

        public MainViewModel()
        {
            MatriculaSeleccionada = new ModeloMatricula();

            try
            {
                _comandoMatricula = new ComandoMatricula(_stringConexion);
                Matriculas.AddRange(_comandoMatricula.ObtenerLista());

                ComandoEstudiante comandoEstudiante = new ComandoEstudiante(_stringConexion);
                Estudiantes.AddRange(comandoEstudiante.ObtenerLista());//os datos da lista van a coleccion de Estudiantes que creamos nesta clase. AddRange engade unha Lista a outra Lista

                ComandoCurso comandoCurso = new ComandoCurso(_stringConexion);
                Cursos.AddRange(comandoCurso.ObtenerLista());
            }
            catch (Exception ex)
            {
                ActualizarAppStatus(ex.Message); //se algo cambia isto notificara a aplicacion e aparecera na ventana indicando un error
            }
        }

        public ModeloCurso CursoMatriculaSeleccionada
        {
            get 
            {
                try
                {
                    var diccionarioCursos = _cursos.ToDictionary(b => b.IdCurso); //crea un diccionario de cursos. Un diccionario ten unha clave (neste caso IdMatricula sera a clave) e esa clave ten un valor ou valores asociados, coa nosa IdMatricula podemos atopar o resto da informacion da persona matriculada e demais
                    if (MatriculaSeleccionada != null && diccionarioCursos.ContainsKey(MatriculaSeleccionada.IdCurso)) //se a matricula seleccionada ten un curso
                    {
                        return diccionarioCursos[MatriculaSeleccionada.IdCurso];//devolvemos ese curso
                    }
                }
                catch (Exception ex) //se algo vai mal pasamos a Excepcion o metodo ActualizarAppStatus
                {
                    ActualizarAppStatus(ex.Message);
                }

                return null; //se non pasa nada, e decir, se non hai un curso asociado a matricula, devolvemos null
               
            }
            set 
            {
                try
                {
                    var cursoMatriculaSeleccionada = value;

                    MatriculaSeleccionada.IdCurso = cursoMatriculaSeleccionada.IdCurso;//asignando un novo curso

                    NotifyOfPropertyChange(() => MatriculaSeleccionada);//a Matricula Seleccionada cambiou, e temos que notificalo a interfaz de usuario
                }
                catch (Exception ex)
                {
                    ActualizarAppStatus(ex.Message);
                }
            }
        }

        public ModeloEstudiante EstudianteMatriculaSeleccionada
        {
            get
            {
                try
                {
                    var diccionarioEstudiantes = _estudiantes.ToDictionary(b => b.IdEstudiante); //crea un diccionario de estudiantes. 
                    if (MatriculaSeleccionada != null && diccionarioEstudiantes.ContainsKey(MatriculaSeleccionada.IdEstudiante)) //se a matricula seleccionada pertente a un estudiante
                    {
                        return diccionarioEstudiantes[MatriculaSeleccionada.IdEstudiante];//devolvemos ese estudiante
                    }
                }
                catch (Exception ex) //se algo vai mal pasamos a Excepcion o metodo ActualizarAppStatus
                {
                    ActualizarAppStatus(ex.Message);
                }

                return null; //se non pasa nada, e decir, se non hai unha estudiante asociada a matricula, devolvemos null

            }
            set
            {
                try
                {
                    var estudianteCursoSeleccionado = value;

                    MatriculaSeleccionada.IdEstudiante = estudianteCursoSeleccionado.IdEstudiante;//asignando un novo estudiante

                    NotifyOfPropertyChange(() => MatriculaSeleccionada);//a Matricula Seleccionada agora conten a Id de Estudiante que foi seleccionada, e cambiou, co que temos que notificalo a interfaz de usuario
                }
                catch (Exception ex)
                {
                    ActualizarAppStatus(ex.Message);
                }
            }
        }

        private void ActualizarAppStatus(string mensaje)
        {
            AppStatus = mensaje;
            NotifyOfPropertyChange(() => AppStatus);
        }

        public BindableCollection<ModeloMatricula> Matriculas //a propiedade _matriculas non pode asociarse coa nosa interfaz de usuario, por iso convertimos a propiedade no constructor e asi poder asociar Matriculas coa interfaz
        {
            get
            {
                return _matriculas;
            }
            set
            {
                _matriculas = value;
            }
        }

        public BindableCollection<ModeloEstudiante> Estudiantes
        {
            get
            {
                return _estudiantes;
            }

            set
            {
                _estudiantes = value;
            }
        }

        public BindableCollection<ModeloCurso> Cursos
        {
            get
            {
                return _cursos;
            }
            set
            {
                _cursos = value;
            }
        }

        public string AppStatus
        {
            get
            {
                return _appStatus;
            }
            set
            {
                _appStatus = value;
            }
        }

        public ModeloMatricula MatriculaSeleccionada
        {
            get
            {
                return _matriculaSeleccionada;
            }
            set
            {
                _matriculaSeleccionada = value;
                NotifyOfPropertyChange(() => MatriculaSeleccionada); //notificamos unha vez cambiamos a Matricula Seleccionada co cal na parte dereita da interfaz Cursos mostrara o curso no que esta esa matricula
                NotifyOfPropertyChange(() => CursoMatriculaSeleccionada); //e polo tanto temos que cambiar os cursos asociados con esa matricula e notificalo, para que cando pulsemos nunha Id de Estudiante, os cursos cambien
                NotifyOfPropertyChange(() => EstudianteMatriculaSeleccionada);//igual que antes pero para Estudiantes
            }
        }

        public void CrearNuevaMatricula()
        {
            try
            {
                MatriculaSeleccionada = new ModeloMatricula();//utilizamos un obxeto de matricula nova vacia
                ActualizarAppStatus("Nova matricula creada");
            }
            catch (Exception ex)
            {
                ActualizarAppStatus(ex.Message);
            }
        }

        public void GuardarMatricula()
        {
            try
            {
                var diccionarioMatriculas = _matriculas.ToDictionary(p => p.IdMatricula);

                if (MatriculaSeleccionada != null) //se a matricula non e null, senon que ten algo
                {
                    _comandoMatricula.ActualizarInsertar(MatriculaSeleccionada);

                    Matriculas.Clear(); //limpamos a lista de matriculas para que as que se vexan despois sexan as actualizadas
                    Matriculas.AddRange(_comandoMatricula.ObtenerLista());

                    ActualizarAppStatus("Matricula gardada");
                }
            }
            catch (Exception ex)
            {
                ActualizarAppStatus(ex.Message);
            }
        }
    }
}
