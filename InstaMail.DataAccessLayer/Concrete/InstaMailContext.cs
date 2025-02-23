using Microsoft.EntityFrameworkCore;
using InstaMail.EntityLayer.Entity;

namespace InstaMail.DataAccessLayer.Concrete;

public class InstaMailContext : DbContext
{
    public InstaMailContext(DbContextOptions<InstaMailContext> options) : base(options)
    {
    }

    public DbSet<EmailMessage> EmailMessages { get; set; }
    public DbSet<EntityLayer.Entity.InstaMail> InstaMails { get; set; }
}