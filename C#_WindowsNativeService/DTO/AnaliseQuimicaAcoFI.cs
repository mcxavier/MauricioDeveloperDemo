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
    public class AnaliseQuimicaAcoFI
    {
        public long id { get; set; }
        public Nullable<long> corrida { get; set; }
        public String aco { get; set; }
        public Nullable<decimal> s { get; set; }
        public Nullable<decimal> c { get; set; }
        public Nullable<decimal> mn { get; set; }
        public Nullable<decimal> p { get; set; }
        public Nullable<decimal> si { get; set; }
        public Nullable<decimal> al { get; set; }
        public Nullable<decimal> n { get; set; }
        public Nullable<decimal> b { get; set; }
        public Nullable<decimal> nb { get; set; }
        public Nullable<decimal> ti { get; set; }
        public Nullable<decimal> cu { get; set; }
        public Nullable<decimal> cr { get; set; }
        public Nullable<decimal> mo { get; set; }
        public Nullable<decimal> ni { get; set; }
        public Nullable<decimal> v { get; set; }
        public Nullable<decimal> ca { get; set; }
        public Nullable<decimal> sn { get; set; }
        public Nullable<decimal> mn_s { get; set; }
        public String turma { get; set; }
        public Nullable<long> turno { get; set; }
        public String amostra { get; set; }
        public Nullable<DateTime> data { get; set; }
        public String codCentro { get; set; }
        public String processo { get; set; }
        public String aplicacao { get; set; }
        public String acoLing { get; set; }
        public String teorP { get; set; }
        public String teorC { get; set; }
        public String consumLigas { get; set; }



        public AnaliseQuimicaAcoFI(Nullable<long> CORRIDA, String ACO, Nullable<decimal> S, Nullable<decimal> C, Nullable<decimal> MN, Nullable<decimal> P, 
            Nullable<decimal> SI, Nullable<decimal> AL, Nullable<decimal> N, Nullable<decimal> B, Nullable<decimal> NB, Nullable<decimal> TI, 
            Nullable<decimal> CU, Nullable<decimal> CR, Nullable<decimal> MO, Nullable<decimal> NI, Nullable<decimal> V, Nullable<decimal> CA,
            Nullable<decimal> SN, Nullable<decimal> MN_S, String TURMA, Nullable<long> TURNO, String AMOSTRA, Nullable<DateTime> DATA, String codCentro,
            String processo, String aplicacao, String acoLing, String teorP, String teorC, String consumLigas)
        {
            this.id = 0;
            this.corrida = CORRIDA;
            this.aco = ACO;
            this.s = S;
            this.c = C;
            this.mn = MN;
            this.p = P;
            this.si = SI;
            this.al = AL;
            this.n = N;
            this.b = B;
            this.nb = NB;
            this.ti = TI;
            this.cu = CU;
            this.cr = CR;
            this.mo = MO;
            this.ni = NI;
            this.v = V;
            this.ca = CA;
            this.sn = SN;
            this.mn_s = MN_S;
            this.turma = TURMA;
            this.turno = TURNO;
            this.amostra = AMOSTRA;
            this.data = DATA;
            this.codCentro = codCentro;
            this.processo = processo;
            this.aplicacao = aplicacao;
            this.acoLing = acoLing;
            this.teorP = teorP;
            this.teorC = teorC;
            this.consumLigas = consumLigas;

        }


    }
}
