using System.Data;
using System.Data.SqlClient;

namespace reportecrystal
{
    class ConsultaSQL
    {
        //String de conexion a sql server, dataset y datatable para datos de sql
        SqlConnection conexion = new SqlConnection("Data Source = 10.10.50.201; Initial Catalog = ANCONA_PROD; user id=sa; password=Pr0c3s0.12");
        DataSet ds;
        //Datos en dgv1, abre conexion, selecciona columnas de la bd, rellena el dataset, cierra conexion y regresa la tabla con los datos
        public DataTable MostrarDatos()
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand("select ItemCode, Dscription, Quantity from DRF1 where DocEntry = '100'", conexion);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ds = new DataSet();
            ad.Fill(ds, "tabla");
            conexion.Close();
            return ds.Tables["tabla"];
        }
    }
}
