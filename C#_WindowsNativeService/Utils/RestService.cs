using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CPS_AnaliseQuimica.Utils;
using Newtonsoft.Json;
using CPS_Service_Lib.LibUtils;

namespace CPS_AnaliseQuimica.Utils
{
    public class RestService
    {

        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public static string ExecWebServicePostAsync(string metodoRest, object obj)
        {
            string retorno = "";
            string token = CPS_Service_Lib.LibUtils.Constantes.CodCentro + "," +
                           CPS_Service_Lib.LibUtils.Constantes.UserService + "," +
                           CPS_Service_Lib.LibUtils.Constantes.PasswordService;

            token = Criptografia.Criptografar(token);


            HttpClient clientPost = new HttpClient();

            try
            {
                if (clientPost != null)
                {
                    clientPost.Dispose();
                }

                clientPost = new HttpClient();
                clientPost.MaxResponseContentBufferSize = Constantes.MaxResponseContentBufferSize;
                clientPost.Timeout = new TimeSpan(0, Constantes.LimteTimeOutMinutosPost, 0);

                string RestUrl = Constantes.Servidor + Constantes.RaizServico + metodoRest + "?token=" + token;
                var uri = new Uri(string.Format(RestUrl, string.Empty));

                var jsonRequest = JsonConvert.SerializeObject(obj);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = clientPost.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var scontent = response.Content.ReadAsStringAsync();
                    retorno = scontent.Result.ToString();
                }
                else
                {
                    var scontentErr = response.Content.ReadAsStringAsync();
                    retorno = "ERRO: " + scontentErr.Result.ToString();
                }

            }
            catch (Exception ex)
            {
                //await App.ShowMessageAsync("ExecWebServicePostAsync(): " + ex.Message);
                retorno = ex.Message;
            }

            clientPost.Dispose();

            return retorno;
        }


    }
}
