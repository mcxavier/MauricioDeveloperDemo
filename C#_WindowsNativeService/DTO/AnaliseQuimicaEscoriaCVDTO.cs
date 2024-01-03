using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_AnaliseQuimica.DTO
{
    // ********************************************************************************************************* //
    //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
    // --------------------------------------------------------------------------------------------------------- //
    //  @Autor: MAURICIO XAVIER                                                                                  //
    //  @Data:  19/05/2020                                                                                       //
    //  @Descricao:                                                                                              //
    // ********************************************************************************************************* //
    public class AnaliseQuimicaEscoriaCVDTO
    {
        public AnaliseQuimicaEscoriaCVDTO()
        {
            listaAnaliseEscoriaCV = new List<AnaliseQuimicaEscoriaCV>(); 
        }
        public List<AnaliseQuimicaEscoriaCV> listaAnaliseEscoriaCV { get; set; }
        public int diasRetroativos { get; set; }
        public string codCentro { get; set; }
    }

}
