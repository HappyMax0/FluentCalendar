// HolidayProvider.cs
// 一个可直接在 .NET 项目中使用的节假日管理类（从 JSON 加载）
// 支持：加载/缓存 JSON、查询某日是否放假、是否调休上班、获取节日名称、导出/更新等。

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable enable

namespace CalendarWinUI3.Models.Utils
{
    public static class HolidayProvider
    {
        //https://raw.githubusercontent.com/NateScarlet/holiday-cn/master/{年份}.json
        private static string baseURL = "https://raw.githubusercontent.com/NateScarlet/holiday-cn/master/";

        public static List<HolidayData> HolidayDatas = new List<HolidayData>();

        /// <summary>
        /// 从 JSON 字符串加载/覆盖
        /// </summary>
        public static async Task<HolidayData?> GetHolidayData(int year)
        {
            var holidayData = HolidayDatas.FirstOrDefault(x => x.Year == year);
            if (holidayData != null) return holidayData;

            string json = string.Empty;
            string path = Path.Combine(AppContext.BaseDirectory, "Assets", $"{year}.json");
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);        
            }
            else
            {
                string url = baseURL + year.ToString() + ".json";

                using HttpClient client = new HttpClient();

                json = await client.GetStringAsync(url);

                //Save Json Data
                File.WriteAllText(path, json);
            }

            try
            {
                if (!string.IsNullOrEmpty(json))
                {
                    // JsonSerializer.Deserialize 可能返回 null，因此返回类型为 HolidayData?
                    HolidayData? data = JsonSerializer.Deserialize<HolidayData>(json);

                    if (data != null && !HolidayDatas.Any(x=>x.Year == year)) 
                    {
                        // 如果成功加载数据，添加到缓存列表中
                        HolidayDatas.Add(data);
                    }

                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }         
        }

        public static bool RemoveHolidayData(int year)
        {
            try
            {
                var holidayData = HolidayDatas.FirstOrDefault(x => x.Year == year);
                if (holidayData != null)
                {
                    HolidayDatas.Remove(holidayData);
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static bool RemoveHolidayDataFile(int year) 
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "Assets", $"{year}.json");
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
            }
            catch (Exception)
            {
            }      

            return false;
        }

        public static void RemoveAllHolidayDatas()
        {
            HolidayDatas.Clear();
        }

        public static void RemoveAllHolidayDataFiles()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets");
            if(Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, "*.json");
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception)
                    {
                        // Handle exceptions if necessary
                    }
                }
            }
        }

        public static async Task<HuangLiDto?> GetHuangli(string date)
        {
            string json = string.Empty;
            try
            {
                using HttpClient client = new();

                json = await client.GetStringAsync(
                    $"http://127.0.0.1:8000/huangli/{date}");
            }
            catch (Exception)
            {
                
            }

            if (string.IsNullOrEmpty(json))
                return null;

            HuangLiDto? data = JsonSerializer.Deserialize<HuangLiDto>(json);

            return data;
        }

        public static string GetHolidayName(DateTime date)
        {
            var holidayData = HolidayProvider.HolidayDatas.FirstOrDefault(x => x.Year == date.Year);
            if (holidayData != null)
            {
                var holiday = holidayData.Days.FirstOrDefault(it=>it.Date == date);
                if(holiday != null)
                    return holiday.Name;
            }
            return string.Empty;
        }
    }

}
