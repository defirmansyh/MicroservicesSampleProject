namespace TiVi.MovieBasketService.Utilities
{
    public class DataHelper
    {
        public static DateTime GetLocalTimes()
        {
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            return currentTime;

        }

        public static DateTime GetLocalDates()
        {
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            return currentTime.Date;

        }

        public static string GetDateString(DateTime date)
        {
            if (date == null)
                return "";
            return date.ToString("dd MMM yyyy");
        }
        public static string GetDateString(DateTime? date)
        {
            if (date == null)
                return "";
            DateTime realDate = (DateTime)date;
            return realDate.ToString("dd MMM yyyy");
        }
        public static string GetDatetimeString(DateTime date)
        {
            if (date == null)
                return "";
            return date.ToString("dd MMM yyyy HH:mm:ss");
        }
        public static string GetDatetimeString(DateTime? date)
        {
            if (date == null)
                return "";
            var realDate = (DateTime)date;
            return realDate.ToString("dd MMM yyyy HH:mm:ss");
        }

        public static string GenerateSerialNumber(int memberRequestId)
        {
            return GetLocalDates().ToString($"IB-yyyyMMdd-{memberRequestId}");
        }

        #region Generate Verification Code Random String
        public static string GenerateVerificationCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
        #endregion

        public static string GetAppsettingValue(string key)
        {
            //Calling Example
            //var x = DataHelper.GetAppsettingValue("ConnectionStrings:LocalConnection"); 
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            return configuration.GetSection(key).Value;

        }
    }
}
