using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReserveNow : MonoBehaviour
{
    public Text Year;
    public Text Month;
    public Text Day;
    public Text TimeInterval;
    public DateTime Time;
    public Dropdown rooms;
    public Text Information;
    private List<string> options = new List<string> { "RoomA", "RoomB", "RoomC", "RoomD"};
    // Start is called before the first frame update
    void Start()
    {
        /*Debug.Log(Time.Year.ToString());
        Debug.Log(Time.Month.ToString());
        Debug.Log(Time.Day.ToString());*/
        Time = DateTime.Now;
        Year.text = Time.Year.ToString();
        Month.text = Time.Month.ToString();
        Day.text = Time.Day.ToString();
        string temp;
        switch (Time.Hour)
        {
            case 9:
                temp = "09:00-10:00";
                break;
            case 10:
                temp = "10:00-11:00";
                break;
            case 11:
                temp = "11:00-12:00";
                break;
            case 12:
                temp = "12:00-13:00";
                break;
            case 13:
                temp = "13:00-14:00";
                break;
            case 14:
                temp = "14:00-15:00";
                break;
            case 15:
                temp = "15:00-16:00";
                break;
            case 16:
                temp = "16:00-17:00";
                break;
            case 17:
                temp = "17:00-18:00";
                break;
            default:
                temp = "09:00-10:00";
                if(Time.Day == 31)
                {
                    print("changemonth");
                    Day.text = "1";
                    Month.text = (Time.Month + 1).ToString();
                }
                else
                {
                    Day.text = (Time.Day + 1).ToString();
                }
                break;
        }
        TimeInterval.text = temp;
        for(int i = 0; i < ReserveData.Data.Count; i++)
        {
            //same date and time, remove unavailable rooms
            if(ReserveData.Data[i].Year == Year.text && ReserveData.Data[i].Month == Month.text && ReserveData.Data[i].Day == Day.text && ReserveData.Data[i].Time == TimeInterval.text)
            {
                if (options.Contains(ReserveData.Data[i].Room))
                {
                    options.Remove(ReserveData.Data[i].Room);
                }
            }
        }
        //rooms = GetComponent<Dropdown>();
        rooms.ClearOptions();
        rooms.AddOptions(options);
        NewReserve.newInfo.Room = options[rooms.value];
        Information.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Time = DateTime.Now;
        Year.text = Time.Year.ToString();
        Month.text = Time.Month.ToString();
        Day.text = Time.Day.ToString();
        string temp;
        switch (Time.Hour)
        {
            case 9:
                temp = "09:00-10:00";
                break;
            case 10:
                temp = "10:00-11:00";
                break;
            case 11:
                temp = "11:00-12:00";
                break;
            case 12:
                temp = "12:00-13:00";
                break;
            case 13:
                temp = "13:00-14:00";
                break;
            case 14:
                temp = "14:00-15:00";
                break;
            case 15:
                temp = "15:00-16:00";
                break;
            case 16:
                temp = "16:00-17:00";
                break;
            case 17:
                temp = "17:00-18:00";
                break;
            default:
                temp = "09:00-10:00";
                if (Time.Day == 31)
                {
                    print("changemonth");
                    Day.text = "1";
                    Month.text = (Time.Month + 1).ToString();
                }
                else
                {
                    Day.text = (Time.Day + 1).ToString();
                }
                break;
        }
        TimeInterval.text = temp;
    }

    public void Search()
    {
        Debug.Log("Searching...");
        if(rooms.options.Count == 0)
        {
            Information.text = "No Available Rooms!";
            Information.color = Color.red;
        }
        else
        {
            SceneManager.LoadScene(4);
            NewReserve.newInfo.Year = Year.text;
            NewReserve.newInfo.Month = Month.text;
            NewReserve.newInfo.Day = Day.text;
            NewReserve.newInfo.Time = TimeInterval.text;
        }

    }

    public void Back()
    {
        Debug.Log("Back");
        SceneManager.LoadScene(0);
    }

    public void roomChoosed()
    {
        NewReserve.newInfo.Room = options[rooms.value];
    }
}
