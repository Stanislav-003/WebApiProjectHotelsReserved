namespace Hotel.Models
{
    public class Place
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal PriceOneNight { get; set; }
        public DateTime AvailableFrom { get; set; } = DateTime.Now;
        public DateTime AvailableTo { get; set; } = DateTime.Now.AddDays(3);
        
        // Навігаційні властивості
        public ICollection<Reservation> Reservations { get; set; }
    }
}
