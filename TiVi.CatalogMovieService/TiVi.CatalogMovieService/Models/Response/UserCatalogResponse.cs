namespace TiVi.UserCatalogService.Models.Response
{
    public class UserCatalogResponse
    {
        public int MovieId { get; set; }

        public string MovieName { get; set; } = null!;

        public short ReleaseYear { get; set; }
    }
}
