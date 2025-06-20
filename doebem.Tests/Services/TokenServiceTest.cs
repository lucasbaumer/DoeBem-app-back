using BackendProjeto.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace doeBem.Tests.Services
{
    public class TokenServiceTest
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public TokenServiceTest()
        {
            var userStore = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);

            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c[It.IsAny<string>()]).Returns((string key) =>
            {
                return key switch
                {
                    "Jwt:Key" => "MinhaChaveSecretaSuperSegura1234!",
                    "Jwt:Issuer" => "MeuIssuer",
                    "Jwt:Audience" => "MeuAudience",
                    _ => null
                };
            });
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnValidJwtToken()
        {
            var user = new IdentityUser { Id = "user123", UserName = "usuario" };
            _mockUserManager.Setup(u => u.GetRolesAsync(user))
                            .ReturnsAsync(new List<string> { "Admin", "User" });

            var tokenService = new TokenService(_mockUserManager.Object, _mockConfiguration.Object);

            var tokenString = await tokenService.GenerateToken(user);

            Assert.False(string.IsNullOrEmpty(tokenString));

            var handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(tokenString));

            var token = handler.ReadJwtToken(tokenString);

            // Verifica claims básicas
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id);
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            Assert.Contains(token.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");

            Assert.Equal("MeuIssuer", token.Issuer);
            Assert.Contains("MeuAudience", token.Audiences);

            var expiration = token.ValidTo;
            Assert.True(expiration > DateTime.UtcNow.AddMinutes(29));
            Assert.True(expiration <= DateTime.UtcNow.AddMinutes(31));
        }
    }
}
