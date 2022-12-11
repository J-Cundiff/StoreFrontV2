﻿using Microsoft.AspNetCore.Mvc;
using StoreFrontV2.Models;
using System.Diagnostics;
using MimeKit; //Added for access to MimeMessage class
using MailKit.Net.Smtp; //Added for access to SmtpClient class


namespace StoreFrontV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        //GET action
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {

            if (!ModelState.IsValid)
            {
                return View(cvm);
            }
            else
            {

                #region Email Setup Steps & Email Info

                //1. Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution
                //2. Go to the Browse tab and search for NETCore.MailKit
                //3. Click NETCore.MailKit
                //4. On the right, check the box next to the UI project, then click "Install"
                //5. Once installed, return here
                //6. Add the following using statements & comments:
                //      - using MimeKit; //Added for access to MimeMessage class
                //      - using MailKit.Net.Smtp; //Added for access to SmtpClient class
                //7. Add an IConfiguration prop in HomeController.cs and update the ctor
                //8. Once added, return here to continue coding email functionality

                //MIME - Multipurpose Internet Mail Extensions - Allows email to contain
                //information other than ASCII, including audio, video, images, and HTML

                //SMTP - Secure Mail Transfer Protocol - An internet protocol (similar to HTTP)
                //that specializes in the collection & transfer of email data
                #endregion
                string message = $"You have received an email from {cvm.Name} (reply to: {cvm.Email}).\n* Subject: {cvm.Subject}\n* Message: \n{cvm.Message}";
                var mm = new MimeMessage();
                mm.From.Add(new MailboxAddress("No Reply", _config.GetValue<string>("Credentials:Email:User")));
                mm.To.Add(new MailboxAddress("You", _config.GetValue<string>("Credentials:Email:Recipient")));

                mm.Subject = cvm.Subject;

                mm.Body = new TextPart("HTML") { Text = message };

                mm.ReplyTo.Add(new MailboxAddress(cvm.Name, cvm.Email));

                using (var client = new SmtpClient())
                {
                    client.Connect(_config.GetValue<string>("Credentials:Email:Client"));

                    client.Authenticate(
                        _config.GetValue<string>("Credentials:Email:User"),
                        _config.GetValue<string>("Credentials:Email:Password")
                        );

                    try
                    {
                        client.Send(mm);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = $"There was an error sending the email.  Please try again later..";
                        return View(cvm);
                    }
                }



            }




            return View("EmailConfirmation", cvm);
        }
    }
}