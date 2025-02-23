using System.ComponentModel.DataAnnotations;

namespace InstaMail.EntityLayer.Entity;

public class InstaMail
{
    [Key]
    public int ID { get; set; }
    public string EmailAddress  { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<EmailMessage> EmailMessages { get; set; } = new List<EmailMessage>();
}