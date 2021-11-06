using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
    public Text connecting;
    // Start is called before the first frame update
    void Start()
    {
        connecting.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reservation()
    {
        Debug.Log("Connecting to google...");
        connecting.text = "Connecting...";
        StartCoroutine(ReadData(3));
    }

    public void normalReserve()
    {
        Debug.Log("Connecting to google...");
        connecting.text = "Connecting...";
        StartCoroutine(ReadData(2));
    }

    public void searchTime()
    {
        Debug.Log("Searching Time...");
        connecting.text = "Connecting...";
        StartCoroutine(ReadData(6));
    }

    public void quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    IEnumerator Wait(float i, int scene)
    {
        yield return new WaitForSeconds(i);
        SceneManager.LoadScene(scene);
        Debug.Log("Connected");
        connecting.text = "";
    }

    IEnumerator ReadData(int k)
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "readall");

        UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxs4-qsoqzxAxR-Uu5txNDyejCAFs98jvx6gZ33IL4SCILY3LV_FGg/exec", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            ReserveData.Data.Clear();
            print(www.downloadHandler.text);
            string[] temp = www.downloadHandler.text.Split(',');
            for (int i = 0; i < temp.Length / ReserveData.column; i++)
            {
                Information nowData;
                nowData.Year = temp[ReserveData.column * i];
                nowData.Month = temp[ReserveData.column * i + 1];
                nowData.Day = temp[ReserveData.column * i + 2];
                nowData.Time = temp[ReserveData.column * i + 3];
                nowData.Name = temp[ReserveData.column * i + 4];
                nowData.Email = temp[ReserveData.column * i + 5];
                nowData.Room = temp[ReserveData.column * i + 6];
                nowData.Password = temp[ReserveData.column * i + 7];
                nowData.Version = temp[ReserveData.column * i + 8];
                nowData.Title = temp[ReserveData.column * i + 9];
                nowData.Details = temp[ReserveData.column * i + 10];
                nowData.MemberNames = temp[ReserveData.column * i + 11];
                nowData.MemberEmails = temp[ReserveData.column * i + 12];
                ReserveData.Data.Add(nowData);
            }
            Debug.Log("Form upload complete!");
            SceneManager.LoadScene(k);
            Debug.Log("Connected");
            connecting.text = "";
        }

    }
}
