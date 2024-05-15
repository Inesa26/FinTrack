namespace FinTrack.Application.Responses
{
    public class UserDto
    {
        public string Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }

        public override string ToString()
        {
            return $"Id: {Id}, First Name: {FirstName}, Last Name: {LastName}, Email: {Email}";
        }
    }
}
