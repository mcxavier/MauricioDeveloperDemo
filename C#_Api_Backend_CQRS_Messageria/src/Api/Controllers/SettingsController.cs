using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Core.Products;
using Core.Repositories;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/[controller]"), ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly CoreContext _context;

        public SettingsController(ISettingsRepository settingsRepository, CoreContext context)
        {
            this._settingsRepository = settingsRepository;
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Task.FromResult(Environment.GetEnvironmentVariables());
            return Ok(result);
        }

        [HttpGet, Route("principal-folders")]
        public IActionResult GetPrincipalFolders()
        {
            var result = new
            {
                AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                TempFolder = Path.GetTempPath(),
            };

            return Ok(result);
        }

        [HttpGet, Route("sizes")]
        public async Task<IActionResult> GetSizes()
        {
            var result = await _settingsRepository.GetSpecifications(ProductSpecificationType.Size.Id);
            return Ok(result.OrderBy(x => x.Value));
        }

        [HttpGet, Route("colors")]
        public async Task<IActionResult> GetColors()
        {
            var result = await _settingsRepository.GetSpecifications(ProductSpecificationType.Color.Id);
            return Ok(result);
        }

        [HttpGet, Route("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _settingsRepository.GetCategories();
            return Ok(result);
        }
    }
}