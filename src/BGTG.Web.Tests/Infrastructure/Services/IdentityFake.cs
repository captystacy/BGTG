using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Moq;

namespace BGTG.Web.Tests.Infrastructure.Services;

public class IdentityFake
{
    public static void Setup(Mock<IHttpContextAccessor> httpContextAccessorMock, string name)
    {
        var httpContextMock = new Mock<HttpContext>();
        var identityMock = new Mock<IIdentity>();
        httpContextMock.SetupGet(x => x.User.Identity).Returns(identityMock.Object);
        identityMock.Setup(x => x.Name).Returns(name);
        identityMock.Setup(x => x.IsAuthenticated).Returns(true);
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
    }
}