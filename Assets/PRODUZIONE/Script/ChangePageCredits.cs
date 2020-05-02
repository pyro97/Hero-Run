using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePageCredits : MonoBehaviour
{
    public GameObject credits;

    public void openPanel(int i)
    {
        if (i == 0)
        {
            credits.transform.GetChild(0).gameObject.SetActive(false);
            credits.transform.GetChild(1).gameObject.SetActive(true);
            credits.transform.GetChild(3).gameObject.SetActive(true);
            credits.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (i == 1)
        {
            print("ok");
            credits.transform.GetChild(0).gameObject.SetActive(true);
            credits.transform.GetChild(1).gameObject.SetActive(false);
            credits.transform.GetChild(3).gameObject.SetActive(false);
            credits.transform.GetChild(4).gameObject.SetActive(true);


        }
    }
}
