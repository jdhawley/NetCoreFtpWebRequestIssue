using System;
using System.IO;
using System.Net;

namespace EnelDataRetrievalLibrary
{
    public class SppRtlmp
    {
        public static void TestRtlmpRequests()
        {
            DateTime operatingDay = DateTime.Today.AddDays(-1);
            for (DateTime intervalEnd = operatingDay.Date.AddMinutes(5); intervalEnd <= operatingDay.AddDays(1); intervalEnd = intervalEnd.AddMinutes(5))
            {
                string year = intervalEnd.AddMinutes(-5).ToString("yyyy");
                string month = intervalEnd.AddMinutes(-5).ToString("MM");
                string day = intervalEnd.AddMinutes(-5).ToString("dd");
                string filename = "RTBM-LMP-SL-" + intervalEnd.ToString("yyyyMMddHHmm") + ".csv";
                string url = $"ftp://pubftp.spp.org/Markets/RTBM/LMP_By_SETTLEMENT_LOC/{year}/{month}/By_Interval/{day}/{filename}";

                var watch = new System.Diagnostics.Stopwatch();
                try
                {
                    watch.Start();
                    string response = GetRtlmp(url);
                    watch.Stop();
                    Console.WriteLine($"SUCCESS ({watch.ElapsedMilliseconds}ms): {filename}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FAILED ({watch.ElapsedMilliseconds}ms): {filename}");
                    Console.WriteLine($"   {ex.ToString()}");
                }
            }
        }

        private static string GetRtlmp(string url)
        {
            FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(url);
            downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            var response = downloadRequest.GetResponse();
            var responseStream = response.GetResponseStream();

            using (StreamReader reader = new StreamReader(responseStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
