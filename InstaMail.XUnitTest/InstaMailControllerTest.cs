using InstaMail.DataAccessLayer.Concrete;
using InstaMail.DataAccessLayer.Services;
using InstaMail.EntityLayer.Entity;
using InstaMail.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace InstaMail.XUnitTest;

public class InstaMailControllerTest
{
    private readonly Mock<InstaMailContext> _mockDb;
    private readonly Mock<DbSet<EntityLayer.Entity.InstaMail>> _mockInstaMailSet;
    private readonly InstaMailController _controller;

    public InstaMailControllerTest()
    {
        // DbSet<InstaMail>'i mock'la
        _mockInstaMailSet = new Mock<DbSet<EntityLayer.Entity.InstaMail>>();
        var data = new List<EntityLayer.Entity.InstaMail>
        {
            new EntityLayer.Entity.InstaMail { ID = 1, EmailAddress = "test@example.com" }
        }.AsQueryable();

        // IQueryable arayüzünü mock'la
        _mockInstaMailSet.As<IQueryable<EntityLayer.Entity.InstaMail>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockInstaMailSet.As<IQueryable<EntityLayer.Entity.InstaMail>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockInstaMailSet.As<IQueryable<EntityLayer.Entity.InstaMail>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockInstaMailSet.As<IQueryable<EntityLayer.Entity.InstaMail>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        // InstaMailContext'i mock'la ve DbSet'i bağla
        _mockDb = new Mock<InstaMailContext>();
        _mockDb.Setup(db => db.InstaMails).Returns(_mockInstaMailSet.Object);

        // Controller'ı başlat
        _controller = new InstaMailController(_mockDb.Object, null); // EmailReceiver burada kullanılmıyorsa null olabilir
    }

    [Fact]
    public void Index_WithValidEmailId_ReturnsViewWithInstaMail()
    {
        // Arrange
        int emailId = 1;
        var instaMail = new EntityLayer.Entity.InstaMail { ID = emailId, EmailAddress = "test@example.com" };
        _mockInstaMailSet.Setup(m => m.Include(It.IsAny<string>())).Returns(_mockInstaMailSet.Object);
        _mockInstaMailSet.Setup(m => m.FirstOrDefault(It.IsAny<Func<EntityLayer.Entity.InstaMail, bool>>())).Returns(instaMail);

        // Act
        var result = _controller.Index(emailId) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(instaMail, result.Model);
        Assert.Equal(emailId, _controller.TempData["EmailId"]);
    }

    [Fact]
    public void Index_WithoutEmailId_ReturnsEmptyView()
    {
        //Act
        var result = _controller.Index(null) as ViewResult;
        
        //Assert
        Assert.NotNull(result);
        Assert.Null(result.Model);
    }

    [Fact]
    public void Index_WithInValıdEmailId_ReturnsEmptyView()
    {
        //Arrange
        int emailId = 999;
        _mockDb.Setup(db => db.InstaMails.Include(e => e.EmailMessages)
                .FirstOrDefault(e => e.ID == emailId))
            .Returns((EntityLayer.Entity.InstaMail)null);
        
        //Act
        var result = _controller.Index(emailId) as ViewResult;
        
        //Assert
        Assert.NotNull(result);
        Assert.Null(result.Model);
                
    }
}