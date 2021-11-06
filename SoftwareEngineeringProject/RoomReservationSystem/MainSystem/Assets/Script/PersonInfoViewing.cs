using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

using System.Diagnostics;
using System.ComponentModel;

public class PersonInfoViewing : MonoBehaviour
{
    public Text HostName;
    public Text HostEmail;
    public Text Title;
    public Text Details;
    public Text MemberList;
    public Text ConnectInfo;
    public GameObject ConfirmationWindow;

    string[] MemberNames = { };
    string[] MemberEmails = { };

    public void Start()
    {
        HostName.GetComponent<Text>().text = SearchReserve.unknownInfo.Name;
        HostEmail.GetComponent<Text>().text = SearchReserve.unknownInfo.Email;
        Title.GetComponent<Text>().text = SearchReserve.unknownInfo.Title;
        Details.GetComponent<Text>().text = SearchReserve.unknownInfo.Details;
        ConfirmationWindow.SetActive(false);

        string temp = "";

        /*for (int i = 0; i < MemberNames.Length; i++)
        {
            Debug.Log(MemberNames[i]);
        }*/

        if (SearchReserve.unknownInfo.MemberNames != "" || SearchReserve.unknownInfo.MemberEmails != "")
        {
            MemberNames = SearchReserve.unknownInfo.MemberNames.Split('/');
            MemberEmails = SearchReserve.unknownInfo.MemberEmails.Split('/');
            for (int i = 0; i < MemberNames.Length; i++)
            {
                temp = temp + MemberNames[i] + ", " + MemberEmails[i] + "\n";
            }
        }

        MemberList.GetComponent<Text>().text = temp;
    }

    public void Edit()
    {
        UnityEngine.Debug.Log("Edit");
        SceneManager.LoadScene(8);
    }

    public void Back()
    {
        UnityEngine.Debug.Log("Back");
        SceneManager.LoadScene(6);
    }

    public void Delete()
    {
        UnityEngine.Debug.Log("Delete");
        ConfirmationWindow.SetActive(true);
    }

    public void CancelDelete()
    {
        UnityEngine.Debug.Log("Cancel");
        ConfirmationWindow.SetActive(false);
    }

    public void ConfirmDelete()
    {
        ConnectInfo.text = "Connecting...";
        UnityEngine.Debug.Log("Confirm");

        //connect to Google Calendar
        //Process p = new Process();
        //p.StartInfo.FileName = "D:/RoomReservationSystem/Calendar_test/Calendar_test/bin/Debug/netcoreapp3.1/Calendar_test.exe";
        //p.StartInfo.Arguments = "delete" + " " + NewReserve.newInfo.Title;
        //p.Start();

        SendEmail();
        UnityEngine.Debug.Log(MemberNames.Length);
        if (MemberNames.Length == 0)
        {
            UnityEngine.Debug.Log("Empty Member");
        }
        else
        {
            for (int i = 0; i < MemberNames.Length; i++)
            {
                SendEmail_CLIENT(MemberNames[i], MemberEmails[i]);
            }
        }
        StartCoroutine(Erase());
        //connect to Google Calendar
        //p.Kill();
    }

    private void SendEmail()
    {
        MailMessage mailMessage = new MailMessage();
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        //Attachment attachment = new Attachment(@"Assets/A.png");    //指定要夾帶的物件路徑

        mailMessage.From = new MailAddress("roomreservationsystem2020@gmail.com", "RoomReservationSystem", System.Text.Encoding.UTF8);
        mailMessage.To.Add(SearchReserve.unknownInfo.Email);

        mailMessage.Subject = "Deleting Success";
        mailMessage.Body = "Hello ";
        mailMessage.Body += SearchReserve.unknownInfo.Name;
        mailMessage.Body += ",\nYou have deleted the reservation successfully\n("
            + SearchReserve.unknownInfo.Year + "/" + SearchReserve.unknownInfo.Month + "/" + SearchReserve.unknownInfo.Day + "\tTime : "
            + SearchReserve.unknownInfo.Time + "\tRoom : " + SearchReserve.unknownInfo.Room + ")\n" 
            + "You can reserve again at any time !\t" 
            + "\n\nSincerely,\nRoom Reservation System\n";
        mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
        mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
        //mailMessage.Attachments.Add(attachment);
        mailMessage.Priority = MailPriority.High;

        smtpClient.Port = 587;
        smtpClient.Credentials = new System.Net.NetworkCredential("roomreservationsystem2020@gmail.com", "friend838") as ICredentialsByHost;
        smtpClient.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback = delegate (object sender,
                                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        smtpClient.Send(mailMessage);

        UnityEngine.Debug.Log("寄信完成！！");
    }

    private void SendEmail_CLIENT(string name, string email)
    {
        MailMessage mailMessage = new MailMessage();
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

        //Attachment attachment = new Attachment(@"Assets/A.png");    //指定要夾帶的物件路徑

        mailMessage.From = new MailAddress("roomreservationsystem2020@gmail.com", "RoomReservationSystem", System.Text.Encoding.UTF8);
        mailMessage.To.Add(email);

        mailMessage.Subject = "Reservation Delete";
        mailMessage.Body = "\nHello ";
        mailMessage.Body += name;
        mailMessage.Body += ",\nThe host of your reservation ("+ SearchReserve.unknownInfo.Name + ") has deleted the reservation." + "\nHere are some informations about the reservation:\n"
                            + "Date:\t" + SearchReserve.unknownInfo.Year + "/" + SearchReserve.unknownInfo.Month + "/" + SearchReserve.unknownInfo.Day + "\t" + SearchReserve.unknownInfo.Time + "\n"
                            + "Room:\t" + SearchReserve.unknownInfo.Room + "\n";
        mailMessage.Body += "Please make sure you have received this message, and we hope you reserve again at anytime.\n\nSincerely,\nRoom Reservation System\n";
        mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
        mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
        //mailMessage.Attachments.Add(attachment);
        mailMessage.Priority = MailPriority.High;

        smtpClient.Port = 587;
        smtpClient.Credentials = new System.Net.NetworkCredential("roomreservationsystem2020@gmail.com", "friend838") as ICredentialsByHost;
        smtpClient.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback = delegate (object sender,
                                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        smtpClient.Send(mailMessage);

        UnityEngine.Debug.Log("寄信完成！！");
    }

    IEnumerator Erase()
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "delete");
        form.AddField("index", SearchReserve.sheetRow);

        UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxs4-qsoqzxAxR-Uu5txNDyejCAFs98jvx6gZ33IL4SCILY3LV_FGg/exec", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            UnityEngine.Debug.Log(www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Form upload complete!");
            SceneManager.LoadScene(10);
            ConnectInfo.text = "";
        }
    }
} 
