using BelotApp.Models;
using BelotApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

[Authorize]
public class ContactsController : Controller
{
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public JsonResult SubmitReport(BugReportVM bugReportVM)
    {
        if (ModelState.IsValid)
        {
            MailMessage mm = new MailMessage("infobeloteapp@gmail.com", "ante.prskalo4@gmail.com");
            mm.Subject = bugReportVM.Subject;
            mm.Body = bugReportVM.Name + " submits a bug report: " + bugReportVM.Description;
            mm.IsBodyHtml = false;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("infobeloteapp@gmail.com", "mtpa ukpi gkei ppqf");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = nc;
            smtpClient.Send(mm);

            return Json(new { success = true });
        }

        return Json(new { success = false });
    }
}

