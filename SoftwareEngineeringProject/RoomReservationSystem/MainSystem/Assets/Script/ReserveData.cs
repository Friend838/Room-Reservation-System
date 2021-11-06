using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

struct Information
{
    public string Year;
    public string Month;
    public string Day;
    public string Time;
    public string Name;
    public string Email;
    public string Room;
    public string Password;
    public string Version;
    public string Title;
    public string Details;
    public string MemberNames;
    public string MemberEmails;
}

static class ReserveData
{
    public static List<Information> Data = new List<Information>();
    public static int column = 13;

    public static string Version = "2.0.0";
}

static class NewReserve
{
    public static Information newInfo;
}

static class SearchReserve
{
    public static Information unknownInfo;
    public static int index;
    public static int sheetRow;
}


