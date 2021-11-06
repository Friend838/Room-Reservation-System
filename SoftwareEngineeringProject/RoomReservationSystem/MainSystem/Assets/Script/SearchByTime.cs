using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SearchByTime : MonoBehaviour
{
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
        //SceneManager.LoadScene(1);
    }

    public void Back()
    {
        Debug.Log("Back");
        SceneManager.LoadScene(0);
    }
}
