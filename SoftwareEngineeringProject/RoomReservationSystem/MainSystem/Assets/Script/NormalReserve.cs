using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalReserve : MonoBehaviour
{
    private string Year;
    private string Month;
    private string Day;
    public Text dayString;
    private List<string> options = new List<string> { "RoomA", "RoomB", "RoomC", "RoomD" };
    public Text TimeInterval;
    public Dropdown rooms;
    public GameObject calendar;
    public Text Information;
    public GameObject Next;
    private string temp_interval;
    private string temp_dayString;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Search()
    {
        Debug.Log("Searching...");
        if (rooms.options.Count == 0)
        {
            Information.text = "No Available Rooms!";
            Information.color = Color.red;
        }
        else
        {
            if (Month[0] == '0')
            {
                int int_month = int.Parse(Month);
                Month = int_month.ToString();
            }
            if (Day[0] == '0')
            {
                int int_day = int.Parse(Day);
                Day = int_day.ToString();
            }

            SceneManager.LoadScene(4);
            NewReserve.newInfo.Year = Year;
            NewReserve.newInfo.Month = Month;
            NewReserve.newInfo.Day = Day;
            NewReserve.newInfo.Time = TimeInterval.text;
            NewReserve.newInfo.Room = options[rooms.value];
        }
    }

    public void CheckChange()
    {
        if(TimeInterval.text != temp_interval)
        {
            Next.SetActive(false);
        }
    }

    public void Back()
    {
        Debug.Log("Back");
        SceneManager.LoadScene(0);
    }
    
    public void openCalendar()
    {
        //Next.SetActive(false);
        Debug.Log("Open Calendar");
        if (calendar != null)
        {
            if(dayString.text != temp_dayString)
            {
                Next.SetActive(false);
            }
            bool isActive = calendar.activeSelf;
            calendar.SetActive(!isActive);
        }
    }

    public void SelectTime()
    {
        Next.SetActive(true);
        string[] temp = dayString.text.Split('/');
        Year = temp[0].ToString();
        Month = temp[1].ToString();
        Day = temp[2].ToString();

        if (Month[0] == '0')
        {
            int int_month = int.Parse(Month);
            Month = int_month.ToString();
        }
        if (Day[0] == '0')
        {
            int int_day = int.Parse(Day);
            Day = int_day.ToString();
        }

        Debug.Log(Year + "/" + Month + "/" + Day);
        options = new List<string> { "RoomA", "RoomB", "RoomC", "RoomD" };

        for (int i = 0; i < ReserveData.Data.Count; i++)
        {
            //same date and time, remove unavailable rooms
            if (ReserveData.Data[i].Year == Year && ReserveData.Data[i].Month == Month && ReserveData.Data[i].Day == Day && ReserveData.Data[i].Time == TimeInterval.text)
            {
                print("delete");
                if (options.Contains(ReserveData.Data[i].Room))
                {
                    options.Remove(ReserveData.Data[i].Room);
                }
            }
        }
        
        rooms.ClearOptions();
        rooms.AddOptions(options);

        temp_interval = TimeInterval.text;
        temp_dayString = dayString.text;
    }
}
