using System;
using System.Globalization;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class ServerTime : MonoBehaviour
{
    public Action<DateTime> DateUpdatedEvent;
    [DllImport("__Internal")]
    private static extern void GetServerTimeInternal();


    private string _serverDate;

    public static ServerTime instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void RequestDate()
    {
        try
        {
            GetServerTimeInternal();
        }
        catch
        {
            DateUpdatedEvent?.Invoke(ConvertToDateTime(DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'")));
        }
        
    }

    private DateTime ConvertToDateTime()
    { 
        //string dateString = "Sat, 19 Aug 2023 22:02:45 GMT";
        DateTimeOffset dateTime = DateTimeOffset.ParseExact(_serverDate, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        DateTime convertedDateTime = dateTime.DateTime;
        return convertedDateTime;
    }
    
    public DateTime ConvertToDateTime(string dateTimeString)
    { 
        //string dateString = "Sat, 19 Aug 2023 22:02:45 GMT";
        DateTimeOffset dateTime = DateTimeOffset.ParseExact(dateTimeString, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
        DateTime convertedDateTime = dateTime.DateTime;
        return convertedDateTime;
    }

    public string ConvertToString(DateTime dateTime)
    {
        return  dateTime.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
    }

    //Call after server responded 
    public void UpdateServerTime(string date)
    {
        _serverDate = date;
        DateUpdatedEvent?.Invoke(ConvertToDateTime());
    }
}
