using System;
using System.Globalization;
using MyBox;
using UnityEngine;

public class SaveLastLoginDate : MonoBehaviour
{
    private void Awake()
    {
        ServerTime.instance.RequestDate();
        ServerTime.instance.DateUpdatedEvent += Write;
    }

    private void Write(DateTime dateTime)
    {
        if (GameProgress.GetData().firstLoginDate.IsNullOrEmpty())
        {
            GameProgress.GetData().firstLoginDate = dateTime.ToString(CultureInfo.InvariantCulture);
        }
        GameProgress.GetData().lastLoginDate = dateTime.ToString(CultureInfo.InvariantCulture);
        ServerTime.instance.DateUpdatedEvent -= Write;
        GameProgress.Save();
    }

}
