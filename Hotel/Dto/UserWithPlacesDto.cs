namespace Hotel.Dto
{
    public class UserWithPlacesDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<PlaceDto> Places { get; set; }
    }
}
