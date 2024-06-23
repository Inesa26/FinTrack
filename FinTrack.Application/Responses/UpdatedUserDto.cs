namespace FinTrack.Application.Responses
{
    public class UpdatedUserDto
    {
        public string Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Token { get; set; }
    }

}