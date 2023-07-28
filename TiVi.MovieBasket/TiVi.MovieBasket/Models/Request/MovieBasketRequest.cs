namespace TiVi.MovieBasketService.Models.Request
{
    public class MovieBasketRequest
    {
        public int MovieId { get; set; }

        public string MovieName { get; set; } = null!;

        public short ReleaseYear { get; set; }
    }
}
