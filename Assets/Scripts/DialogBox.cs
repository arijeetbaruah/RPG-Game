using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour
{
    public GameObject FullText;
    public GameObject gridText;

    public void setFullText(string txt)
    {
        FullText.SetActive(true);
        gridText.SetActive(false);

        TextMeshProUGUI meshText = FullText.GetComponentInChildren<TextMeshProUGUI>();
        meshText.SetText(txt);
    }
}
