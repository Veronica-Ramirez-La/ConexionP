using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ConexionP.Clases
{
    public class ClaseSql
    {
       
        private SqlConnection conexion = new SqlConnection(@"Data Source=DESKTOP-MJOIITI\SQLEXPRESS;Initial Catalog=ReinaMadreProyect;Integrated Security =True;");
       
        private string resultado;

      
        public string Resultado
        {
            get
            {
                return this.resultado;
            }
            set
            {
                resultado = value;
            }
        }

        public SqlConnection Conexion
        {
            get
            {
                return conexion;
            }
        }

              public void EjecutaProcedimientoAlmacenado(string _nombreProcedimiento, List<string> _parametros, List<object> _valores, bool? _enviaCorreo = null)
        {
            if (_parametros.Count != _valores.Count)
            {
                throw new Exception("Los parámetros son diferentes a los valores.");
            }
            SqlCommand comando = new SqlCommand();
            comando.Connection = this.conexion;
            comando.CommandText = _nombreProcedimiento;
            comando.CommandType = CommandType.StoredProcedure;
            for (int x = 0; x < _parametros.Count; x++)
            {
                comando.Parameters.AddWithValue(_parametros[x], _valores[x]);
            }
            try
            {
                comando.Connection.Open();
                this.Resultado = Convert.ToString(comando.ExecuteScalar());
                comando.Connection.Close();
                comando.Dispose();
            }
            catch (SqlException ex)
            {
                //obtener el metodo que levanta la excepcion
                //ex.TargetSite;
                //obtener la clase que define al miembro que levanta la excepcion
                //ex.TargetSite.DeclaringType;
                //obtener el tipo del miembro que levanta la excepcion
                //ex.TargetSite.MemberType;
                // obtener el nombre de la aplicacion que levanta la excepcion
                //ex.Source;
                if (_enviaCorreo == true || _enviaCorreo == null)
                {
                    //objetoCorreo.EnviarCorreoErrores(ex.TargetSite.DeclaringType.ToString(), ex.TargetSite.ToString(), ex.Source.ToString(), ex.Message, true);
                }
                comando.Connection.Close();
                // TODO: poner un mensaje menos explicito
                comando.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// Método que regresa una tabla como resultado de un procedimiento almacenado contra el servidor principal.
        /// </summary>
        /// <param name="_nombreProcedimiento">Nombre del procedimiento almacenado a ejecutar.</param>
        /// <param name="_parametros">Lista de parámetros que usara el procedimiento almacenado.</param>
        /// <param name="_valores">Lista de valores que usara el procedimiento almacenado.</param>
        /// <param name="_conexion">indique si la conexion a utilizar debera de ser distini a la por default</param>
        /// <returns>Regresa una tabla con datos cargados del resultado del procedimiento almacenado.</returns>
        public DataTable RegresaTabla(string _nombreProcedimiento, List<string> _parametros, List<object> _valores)
        {
            DataTable tabla = new DataTable();
            if (_parametros.Count != _valores.Count)
            {
                throw new Exception("Los parámetros son diferentes a los valores.");
            }
            SqlDataAdapter datos = new SqlDataAdapter();
            datos.SelectCommand = new SqlCommand();
            datos.SelectCommand.Connection = this.conexion;
            datos.SelectCommand.CommandText = _nombreProcedimiento;
            datos.SelectCommand.CommandType = CommandType.StoredProcedure;
            for (int x = 0; x < _parametros.Count; x++)
            {
                datos.SelectCommand.Parameters.AddWithValue(_parametros[x], _valores[x]);
            }
            try
            {
                //datos.SelectCommand.Connection.Close();
                //datos.SelectCommand.Connection.Open();
                datos.Fill(tabla);
                //datos.SelectCommand.Connection.Close();
            }
            catch (SqlException ex)
            {
                datos.SelectCommand.Connection.Close();
                //objetoCorreo.EnviarCorreoErrores(ex.TargetSite.DeclaringType.ToString(), ex.TargetSite.ToString(), ex.Source.ToString(), ex.Message, true);
                // TODO: poner un mensaje menos explicito
                //throw ex;
            }
            finally
            {
                datos.SelectCommand.Connection.Close();
            }
            datos.Dispose();
            return tabla;
        }
    }
}
