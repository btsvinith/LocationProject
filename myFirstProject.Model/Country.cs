namespace myFirstProject.Model
{
    public class Country
    {
        public int Id { get; set; }
        public string? CountryName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
