namespace TiVi.MovieBasketService.Models.Base
{
    public class BaseJsonResponseError
    {
        public string? message { get; set; } = null;
        public string? error_message { get; set; } = null;

        public BaseJsonResponseError(string? message, string? error_message)
        {
            this.message = message;
            this.error_message = error_message;
        }
    }
}
