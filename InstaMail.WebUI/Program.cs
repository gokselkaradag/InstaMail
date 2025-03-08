using InstaMail.BusinessLayer.Mail;
using InstaMail.DataAccessLayer.Concrete;
using InstaMail.DataAccessLayer.Services;
using InstaMail.EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
    new SmtpEmailSender(
        builder.Configuration["EmailSender:Host"],
        builder.Configuration.GetValue<int>("EmailSender:Port"),
        builder.Configuration.GetValue<bool>("EmailSender:EnableSSL"),
        builder.Configuration["EmailSender:Username"],
        builder.Configuration["EmailSender:Password"])
);

builder.Services.AddDbContext<InstaMailContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<EmailReceiver>(); 

builder.Services.Configure<ImapSettings>(builder.Configuration.GetSection("ImapSettings"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<ImapSettings>>().Value);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=InstaMail}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();