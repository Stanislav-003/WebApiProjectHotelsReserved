namespace Hotel.Models
{
    public class Reservation
    {
        // Зовнішні ключі
        public Guid UserId { get; set; }
        public Guid PlaceId { get; set; } 

        // Навігаційні властивості
        public User User { get; set; }
        public Place Place { get; set; }
    }
}
