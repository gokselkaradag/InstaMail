using System.ComponentModel.DataAnnotations;

namespace InstaMail.EntityLayer.Entity;

public class EmailMessage
{
    [Key]
    public int ID { get; set; }
    public string Sender { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime Time { get; set; }
    public virtual InstaMail InstaMail { get; set; }
    public int InstaMailID { get; set; }
}