using System.Data;
using System.Data.SqlClient;

namespace proyecto_condominios.DatabaseHelper
{
    public class DatabaseHelper
    {
        //Chris - CHRISTOPHER\SQLEXPRESS02
        //Jorge - LAPTOP-KK5T2UNQ\SQLEXPRESS
        //Daniel-
        //Fran  -

        const string servidor = @"localhost";
        const string baseDatos = "CondominiosProgra5";
        const string strConexion = "Data Source="+ servidor + ";Initial Catalog=" + baseDatos + ";Integrated Security=True";

        public static DataTable ExecuteSelect(string procedure, List<SqlParameter>? param)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strConexion))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = procedure;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;

                    if (param != null)
                    {
                        foreach (SqlParameter item in param)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
            catch
            {
                throw;
            }
        }

        public static DataTable ExecuteStoreProcedure(string procedure, List<SqlParameter>? param)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strConexion))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = procedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    if (param != null)
                    {
                        foreach (SqlParameter item in param)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExecStoreProcedure(string procedure, List<SqlParameter>? param)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strConexion))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = procedure;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    if (param != null)
                    {
                        foreach (SqlParameter item in param)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
