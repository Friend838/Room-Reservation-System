using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class contactGoogle : MonoBehaviour
{
    public int Row;
    void Start()
    {
        //StartCoroutine(Upload());
    }
    public void ReadData()
    {
        StartCoroutine(Upload());
    }
    IEnumerator Upload()
    {

        WWWForm form = new WWWForm();
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        form.AddField("method", "readall");
        //form.AddField("name", "蘇逸康");
        //form.AddField("date", "11/1");
        //form.AddField("time", "10:00-11:00");
        //form.AddField("email", "friend838@gmail.com");
        //form.AddField("row", Row);
        //form.AddField("column", 2);

        UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxs4-qsoqzxAxR-Uu5txNDyejCAFs98jvx6gZ33IL4SCILY3LV_FGg/exec", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            print(www.downloadHandler.text);
            string[] temp = www.downloadHandler.text.Split(',');
            for(int i = 0; i < temp.Length / ReserveData.column; i++)
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
            for(int i = 0; i < ReserveData.Data.Count; i++)
            {
                Debug.Log(ReserveData.Data[i].Year + "/" + ReserveData.Data[i].Month + "/" + ReserveData.Data[i].Day + "  " + ReserveData.Data[i].Time);
                Debug.Log("姓名:" + ReserveData.Data[i].Name + "\tEmail:" + ReserveData.Data[i].Email + "\t房間:" + ReserveData.Data[i].Room + "\t密碼:" + NewReserve.newInfo.Password);
            }
            Debug.Log("Form upload complete!");
        }
        
    }
}
