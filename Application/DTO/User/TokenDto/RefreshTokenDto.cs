namespace Application.DTO.User.TokenDto;

public record RefreshTokenDto
{
    public string AccessToken { get; set; }
}