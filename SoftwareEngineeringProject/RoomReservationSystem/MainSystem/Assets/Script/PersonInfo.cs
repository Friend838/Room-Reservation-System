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
// struct Member
// {
//     public string Name;
//     public string Email;
// };


public class PersonInfo : MonoBehaviour
{
    public Text Name;
    public Text Email;
    public Text ConnectInfo;
    public Text Title1;
    public Text details;
    public InputField membername;
    public InputField memberemail;
    public Text memberlist;
    private string password;
    private string List_membername = "";
    private string List_memberemail = "";
    private List<string> memberemail_array = new List<string> { };
    private List<string> membername_array = new List<string> { };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public void AddMember()
    {
        if (membername.text == "" || memberemail.text == "")
        {
            ConnectInfo.text = "Please complete member";
            ConnectInfo.fontSize = 50;
            ConnectInfo.color = Color.red;
            return;
        }
        else
        {
            string newmember = "";
            newmember = membername.text + ", " + memberemail.text;
            memberemail_array.Insert(0, memberemail.text);
            membername_array.Insert(0, membername.text);
            if (memberlist.text == "")
            {
                memberlist.text = newmember;
                List_membername = membername.text;
                List_memberemail = memberemail.text;
            }
            else
            {
                memberlist.text = memberlist.text + ";" + newmember;
                List_membername = List_membername + "/" + membername.text;
                List_memberemail = List_memberemail + "/" + memberemail.text;
            }
            membername.text = "";
            memberemail.text = "";
        }
        UnityEngine.Debug.Log(Title1.text);
    }

    public void Confirm()
    {
        if (Name.text == "" || Email.text == "" || Title1.text == "")
        {
            ConnectInfo.text = "Type host name, email\nand title at least";
            ConnectInfo.fontSize = 50;
            ConnectInfo.color = Color.red;
            return;
        }
        if (IsValidEmail(Email.text) == false)
        {
            ConnectInfo.text = "Type a valid email";
            ConnectInfo.fontSize = 50;
            ConnectInfo.color = Color.red;
            return;
        }
        ConnectInfo.text = "Connecting...";
        UnityEngine.Debug.Log("Confirming...");

        NewReserve.newInfo.Name = Name.text;
        NewReserve.newInfo.Email = Email.text;
        password = GeneratePassword();
        NewReserve.newInfo.Password = password;
        NewReserve.newInfo.Version = ReserveData.Version;
        NewReserve.newInfo.Title = Title1.text;
        NewReserve.newInfo.Details = details.text;
        NewReserve.newInfo.MemberNames = List_membername;
        NewReserve.newInfo.MemberEmails = List_memberemail;
        UnityEngine.Debug.Log(password);
        UnityEngine.Debug.Log(NewReserve.newInfo.Year + "/" + NewReserve.newInfo.Month + "/" + NewReserve.newInfo.Day + "  " + NewReserve.newInfo.Time);
        UnityEngine.Debug.Log("姓名:" + NewReserve.newInfo.Name + "\tEmail:" + NewReserve.newInfo.Email + "\t房間:" + NewReserve.newInfo.Room + "\t密碼:" + NewReserve.newInfo.Password);

        //connect to Google Calendar
        //Process p = new Process();
        //p.StartInfo.FileName = "D:/RoomReservationSystem/Calendar_test/Calendar_test/bin/Debug/netcoreapp3.1/Calendar_test.exe";
        //p.StartInfo.Arguments = "insert" + " " + NewReserve.newInfo.Year + " " + NewReserve.newInfo.Month + " " + NewReserve.newInfo.Day + " " + NewReserve.newInfo.Time + " " + NewReserve.newInfo.Title + " " + NewReserve.newInfo.Details;
        //p.Start();
       
        SendEmail_HOST(Email.text);
        for (int i = 0; i < memberemail_array.Count; i++)
        {
            SendEmail_CLIENT(membername_array[i], memberemail_array[i]);
        }
        StartCoroutine(Upload());
        //connect to Google Calendar
        //p.Kill();
    }

    private void SendEmail_HOST(string email)
    {
        MailMessage mailMessage = new MailMessage();
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

        //Attachment attachment = new Attachment(@"Assets/A.png");    //指定要夾帶的物件路徑

        mailMessage.From = new MailAddress("roomreservationsystem2020@gmail.com", "RoomReservationSystem", System.Text.Encoding.UTF8);
        mailMessage.To.Add(email);

        mailMessage.Subject = "Reservation Success";
        mailMessage.Body = "\nHello ";
        mailMessage.Body += NewReserve.newInfo.Name;
        mailMessage.Body += ",\nYou have reserved an discussion room successfully,\nhere are some informations for your reservation:\n";
        mailMessage.Body = mailMessage.Body + "Date:\t" + NewReserve.newInfo.Year + "/" + NewReserve.newInfo.Month + "/" + NewReserve.newInfo.Day + "\t" + NewReserve.newInfo.Time + "\n"
                            + "Room:\t" + NewReserve.newInfo.Room + "\n" + "Password:\t" + NewReserve.newInfo.Password + "\n"
                            + "Please keep your password carefully. Password is used for Cancellation and Inquiry.\n\nSincerely,\nRoom Reservation System\n";
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

        mailMessage.Subject = "Reservation Success";
        mailMessage.Body = "\nHello ";
        mailMessage.Body += name;
        mailMessage.Body += ",\nYou have been invited to a activity held by " + NewReserve.newInfo.Name + ",\nhere are some informations for the reservation:\n"
                            + "Date:\t" + NewReserve.newInfo.Year + "/" + NewReserve.newInfo.Month + "/" + NewReserve.newInfo.Day + "\t" + NewReserve.newInfo.Time + "\n"
                            + "Room:\t" + NewReserve.newInfo.Room + "\n";
        mailMessage.Body += "Title:\t" + NewReserve.newInfo.Title + "\nDescription:\t" + NewReserve.newInfo.Details;
        mailMessage.Body += "\nPlease make sure you have received this message.\n\nSincerely,\nRoom Reservation System\n";
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

    private string GeneratePassword()
    {
        Random Rnd = new Random();
        string result = "";
        for(int i = 0; i < 6; i++)
        {
            result += Convert.ToChar(Rnd.Next(65, 90));
        }
        return result;
    }

    public void Back()
    {
        UnityEngine.Debug.Log("Back");
        SceneManager.LoadScene(0);
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "write");
        form.AddField("name", NewReserve.newInfo.Name);
        form.AddField("year", NewReserve.newInfo.Year);
        form.AddField("month", NewReserve.newInfo.Month);
        form.AddField("day", NewReserve.newInfo.Day);
        form.AddField("time", NewReserve.newInfo.Time);
        form.AddField("email", NewReserve.newInfo.Email);
        form.AddField("room", NewReserve.newInfo.Room);
        form.AddField("password", NewReserve.newInfo.Password);
        form.AddField("version", NewReserve.newInfo.Version);
        form.AddField("title", NewReserve.newInfo.Title);
        form.AddField("details", NewReserve.newInfo.Details);
        form.AddField("membernames", NewReserve.newInfo.MemberNames);
        form.AddField("memberemails", NewReserve.newInfo.MemberEmails);

        UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxs4-qsoqzxAxR-Uu5txNDyejCAFs98jvx6gZ33IL4SCILY3LV_FGg/exec", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            UnityEngine.Debug.Log(www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Form upload complete!");
            SceneManager.LoadScene(5);
            ConnectInfo.text = "";
        }
    }
}
