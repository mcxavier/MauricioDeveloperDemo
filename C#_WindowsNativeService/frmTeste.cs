using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CPS_AnaliseQuimica.DataBase;

namespace CPS_AnaliseQuimica
{
    public partial class frmTeste : Form
    {
        public frmTeste()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<string> lista = new List<string>();
            SqlServerTools sql = new SqlServerTools(new EventLog(), lista);
            OracleTools ora = new OracleTools(new EventLog(), lista);

            // ora.GetCheckAnalise();

            // sql.GetGusa();
            // sql.GetAcoMLC();
            // sql.GetGusaAFA();
            // sql.GetAcoConvertedor();
            // sql.GetAcoFinal();
            // sql.GetAcoPanela();
            // sql.GetAcoFornoPanela();
            // sql.GetEscoriaCV();
            // sql.GetEscoriaFP();
             sql.GetEscoriaPA();
        }
    }
}
