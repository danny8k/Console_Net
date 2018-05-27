using System;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace Console_NET
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloadUrl = "https://video-weaver.mia02.hls.ttvnw.net/v1/playlist/CrcETUEtW0yhbWwxbQ3DZ_p6J9cee1iupo6JY_0thUXcsYZJFui3EmfJYhJi237GTUdrz-5zrkYWUnzVJmNPyElITt6nUD2a8RMa6FDqvs432j20BiX3nrFKnkyKqHSgTkDNVq59xUECyEROrBcVv7u4278l3cksAwYk64FQP7fp2ZRsAbyG7ZtvJvrxecnWy-wjg7vY9nE7SPs_LJV_GUxHcYnN8emyPt1KIiBjX7oeu9oiaKLFZHjiApmaDF73l73sGEu1hUV_VIn3QCw6Z-Ws5Wm52EjysEFxp--3JDGfL0ZRtDCrADLCpN-Pg9ljOxaALkUNrUl2RSCypabaDuzsDEvKAa21gTZlRg2RR7Byy4l90lnNWYzf5izJal7hBKb2-OVQqwwqCAUXBBSkvtodNaZN0VsEpuDn-aq4uwiLbp1rI-LhWhD6Jdn1_bncCf8cF6BigKZDPJD0T2DwSSrN1OpoQZpO_c-EMI0H2owlX7Roya5sxehibRw96KRWiByWRqB0SeFWw9mTRZu5aS_eccO2Vc6gvxUt-bMyr5QE8qbJ-IMS5ATXmXSO6mMZq1ggTEFVZclnnSIsS3Yop13zu9MAGb-eY12JpQx6nKV1aLTSsVBibEbyPxEXT6UnCyqOhvWJ3nW64iIaTIt8Hs9Ow9qKHDKa9IhSxmXr57CRdzkOxoZDmOPxsN08A0XiwGcXNXlGvWtnWHflV5TnKoqUC6GBShTM-pfdGXgU4cvjWG2wUa0n8ZdcEhBkRg_tDcJb8JlV1OVIME0vGgzsmkxFo142vr4b18M.m3u8";

            var tsFiles = LoadPlaylist(new Uri(downloadUrl));

            foreach (var ts in tsFiles)
            {
                Console.WriteLine(ts);
            }
        }

        static List<Uri> LoadPlaylist(Uri source)
        {
            var tsList = new List<Uri>();

            using (var client = new WebClient())
            {

                string playlistContent = client.DownloadString(source);
                string[] playlistLines = playlistContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in playlistLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line[0] == '#') continue;

                    Uri file;
                    if (!Uri.TryCreate(source, line, out file))
                    {
                        Console.WriteLine("Invalid line: '{0}'", line);
                        continue;
                    }

                    string extension = Path.GetExtension(file.LocalPath);
                    if (extension.StartsWith(".ts", StringComparison.OrdinalIgnoreCase))
                    {
                        tsList.Add(file);
                    }

                }

            }

            return tsList;
        }
    }
}
