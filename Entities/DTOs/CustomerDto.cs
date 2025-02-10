namespace CustomerRecognitionService.Entities.DTOs
{
    public record CustomerDto(int Id, string FirstName, string LastName, string Email, string PhoneNumber, string Address);
}
