using Application.Contracts.Users;
using Application.DTO.Users.TokenDto;
using Application.DTO.Users.UserDto;
using Application.UseCases.Users.Commands.UserCommands.Authenticate;
using Domain.Entities.User;
using Domain.ErrorHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Users.Commands.UserCommands;

public class AuthenticateUser
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IAuthManagerService> _authManagerMock;
    private readonly AuthenticateUserCommandHandler _handler;

    public AuthenticateUser()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);

        _authManagerMock = new Mock<IAuthManagerService>();

        _handler = new AuthenticateUserCommandHandler(_userManagerMock.Object, _authManagerMock.Object);
    }

    [Fact]
    public async Task Handle_AuthenticateUserWithCorrectData_ReturnsAccessToken()
    {
        // Arrange
        var dto = new UserForAuthenticationDto
        {
            UserName = "testuser",
            Password = "Pass123!"
        };

        var user = new User { UserName = dto.UserName };
        var tokenDto = new TokenDto("access123", "refresh123");

        var context = new DefaultHttpContext();

        _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _authManagerMock.Setup(x => x.ValidateUser(dto))
            .ReturnsAsync(true);

        _authManagerMock.Setup(x => x.CreateTokens(user, true))
            .ReturnsAsync(tokenDto);

        var command = new AuthenticateUserCommand
        {
            UserForAuthenticationDto = dto,
            HttpContext = context
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        var setCookieHeader = context.Response.Headers["Set-Cookie"].ToString();

        // Assert
        Assert.Equal("access123", result);
        Assert.Contains("refresh123", setCookieHeader);
    }

    [Fact]
    public async Task Handle_AuthenticateUserWhenHeIsNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var dto = new UserForAuthenticationDto
        {
            UserName = "notfound",
            Password = "Pass123!"
        };

        _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
            .ReturnsAsync((User)null);

        var command = new AuthenticateUserCommand
        {
            UserForAuthenticationDto = dto,
            HttpContext = new DefaultHttpContext()
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_AuthenticateUserWithInvalidPassword_ThrowsUnauthorizedException()
    {
        // Arrange
        var dto = new UserForAuthenticationDto
        {
            UserName = "testuser",
            Password = "wrongPass"
        };

        var user = new User { UserName = dto.UserName };

        _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(false);

        var command = new AuthenticateUserCommand
        {
            UserForAuthenticationDto = dto,
            HttpContext = new DefaultHttpContext()
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_AuthenticateUserWithInvalidTokens_ThrowsUnauthorizedException()
    {
        // Arrange
        var dto = new UserForAuthenticationDto
        {
            UserName = "testuser",
            Password = "Pass123!"
        };

        var user = new User { UserName = dto.UserName };
        var invalidTokenDto = new TokenDto(null, null);

        _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _authManagerMock.Setup(x => x.ValidateUser(dto))
            .ReturnsAsync(true);

        _authManagerMock.Setup(x => x.CreateTokens(user, true))
            .ReturnsAsync(invalidTokenDto);

        var command = new AuthenticateUserCommand
        {
            UserForAuthenticationDto = dto,
            HttpContext = new DefaultHttpContext()
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}