using InstaMail.DataAccessLayer.Concrete;
using InstaMail.DataAccessLayer.Services;
using InstaMail.EntityLayer.Entity;
using InstaMail.WebUI.Controllers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InstaMail.XUnitTest;

public class InstaMailControllerTest
{
    private readonly Mock<InstaMailContext> _mockContext;
    private readonly Mock<EmailReceiver> _mockEmailReceiver;
    private readonly InstaMailController _controller;

    public InstaMailControllerTest()
    {
        _mockContext = new Mock<InstaMailContext>();
        _mockEmailReceiver = new Mock<EmailReceiver>();
        _controller = new InstaMailController(_mockContext.Object, _mockEmailReceiver.Object);
    }

    [Fact]
    public void Index_WithhEmailId_ReturnsViewWithInstaMail()
    {
        //Arrange
        
        int emailId = 1; 
        var instaMail = new EntityLayer.Entity.InstaMail
        {
            ID = emailId,
            EmailAddress = "test@test.com",
            EmailMessages = new List<EmailMessage>()
        };
        
        var mockDbSet = new Mock<DbSet<EntityLayer.Entity.InstaMail>>();
        //mockDbSet.As<IQueryable<EntityLayer.Entity.InstaMail>>().Setup(m => m.Provider).Returns(instaMail.AsQueryable().Provider);
        
    }
}