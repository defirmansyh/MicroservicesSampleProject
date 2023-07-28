using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace TiVi.AuthService.Utilities
{
    public class ClaimIdentity
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        public ClaimIdentity(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        private static ClaimIdentity instance = new ClaimIdentity(new HttpContextAccessor());
        public static ClaimIdentity Instance
        {
            get { return instance; }
        }


        public string Claim(string param)
        {

            if (_httpContextAccessor.HttpContext.User.Claims.Count() > 0)
            {
                var value = _httpContextAccessor.HttpContext.User.Claims.Where(a => a.Type.ToLower() == param.ToLower()).Select(a => String.IsNullOrEmpty(a.Value) ? "" : a.Value).FirstOrDefault().ToString();
                return value;
            }
            else
            {
                var token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
                if (!String.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.ReadToken(token);
                    var jsonToken = JsonConvert.SerializeObject(securityToken);
                    var previewObject = JsonConvert.DeserializeObject<CustomTokenModel>(jsonToken);
                    var value = previewObject.claims.Where(a => a.type == param).Select(a => String.IsNullOrEmpty(a.value) ? "" : a.value).FirstOrDefault().ToString();
                    return value;
                }
                else
                {
                    return "";
                }
            }

        }
        public HttpContext GetHttpRequest
        {
            get
            {
                return _httpContextAccessor.HttpContext;
            }
        }
        public static HttpContext ClaimHttpRequest
        {
            get
            {
                return Instance.GetHttpRequest;
            }
        }
        public static int CurrentUserId
        {
            get
            {
                var CheckIdentity = String.IsNullOrEmpty(Instance.Claim("CurrentUserID")) ? 0 : Convert.ToInt32(Instance.Claim("CurrentUserID"));

                return CheckIdentity;

            }
        }

        public static string CurrentUserName
        {
            get
            {
                var CheckIdentity = Instance.Claim("CurrentUserName");

                return CheckIdentity;

            }
        }

        public static bool IsAdmin
        {
            get
            {
                var CheckIdentity = String.IsNullOrEmpty(Instance.Claim("IsAdmin")) ? false : Convert.ToBoolean(Instance.Claim("IsAdmin"));

                return CheckIdentity;

            }

        }

        public static string CurrentEmail
        {
            get
            {
                var CheckIdentity = Instance.Claim("CurrentEmail");

                return CheckIdentity;

            }
        }

        public static string Password
        {
            get
            {
                var CheckIdentity = Instance.Claim("Password");

                return CheckIdentity;

            }
        }
    }
}
