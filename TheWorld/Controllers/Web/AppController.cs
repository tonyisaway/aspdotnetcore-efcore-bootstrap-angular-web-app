namespace TheWorld.Controllers.Web
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using TheWorld.Services;
    using TheWorld.ViewModels;

    public class AppController : Controller
    {
        private IMailService mailService;

        private IConfigurationRoot config;

        public AppController(IMailService mailService, IConfigurationRoot config)
        {
            this.mailService = mailService;
            this.config = config;
        }
        public IActionResult Index()
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
