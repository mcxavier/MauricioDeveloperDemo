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
    public class AnaliseQuimicaGusaAFA
    {
        public long id { get; set; }
        public Nullable<long> corrida { get; set; }
        public Nullable<long> torpedo { get; set; }
        public Nullable<decimal> s { get; set; }
        public Nullable<decimal> c { get; set; }
        public Nullable<decimal> mn { get; set; }
        public Nullable<decimal> p { get; set; }
        public Nullable<decimal> si { get; set; }
        public Nullable<decimal> ti { get; set; }
        public Nullable<decimal> cr { get; set; }
        public Nullable<decimal> ni { get; set; }
        public String turma { get; set; }
        public String mes { get; set; }
        public Nullable<long> turno { get; set; }
        public Nullable<DateTime> data { get; set; }
        public String codCentro { get; set; }




        public AnaliseQuimicaGusaAFA(Nullable<long> CORRIDA, Nullable<long> TORPEDO, Nullable<decimal> S, Nullable<decimal> C, Nullable<decimal> MN, Nullable<decimal> P, Nullable<decimal> SI, 
            Nullable<decimal> TI, Nullable<decimal> CR, Nullable<decimal> NI, String TURMA, String MES, Nullable<long> TURNO, Nullable<DateTime> DATA, String codCentro)
        {
            this.id = 0;
            this.corrida = CORRIDA;
            this.torpedo = TORPEDO;
            this.s = S;
            this.c = C;
            this.mn = MN;
            this.p = P;
            this.si = SI;
            this.ti = TI;
            this.cr = CR;
            this.ni = NI;
            this.turma = TURMA;
            this.mes = MES;
            this.turno = TURNO;
            this.data = DATA;
            this.codCentro = codCentro;

        }

    }
}
