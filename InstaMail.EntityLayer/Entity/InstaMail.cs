using System.ComponentModel.DataAnnotations;

namespace InstaMail.EntityLayer.Entity;

public class InstaMail
{
    [Key]
    public int ID { get; set; }
    public string EmailAddress  { get; set; }
    public DateTime DateTime { get; set; }
}