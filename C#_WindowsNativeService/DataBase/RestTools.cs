using CPS_AnaliseQuimica.DTO;
using CPS_AnaliseQuimica.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_AnaliseQuimica.DataBase
{
    // ********************************************************************************************************* //
    //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
    // --------------------------------------------------------------------------------------------------------- //
    //  @Autor: MAURICIO XAVIER                                                                                  //
    //  @Data:  19/05/2020                                                                                       //
    //  @Descricao:                                                                                              //
    // ********************************************************************************************************* //
    public class RestTools
    {
        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaGusa(AnaliseQuimicaGusaDTO gusa)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//gusacv", gusa);
            return sRetorno;
        }

        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaAcoMLC(AnaliseQuimicaAcoMLCDTO acoMLC)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//acoMLC", acoMLC);
            return sRetorno;
        }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaGusaAFA(AnaliseQuimicaGusaAFADTO gusaAFA)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//gusaAFA", gusaAFA);
            return sRetorno;
        }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaAcoCV(AnaliseQuimicaAcoCVDTO acoCV)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//acoCV", acoCV);
            return sRetorno;
        }

        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaCheckAnalise(AnaliseQuimicaCheckAnaliseDTO checkanalise)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//checkanalise", checkanalise);
            return sRetorno;
        }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaAcoFI(AnaliseQuimicaAcoFIDTO acoFI)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//acoFI", acoFI);
            return sRetorno;
        }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaAcoFP(AnaliseQuimicaAcoFPDTO acoFP)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//acoFP", acoFP);
            return sRetorno;
        }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaAcoPA(AnaliseQuimicaAcoPADTO acoPA)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//acoPA", acoPA);
            return sRetorno;
        }

        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaEscoriaCV(AnaliseQuimicaEscoriaCVDTO escoriaCV)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//escoriaCV", escoriaCV);
            return sRetorno;
        }



        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaEscoriaFP(AnaliseQuimicaEscoriaFPDTO escoriaFP)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("/analiseQuimica//escoriaFP", escoriaFP);
            return sRetorno;
        }

        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public string AnaliseQuimicaEscoriaPA(AnaliseQuimicaEscoriaPADTO escoriaPA)
        {
            string sRetorno = RestService.ExecWebServicePostAsync("//analiseQuimica//escoriaPA", escoriaPA);
            return sRetorno;
        }
        

    }
}
