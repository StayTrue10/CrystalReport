using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace reportecrystal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void btnBuscar_Click(object sender, EventArgs e)
        {
            //Conexión, busca registros iguales, los agrega a una tabla y se asigna la vista al dgv
            SqlConnection conexion = new SqlConnection("Data Source = 10.10.50.201; Initial Catalog = ANCONA_PROD; user id=sa; password=Pr0c3s0.12");
            SqlDataAdapter datos = new SqlDataAdapter("Select DisTinct T2.ItemCode, T2.Dscription,t2.Quantity, t3.Codebars, T1.Docnum, T1.DocDate, T1.DocTime, t2.DocEntry,T2.LineNum," +
            "t1.numatcard FROM ODRF T1 INNER JOIN DRF1 T2 ON T1.DocEntry = T2.DocEntry INNER JOIN OITM T3 ON T3.ItemCode = T2.ItemCode where T2.ItemCode LIKE '%" + this.tbBuscar.Text + "%'", conexion);
            DataSet data = new DataSet();
            datos.Fill(data, "tabla");
            dataGridView1.DataSource = data.Tables["tabla"];
        }
        void btnImprimir_Click(object sender, EventArgs e)
        {
            //Tabla para guardar las filas seleccionadas (Value = 1), agrega n filas de la la misma fila (Quantity)
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("ItemCode", typeof(string));
            dt.Columns.Add("Dscription", typeof(string));
            dt.Columns.Add("Quantity", typeof(Int32));
            dt.Columns.Add("Codebars", typeof(string));
            dt.Columns.Add("Docnum", typeof(Int32));
            dt.Columns.Add("DocDate", typeof(string));
            dt.Columns.Add("DocTime", typeof(string));
            dt.Columns.Add("DocEntry", typeof(Int32));
            dt.Columns.Add("LineNum", typeof(Int32));
            dt.Columns.Add("NumatCard", typeof(string));
            foreach (DataGridViewRow dgv in dataGridView1.Rows)
            {
                if (Convert.ToInt32(dgv.Cells[0].Value) == 1)
                {
                    for (int i = 0; i < Convert.ToInt32(dgv.Cells[3].Value); i++)
                        dt.Rows.Add(dgv.Cells[1].Value, dgv.Cells[2].Value, dgv.Cells[3].Value, dgv.Cells[4].Value, dgv.Cells[5].Value, dgv.Cells[6].Value, dgv.Cells[7].Value, dgv.Cells[8].Value, dgv.Cells[9].Value, dgv.Cells[10].Value);
                }
            }
            //Agrega la tabla con los datos seleccionados, crea archivo xml para lectura de los datos en Crystal, nuevo reporte, asignacion del DataSet y el Viewer
            ds.Tables.Add(dt);
            ds.WriteXmlSchema("Sample.xml");
            CrystalReport1 cr = new CrystalReport1();
            cr.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cr;
        }
        void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Click solo en la primera columna (CheckBox)
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                //Verifica si esta seleccionada una fila y cambia de valor al seleccionar una fila
                if (Convert.ToInt32(dataGridView1[0, e.RowIndex].Value) == 0)
                    dataGridView1[0, e.RowIndex].Value = 1;
                else
                    dataGridView1[0, e.RowIndex].Value = 0;
            }
        }
    }
}
