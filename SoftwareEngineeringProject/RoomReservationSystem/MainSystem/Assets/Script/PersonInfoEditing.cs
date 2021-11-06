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

public class PersonInfoEditing : MonoBehaviour
{
    public GameObject HostName;
    public GameObject HostEmail;
    public GameObject Title;
    public GameObject Details;
    public Text ConnectInfo;

    public InputField membername;
    public InputField memberemail;

    private string List_membername = "";
    private string List_memberemail = "";
    
    private List<string> MemberNames_Array = new List<string>();
    private List<string> MemberEmails_Array = new List<string>();
    private List<string> options = new List<string>();

    public Dropdown members;

    // Start is called before the first frame update
    void Start()
    {
        string[] MemberNames = SearchReserve.unknownInfo.MemberNames.Split('/');
        string[] MemberEmails = SearchReserve.unknownInfo.MemberEmails.Split('/');
        foreach (string i in MemberNames)
        {
            MemberNames_Array.Add(i);
        }

        foreach(string i in MemberEmails)
        {
            MemberEmails_Array.Add(i);
        }

        HostName.GetComponent<InputField>().placeholder.GetComponent<Text>().text = SearchReserve.unknownInfo.Name;
        HostName.GetComponent<InputField>().text = SearchReserve.unknownInfo.Name;

        HostEmail.GetComponent<InputField>().placeholder.GetComponent<Text>().text = SearchReserve.unknownInfo.Email;
        HostEmail.GetComponent<InputField>().text = SearchReserve.unknownInfo.Email;

        Title.GetComponent<InputField>().placeholder.GetComponent<Text>().text = SearchReserve.unknownInfo.Title;
        Title.GetComponent<InputField>().text = SearchReserve.unknownInfo.Title;

        Details.GetComponent<InputField>().placeholder.GetComponent<Text>().text = SearchReserve.unknownInfo.Details;
        Details.GetComponent<InputField>().text = SearchReserve.unknownInfo.Details;

        if(MemberNames[0] != "")
        {
            for (int i = 0; i < MemberNames.Length; i++)
            {
                options.Add(MemberNames[i] + ", " + MemberEmails[i]);
            }
        }
        //Debug.Log(options.Count);
        members.ClearOptions();
        members.AddOptions(options);
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
            MemberNames_Array.Add(membername.text);
            MemberEmails_Array.Add(memberemail.text);

            string newmember = "";
            newmember = membername.text + ", " + memberemail.text;
            options.Add(newmember);             //更新存放(名字, 電郵)的list
            members.ClearOptions();             //更新下拉視窗
            members.AddOptions(options);     

            membername.text = "";
            memberemail.text = "";
        }
    }

    public void DeleteMember()
    {
        if(members.options.Count == 0)
        {
            ConnectInfo.text = "no more member";
        }
        else
        {
            int target = members.value;
            options.RemoveAt(target);
            MemberNames_Array.RemoveAt(target);
            MemberEmails_Array.RemoveAt(target);
            members.ClearOptions();
            members.AddOptions(options);
        }
    }

    public void Confirm()
    {
        if (HostName.GetComponent<InputField>().text == "" || HostEmail.GetComponent<InputField>().text == "" || Title.GetComponent<InputField>().text == "")
        {
            ConnectInfo.text = "table incomplete";
            Debug.Log("User hasn't completed.");
        }
        else if (IsValidEmail(HostEmail.GetComponent<InputField>().text) == false)
        {
            ConnectInfo.text = "invalid email";
            ConnectInfo.fontSize = 50;
            ConnectInfo.color = Color.red;
        }
        else
        {
            ConnectInfo.text = "Connecting...";
            Debug.Log("Confirming...");

            for(int i = 0; i < MemberNames_Array.Count; i++)
            {
                if(List_membername == "")
                {
                    List_membername = MemberNames_Array[i];
                    List_memberemail = MemberEmails_Array[i];
                }
                else
                {
                    List_membername = List_membername + "/" + MemberNames_Array[i];
                    List_memberemail = List_memberemail + "/" + MemberEmails_Array[i];
                }
            }

            SearchReserve.unknownInfo.Name = HostName.GetComponent<InputField>().text;
            SearchReserve.unknownInfo.Email = HostEmail.GetComponent<InputField>().text;
            SearchReserve.unknownInfo.Title = Title.GetComponent<InputField>().text;
            SearchReserve.unknownInfo.Details = Details.GetComponent<InputField>().text;
            SearchReserve.unknownInfo.MemberNames = List_membername;
            SearchReserve.unknownInfo.MemberEmails = List_memberemail;
            Debug.Log("姓名:" + SearchReserve.unknownInfo.Name + "\tEmail:" + SearchReserve.unknownInfo.Email);
            Debug.Log("與會人員:" + SearchReserve.unknownInfo.MemberNames + "\t與會人員EMAIL:" + SearchReserve.unknownInfo.MemberEmails);

            //ReserveData.Data[SearchReserve.index] = SearchReserve.unknownInfo;
            SendEmail();
            if(List_membername != "")
            {
                for (int i = 0; i < MemberNames_Array.Count; i++)
                {
                    SendEmail_CLIENT(MemberNames_Array[i], MemberEmails_Array[i]);
                }
            }
            StartCoroutine(Upload());
        }
    }

    private void SendEmail()
    {
        MailMessage mailMessage = new MailMessage();
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        //Attachment attachment = new Attachment(@"Assets/A.png");    //指定要夾帶的物件路徑

        mailMessage.From = new MailAddress("roomreservationsystem2020@gmail.com", "RoomReservationSystem", System.Text.Encoding.UTF8);
        mailMessage.To.Add(SearchReserve.unknownInfo.Email);

        mailMessage.Subject = "Editing Success";
        mailMessage.Body = "Hello ";
        mailMessage.Body += SearchReserve.unknownInfo.Name;
        mailMessage.Body += ",\nYou have edited the reservation successfully.\n("
            + SearchReserve.unknownInfo.Year + "/" + SearchReserve.unknownInfo.Month + "/" + SearchReserve.unknownInfo.Day + "\tTime : " 
            + SearchReserve.unknownInfo.Time +"\tRoom : " + SearchReserve.unknownInfo.Room + ")\n" 
            + "This is Password if you want to check the information again :\t" + SearchReserve.unknownInfo.Password
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

        Debug.Log("寄信完成！！");
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
        mailMessage.Body += ",\nThe host of your reservation (" + SearchReserve.unknownInfo.Name + ") has edited the reservation." + "\nhere are some informations about the reservation:\n"
                            + "Date:\t" + SearchReserve.unknownInfo.Year + "/" + SearchReserve.unknownInfo.Month + "/" + SearchReserve.unknownInfo.Day + "\t" + SearchReserve.unknownInfo.Time + "\n"
                            + "Room:\t" + SearchReserve.unknownInfo.Room + "\n";
        mailMessage.Body += "Please make sure you have received this message.\n\nSincerely,\nRoom Reservation System\n";
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

        Debug.Log("寄信完成！！");
    }

    public void Back()
    {
        Debug.Log("Back");
        SceneManager.LoadScene(7);
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("method", "edit");
        form.AddField("index", SearchReserve.sheetRow);
        form.AddField("name", SearchReserve.unknownInfo.Name);
        form.AddField("year", SearchReserve.unknownInfo.Year);
        form.AddField("month", SearchReserve.unknownInfo.Month);
        form.AddField("day", SearchReserve.unknownInfo.Day);
        form.AddField("time", SearchReserve.unknownInfo.Time);
        form.AddField("email", SearchReserve.unknownInfo.Email);
        form.AddField("room", SearchReserve.unknownInfo.Room);
        form.AddField("password", SearchReserve.unknownInfo.Password);
        form.AddField("version", SearchReserve.unknownInfo.Version);
        form.AddField("title", SearchReserve.unknownInfo.Title);
        form.AddField("details", SearchReserve.unknownInfo.Details);
        form.AddField("membernames", SearchReserve.unknownInfo.MemberNames);
        form.AddField("memberemails", SearchReserve.unknownInfo.MemberEmails);

        UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxs4-qsoqzxAxR-Uu5txNDyejCAFs98jvx6gZ33IL4SCILY3LV_FGg/exec", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            SceneManager.LoadScene(9);
            ConnectInfo.text = "";
        }
    }
}
