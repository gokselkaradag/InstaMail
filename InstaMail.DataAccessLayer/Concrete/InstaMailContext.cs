using Microsoft.EntityFrameworkCore;
using InstaMail.EntityLayer.Entity;

namespace InstaMail.DataAccessLayer.Concrete;

public class InstaMailContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=89.252.180.91\\MSSQLSERVER2016;Initial Catalog=gokselka_instamail;User Id=gokselka_mail;Password=7h~4Y6t5n;TrustServerCertificate=True");
    }
    
    public DbSet<EmailMessage> EmailMessages { get; set; }
    public DbSet<EntityLayer.Entity.InstaMail> InstaMails { get; set; }
}