using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPS_AnaliseQuimica
{
    static class Program
    {

        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        static void Main(string[] args)
        {

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new CPS_AnaliseQuimica.CPSAnaliseQuimica()
            //};
            //ServiceBase.Run(ServicesToRun);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmTeste());

        }
    }
}
