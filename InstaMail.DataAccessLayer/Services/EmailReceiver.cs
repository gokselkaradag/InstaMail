using InstaMail.EntityLayer.Entity;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace InstaMail.DataAccessLayer.Services;

public class EmailReceiver
{
    private readonly ImapSettings _imapSettings;

    public EmailReceiver(ImapSettings imapSettings)
    {
        _imapSettings = imapSettings;
    }

    public List<MimeMessage> FetchEmails(string emailAddress, Action<MimeMessage> processEmail)
    {
        List<MimeMessage> emails = new List<MimeMessage>();

        using (var client = new ImapClient())
        {
            client.Connect(_imapSettings.Host, _imapSettings.Port, _imapSettings.UseSsl);
            client.Authenticate(_imapSettings.Username, _imapSettings.Password);
            
            var inbox = client.Inbox;
            inbox.Open(MailKit.FolderAccess.ReadOnly);
            
            var uids = inbox.Search(SearchQuery.ToContains(emailAddress));
            foreach (var uid in uids)
            {
                var message = inbox.GetMessage(uid);
                emails.Add(message);
            }
            client.Disconnect(true);
        }
        return emails;
    }
}