namespace SocialMediaApi.Models.Dtos;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;

    public User ToModel()
    {
        return new User
        {
            Username = Username,
        };
    }
}
