/* clasefunciones.cs .
 * 
 *  
 * Desarrollado por Roberto Sanz Jimeno
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace FuncionesSQLServer
{
    class FuncionesSQL
    {

        public static void consulta_INSERT_DELETE(string query, string cadena_conexion)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadena_conexion))
            using (SqlCommand insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = query;
                if (insertCommand.Connection.State == System.Data.ConnectionState.Open)
                    try
                    {
                        insertCommand.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        //MessageBox.Show("excepcion");
                        throw;
                    }
                else
                {
                    try
                    {
                        insertCommand.Connection.Open();
                        insertCommand.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        //MessageBox.Show("excepcion");
                        query = cadena_conexion;
                    }
                    finally
                    {

                        insertCommand.Connection.Close();
                        connection.Close();
                    }
                }
            }
        }
      
        public static string sacar_dato_dataset(DataSet ds, string tabla, int fila, string columna)
        {
            // ds = dataset donde se busca
            // tabla = nombre de la tabla, 
            // fila = fila de la tabla
            // columna = nombre de la columna de la tabla
            string dato = "";

            try
            {
                DataRow datofila = ds.Tables[tabla].Rows[fila];
                dato = datofila[columna].ToString();
            }
            catch
            {
                dato = "";
            }
            return dato;
        }
            
        public static DataSet Consulta_SELECT(string aquery, DataSet undataset, string cadena_conexion, bool mensaje)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadena_conexion))
            {
                undataset.Clear();

                System.Data.SqlClient.SqlDataAdapter adapter1 = new System.Data.SqlClient.SqlDataAdapter(aquery, connection);
                try
                {
                    adapter1.Fill(undataset);
                }
                catch (System.Data.SqlClient.SqlException de)
                {
                    if (mensaje == true)
                    {
                        MessageBox.Show(de.Message);
                        mensaje = mensaje;
                    }
                    else
                    {

                    }
                }
                finally
                {

                    connection.Close();
                }
            }
            return undataset;
        }

        public static DataSet Consulta_SELECT(string aquery, DataSet undataset, string cadena_conexion, bool mensaje, string añadir_a_continuacion)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadena_conexion))
            {
                //undataset.Clear();

                System.Data.SqlClient.SqlDataAdapter adapter1 = new System.Data.SqlClient.SqlDataAdapter(aquery, connection);
                try
                {
                    adapter1.Fill(undataset);
                }
                catch (System.Data.SqlClient.SqlException de)
                {
                    if (mensaje == true)
                    {
                        MessageBox.Show(de.Message);
                        mensaje = mensaje;
                    }
                    else
                    {

                    }
                }
                finally
                {

                    connection.Close();
                }
            }
            return undataset;
        }

        public static DataSet Consulta_SELECT(string aquery, DataSet undataset, string cadena_conexion)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadena_conexion))
            {
                undataset.Clear();


                int valor_timeout = connection.ConnectionTimeout;




                System.Data.SqlClient.SqlDataAdapter adapter1 = new System.Data.SqlClient.SqlDataAdapter(aquery, connection);
                try
                {
                    adapter1.Fill(undataset);
                }
                catch (System.Data.SqlClient.SqlException de)
                {
                    MessageBox.Show(de.Message);
                }

                catch (InvalidOperationException de)
                {
                    MessageBox.Show(de.Message);
                }

                finally
                {

                    connection.Close();
                }
            }
            return undataset;
        }

        public static void actualizar_añadir(System.Data.SqlClient.SqlConnection connection, string cadena)
        {
            using (SqlCommand insertCommando = connection.CreateCommand())
            {
                insertCommando.CommandText = cadena;
                if (insertCommando.Connection.State == System.Data.ConnectionState.Open)
                    try
                    {
                        insertCommando.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException de)
                    {
                        MessageBox.Show(de.Message);
                    }
                else
                {
                    try
                    {
                        insertCommando.Connection.Open();
                        insertCommando.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException de)
                    {
                        MessageBox.Show(de.Message);
                    }
                    finally
                    {

                        insertCommando.Connection.Close();
                        connection.Close();
                    }
                }
            }
        }

        public static void actualizar_borrar(System.Data.SqlClient.SqlConnection connection, string cadena)
        {
            using (SqlCommand insertCommando = connection.CreateCommand())
            {
                insertCommando.CommandText = cadena;
                if (insertCommando.Connection.State == System.Data.ConnectionState.Open)
                    try
                    {
                        insertCommando.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException de)
                    {
                        MessageBox.Show(de.Message);
                    }
                else
                {
                    try
                    {
                        insertCommando.Connection.Open();
                        insertCommando.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException de)
                    {
                        MessageBox.Show(de.Message);
                    }
                    finally
                    {

                        insertCommando.Connection.Close();
                        connection.Close();
                    }
                }
            }
        }
        
        public static int escalar(string cadena_conexion, string query)
        {
            int devolver;
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadena_conexion))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                try
                {
                    bool correcto;
                    correcto = int.TryParse(command.ExecuteScalar().ToString(), out devolver);
                    //consola.AppendText("configurado =" + configurado);
                }
                catch (NullReferenceException)
                {
                    //MessageBox.Show("No se ha podido realizar la consulta","ERROR");
                    devolver = 0;
                }
                finally
                {
                    command.Connection.Close();
                    connection.Close();
                }
            }
            return devolver;
        }
        
        public static string escalar_cadena(string cadena_conexion, string query)
        {
            string cadena;
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadena_conexion))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                try
                {
                    cadena = command.ExecuteScalar().ToString();
                    //consola.AppendText("configurado =" + configurado);
                }
                catch (NullReferenceException)
                {
                    //Funciones.sacar_ventana_texto("No se ha podido realizar la consulta","ERROR");
                    cadena = "";
                }
                finally
                {
                    command.Connection.Close();
                    connection.Close();
                }
            }
            return cadena;
        }
       
        public static DataSet Consulta_SELECT_para_foreach_y_visualizar_datagrid(string aquery, DataSet undataset, string cadena_conexion, DataGridView datagridview)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadena_conexion))
            {
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(aquery, connection);
                try
                {
                    adapter.Fill(undataset);
                    datagridview.DataSource = undataset.Tables[0];
                    datagridview.Refresh();
                    datagridview.Show();
                }
                catch (System.Data.SqlClient.SqlException de)
                {
                    MessageBox.Show(de.Message);
                    ;
                }
                finally
                {

                    connection.Close();
                }
            }
            return undataset;
        }
        
        public static void MakeNonEditableGridView(DataGridView MyDataGridView)
        {
            int MyNumeroFilas = 0;
            int MyNumeroColumnas = 0;
            //Hacemos un for para acceder a cada fila
            for (MyNumeroFilas = 0; MyNumeroFilas <= MyDataGridView.RowCount - 1; MyNumeroFilas++)
            {
                //Dentro hacemos el for para cada columna de cada fila
                for (MyNumeroColumnas = 0; MyNumeroColumnas <= MyDataGridView.ColumnCount - 2; MyNumeroColumnas++)
                {
                    //hay que hacer una celda no accesible
                    MyDataGridView.Rows[MyNumeroFilas].Cells[MyNumeroColumnas].ReadOnly = true;
                }
            }
        }
        
        
    }
}
