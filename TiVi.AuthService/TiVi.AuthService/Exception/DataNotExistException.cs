using System.Net;

namespace ECommerceATK.ViewException
{
    [Serializable()]
    public class DataNotExistException : Exception
    {
        public DataNotExistException()
        {
            this.Data["Cause"] = "Data Not Found";
            this.Data["Message"] = "Data Not Found";
            this.Data["StatusCode"] = (int)HttpStatusCode.OK;
        }

        public DataNotExistException(string obj)
        {
            obj = string.IsNullOrEmpty(obj)? string.Empty : obj + " ";
            this.Data["Cause"] = $"Data {obj}Not Found";
            this.Data["Message"] = $"Data {obj}Not Found";
            this.Data["StatusCode"] = (int)HttpStatusCode.NotFound;
        }
    }
}