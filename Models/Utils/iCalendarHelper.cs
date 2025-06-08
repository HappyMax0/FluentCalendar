using Ical.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Exception = System.Exception;
using Uri = System.Uri;

namespace CalendarWinUI3.Models.Utils
{
    public static class iCalendarHelper
    {
        //public string icsUrl = @"https://www.shuyz.com/githubfiles/china-holiday-calender/master/holidayCal.ics";

        public static List<Calendar> Calendars = new List<Calendar>();

        public static async Task<List<Calendar>> GetICS()
        {
            Calendars.Clear();
            List<StorageFile> files = await GetLocalFolderFilesAsync();
            foreach (StorageFile file in files)
            {
                if (File.Exists(file.Path))
                {
                    string icalText = File.ReadAllText(file.Path);

                    var calendar = Calendar.Load(icalText);

                    Calendars.Add(calendar);
                   
                }
            }

            return Calendars;
        }

        public static void AddCalendar(string fileName)
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);

            string icalText = File.ReadAllText(path);

            var calendar = Calendar.Load(icalText);

            Calendars.Add(calendar);
        }

        public static async Task<List<StorageFile>> GetLocalFolderFilesAsync()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> files = await localFolder.GetFilesAsync();
            return new List<StorageFile>(files);
        }

        public static async Task<String> DownloadAndSaveFileAsync(string icsUrl)
        {
            try
            {
                Uri uri = new Uri(icsUrl, UriKind.Absolute);
                using HttpClientHandler handler = new HttpClientHandler();
                using HttpClient client = new HttpClient(handler)
                {
                    Timeout = System.TimeSpan.FromSeconds(30) // 设置 30 秒超时
                };
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] 开始请求: {icsUrl}");

                using var cts = new CancellationTokenSource(System.TimeSpan.FromSeconds(10));
                string content = await client.GetStringAsync(uri).ConfigureAwait(false);

                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] 下载完成，内容长度: {content.Length} 字符");

                string fileName = icsUrl.Split("/").LastOrDefault();
                // 获取应用的数据文件夹
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                // 写入文件
                await FileIO.WriteTextAsync(file, content);

                Console.WriteLine($"文件已保存到: {file.Path}");

                return fileName;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"网络请求失败: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"内部异常: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("请求超时或被取消");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"下载失败: {ex.Message}");
            }
            
            return string.Empty;
        }
    }
}
