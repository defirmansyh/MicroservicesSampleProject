namespace TiVi.UserCatalogService.Models.Base
{
    public class BasePagingResponse<T>
    {
        public List<T> data { get; set; }

        public int total_record { get; set; }
    }
}
