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

    public ActionResult Index()
    {
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

        return RedirectToAction("Inbox", new { emailId = ınstaMail.ID });
    }

    public IActionResult Inbox(int emailId)
    {
        var ınstaMail = _db.InstaMails
            .Include(e => e.EmailMessages)
            .FirstOrDefault(e => e.ID == emailId);
        
        return View(ınstaMail);
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