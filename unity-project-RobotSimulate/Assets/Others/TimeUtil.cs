using System;
using System.Globalization;
using UnityEngine;

public static class TimeUtil
{
    public static readonly int MIN_TO_SECOND = 60;
    public static readonly int HOUR_TO_MIN = 60;
    public static readonly int HOUR_TO_SECOND = 3600; //60 * 60;
    public static readonly int DAY_TO_SECOND = 86400; //24 * 60 * 60;
    
    public static readonly DateTime START_TIME = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
    public static readonly DateTime UTC_START_TIME = new DateTime(1970, 1, 1);
    
    //UnbiasedTime.tick unit : 100 nanosecond
    public static int TICK_TO_SECOND = 10000000;
    
    //from AD1_1_1 to 1970_1_1 timeStamp 
    public static long STAMP_1970_1_1 = 62135596800;
    
    public static int CHEAT_TIME_OFFSET = 0;
    

    /// <summary>
    /// 返回时间戳
    /// </summary>
    public static int GetCurrentTimeStamp(bool isGetServerTime = false)
    {
        if (isGetServerTime)
        {
            int? value = GetServerTime();
            if (value.HasValue)
            {
                return value.Value;
            }
            else
            {
                long timeStamp = DateTime.Now.ToUniversalTime().Ticks / TICK_TO_SECOND - STAMP_1970_1_1;
                return (int) timeStamp + CHEAT_TIME_OFFSET;
            }
        }
        else
        {
            long timeStamp = DateTime.Now.ToUniversalTime().Ticks / TICK_TO_SECOND - STAMP_1970_1_1;
            return (int) timeStamp + CHEAT_TIME_OFFSET;
        }
    }
    
    /// <summary>
    /// 获取固定日期的时间戳
    /// </summary>
    public static int GetTimeStamp(DateTime dateTime)
    {
        long timeStamp = dateTime.Ticks / TICK_TO_SECOND - STAMP_1970_1_1;
        return (int) timeStamp + CHEAT_TIME_OFFSET;
    }
    
    public static DateTime ConvertTimeSpanToDateTime(int unixTimeStamp)
    {
        return UTC_START_TIME.AddSeconds(unixTimeStamp);
    }

    public static DateTime GetCurrentTime()
    {
        return DateTime.UtcNow.AddSeconds(CHEAT_TIME_OFFSET);
    }

    #region NetworkTime

    private static int? _serverTimestamp;
    private static float _startRealTime;
    
    
    public static int? GetServerTime()
    {
        if (_serverTimestamp.HasValue)
            return _serverTimestamp.Value + (int)(Time.realtimeSinceStartup - _startRealTime);
        else
            return null;
    }

    #endregion

    public static string GetCurrentTimeString()
    {
        var year = $"{DateTime.Now.Year}";
        var month = DateTime.Now.Month < 10 ? $"0{DateTime.Now.Month}" : $"{DateTime.Now.Month}";
        var day = DateTime.Now.Day < 10 ? $"0{DateTime.Now.Day}" : $"{DateTime.Now.Day}";
        return $"{year}.{month}.{day}";
    }
}
