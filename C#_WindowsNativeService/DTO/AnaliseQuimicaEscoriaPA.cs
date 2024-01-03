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
    public class AnaliseQuimicaEscoriaPA
    {
        public long id { get; set; }
        public Nullable<long> corrida { get; set; }
        public String aco { get; set; }
        public Nullable<decimal> sio2 { get; set; }
        public Nullable<decimal> fe { get; set; }
        public Nullable<decimal> mno { get; set; }
        public Nullable<decimal> p2o5 { get; set; }
        public Nullable<decimal> al2o3 { get; set; }
        public Nullable<decimal> cao { get; set; }
        public Nullable<decimal> mgo { get; set; }
        public Nullable<decimal> bas { get; set; }
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




        public AnaliseQuimicaEscoriaPA(Nullable<long> CORRIDA, String ACO, Nullable<decimal> SIO2, Nullable<decimal> FE, Nullable<decimal> MNO, Nullable<decimal> P2O5,
            Nullable<decimal> AL2O3, Nullable<decimal> CAO, Nullable<decimal> MGO, String TURMA, Nullable<long> TURNO, String AMOSTRA, Nullable<DateTime> DATA, 
            String codCentro, String processo, String aplicacao, String acoLing, String teorP, String teorC, String consumLigas)
        {
            // calculo da basicidade
            if (SIO2 > 0)
            {
                this.bas = (CAO / SIO2);
            }
            else
            {
                this.bas = 0;
            }


            this.id = 0;
            this.corrida = CORRIDA;
            this.aco = ACO;
            this.sio2 = SIO2;
            this.fe = FE;
            this.mno = MNO;
            this.p2o5 = P2O5;
            this.al2o3 = AL2O3;
            this.cao = CAO;
            this.mgo = MGO;
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
