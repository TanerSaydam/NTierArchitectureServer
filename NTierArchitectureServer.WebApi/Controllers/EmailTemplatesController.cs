using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTierArchitectureServer.Business.Services.EmailTemplateServices;
using NTierArchitectureServer.Business.Services.EmailTemplateServices.Dtos;

namespace NTierArchitectureServer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Bearer")]
    public class EmailTemplatesController : ControllerBase
    {
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailTemplatesController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(EmailTemplateDto emailTemplateDto)
        {
            await _emailTemplateService.AddAsync(emailTemplateDto);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(EmailTemplateDto emailTemplateDto)
        {
            await _emailTemplateService.UpdateAsync(emailTemplateDto);
            return NoContent();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            await _emailTemplateService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _emailTemplateService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _emailTemplateService.GetAll();
            return Ok(result);  
        }
    }
}
