using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS_AnaliseQuimica.Utils;

namespace CPS_AnaliseQuimica.DataBase
{
    // ********************************************************************************************************* //
    //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
    // --------------------------------------------------------------------------------------------------------- //
    //  @Autor: MAURICIO XAVIER                                                                                  //
    //  @Data:  19/05/2020                                                                                       //
    //  @Descricao:                                                                                              //
    // ********************************************************************************************************* //
    public class SqlBase
    {
        //public static string SelectCheckAnalise = " SELECT CORRIDA, PARTICAO, LOTE, PEX, DATA_LINGOTAMENTO, DATA_APONTAMENTO_PRODUCAO, DATA_APONTAMENTO_QUALIDADE, ACO_LINGOTADO, ACO_PRODUZIDO, ACO_LIBERADO, DESOX_PRODUZIDO, DESOX_LIBERADO, MATERIAL_PRODUZIDO, " +
        //    " MATERIAL_LIBERADO, CLIENTE_PRODUZIDO, CLIENTE_LIBERADO, SEGMENTO_PRODUZIDO, SEGMENTO_LIBERADO, BITOLA, NUMERO_ROLOS, PESO_CORRIDA, DESCRICAO_CARACTERISTICA, APELIDO_CARACTERISTICA, VALOR, RESPONSAVEL_ENSAIO, DATA_FECHAMENTO_ENSAIO, RESPONSAVEL_ATUALIZACAO, " +
        //    " DTHRATUALIZACAO, MATERIAL_COMPONENTE, TURNO, TURMA, CODIGO_LAMINADOR, NULL AS PROCESSO, NULL AS APLICACAO, NULL AS ACO_LING, NULL AS TEOR_P, NULL AS TEOR_C, NULL AS CONSUM_LIGAS " +
        //    " FROM BELGOMES.TRACE_CHECKANALISE_CORRIDA " +
        //    " where DATA_APONTAMENTO_PRODUCAO between(SYSDATE - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(SYSDATE) ";

        public static string SelectCheckAnalise = " SELECT 1545, 'P', 100, 'PEX VALUE', SYSDATE, SYSDATE, SYSDATE, '1006L', '1006L', '1006L', 'S', 'N', 'MATER PRODUZIDO', 'MATER LIBERADO', 'CLIENTE PRODUZIDO', 'CLIENTE LIBERADO', 'SEGMEN PRODUZIDO', 'SEGMEN LIBERADO', 'BITOLA', " +
            " 2, 1234, 'CARACTERISTICA', 'APELIDO', 'VALOR', 'RESP ENSAIO', SYSDATE, 'RESP ATUALIZ', SYSDATE, 'MATERIAL COMPONENTE', '1','A','CDLMN','CHQ', 'PROC.COMUM', 'AP', 'AP','BTC', 'BL' FROM DUAL ";




        public static string SelectGusa = " Select B.TIJDLADEKONV as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, " +
            " (CAST((Select top 1 D.WRDE_S From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC ) AS DECIMAL)/10000) as S, " +
            " (CAST((Select top 1 D.WRDE_C From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC) AS DECIMAL)/ 10000) as C, " +
            " (CAST((Select top 1 D.WRDE_MN From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC) AS DECIMAL)/10000) as Mn, " +
            " (CAST((Select top 1 D.WRDE_P From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC ) AS DECIMAL)/10000) as P, " +
            " (CAST((Select top 1 D.WRDE_SI From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC) AS DECIMAL)/10000) as Si, " +
            " (CAST((Select top 1 D.WRDE_Ti From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC ) AS DECIMAL)/10000) as Ti, " +
            " Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora, " +
            " (CAST((Select top 1 D.WRDE_Cr From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC ) AS DECIMAL)/10000) as Cr, " +
            " (CAST((Select top 1 D.WRDE_Ni From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC ) AS DECIMAL)/10000) as Ni, " +
            " (CAST((Select top 1 D.WRDE_Cu From LADIANAL D Where (D.LADINUMR = B.LADINUMR) and(MATECODE = 410) and(D.MONSNUMR = (Select MAX(E.MONSNUMR) From LADIANAL E Where ((E.LADINUMR = D.LADINUMR)) and(E.MATECODE = 410))) ORDER BY TIMESTAM DESC ) AS DECIMAL)/10000) as Cu, " +
            " B.TURMA, B.TURNO, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS " +
            " From LADI B LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K' left join GIETKONT G on G.LADINUMR = B.LADINUMR " +
            " WHERE B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
            " Order By B.TIJDLADEKONV ";



        public static string SelectGusaAFA = " select A.DATERECOKORR as DATA, CAST(A.AFTP AS INT) AS CORRIDA, NULL AS ACO, CAST(ANAL_S AS FLOAT) AS S, CAST(ANAL_CCH AS FLOAT) AS C, CAST(ANAL_MN AS FLOAT) AS MN, CAST(ANAL_P AS FLOAT) AS P, CAST(ANAL_SI AS FLOAT) AS SI, CAST(ANAL_TI AS FLOAT) AS TI, SUBSTRING(CONVERT(VARCHAR, A.DATERECOKORR, 108), 1, 5) AS HORA, CAST(ANAL_CR AS FLOAT) AS CR, CAST(ANAL_NI AS FLOAT) AS NI, " +
            " NULL AS CU, 'A' AS TURMA, case  when((datepart(hour, A.DATERECOKORR) >= 23) or(datepart(hour, A.DATERECOKORR) < 7)) then 1 when((datepart(hour, A.DATERECOKORR) >= 7) and(datepart(hour, A.DATERECOKORR) < 15)) then 2 when((datepart(hour, A.DATERECOKORR) >= 15) and(datepart(hour, A.DATERECOKORR) < 23)) then 2 else 0 end as TURNO " +
            " from ANALRY A WHERE((ANAL_S > 0) OR (ANAL_CCH > 0) OR (ANAL_MN > 0) OR (ANAL_P > 0) OR (ANAL_SI > 0) OR (ANAL_TI > 0) OR (ANAL_CR > 0) OR (ANAL_NI > 0))  AND A.DATERECOKORR between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) Order By A.DATERECOKORR ";

        

        public static string SelectAcoCV = " Select B.TIJDLADEKONV as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, CAST(D.MONSNUMR AS VARCHAR(1)) as Amostra, " +
            " (CAST(D.WRDE_S AS DECIMAL) / 10000) as S, (CAST(D.WRDE_C AS DECIMAL) / 10000) as C, (CAST(D.WRDE_Mn AS DECIMAL)/10000) as Mn, (CAST(D.WRDE_P AS DECIMAL)/10000) as P, (CAST(D.WRDE_Si AS DECIMAL)/10000) as Si, " +
			" (CAST(D.WRDE_Al AS DECIMAL)/10000) as Al, (CAST(D.WRDE_N AS DECIMAL)/10000) as N, (CAST(D.WRDE_B AS DECIMAL)/10000) as B, (CAST(D.WRDE_Nb AS DECIMAL)/10000) as Nb, (CAST(D.WRDE_Ti AS DECIMAL)/10000) as Ti,  " +
            " (CAST(D.WRDE_Cu AS DECIMAL)/10000) as Cu, (CAST(D.WRDE_Cr AS DECIMAL)/10000) as Cr, (CAST(D.WRDE_Mo AS DECIMAL)/10000) as Mo, (CAST(D.WRDE_Ni AS DECIMAL)/10000) as Ni, (CAST(D.WRDE_V AS DECIMAL)/10000) as V, " +
			" (CAST(D.WRDE_Ca AS DECIMAL)/10000) as Ca, (CAST(D.WRDE_Sn AS DECIMAL)/10000) as Sn,  " +
            " (CAST((Select TOP 1 E.CELX_IST From METI E Where(E.LADINUMR = B.LADINUMR) and(E.METINAAM = 'TK') and E.DATEMETI_IST = (Select Max(F.DATEMETI_IST) From METI F Where (F.LADINUMR = E.LADINUMR) and(F.METINAAM = 'TK'))) as DECIMAL)/10000) as CX_C,  " +
            " Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora, B.TURMA, B.TURNO, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS " +
            " From LADI B INNER JOIN LADIANAL D ON (D.LADINUMR = B.LADINUMR) and(D.MATECODE = 420) LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K'  left join GIETKONT G on G.LADINUMR = B.LADINUMR " +
            " WHERE B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
            " Order By B.TIJDLADEKONV ";




        public static string SelectAcoMLC = " Select B.TIJDLADEKONV as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, CAST(D.MONSNUMR AS VARCHAR(1)) as Amostra, " +
            " (CAST(D.WRDE_S AS DECIMAL) / 10000) as S, (CAST(D.WRDE_C AS DECIMAL) / 10000) as C, (CAST(D.WRDE_Mn AS DECIMAL)/10000) as Mn, (CAST(D.WRDE_P AS DECIMAL)/10000) as P, (CAST(D.WRDE_Si AS DECIMAL)/10000) as Si, " +
            " (CAST(D.WRDE_Al AS DECIMAL)/10000) as Al, (CAST(D.WRDE_N AS DECIMAL)/10000) as N, (CAST(D.WRDE_B AS DECIMAL)/10000) as B, (CAST(D.WRDE_Nb AS DECIMAL)/10000) as Nb, (CAST(D.WRDE_Ti AS DECIMAL)/10000) as Ti,  " +
            " (CAST(D.WRDE_Cu AS DECIMAL)/10000) as Cu, (CAST(D.WRDE_Cr AS DECIMAL)/10000) as Cr, (CAST(D.WRDE_Mo AS DECIMAL)/10000) as Mo, (CAST(D.WRDE_Ni AS DECIMAL)/10000) as Ni, (CAST(D.WRDE_V AS DECIMAL)/10000) as V, " +
            " (CAST(D.WRDE_Ca AS DECIMAL)/10000) as Ca, (CAST(D.WRDE_Sn AS DECIMAL)/10000) as Sn,  " +
            " (CAST((Select TOP 1 E.CELX_IST From METI E Where(E.LADINUMR = B.LADINUMR) and(E.METINAAM = 'TK') and E.DATEMETI_IST = (Select Max(F.DATEMETI_IST) From METI F Where (F.LADINUMR = E.LADINUMR) and(F.METINAAM = 'TK'))) as DECIMAL)/10000) as CX_C,  " +
            " Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora, B.TURMA, B.TURNO, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS " +
            " From LADI B INNER JOIN  LADIANAL D ON (D.LADINUMR = B.LADINUMR) and(D.MATECODE = 470) LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K' left join GIETKONT G on G.LADINUMR = B.LADINUMR " +
            " WHERE B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
            " Order By B.TIJDLADEKONV ";



        public static string SelectAcoPA = " Select B.TIJDLADEKONV as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, CAST(D.MONSNUMR AS VARCHAR(1)) as Amostra, " +
            " (CAST(D.WRDE_S AS DECIMAL) / 10000) as S, (CAST(D.WRDE_C AS DECIMAL) / 10000) as C, (CAST(D.WRDE_Mn AS DECIMAL)/10000) as Mn, (CAST(D.WRDE_P AS DECIMAL)/10000) as P, (CAST(D.WRDE_Si AS DECIMAL)/10000) as Si, " +
            " (CAST(D.WRDE_Al AS DECIMAL)/10000) as Al, (CAST(D.WRDE_N AS DECIMAL)/10000) as N, (CAST(D.WRDE_B AS DECIMAL)/10000) as B, (CAST(D.WRDE_Nb AS DECIMAL)/10000) as Nb, (CAST(D.WRDE_Ti AS DECIMAL)/10000) as Ti,  " +
            " (CAST(D.WRDE_Cu AS DECIMAL)/10000) as Cu, (CAST(D.WRDE_Cr AS DECIMAL)/10000) as Cr, (CAST(D.WRDE_Mo AS DECIMAL)/10000) as Mo, (CAST(D.WRDE_Ni AS DECIMAL)/10000) as Ni, (CAST(D.WRDE_V AS DECIMAL)/10000) as V, " +
            " (CAST(D.WRDE_Ca AS DECIMAL)/10000) as Ca, (CAST(D.WRDE_Sn AS DECIMAL)/10000) as Sn,  " +
            " (CAST((Select TOP 1 E.CELX_IST From METI E Where(E.LADINUMR = B.LADINUMR) and(E.METINAAM = 'TP') and E.DATEMETI_IST = (Select Max(F.DATEMETI_IST) From METI F Where (F.LADINUMR = E.LADINUMR) and(F.METINAAM = 'TP'))) as DECIMAL)/10000) as CX_P,  " +
            " Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora, B.TURMA, B.TURNO, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS " +
            " From LADI B INNER JOIN LADIANAL D ON (D.LADINUMR = B.LADINUMR) and(D.MATECODE = 451) LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K' left join GIETKONT G on G.LADINUMR = B.LADINUMR " +
            " WHERE B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
            " Order By B.TIJDLADEKONV ";





        public static string SelectAcoFP = " Select B.TIJDLADEKONV as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, CAST(D.MONSNUMR AS VARCHAR(1)) as Amostra, " +
            " (CAST(D.WRDE_S AS DECIMAL) / 10000) as S, (CAST(D.WRDE_C AS DECIMAL) / 10000) as C, (CAST(D.WRDE_Mn AS DECIMAL) / 10000) as Mn, (CAST(D.WRDE_P AS DECIMAL) / 10000) as P, " +
            " (CAST(D.WRDE_Si AS DECIMAL) / 10000) as Si, (CAST(D.WRDE_Al AS DECIMAL) / 10000) as Al, (CAST(D.WRDE_N AS DECIMAL) / 10000) as N, (CAST(D.WRDE_B AS DECIMAL) / 10000) as B, " +
			" (CAST(D.WRDE_Nb AS DECIMAL) / 10000) as Nb, (CAST(D.WRDE_Ti AS DECIMAL) / 10000) as Ti, (CAST(D.WRDE_Cu AS DECIMAL) / 10000) as Cu, (CAST(D.WRDE_Cr AS DECIMAL) / 10000) as Cr, " + 
			" (CAST(D.WRDE_Mo AS DECIMAL) / 10000) as Mo, (CAST(D.WRDE_Ni AS DECIMAL) / 10000) as Ni, (CAST(D.WRDE_V AS DECIMAL) / 10000) as V, (CAST(D.WRDE_Ca AS DECIMAL) / 10000) as Ca, " +
            " (CAST(D.WRDE_Sn AS DECIMAL) / 10000) as Sn, Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora, " +
            " COALESCE(B.TURMA, '') as turma, COALESCE(B.TURNO, 0) as turno, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS " +
            " From LADI B INNER JOIN LADIANAL D ON (D.LADINUMR = B.LADINUMR) and(D.MATECODE = 457) LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K' left join GIETKONT G on G.LADINUMR = B.LADINUMR " +
            " where B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
            " Order By B.TIJDLADEKONV ";




        public static string SelectAcoFI = " Select CAST(B.TIJDLADEKONV AS DATETIME) as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, CAST(D.MONSNUMR as VARCHAR(1)) as AMOSTRA, " +
            " (CAST(D.WRDE_S AS DECIMAL) / 10000) as S, (CAST(D.WRDE_C AS DECIMAL) / 10000) as C, (CAST(D.WRDE_Mn AS DECIMAL)/10000) as Mn, (CAST(D.WRDE_P AS DECIMAL)/10000) as P, (CAST(D.WRDE_Si AS DECIMAL)/10000) as Si, " +
			" (CAST(D.WRDE_Al AS DECIMAL)/10000) as Al, (CAST(D.WRDE_N AS DECIMAL)/10000) as N, (CAST(D.WRDE_B AS DECIMAL)/10000) as B, (CAST(D.WRDE_Nb AS DECIMAL)/10000) as Nb, (CAST(D.WRDE_Ti AS DECIMAL)/10000) as Ti, " +
            " (CAST(D.WRDE_Cu AS DECIMAL)/10000) as Cu, (CAST(D.WRDE_Cr AS DECIMAL)/10000) as Cr, (CAST(D.WRDE_Mo AS DECIMAL)/10000) as Mo, (CAST(D.WRDE_Ni AS DECIMAL)/10000) as Ni, (CAST(D.WRDE_V AS DECIMAL)/10000) as V, " +
			" (CAST(D.WRDE_Ca AS DECIMAL)/10000) as Ca, (CAST(D.WRDE_Sn AS DECIMAL)/10000) as Sn, Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora, " +
            " B.TURMA, B.TURNO, (CAST(D.WRDE_Mn AS DECIMAL) / (CASE WHEN (D.WRDE_S > 0) THEN COALESCE(CAST(D.WRDE_S AS DECIMAL),1) ELSE 1 END) ) as MN_S, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS " +
            " From LADI B INNER JOIN LADIANAL D ON (D.LADINUMR = B.LADINUMR) and (D.MATECODE = 460) LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K' left join GIETKONT G on G.LADINUMR = B.LADINUMR  " +
            " WHERE B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
            " Order By B.TIJDLADEKONV ";



        public static string SelectEscoriaPA = " Select B.TIJDLADEKONV as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, CAST(D.MONSNUMR AS VARCHAR(1)) as Analise,  " +
             " (CAST(D.WRDE_SiO2 AS DECIMAL) / 10000) as SiO2, (CAST(D.WRDE_Fe AS DECIMAL) / 10000) as Fe, (CAST(D.WRDE_MnO AS DECIMAL)/10000) as MnO, (CAST(D.WRDE_P2O5 AS DECIMAL)/10000) as P2O5, (CAST(D.WRDE_Al2O3 AS DECIMAL)/10000) as Al2O3, " +
			 " (CAST(D.WRDE_CaO AS DECIMAL)/10000) as CaO, (CAST(D.WRDE_MgO AS DECIMAL)/10000) as MgO, Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora,  " +
             " B.TURMA, B.TURNO, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS  " +
             " From LADI B INNER JOIN SLAKANAL D ON (D.LADINUMR = B.LADINUMR) and (D.MATECODE = 651) LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K' left join GIETKONT G on G.LADINUMR = B.LADINUMR  " +
             " WHERE B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
             " Order By B.TIJDLADEKONV ";


        public static string SelectEscoriaFP = " Select B.TIJDLADEKONV as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, CAST(D.MONSNUMR AS VARCHAR(1)) as AMOSTRA,   " +
                         " (CAST(D.WRDE_SiO2 AS DECIMAL) / 10000) as SiO2, (CAST(D.WRDE_Fe AS DECIMAL) / 10000) as Fe, (CAST(D.WRDE_MnO AS DECIMAL)/10000) as MnO, (CAST(D.WRDE_P2O5 AS DECIMAL)/10000) as P2O5, (CAST(D.WRDE_Al2O3 AS DECIMAL)/10000) as Al2O3, " +
		                 " (CAST(D.WRDE_CaO AS DECIMAL)/10000) as CaO, (CAST(D.WRDE_MgO AS DECIMAL)/10000) as MgO, Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora, " +
                         " B.TURMA, B.TURNO, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS  " +
                         " From LADI B INNER JOIN SLAKANAL D ON (D.LADINUMR = B.LADINUMR) and(D.MATECODE = 661) LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K'  left join GIETKONT G on G.LADINUMR = B.LADINUMR  " +
                         " WHERE B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
                         " Order By B.TIJDLADEKONV  ";


        public static string SelectEscoriaCV = " Select B.TIJDLADEKONV as Data, B.LADINUMR as Corrida, COALESCE(B.ANALCODE_AFK, '-*-*-') as Aco, CAST(D.MONSNUMR AS VARCHAR(1)) as Amostra, " +
                       " (CAST(D.WRDE_SiO2 AS DECIMAL) / 10000) as SiO2, (CAST(D.WRDE_Fe AS DECIMAL) / 10000) as Fe, (CAST(D.WRDE_MnO AS DECIMAL)/10000) as MnO, (CAST(D.WRDE_P2O5 AS DECIMAL)/10000) as P2O5,  (CAST(D.WRDE_Al2O3 AS DECIMAL)/10000) as Al2O3, " +
			           " (CAST(D.WRDE_CaO AS DECIMAL)/10000) as CaO, (CAST(D.WRDE_MgO AS DECIMAL)/10000) as MgO,  Substring(CONVERT(VARCHAR, B.TIJDLADEKONV, 120 ), 12, 8 ) as Hora, " +
                       " B.TURMA, B.TURNO, CAST(A.PROCESSO AS VARCHAR(40)) AS PROCESSO, CAST(A.GRUPO AS VARCHAR(30)) AS APLICACAO, CAST(G.ACO_LNG AS VARCHAR(5)) AS ACO_LING, CAST(A.CLASSE_P AS VARCHAR(3)) AS TEOR_P, CAST(A.CLASSE_C AS VARCHAR(3)) AS TEOR_C, CAST(A.CLASSE_LIGA AS VARCHAR(3)) AS CONSUM_LIGAS " +
                       " From LADI B INNER JOIN SLAKANAL D ON (D.LADINUMR = B.LADINUMR) and(D.MATECODE = 641) LEFT JOIN ANALPARA A ON B.ANALCODE_AFK = A.ANALCODE AND A.TYPEANAL = 'K'  left join GIETKONT G on G.LADINUMR = B.LADINUMR  " +
                       " WHERE B.TIJDLADEKONV between (getdate() - " + Constantes.DiasRetroativosAquisicao.ToString() + ") and(getdate()) " +
                       " Order By B.TIJDLADEKONV ";


    }
}
