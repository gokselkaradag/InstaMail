using InstaMail.BusinessLayer.Helpers;
using InstaMail.DataAccessLayer.Concrete;
using InstaMail.DataAccessLayer.Services;
using InstaMail.EntityLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace InstaMail.WebUI.Controllers;

public class InstaMailController : Controller
{
    private readonly InstaMailContext _db;
    private readonly EmailReceiver _emailReceiver;

    public InstaMailController(InstaMailContext db, EmailReceiver emailReceiver)
    {
        _db = db;
        _emailReceiver = emailReceiver;
    }

    public ActionResult Index(int? emailId)
    {
        if (emailId.HasValue)
        {
            var ınstaMail = _db.InstaMails
                .Include(e => e.EmailMessages)
                .FirstOrDefault(e => e.ID == emailId.Value);

            if (ınstaMail != null)
            {
                TempData["EmailId"] = emailId.Value;
                return View(ınstaMail);
            }
        }
        return View();
    }

    [HttpPost]
    public IActionResult CreateEmail()
    {
        var ınstaMail = new EntityLayer.Entity.InstaMail
        {
            EmailAddress = RandomEmailGenerator.Generate(),
            CreatedAt = DateTime.Now
        };

        _db.InstaMails.Add(ınstaMail);
        _db.SaveChanges();

        FetchEmailsForInstaMail(ınstaMail.ID);
        TempData["EmailId"] = ınstaMail.ID;
        
        return RedirectToAction("Index", new { emailId = ınstaMail.ID });
    }

    private void FetchEmailsForInstaMail(int emailId)
    {
        var ınstaMail = _db.InstaMails.Find(emailId);
        if (ınstaMail == null) return;

        _emailReceiver.FetchEmails(ınstaMail.EmailAddress, message =>
        {
            var emailMessage = new EmailMessage
            {
                InstaMailID = emailId,
                Sender = message.From.ToString(),
                Subject = message.Subject,
                Body = message.TextBody,
                Time = DateTime.Now
            };
            
            _db.EmailMessages.Add(emailMessage);
            _db.SaveChanges();
        });
    }
}




