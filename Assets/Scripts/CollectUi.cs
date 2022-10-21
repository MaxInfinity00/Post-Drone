using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectUi : MonoBehaviour
{


    [SerializeField]private TMP_Text coconutText;
    private int coconutCount = 0;

  
    public void updateCoconutCount()
    {

        coconutCount++;
        coconutText.text = coconutCount.ToString() + "/7";


    }

}
