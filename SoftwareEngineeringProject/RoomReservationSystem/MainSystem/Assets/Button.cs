using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Text buttonName;

    public Text Day;
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        if(Main.access == true)
        {
            buttonName.text = Main.s;
            Main.access = false;
            Main.s = "";
        }
        else
        {
            Debug.LogWarning("Generate Failed");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Day.text = buttonName.text;
    }
    bool lessThanTen(string day)
    {
        if(day == "1" || day == "2" || day == "3" || day == "4" || day == "5" || day == "6" || day == "7" || day == "8" || day == "9")
            return true;
        return false;
    }
    public void clicked()
    {
        string[] temp = Day.text.Split('/');
        if (buttonName.text != " ")
        {
            if(lessThanTen(buttonName.text))
                temp[2] = "0" + buttonName.text;
            else
                temp[2] = buttonName.text;
        }
        Day.text = temp[0] + "/" + temp[1] + "/" + temp[2];
        Debug.Log(buttonName.text);
    }
}
