using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.Login;
using TN.Authentication.Application.ServiceInterface;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.IRepository;
using Xunit;

namespace ES.Infrastructure.Test.Authentication
{
    public class LogInServiceTest
    {

        [Fact]
        public async Task LogIn_ValidCredentials_ReturnsToken()
        {

            //Arrange
            var mockRepo = new Mock<IUserServices>();
            var mockTokenServices = new Mock<ITokenService>();



            var loginCommand = new LoginCommand("Sumaney", "password123");
            var expectedResponse = new LoginResponse("mock-jwt-token", "1234");

            mockRepo.Setup(s => s.Login(loginCommand))
                .ReturnsAsync(Result<LoginResponse>.Success(expectedResponse));

            // Act
            var result = await mockRepo.Object.Login(loginCommand);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.token.Should().Be("mock-jwt-token");
            result.Message.Should().BeNull();


        }

        [Fact]
        public async Task LogIn_InvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var mockUserService = new Mock<IUserServices>();
            var loginCommand = new LoginCommand("Sumaney", "wrongpassword");

            mockUserService.Setup(s => s.Login(loginCommand))
                .ReturnsAsync(Result<LoginResponse>.Failure("Invalid Credentials"));

            // Act
            var result = await mockUserService.Object.Login(loginCommand);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("Invalid Credentials");
            result.Data.Should().BeNull();
        }

        [Fact]
        public void AddNumbers_ShouldReturnSum()
        {
            var result = 2 + 3;
            Assert.Equal(5, result);
        }
    }
}
