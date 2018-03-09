using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reportecrystal
{
    public partial class Form1 : Form
    {
        //Clase ConsultaSQL, dataset y datatable nuevos para los datos
        ConsultaSQL sql = new ConsultaSQL();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();
            //Datos del servidor & nueva tabla, (tablas del mismo tamaño)
            dataGridView1.DataSource = sql.MostrarDatos();
            dt.Columns.Add("ItemCode", typeof(string));
            dt.Columns.Add("Dscription", typeof(string));
            dt.Columns.Add("Quantity", typeof(string));
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Click solo en la primera columna (CheckBox)
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                //Verifica valor, 1=Seleccionado (int o bool) y cambia el valor de la primera columna
                if (Convert.ToInt32(dataGridView1[0, e.RowIndex].Value) == 0)
                    dataGridView1[0, e.RowIndex].Value = 1;
                else
                    dataGridView1[0, e.RowIndex].Value = 0;
            }
        }
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            //Limpia los datos de la tabla, recorre todas la filas y agrega solo las filas seleccionadas omitiendo la primera columna
            dt.Clear();
            foreach (DataGridViewRow dgv in dataGridView1.Rows)
            {
                if (Convert.ToInt32(dgv.Cells[0].Value) == 1)
                dt.Rows.Add(dgv.Cells[1].Value, dgv.Cells[2].Value, dgv.Cells[3].Value);
            }
            //Reconstruye la tabla, agrega la tabla con los datos seleccionados, crea archivo xml para lectura de los datos en crystal, nuevo reporte, asignacion del dataset y el viewer 
            ds.Tables.Clear();
            ds.Tables.Add(dt);
            ds.WriteXmlSchema("Sample.xml");
            CrystalReport1 cr = new CrystalReport1();
            cr.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cr;
        }
    }
}
