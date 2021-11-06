using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class ReservationSearch : MonoBehaviour
{
    public Text Name;
    public Text Email;
    public Text Password;
    public Text Connecting;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("總預約數 : " + ReserveData.Data.Count);
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void Confirm()
    {
        if(Name.text == "" || Email.text == "" || Password.text == "")
        {
            Connecting.text = "Table Incomplete";
        }
        else
        {
            Connecting.text = "Connecting...";
            Debug.Log("Confirming...");

            SearchReserve.unknownInfo.Name = Name.text;
            SearchReserve.unknownInfo.Email = Email.text;
            SearchReserve.unknownInfo.Password = Password.text;
            Debug.Log("姓名:" + SearchReserve.unknownInfo.Name + "\tEmail:" + SearchReserve.unknownInfo.Email + "\t密碼:" + SearchReserve.unknownInfo.Password);

            bool target = false;

            //Debug.Log(ReserveData.Data[0].Password);

            for (int i = 0; i < ReserveData.Data.Count; i++)
            {
                if (ReserveData.Data[i].Year == "")
                {
                    break;
                }

                if (string.Equals(SearchReserve.unknownInfo.Password, ReserveData.Data[i].Password))
                {
                    SearchReserve.index = i;
                    SearchReserve.sheetRow = i + 2;
                    SearchReserve.unknownInfo = ReserveData.Data[i];
                    target = true;

                    if(Name.text != SearchReserve.unknownInfo.Name || Email.text != SearchReserve.unknownInfo.Email)
                    {
                        Connecting.text = "name or email is wrong";
                        Connecting.fontSize = 70;
                        return;
                    }
                }
            }

            if(target == false)
            {
                Connecting.text = "Can't find";
            }
            else
            {
                Debug.Log(target);
                Debug.Log("目標 : " + SearchReserve.index);
                Debug.Log("目標 : " + SearchReserve.sheetRow);

                SceneManager.LoadScene(7);
            }
        }
    }

    public void Back()
    {
        Debug.Log("Back");
        SceneManager.LoadScene(0);
    }
}
