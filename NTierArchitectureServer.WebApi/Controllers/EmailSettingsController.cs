﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NTierArchitectureServer.Business.Services.EmailSettingServices;
using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSettingsController : ControllerBase
    {
        private readonly IEmailSettingService _emailSettingService;

        public EmailSettingsController(IEmailSettingService emailSettingService)
        {
            _emailSettingService = emailSettingService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            return Ok(await _emailSettingService.GetFirstAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmailSetting emailSetting)
        {
            await _emailSettingService.UpdateAsync(emailSetting);
            return NoContent();
        }
    }
}
