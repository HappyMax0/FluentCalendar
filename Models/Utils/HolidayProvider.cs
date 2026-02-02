// HolidayProvider.cs
// 一个可直接在 .NET 项目中使用的节假日管理类（从 JSON 加载）
// 支持：加载/缓存 JSON、查询某日是否放假、是否调休上班、获取节日名称、导出/更新等。

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace CalendarWinUI3.Models.Utils
{
    public class HolidayInfo
    {
        [JsonPropertyName("holiday")]
        public List<string> Holidays { get; set; }

        [JsonPropertyName("workdays")]
        public List<string> Workdays { get; set; }
    }

    public class HolidayProvider : IDisposable
    {
        private readonly ReaderWriterLockSlim _lock = new();
        private Dictionary<string, Dictionary<string, HolidayInfo>> _data = new();

        /// <summary>
        /// 从 JSON 文件加载（文件路径）
        /// JSON 推荐结构：{ "2025": [ { "name": "元旦", "holiday": ["2025-01-01"], "workdays": [] } ] }
        /// 或者使用与你之前生成的那个年份-对象结构。此加载会覆盖内存中已有数据。
        /// </summary>
        public void LoadFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            var text = File.ReadAllText(filePath);
            LoadFromJson(text);
        }

        /// <summary>
        /// 从 JSON 字符串加载/覆盖
        /// </summary>
        public void LoadFromJson(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            // 我们先尝试把 JSON 解析成 Dictionary<string, List<HolidayEntry>>，如果失败则尝试兼容其他形式
            //try
            //{
            //    var maybe = JsonSerializer.Deserialize<Dictionary<string, List<HolidayEntry>>>(json, options);
            //    if (maybe != null)
            //    {
            //        var parsed = new Dictionary<int, List<HolidayEntry>>();
            //        foreach (var kv in maybe)
            //        {
            //            if (int.TryParse(kv.Key, out var y)) parsed[y] = kv.Value ?? new List<HolidayEntry>();
            //        }
            //        _lock.EnterWriteLock();
            //        try { _data = parsed; }
            //        finally { _lock.ExitWriteLock(); }
            //        return;
            //    }
            //}
            //catch
            //{
            //    // ignore and try another shape below
            //}

            // 兼容另一种顶层结构（HolidayDb），用于更灵活的 JSON 结构
            try
            {
                var db = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, HolidayInfo>>>(json);
                if (db != null)
                {
                    _data = db;
                    return;
                }
            }
            catch
            {
                // 若仍失败，抛异常
                throw new InvalidDataException("无法解析传入的节假日 JSON。请检查结构是否为 { \"2025\": [ ... ] } 或兼容格式。");
            }
        }

        /// <summary>
        /// 获取某天是否为法定放假日（true = 放假），并返回节日名（若有）
        /// 优先级：holiday -> workdays(上班) 会覆盖放假
        /// </summary>
        public (bool isHoliday, string holidayName) IsHoliday(DateTime date)
        {
            var d = date.Date;
            var y = d.Year;
            _lock.EnterReadLock();
            try
            {
                // check year and also adjacent years (部分节日可能跨年)
                var yearsToCheck = new[] { (y - 1).ToString(), y.ToString(), (y + 1).ToString() };
                // Dictionary<string, Dictionary<string, HolidayInfo>>
                foreach (var year in _data)
                {
                    //2025
                    if (!yearsToCheck.Contains(year.Key)) continue;

                    //Dictionary<string, HolidayInfo>
                    foreach (var holidayInfo in year.Value)
                    {
                        foreach (var holiday in holidayInfo.Value.Holidays)
                        {
                            if (DateTime.TryParse(holiday, out var hd) && hd.Date == d)
                                return (true, holidayInfo.Key);
                        }
                    }
                    // 注意：如果同一天既在 holiday 中又在 workdays 中，应以 workdays（上班）为准。
                }
            }
            finally { _lock.ExitReadLock(); }

           

            return (false, null);
        }

        /// <summary>
        /// 判断某天是否是调休日（即上班日，true = 需要上班）
        /// </summary>
        public (bool isWorkdayOverride, string holidayName) IsWorkdayOverride(DateTime date)
        {
            var d = date.Date;
            var y = d.Year;
            // 再检查是否在 workdays（上班）中 —— 如果在上班列表，则返回 false
            _lock.EnterReadLock();
            try
            {
                var yearsToCheck = new[] { (y - 1).ToString(), y.ToString(), (y + 1).ToString() };
                foreach (var year in _data)
                {
                    if (!yearsToCheck.Contains(year.Key)) continue;
                    foreach (var holidayInfo in year.Value)
                    {
                        foreach (var workday in holidayInfo.Value.Workdays)
                        {
                            if (DateTime.TryParse(workday, out var hd) && hd.Date == d)
                                return (true, holidayInfo.Key);
                        }

                    }
                    // 注意：如果同一天既在 holiday 中又在 workdays 中，应以 workdays（上班）为准。
                }
            }
            finally { _lock.ExitReadLock(); }
            return (false, null);
        }

        public ObservableCollection<Holiday> GetHolidays()
        {
            _lock.EnterReadLock();

            try
            {
                var list = new ObservableCollection<Holiday>();
                foreach (var year in _data)
                {
                    foreach (var holidayInfo in year.Value)
                    {
                        list.Add(new Holiday
                        {
                            Name = holidayInfo.Key,
                            Holidays = holidayInfo.Value.Holidays ?? new List<string>(),
                            Workdays = holidayInfo.Value.Workdays ?? new List<string>()
                        });
                    }
                }
                return list;
            }
            finally { _lock.ExitReadLock(); }
            
        }

        public void Dispose()
        {
            _lock?.Dispose();
        }
    }

    // 使用示例（注释）：
    // var provider = new HolidayProvider();
    // provider.LoadFromJson(File.ReadAllText("holidays.json"));
    // var (isHoliday, name) = provider.IsHoliday(new DateTime(2025,1,1));
    // var isOverrideWorkday = provider.IsWorkdayOverride(new DateTime(2025,1,26));
    // Console.WriteLine(isHoliday ? $"放假: {name}" : (isOverrideWorkday ? "调休日（上班）" : "工作日"));
}
