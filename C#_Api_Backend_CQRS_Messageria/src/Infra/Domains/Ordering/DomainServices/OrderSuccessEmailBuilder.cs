using System.Collections.Generic;
using System.Globalization;
using Core.Domains.Ordering.DomainServices;
using Core.Domains.Ordering.Models;
using Core.Models.Core.Ordering;

namespace Infra.DomainInfra.DomainServicesImplementation
{
    public class OrderSuccessEmailBuilder : IOrderEmailBuilder
    { // TODO: pensar essa classe de um modo mais inteligente, com Ioc e padronizações para outros clientes

        private string _bodyTemplate { get; set; }
        private string _itemTemplate { get; set; }

        public OrderSuccessEmailBuilder()
        {
            this._bodyTemplate = GetBodyTemplate();
            this._itemTemplate = GetItemTemplate();
        }

        public string BuildEmailBody(Order order)
        {
            var body = this._bodyTemplate;

            body = body.Replace("#ORDER_ID#", order.GetReference());
            body = body.Replace("#ITEMS_RENDER#", this.BuildItems(order.Items));
            body = body.Replace("#ORDER_TOTAL_PRICE#", order.GetNetTotal().ToString());

            return body;
        }

        private string BuildItems(IEnumerable<OrderItem> items)
        {
            var all = string.Empty;

            foreach (var item in items)
            {
                var section = this._itemTemplate;

                section = section.Replace("#ITEM_NAME#", item.ProductName);
                section = section.Replace("#ITEM_QUANTITY#", item.Units.ToString());
                section = section.Replace("#ITEM_UNIT_PRICE#", item.GetNetValue().ToString(CultureInfo.InvariantCulture));
                section = section.Replace("#ITEM_DESCRIPTION#", item.Description);
                section = section.Replace("#ITEM_PICTURE_URL#", item.UrlPicture);

                all += section;
            }

            return all;
        }


        // TODO: TIRRAR ISO DAQUI PELo AMOe DE DEUS ...
        private string GetBodyTemplate()
        {
            return $@"
                <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                <html xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office' style='width:100%;font-family:'Open Sans', sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'>
                 <head> 
                  <meta charset='UTF-8'> 
                  <meta content='width=device-width, initial-scale=1' name='viewport'> 
                  <meta name='x-apple-disable-message-reformatting'> 
                  <meta http-equiv='X-UA-Compatible' content='IE=edge'> 
                  <meta content='telephone=no' name='format-detection'> 
                  <title>Aramis</title> 
                  <!--[if (mso 16)]>
                    <style type='text/css'>
                    a {{text-decoration: none;}}
                    </style>
                    <![endif]--> 
                  <!--[if gte mso 9]><style>sup {{ font-size: 100% !important; }}</style><![endif]--> 
                  <!--[if !mso]><!-- --> 
                  <link href='https://fonts.googleapis.com/css?family=Oswald:300,700&display=swap' rel='stylesheet'> 
                  <!--<![endif]--> 
                  <!--[if gte mso 9]>
                <xml>
                    <o:OfficeDocumentSettings>
                    <o:AllowPNG></o:AllowPNG>
                    <o:PixelsPerInch>96</o:PixelsPerInch>
                    </o:OfficeDocumentSettings>
                </xml>
                <![endif]--> 
                  <!--[if !mso]><!-- --> 
                  <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,400i,700,700i' rel='stylesheet'> 
                  <!--<![endif]--> 
                  <style type='text/css'>
                @media only screen and (max-width:600px) {{p, ul li, ol li, a {{ font-size:12px!important; line-height:150%!important }} h1 {{ font-size:12px!important; text-align:left; line-height:120% }} h2 {{ font-size:20px!important; text-align:left; line-height:120% }} h3 {{ font-size:14px!important; text-align:left; line-height:120% }} h1 a {{ font-size:12px!important; text-align:left }} h2 a {{ font-size:20px!important; text-align:left }} h3 a {{ font-size:14px!important; text-align:left }} .es-menu td a {{ font-size:12px!important }} .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a {{ font-size:12px!important }} .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a {{ font-size:12px!important }} .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a {{ font-size:12px!important }} *[class='gmail-fix'] {{ display:none!important }} .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 {{ text-align:center!important }} .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3 {{ text-align:right!important }} .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 {{ text-align:left!important }} .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img {{ display:inline!important }} .es-button-border {{ display:block!important }} a.es-button {{ font-size:14px!important; display:block!important; border-bottom-width:20px!important; border-right-width:0px!important; border-left-width:0px!important; border-top-width:20px!important }} .es-btn-fw {{ border-width:10px 0px!important; text-align:center!important }} .es-adaptive table, .es-btn-fw, .es-btn-fw-brdr, .es-left, .es-right {{ width:100%!important }} .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header {{ width:100%!important; max-width:600px!important }} .es-adapt-td {{ display:block!important; width:100%!important }} .adapt-img {{ width:100%!important; height:auto!important }} .es-m-p0 {{ padding:0px!important }} .es-m-p0r {{ padding-right:0px!important }} .es-m-p0l {{ padding-left:0px!important }} .es-m-p0t {{ padding-top:0px!important }} .es-m-p0b {{ padding-bottom:0!important }} .es-m-p20b {{ padding-bottom:20px!important }} .es-mobile-hidden, .es-hidden {{ display:none!important }} tr.es-desk-hidden, td.es-desk-hidden, table.es-desk-hidden {{ display:table-row!important; width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important }} .es-desk-menu-hidden {{ display:table-cell!important }} table.es-table-not-adapt, .esd-block-html table {{ width:auto!important }} table.es-social {{ display:inline-block!important }} table.es-social td {{ display:inline-block!important }} .es-m-p5 {{ padding:5px!important }} .es-m-p5t {{ padding-top:5px!important }} .es-m-p5b {{ padding-bottom:5px!important }} .es-m-p5r {{ padding-right:5px!important }} .es-m-p5l {{ padding-left:5px!important }} .es-m-p10 {{ padding:10px!important }} .es-m-p10t {{ padding-top:10px!important }} .es-m-p10b {{ padding-bottom:10px!important }} .es-m-p10r {{ padding-right:10px!important }} .es-m-p10l {{ padding-left:10px!important }} .es-m-p15 {{ padding:15px!important }} .es-m-p15t {{ padding-top:15px!important }} .es-m-p15b {{ padding-bottom:15px!important }} .es-m-p15r {{ padding-right:15px!important }} .es-m-p15l {{ padding-left:15px!important }} .es-m-p20 {{ padding:20px!important }} .es-m-p20t {{ padding-top:20px!important }} .es-m-p20r {{ padding-right:20px!important }} .es-m-p20l {{ padding-left:20px!important }} .es-m-p25 {{ padding:25px!important }} .es-m-p25t {{ padding-top:25px!important }} .es-m-p25b {{ padding-bottom:25px!important }} .es-m-p25r {{ padding-right:25px!important }} .es-m-p25l {{ padding-left:25px!important }} .es-m-p30 {{ padding:30px!important }} .es-m-p30t {{ padding-top:30px!important }} .es-m-p30b {{ padding-bottom:30px!important }} .es-m-p30r {{ padding-right:30px!important }} .es-m-p30l {{ padding-left:30px!important }} .es-m-p35 {{ padding:35px!important }} .es-m-p35t {{ padding-top:35px!important }} .es-m-p35b {{ padding-bottom:35px!important }} .es-m-p35r {{ padding-right:35px!important }} .es-m-p35l {{ padding-left:35px!important }} .es-m-p40 {{ padding:40px!important }} .es-m-p40t {{ padding-top:40px!important }} .es-m-p40b {{ padding-bottom:40px!important }} .es-m-p40r {{ padding-right:40px!important }} .es-m-p40l {{ padding-left:40px!important }} }}
                #outlook a {{
	                padding:0;
                }}
                .ExternalClass {{
	                width:100%;
                }}
                .ExternalClass,
                .ExternalClass p,
                .ExternalClass span,
                .ExternalClass font,
                .ExternalClass td,
                .ExternalClass div {{
	                line-height:100%;
                }}
                .es-button {{
	                mso-style-priority:100!important;
	                text-decoration:none!important;
                }}
                a[x-apple-data-detectors] {{
	                color:inherit!important;
	                text-decoration:none!important;
	                font-size:inherit!important;
	                font-family:inherit!important;
	                font-weight:inherit!important;
	                line-height:inherit!important;
                }}
                .es-desk-hidden {{
	                display:none;
	                float:left;
	                overflow:hidden;
	                width:0;
	                max-height:0;
	                line-height:0;
	                mso-hide:all;
                }}
                </style> 
                 </head> 
                 <body style='width:100%;font-family:'Open Sans', sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'> 
                  <div class='es-wrapper-color' style='background-color:#F5F5F5'> 
                   <!--[if gte mso 9]>
			                <v:background xmlns:v='urn:schemas-microsoft-com:vml' fill='t'>
				                <v:fill type='tile' color='#f5f5f5'></v:fill>
			                </v:background>
		                <![endif]--> 
                   <table class='es-wrapper' width='100%' cellspacing='0' cellpadding='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top'> 
                     <tr style='border-collapse:collapse'> 
                      <td valign='top' style='padding:0;Margin:0'> 
                       <table cellpadding='0' cellspacing='0' class='es-header' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:#1B2A2F;background-repeat:repeat;background-position:center top'> 
                         <tr style='border-collapse:collapse'> 
                          <td align='center' bgcolor='#fff' style='padding:0;Margin:0;background-color:#FFFFFF'> 
                           <table class='es-header-body' cellspacing='0' cellpadding='0' bgcolor='#fff' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px'> 
                             <tr style='border-collapse:collapse'> 
                              <td align='left' style='padding:0;Margin:0;padding-top:25px;padding-bottom:40px'> 
                               <table cellspacing='0' cellpadding='0' width='100%' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                 <tr style='border-collapse:collapse'> 
                                  <td class='es-m-p0r' valign='top' align='center' style='padding:0;Margin:0;width:600px'> 
                                   <table width='100%' cellspacing='0' cellpadding='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='center' class='es-m-txt-c' style='padding:0;Margin:0;padding-top:20px;font-size:0px'><a target='_blank' href='https://www.aramis.com.br/' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'Open Sans', sans-serif;font-size:14px;text-decoration:underline;color:#EF0D33'><img src='https://www.aramis.com.br/arquivos/logo.png' alt style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic' width='150'></a></td> 
                                     </tr> 
                                   </table></td> 
                                 </tr> 
                               </table></td> 
                             </tr> 
                           </table></td> 
                         </tr> 
                       </table> 
                       <table cellpadding='0' cellspacing='0' class='es-content' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%'> 
                         <tr style='border-collapse:collapse'> 
                          <td align='center' style='padding:0;Margin:0'> 
                           <table bgcolor='#ffffff' class='es-content-body' align='center' cellpadding='0' cellspacing='0' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#F5F5F5;width:600px'> 
                             <tr style='border-collapse:collapse'> 
                              <td align='left' style='Margin:0;padding-left:15px;padding-right:15px;padding-top:40px;padding-bottom:40px'> 
                               <table cellpadding='0' cellspacing='0' width='100%' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                 <tr style='border-collapse:collapse'> 
                                  <td align='center' valign='top' style='padding:0;Margin:0;width:570px'> 
                                   <table cellpadding='0' cellspacing='0' width='100%' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='center' class='es-m-txt-l' style='padding:0;Margin:0'><h3 style='Margin:0;line-height:17px;mso-line-height-rule:exactly;font-family:Oswald, sans-serif;font-size:14px;font-style:normal;font-weight:bold;color:#888888;letter-spacing:0px'>PEDIDO #ORDER_ID#</h3></td> 
                                     </tr> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='center' style='padding:0;Margin:0'><h1 style='Margin:0;line-height:34px;mso-line-height-rule:exactly;font-family:Oswald, sans-serif;font-size:28px;font-style:normal;font-weight:bold;color:#262626'>ITENS DO PEDIDO</h1></td> 
                                     </tr> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='center' style='padding:0;Margin:0'><h3 style='Margin:0;line-height:17px;mso-line-height-rule:exactly;font-family:Oswald, sans-serif;font-size:14px;font-style:normal;font-weight:bold;color:#888888;letter-spacing:0px'><br><span style='color:#FF0000'>SEU PEDIDO</span> FOI APROVADO E SERÁ ENVIADO EM BREVE. </h3></td> 
                                     </tr> 
                                   </table></td> 
                                 </tr> 
                               </table></td> 
                             </tr> 
                             <tr style='border-collapse:collapse'> 
                              <td align='left' style='Margin:0;padding-top:20px;padding-bottom:20px;padding-left:20px;padding-right:20px;background-color:#000000' bgcolor='#000'> 
                               <!--[if mso]><table style='width:560px' cellpadding='0' cellspacing='0'><tr><td style='width:75px' valign='top'><![endif]--> 
                               <table class='es-left' cellspacing='0' cellpadding='0' align='left' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left'> 
                                 <tr style='border-collapse:collapse'> 
                                  <td class='es-m-p0r' valign='top' align='center' style='padding:0;Margin:0;width:75px'> 
                                   <table width='100%' cellspacing='0' cellpadding='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                     <tr class='es-mobile-hidden' style='border-collapse:collapse'> 
                                      <td align='center' style='padding:0;Margin:0'><h4 style='Margin:0;line-height:120%;mso-line-height-rule:exactly;font-family:Oswald, sans-serif;color:#FFFFFF'>Item</h4></td> 
                                     </tr> 
                                   </table></td> 
                                 </tr> 
                               </table> 
                               <!--[if mso]></td><td style='width:40px'></td><td style='width:445px' valign='top'><![endif]--> 
                               <table cellspacing='0' cellpadding='0' align='right' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                 <tr style='border-collapse:collapse'> 
                                  <td align='left' style='padding:0;Margin:0;width:445px'> 
                                   <table width='100%' cellspacing='0' cellpadding='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='center' style='padding:0;Margin:0'> 
                                       <table style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%' class='cke_show_border' cellspacing='1' cellpadding='1' border='0' role='presentation'> 
                                         <tr style='border-collapse:collapse'> 
                                          <td style='padding:0;Margin:0;font-size:13px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;font-family:'Open Sans', sans-serif;line-height:17px;color:#FFFFFF'><strong>Nome</strong></p></td> 
                                          <td style='padding:0;Margin:0;text-align:center;font-size:13px;line-height:13px' width='15%'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;font-family:'Open Sans', sans-serif;line-height:17px;color:#FFFFFF'><strong>Qnt</strong></p></td> 
                                          <td style='padding:0;Margin:0;text-align:center;font-size:13px;line-height:13px' width='30%'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;font-family:'Open Sans', sans-serif;line-height:17px;color:#FFFFFF'><strong>Preço</strong></p></td> 
                                         </tr> 
                                       </table></td> 
                                     </tr> 
                                   </table></td> 
                                 </tr> 
                               </table> 
                               <!--[if mso]></td></tr></table><![endif]--></td> 
                             </tr> 

                             #ITEMS_RENDER#

                             <tr style='border-collapse:collapse'> 
                              <td align='left' style='padding:0;Margin:0;background-position:center center'> 
                               <table cellpadding='0' cellspacing='0' width='100%' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                 <tr style='border-collapse:collapse'> 
                                  <td align='center' valign='top' style='padding:0;Margin:0;width:600px'> 
                                   <table cellpadding='0' cellspacing='0' width='100%' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='center' style='Margin:0;padding-top:5px;padding-bottom:5px;padding-left:15px;padding-right:15px;font-size:0'> 
                                       <table border='0' width='100%' height='100%' cellpadding='0' cellspacing='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                         <tr style='border-collapse:collapse'> 
                                          <td style='padding:0;Margin:0px;border-bottom:1px solid #666666;background:none;height:1px;width:100%;margin:0px'></td> 
                                         </tr> 
                                       </table></td> 
                                     </tr> 
                                   </table></td> 
                                 </tr> 
                               </table></td> 
                             </tr>
                              
                             <tr style='border-collapse:collapse'> 
                              <td align='left' style='Margin:0;padding-left:10px;padding-right:10px;padding-top:40px;padding-bottom:40px'> 
                               <table cellpadding='0' cellspacing='0' width='100%' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                 <tr style='border-collapse:collapse'> 
                                  <td align='center' valign='top' style='padding:0;Margin:0;width:580px'> 
                                   <table cellpadding='0' cellspacing='0' width='100%' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                   
                                    <td align='left' style='padding:0;Margin:0'> 
                                      <strong>VALOR TOTAL: #ORDER_TOTAL_PRICE#</strong> 
                                    </td>





                                   </table></td> 
                                 </tr> 
                               </table></td> 
                             </tr> 
                           </table></td> 
                         </tr> 
                       </table> 
                       <table class='es-footer' cellspacing='0' cellpadding='0' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:#111517;background-repeat:repeat;background-position:center top'> 
                         <tr style='border-collapse:collapse'> 
                          <td align='center' bgcolor='#fff' style='padding:0;Margin:0;background-color:#FFFFFF'> 
                           <table class='es-footer-body' cellspacing='0' cellpadding='0' bgcolor='#fff' align='center' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px'> 
                             <tr style='border-collapse:collapse'> 
                              <td align='left' style='Margin:0;padding-left:20px;padding-right:20px;padding-top:40px;padding-bottom:40px'> 
                               <!--[if mso]><table style='width:560px' cellpadding='0' 
                                        cellspacing='0'><tr><td style='width:175px' valign='top'><![endif]--> 
                               <table class='es-left' cellspacing='0' cellpadding='0' align='left' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left'> 
                                 <tr style='border-collapse:collapse'> 
                                  <td class='es-m-p20b' align='left' style='padding:0;Margin:0;width:175px'> 
                                   <table width='100%' cellspacing='0' cellpadding='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='center' class='es-m-txt-c' style='padding:0;Margin:0;font-size:0px'><a target='_blank' href='https://viewstripo.email/' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'Open Sans', sans-serif;font-size:12px;text-decoration:underline;color:#EF0D33'><img src='https://www.aramis.com.br/arquivos/logo.png' alt style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic' width='150'></a></td> 
                                     </tr> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='center' class='es-m-txt-c' style='padding:0;Margin:0;padding-top:20px;font-size:0'> 
                                       <table cellpadding='0' cellspacing='0' class='es-table-not-adapt es-social' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                         <tr style='border-collapse:collapse'> 
                                          <td align='center' valign='top' style='padding:0;Margin:0;padding-right:10px'><a target='_blank' href='https://www.facebook.com/aramismenswear' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'Open Sans', sans-serif;font-size:12px;text-decoration:underline;color:#EF0D33'>
                                            <img src='https://w7.pngwing.com/pngs/973/323/png-transparent-facebook-logo-social-media-facebook-linkedin-estate-agent-computer-icons-background-black-logo-internet-property.png' alt='Fb' title='Facebook' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic'></a>
                                          </td> 
                                          <td align='center' valign='top' style='padding:0;Margin:0;padding-right:10px'><a target='_blank' href='https://www.youtube.com/channel/UCJXYr3nD41n1kaYAzGXJ-dg' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'Open Sans', sans-serif;font-size:12px;text-decoration:underline;color:#EF0D33'>
                                            <img src='https://toppng.com/uploads/preview/youtube-black-logo-11549680987z3oylzt1lb.png' alt='Yt' title='Youtube' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic'></a></td> 
                                          <td align='center' valign='top' style='padding:0;Margin:0;padding-right:10px'><a target='_blank' href='https://www.instagram.com/aramismenswear/' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'Open Sans', sans-serif;font-size:12px;text-decoration:underline;color:#EF0D33'>
                                            <img src='https://i.pinimg.com/originals/6b/42/c7/6b42c7750811d31baa3659803af22052.png' alt='Ig' title='Instagram' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic'></a></td> 
                                          <td align='center' valign='top' style='padding:0;Margin:0;padding-right:10px'><a target='_blank' href='https://www.linkedin.com/company/aramis-menswear/' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'Open Sans', sans-serif;font-size:12px;text-decoration:underline;color:#EF0D33'><img src='https://image.flaticon.com/icons/svg/38/38669.svg' alt='In' title='Linkedin' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic'></a></td> 
                                          <td align='center' valign='top' style='padding:0;Margin:0'><a target='_blank' href='https://open.spotify.com/user/radioaramis?si=YkJO6jllRBmViPM6vEIvFw' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'Open Sans', sans-serif;font-size:12px;text-decoration:underline;color:#EF0D33'><img src='https://p7.hiclipart.com/preview/833/349/159/spotify-logo-streaming-media-apple-music-others-thumbnail.jpg' alt='Sp' title='Spotify' width='24' height='24' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic'></a></td> 
                                         </tr> 
                                       </table></td> 
                                     </tr> 
                                   </table></td> 
                                 </tr> 
                               </table> 
                               <!--[if mso]></td><td style='width:20px'></td><td style='width:365px' valign='top'><![endif]--> 
                               <table class='es-right' cellspacing='0' cellpadding='0' align='right' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right'> 
                                 <tr style='border-collapse:collapse'> 
                                  <td align='left' style='padding:0;Margin:0;width:365px'> 
                                   <table width='100%' cellspacing='0' cellpadding='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                     <tr style='border-collapse:collapse'> 
                                      <td align='left' class='es-m-txt-c' style='padding:0;Margin:0;padding-top:20px;padding-bottom:20px'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:9px;font-family:'Open Sans', sans-serif;line-height:14px;color:#000000'>TODOS OS DIREITOS RESERVADOS. A ARAMIS RESERVA-SE NO DIREITO DE CORRIGIR OU ALTERAR PREÇOS, PROMOÇÕES E DISPONIBILIDADE DE ESTOQUE A QUALQUER MOMENTO, SEM PRÉVIO AVISO. VCI VANGUARD CONFECÇÕES IMPORTADAS S/A - CNPJ: 00.311.557/0064-27 - RUA PORTO ALEGRE, 307 - NOVA ZELÂNDIA - 29175-706 - SERRA/ES</p></td> 
                                     </tr> 
                                   </table></td> 
                                 </tr> 
                               </table> 
                               <!--[if mso]></td></tr></table><![endif]--></td> 
                             </tr> 
                           </table></td> 
                         </tr> 
                       </table></td> 
                     </tr> 
                   </table> 
                  </div>  
                 </body>
                </html>
            ";
        }

        private string GetItemTemplate()
        {
            return @"<tr style='border-collapse:collapse'> 
                      <td class='esdev-adapt-off' align='left' style='padding:0;Margin:0;padding-top:15px;padding-left:15px;padding-right:15px'> 
                        <table cellpadding='0' cellspacing='0' class='esdev-mso-table' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:570px'> 
                          <tr style='border-collapse:collapse'> 
                          <td class='esdev-mso-td' valign='top' style='padding:0;Margin:0'> 
                            <table class='es-left' cellspacing='0' cellpadding='0' align='left' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left'> 
                              <tr style='border-collapse:collapse'> 
                              <td class='es-m-p0r' valign='top' align='center' style='padding:0;Margin:0;width:120px'> 
                                <table width='100%' cellspacing='0' cellpadding='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                  <tr style='border-collapse:collapse'> 
                                    <td align='center' style='padding:0;Margin:0;padding-right:20px;font-size:0px'>
                                      <img src='#ITEM_PICTURE_URL#' style='display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;' width='100' height='100'>
                                    </td>
                                  </tr> 
                                </table></td> 
                              </tr> 
                            </table></td> 
                          <td class='esdev-mso-td' valign='top' style='padding:0;Margin:0'> 
                            <table cellspacing='0' cellpadding='0' align='right' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                              <tr style='border-collapse:collapse'> 
                              <td align='left' style='padding:0;Margin:0;width:480px'> 
                                <table width='100%' cellspacing='0' cellpadding='0' role='presentation' style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px'> 
                                  <tr style='border-collapse:collapse'> 
                                  <td align='left' style='padding:0;Margin:0;padding-bottom:10px'> 
                                    <table style='mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%' class='cke_show_border' cellspacing='1' cellpadding='1' border='0' role='presentation'> 
                                      <tr style='border-collapse:collapse'>                    
                                        <td style='padding:0;Margin:0'>
                                          <h1 style='Margin:0;line-height:14px;mso-line-height-rule:exactly;font-family:Oswald, sans-serif;font-size:12px;font-style:normal;font-weight:bold;color:#262626'>
                                            #ITEM_NAME#
                                          </h1>
                                          <span style='Margin:0;font-size:12px;font-family:'Open Sans', sans-serif;line-height:21px;color:#999999; line-break: auto'>
                                            #ITEM_DESCRIPTION#
                                          </span>
                                      </td> 
                                      <td style='padding:0;Margin:0;text-align:center' width='15%' align='center'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;font-family:'Open Sans', sans-serif;line-height:21px;color:#262626'>#ITEM_QUANTITY#</p></td> 
                                      <td style='padding:0;Margin:0;text-align:center' width='30%'><p style='Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:14px;font-family:'Open Sans', sans-serif;line-height:21px;color:#262626'>#ITEM_UNIT_PRICE#</p></td> 
                                      </tr> 
                                    </table></td> 
                                  </tr> 
                                </table></td> 
                              </tr> 
                            </table></td> 
                          </tr> 
                        </table>
                      </td> 
                    </tr> 
                    ";
        }
    }
}