using System.Net;

namespace ECommerceATK.ViewException
{
    [Serializable()]
    public class DataAlreadyExistException : Exception
    {
        public DataAlreadyExistException()
        {
            this.Data["Cause"] = "Data already exist";
            this.Data["Message"] = "Data already exist";
            this.Data["StatusCode"] = (int)HttpStatusCode.OK;
        }
    }
}