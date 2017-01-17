namespace TheWorld.Controllers.Web
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using TheWorld.Models;
    using TheWorld.Services;
    using TheWorld.ViewModels;

    public class AppController : Controller
    {
        private readonly IMailService mailService;

        private readonly IConfigurationRoot config;

        private readonly IWorldRepository repository;

        private ILogger<AppController> logger;

        public AppController(
            IMailService mailService, 
            IConfigurationRoot config, 
            IWorldRepository repository,
            ILogger<AppController> logger)
        {
            this.mailService = mailService;
            this.config = config;
            this.repository = repository;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Trips()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com"))
            {
                this.ModelState.AddModelError(string.Empty, "We don't support AOL addresses");
            }

            if (this.ModelState.IsValid)
            {
                this.mailService.SendMail(this.config["MailSettings:ToAddress"], model.Email, "From the World.", model.Message);
            }

            this.ViewBag.UserMessage = "Message Sent";
            this.ModelState.Clear();

            return this.View();
        }

        public IActionResult About()
        {
            return this.View();
        }
    }
}
