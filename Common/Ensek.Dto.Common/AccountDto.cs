namespace Ensek.Dto.Common;

public record AccountDto
{
    public required int AccountId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
}