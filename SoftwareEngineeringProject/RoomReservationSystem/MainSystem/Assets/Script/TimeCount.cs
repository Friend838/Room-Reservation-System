using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    private DateTime NowTime;
    public Text now;
    // Start is called before the first frame update
    void Start()
    {
        NowTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        NowTime = DateTime.Now;
        string temp;
        string hour = NowTime.Hour + "";
        if (NowTime.Hour < 10)
        {
            hour = "0" + hour;
        }
        string minute = NowTime.Minute + "";
        if (NowTime.Minute < 10)
        {
            minute = "0" + minute;
        }
        string second = NowTime.Second + "";
        if (NowTime.Second < 10)
        {
            second = "0" + second;
        }
        temp = NowTime.Year + "/" + NowTime.Month + "/" + NowTime.Day + " " + hour + ":" + minute + ":" + second;
        now.text = temp;
    }
}
