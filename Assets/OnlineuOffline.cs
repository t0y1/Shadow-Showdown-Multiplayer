using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class OnlineuOffline : MonoBehaviour
{
    public Button Online;
    public Button Offline;

    public GameObject panelOnuOff;
    public GameObject panelMulti;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On()
    {
        panelOnuOff.SetActive (false);
        panelMulti.SetActive(true);
    }

    public void off()
    {
        SceneManager.LoadScene("OgOffline");
    }
}
