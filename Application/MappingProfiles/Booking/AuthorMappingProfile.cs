using Application.DTO.Booking.AuthorDto;
using Application.UseCases.Booking.Commands.AuthorCommands.CreateAuthor;
using Application.UseCases.Booking.Commands.AuthorCommands.UpdateAuthor;
using Domain.Entities.Booking;

namespace Application.MappingProfiles.Booking;

public static class AuthorMapper
{
    public static Author CommandToEntity(CreateAuthorCommand command)
    {
        return new Author
        {
            AuthorId = Random.Shared.Next(1, int.MaxValue),
            LastName = command.AuthorDto.LastName,
            FirstName = command.AuthorDto.FirstName
        };
    }

    public static AuthorDto EntityToDto(Author author)
    {
        return new AuthorDto
        {
            FirstName = author.FirstName,
            LastName = author.LastName
        };
    }
    
    public static void CommandToEntityInUpdate(UpdateAuthorCommand command, ref Author author)
    {
        author.AuthorId = command.AuthorId;
        author.LastName = command.AuthorDto.LastName;
        author.FirstName = command.AuthorDto.FirstName;
    }

    public static IEnumerable<AuthorDto> EntitiesToDtos(IEnumerable<Author> authors)
    {
        return authors.Select(EntityToDto);
    }
}