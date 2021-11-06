using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Diagnostics;

using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

public class Main : MonoBehaviour
{
    public static bool access = false;
    public static string s = "";
    public GameObject canvas;
    public GameObject button;

    public Text[] buttonlist;

    public Text monthText;
    public string monthInput;
    public string yearInput;
    private string weekstr = DateTime.Now.DayOfWeek.ToString();

    //private int[] IDlist = new int[42];
    public int now_month = DateTime.Now.Month;
    public int now_year = DateTime.Now.Year;
    public Text output;
    private bool done = false;
    private GameObject button1;
    private string monthInfo = "";
    // Start is called before the first frame update


    public string GetMonthName(int month)
    {
        string result = "";
        switch(month)
        {
            case 1:
                result = "Janurary";
                break;
            case 2:
                result = "Feburary";
                break;
            case 3:
                result = "March";
                break;
            case 4:
                result = "April";
                break;
            case 5:
                result = "May";
                break;
            case 6:
                result = "June";
                break;
            case 7:
                result = "July";
                break;
            case 8:
                result = "August";
                break;
            case 9:
                result = "September";
                break;
            case 10:
                result = "October";
                break;
            case 11:
                result = "November";
                break;
            case 12:
                result = "December";
                break;
        }
        return result;
    }
 

    void Start()
    {
        monthText.text = GetMonthName(now_month) + ", " + now_year.ToString();
        if (now_month < 10)
            monthInput = "0" + now_month.ToString();
        else
            monthInput = now_month.ToString();
        yearInput = now_year.ToString();
        output.text = yearInput + "/" + monthInput + "/01";
    }

    /// <summary>
    /// This returns which day of the week the month is starting on
    /// </summary>
    int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        //DayOfWeek Sunday == 0, Saturday == 6 etc.
        return (int)temp.DayOfWeek;
    }
    /// <summary>
    /// Gets the number of days in the given month.
    /// </summary>
    int GetTotalNumberOfDays(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }

    // Update is called once per frame
    void Update()
    {
        int startDay = GetMonthStartDay(now_year, now_month);
        int endDay = GetTotalNumberOfDays(now_year, now_month);


            for(int i = 0; i < 6; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    int currDay = (i * 7) + j;
                    if(currDay < startDay || currDay - startDay >= endDay)
                    {
                        buttonlist[i * 7 + j].text = " ";
                        //buttonlist[i * 7 + j].color = Color.yellow;  // works well
                    }
                    else
                    {
                        buttonlist[i * 7 + j].text = (currDay - startDay + 1).ToString();
                    }
                }
            }
    }

    public void clickedNext()
    {    
        done = false;
        if(now_month == 12)
        {
            now_month = 1;
            now_year++;
            monthText.text = GetMonthName(now_month) + ", " + now_year.ToString();
        }
        else
        {
            now_month++;
            monthText.text = GetMonthName(now_month) + ", " + now_year.ToString();
        }

        if (now_month < 10)
            monthInput = "0" + now_month.ToString();
        else
            monthInput = now_month.ToString();
        yearInput = now_year.ToString();
        output.text = yearInput + "/" + monthInput + "/01";
        //string time = now_year + now_month + "01"; // 把字符串类型日期转换为日期类型
        //System.DateTime t = System.DateTime.ParseExact(time, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
    }
    public void clickedPrev()
    {
        done = false;
        if(now_month == 1)
        {
            now_month = 12;
            now_year--;
            monthText.text = GetMonthName(now_month) + ", " + now_year.ToString();
        }
        else
        {
            now_month--;
            monthText.text = GetMonthName(now_month) + ", " + now_year.ToString();
        }
        if (now_month < 10)
            monthInput = "0" + now_month.ToString();
        else
            monthInput = now_month.ToString();
        yearInput = now_year.ToString();
        output.text = yearInput + "/" + monthInput + "/01";
    }
}
