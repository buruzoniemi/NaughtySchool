using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIController : MonoBehaviour
{
    [SerializeField] private Toggle[] toggle;
    private Text[] texts;


    // Start is called before the first frame update
    void Start()
    {
        texts = new Text[toggle.Length];
        for(int ti = 0; ti < toggle.Length; ti++)
        {
            texts[ti] = toggle[ti].transform.Find("Label").GetComponent<Text>();
            texts[ti].text = MischiefManager.instance.GetTaskName(ti);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int ti = 0; ti < toggle.Length; ti++)
        {
            toggle[ti].isOn = MischiefManager.instance.GetTaskComp(ti);
        }

    }
}
