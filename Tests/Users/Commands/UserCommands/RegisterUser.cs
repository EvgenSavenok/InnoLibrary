using Application.DTO.Users.UserDto;
using Application.UseCases.Users.Commands.UserCommands.Register;
using Domain.Entities.User;
using Domain.ErrorHandlers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Users.Commands.UserCommands;

public class RegisterUser
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUser()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);

        _handler = new RegisterUserCommandHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_CreateUserWithCorrectUserDto_ReturnsIdentityResult()
    {
        // Arrange
        var dto = new UserForRegistrationDto
        {
            UserName = "newuser",
            Email = "newuser@mail.com",
            Password = "Pass123!",
            Role = UserForRegistrationDto.UserRole.User
        };

        _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
            .ReturnsAsync((User)null);

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User)null);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), dto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        var command = new RegisterUserCommand { UserForRegistrationDto = dto };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        _userManagerMock.Verify(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_CreateUserWithAlreadyExistingUsername_ThrowsValidationException()
    {
        // Arrange
        var dto = new UserForRegistrationDto
        {
            UserName = "existinguser",
            Email = "new@mail.com",
            Password = "Pass123!",
            Role = UserForRegistrationDto.UserRole.User
        };

        _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
            .ReturnsAsync(new User());

        var command = new RegisterUserCommand { UserForRegistrationDto = dto };

        // Act & Assert
        await Assert.ThrowsAsync<AlreadyExistsException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task Handle_CreateUserWithAlreadyExistingEmail_ThrowsAlreadyExistsException()
    {
        // Arrange
        var dto = new UserForRegistrationDto
        {
            UserName = "newuser",
            Email = "existing@mail.com",
            Password = "Pass123!",
            Role = UserForRegistrationDto.UserRole.User
        };

        _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
            .ReturnsAsync((User)null);

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(new User());

        var command = new RegisterUserCommand { UserForRegistrationDto = dto };

        // Act & Assert
        await Assert.ThrowsAsync<AlreadyExistsException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CreateUserWithIdentityError_ThrowsBadRequestException()
    {
        // Arrange
        var dto = new UserForRegistrationDto
        {
            UserName = "newuser",
            Email = "newuser@mail.com",
            Password = "Pass123!",
            Role = UserForRegistrationDto.UserRole.User
        };

        _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
            .ReturnsAsync((User)null);

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User)null);

        var failedResult = IdentityResult.Failed(new IdentityError { Description = "Some error" });
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), dto.Password))
            .ReturnsAsync(failedResult);

        var command = new RegisterUserCommand { UserForRegistrationDto = dto };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}