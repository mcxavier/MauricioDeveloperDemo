using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Api.App_Infra;
using Core.SharedKernel;
using Core.Repositories;
using Infra.EntitityConfigurations.Contexts;
using Core.Models.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Infra.Extensions;
using Infra.ExternalServices.Azure;
using Infra.ExternalServices.Azure.Configurations;
using Api.Requests;
using Api.ViewModels;
using System.Text.RegularExpressions;
using Infra.ExternalServices.Authentication;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Core.Models.Identity.Companies;
using Core.Domains.Company.Dtos;
using Newtonsoft.Json;


namespace Api.Controllers
{
    [ServiceFilter(typeof(SmartSalesAuthorizeAttribute))]
    [Route("api/v1/[controller]"), ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IdentityContext _context;
        private readonly long MAX_CONTENT_LENGTH = 1024 * 1024 * 5;
        private readonly IAzureStorageIntegrationServices azureServices;
        private readonly AzureStorageConfig storageConfig;
        private readonly SmartSalesIdentity _identity;

        public CompanyController(IStoreRepository context,
                                        IdentityContext dbContext,
                                        IAzureStorageIntegrationServices azureStorageServices,
                                        SmartSalesIdentity identity)
        {
            this._storeRepository = context;
            this._identity = identity;
            _context = dbContext;
            azureServices = azureStorageServices;
            storageConfig = azureStorageServices.GetStorageConfig();
        }

        [HttpGet, Route("store-addresses")]
        public async Task<IActionResult> GetStoresAdddress()
        {
            try
            {   //Esse trecho pega o PortalUrl que está na requisição.
                var context = HttpContext;
                string storePortal = this._identity.CurrentStorePortal;
                Guid companyId = this._identity.CurrentCompany ?? Guid.Empty;

                var stores = await this._storeRepository.GetAllStoresAsync(companyId);//Sem essa linha não é possivel fazer a validação do endereço

                var result = await _context.StoreAddresses.FirstOrDefaultAsync(x => x.Store.PortalUrl == storePortal);

                result.Store = null;

                return Ok(new Response
                {
                    Message = "Endereço encontrado",
                    Payload = result,
                });
            }
            catch
            {
                return NotFound(new Response
                {
                    Message = "Endereço não encontrado",
                    Payload = new object[] { },
                });
            }
        }

        [HttpGet, Route("contact-info")]
        public async Task<IActionResult> GetStoreContactInfo()
        {
            try
            {   //Esse trecho pega o PortalUrl que está na requisição.
                var context = HttpContext;
                string storePortal = this._identity.CurrentStorePortal;
                Guid companyId = this._identity.CurrentCompany ?? Guid.Empty;

                var stores = await this._storeRepository.GetAllStoresAsync(companyId);//Sem essa linha não é possivel fazer a validação do endereço

                var firstResult = stores.FirstOrDefault(x => x.PortalUrl == storePortal);

                if (firstResult == null)
                    throw new Exception("First Result came empty.");

                return Ok(new Response
                {
                    Message = "Informações de contato encontradas",
                    Payload = new StoreContactInfo
                    {
                        Name = firstResult.Description,
                        Phone = firstResult.Phone
                    },
                });
            }
            catch
            {
                return NotFound(new Response
                {
                    Message = "Informações de contato não encontradas",
                    Payload = new object[] { },
                });
            }
        }

        [HttpGet, Route("sales-config")]
        public async Task<IActionResult> GetStoreSalesConfig()
        {
            try
            {
                Guid companyId = this._identity.CurrentCompany ?? Guid.Empty;
                var firstResult = _context.CompanySettings.FirstOrDefault(x => x.TypeId == (int)CompanySettingsTypeEnum.SalesConfig
                                            && x.CompanyId == companyId);

                if (firstResult == null)
                {
                    return NotFound(new Response
                    {
                        Message = "Nenhuma configuração de vendas encontrada para esta companhia.",
                        Payload = new object[] { },
                    });
                }

                return Ok(new Response
                {
                    Message = "Configurações de vendas encontradas",
                    Payload = JsonConvert.DeserializeObject<SalesConfigDto>(firstResult.Value),
                });
            }
            catch
            {
                return StatusCode(500, new Response
                {
                    Message = "Erro interno do Servidor.",
                    Payload = new object[] { },
                });
            }
        }

        [HttpGet, Route("stores")]
        public async Task<IActionResult> GetStores()
        {
            Guid companyId = this._identity.CurrentCompany ?? Guid.Empty;
            var result = await this._storeRepository.GetAllStoresAsync(companyId);

            if (!result.Any())
            {
                return NotFound(new Response
                {
                    Message = "Nenhuma loja encontrada",
                    Payload = new object[] { },
                });
            }

            return Ok(new Response
            {
                Message = "lojas encontradas com sucesso",
                Payload = result,
            });
        }

        private string GetBlobFolderName(string storePortal)
        {
            return "tenant_" + storePortal;
        }

        private string GetBlobName(string storePortal, string contentName, string fileExt = "jpg")
        {
            return GetBlobFolderName(storePortal) + "/" + contentName + "." + fileExt;
        }

        private string GetBlobURL(AzureStorageConfig storageConfig, string storePortal, string contentName, string fileExt = "jpg")
        {
            return storageConfig.BaseUrl + storageConfig.ContainerName + "/" + GetBlobName(storePortal, contentName, fileExt);
        }

        [HttpPost, Route("visual-identity")]
        public async Task<IActionResult> PostVisual([FromBody] UpdateVisualIdentityRequest request)
        {
            var context = HttpContext;
            try
            {
                string storePortal = this._identity.CurrentStorePortal;
                if (this._identity.CurrentStorePortal != storePortal)
                {
                    return Unauthorized(new Response
                    {
                        IsError = true,
                        Message = "Não autorizado. Nome de loja não confere."
                    });
                }

                var firstResult = this._context.Stores.FirstOrDefault(x => x.PortalUrl == storePortal);
                if (firstResult == null)
                {
                    return NotFound(new Response
                    {
                        IsError = true,
                        Message = "Nenhuma loja encontrada."
                    });
                }

                if (request.StorePortal != null && request.StorePortal.Length > 3 && request.StorePortal != storePortal)
                {
                    var newFirstResult = this._context.Stores.FirstOrDefault(x => x.PortalUrl == request.StorePortal);
                    if (newFirstResult != null)
                    {
                        return StatusCode(500, new Response
                        {
                            IsError = true,
                            Message = "Nome de loja indisponível."
                        });
                    }
                    else
                    {
                        await this.azureServices.RenameFolder(this.storageConfig.ContainerName, GetBlobFolderName(storePortal), GetBlobFolderName(request.StorePortal));
                        firstResult.PortalUrl = request.StorePortal;
                        this._context.Stores.Update(firstResult);
                        await this._context.SaveChangesAsync();
                        storePortal = request.StorePortal;
                    }
                }

                // Define a regular expression for zipCode and rgb color validation.
                Regex zipCodeRegex = new Regex(@"(^\d{5}$)|(^\d{9}$)|(^\d{5}-\d{4}$)|(^\d{5}-\d{3}$)",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex rgbRegex = new Regex(@"(?:#|0x)(?:[a-f0-9]{3}|[a-f0-9]{6})",
                  RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if ((request.ZipCode != null && !zipCodeRegex.IsMatch(request.ZipCode)) ||
                    !rgbRegex.IsMatch(request.MainColor) ||
                    !rgbRegex.IsMatch(request.SecondaryColor))
                {
                    string message = "Invalid ";
                    if ((request.ZipCode != null && !zipCodeRegex.IsMatch(request.ZipCode))) message += "zipCode ";
                    if (!rgbRegex.IsMatch(request.MainColor)) message += "mainColor ";
                    if (!rgbRegex.IsMatch(request.SecondaryColor)) message += "secondaryColor ";
                    return UnprocessableEntity(new Response
                    {
                        IsError = true,
                        Message = message,
                    });
                }

                var stream = new MemoryStream(2048);
                VisualIdentityViewModel visualIdentity = new VisualIdentityViewModel
                {
                    ZipCode = request.ZipCode,
                    ProductsQuant = request.ProductsQuant ?? 15,
                    Freight = request.Freight,
                    Additional = request.Additional,
                    MainColor = request.MainColor,
                    SecondaryColor = request.SecondaryColor,
                    Range = request.Range,
                    WhatsApp = request.WhatsApp,
                    FooterMessage = request.FooterMessage
                };
                string visualString = JsonConvert.SerializeObject(visualIdentity);
                byte[] jsonByteContent = System.Text.Encoding.UTF8.GetBytes(visualString);
                stream.Write(jsonByteContent);
                stream.Position = 0;
                const string contentType = "application/json";
                const string contentName = "visual_identity";

                string blobName = GetBlobName(storePortal, contentName, "json");
                azureServices.SendStreamToBlob(stream, storageConfig.ContainerName, blobName, contentType);
                stream.Close();
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    IsError = true,
                    Message = e.ToString(),
                });
            }

            return Ok(new Response
            {
                IsError = false,
                Message = "Identidade visual atualizada com sucesso.",
            });
        }

        [HttpGet, Route("visual-identity/{storePortal}")]
        public async Task<IActionResult> GetVisual([FromRoute] string storePortal)
        {
            var context = HttpContext;
            HttpResponseMessage response;
            string responseString;
            VisualIdentityViewModel visualIdentity;
            if (storePortal.CompareTo("undefined") == 0)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new Response
                {
                    IsError = true,
                    Message = HttpStatusCode.BadRequest.ToString(),
                });
            }
            try
            {
                using (var client = new HttpClient())
                {

                    string visual_identity_file_path = "/" + storageConfig.ContainerName
                                                        + "/"
                                                        + "tenant_"
                                                        + storePortal
                                                        + "/"
                                                        + "visual_identity.json";

                    string hostname = storageConfig.BaseBlobUrl;
                    client.BaseAddress = new Uri(hostname);
                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var request = new HttpRequestMessage(HttpMethod.Get, visual_identity_file_path);
                    request.Headers.Add("Accept", "*/*");
                    request.Headers.Add("Connection", "keep-alive");
                    request.Headers.Add("Host", client.BaseAddress.Host);

                    response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        responseString = response.Content.ReadAsStringAsync().Result;
                        visualIdentity = Newtonsoft.Json.JsonConvert.DeserializeObject<VisualIdentityViewModel>(responseString);
                    }
                    else
                    {
                        return StatusCode(((int)response.StatusCode), new Response
                        {
                            IsError = true,
                            Message = response.StatusCode.ToString(),
                        });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    IsError = true,
                    Message = e.ToString(),
                });
            }

            return Ok(visualIdentity);
        }

        [HttpGet, Route("{storePortal}/exists")]
        public async Task<IActionResult> Exists([FromRoute] string storePortal)
        {
            try
            {
                var firstResult = this._context.Stores.FirstOrDefault(x => x.PortalUrl == storePortal);
                if (firstResult == null)
                {
                    return NotFound(new Response
                    {
                        Message = "Nenhuma loja encontrada",
                    });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    Message = e.ToString(),
                });
            }

            return Ok(new Response
            {
                Message = "Loja encontrada",
            });
        }

        [HttpPost, Route("logo"), Route("banner")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> PostLogo()
        {
            var context = HttpContext;
            string requestPath = context.Request.Path.ToString();
            bool isLogo = requestPath.EndsWith("/logo") || requestPath.EndsWith("/logo/");
            bool isBanner = requestPath.EndsWith("/banner") || requestPath.EndsWith("/banner/");
            string contentName = isBanner ? "banner" : (isLogo ? "logo" : "unknown");
            if (!isLogo && !isBanner)
            {
                return UnprocessableEntity(new Response
                {
                    Message = "Conteúdo não reconhecido",
                });
            }
            try
            {
                string storePortal = this._identity.CurrentStorePortal;
                var firstResult = this._context.Stores.FirstOrDefault(x => x.PortalUrl == storePortal);

                if (firstResult == null)
                {
                    return NotFound(new Response
                    {
                        Message = "Nenhuma loja encontrada",
                    });
                }
                var body = context.Request.Body;
                var form = context.Request.Form;
                var files = form.Files;
                int filecount = files.Count();
                if (filecount != 1)
                {
                    return UnprocessableEntity(new Response
                    {
                        Message = "Envie apenas um arquivo por request.",
                    });
                }


                foreach (var file in files)
                {
                    bool fileLengthConditionError = file.Length > MAX_CONTENT_LENGTH;
                    bool fileFieldFile = file.Name.CompareTo("file") == 0;
                    bool fileFieldBanner = file.Name.CompareTo("banner") == 0;
                    bool fileFieldLogo = file.Name.CompareTo("logo") == 0;

                    if (fileLengthConditionError)
                    {
                        return UnprocessableEntity(new Response
                        {
                            Message = "Tamanho do conteúdo excede tamanho máximo permitido de " + MAX_CONTENT_LENGTH + " bytes",
                        });
                    }
                    else if (fileFieldFile || (fileFieldBanner == isBanner) || (fileFieldLogo == isLogo))
                    {
                        string filePath = Path.GetTempFileName();
                        var stream = System.IO.File.Create(filePath, (int)MAX_CONTENT_LENGTH);
                        await file.CopyToAsync(stream);
                        stream.Flush();
                        stream.Close();
                        string blobName = GetBlobName(storePortal, contentName);
                        azureServices.SendFileToBlob(storageConfig.ContainerName, blobName, filePath, file.ContentType);
                        System.IO.File.Delete(filePath);
                    }
                    else
                    {
                        return UnprocessableEntity(new Response
                        {
                            Message = "Campo de arquivo inválido.",
                        });
                    }
                }


            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    Message = e.ToString(),
                });
            }
            return Ok(new Response
            {
                Message = contentName + " atualizado(a) com sucesso",
            });
        }

        [HttpGet, Route("stores/near-cep/{cep}")]
        public async Task<IActionResult> GetStoresNearCEP([FromRoute] string cep)
        {
            var stores = new List<Store>();

            Store sameStore = await this._storeRepository.GetStoreByPortalNameAsync(this._identity.CurrentStorePortal);
            if (sameStore != null) stores.Add(sameStore);

            var result = stores.Select(x =>
            {
                var address = this._storeRepository.GetStoreAddressAsync(x.Id).Result;
                return new
                {
                    StoreId = x.Id,
                    StoreName = x.Name,
                    StreetName = address?.StreetName,
                    StreetNumber = address?.StreetNumber,
                    Reference = address?.Reference,
                    CityName = address?.CityName,
                    StateName = address?.StateName,
                    Complement = address?.Complement
                };
            });

            if (!result.Any())
            {
                return NotFound(new Response
                {
                    Message = "Nenhuma loja encontrada",
                    Payload = new object[] { },
                });
            }

            return Ok(new Response
            {
                Message = "Lojas encontradas com sucesso",
                Payload = result,
            });
        }
    }
}