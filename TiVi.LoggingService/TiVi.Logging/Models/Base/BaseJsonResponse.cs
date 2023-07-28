using Newtonsoft.Json;

namespace TiVi.Logging.Models.Base
{
    public class BaseJsonResponse<T>
    {
        public bool is_success { get; set; }
        public IList<BaseJsonResponseError> errors { get; set; } = new List<BaseJsonResponseError>();
        public T? data { set; get; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
