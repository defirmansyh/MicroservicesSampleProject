namespace TiVi.MovieBasketService.Models.Response
{
    public class MovieBasketResponse
    {
        public int MovieId { get; set; }

        public string MovieName { get; set; } = null!;

        public short ReleaseYear { get; set; }
    }
}
