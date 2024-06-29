/*
 * Script for UI to indicate time limit 
 * with a circular gauge
 * Created by Misora Tanaka
 * Inheritance: IUI_Interface
 * 
 * date: 24/02/15
 * --- Log ---
 * 02/15: Change comments to English
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCircleController : MonoBehaviour, IUI_Interface
{
    // Canvas Component
    [SerializeField] private Canvas canvas;
    // Gauge Image
    private Image circle;
    // Time to accept input
    private float inputTime;
    // Gauge is Working?
    private bool isWorking;


    void Start()
    {
        // Scripts that have their own
        GameObject obj = this.gameObject;
        // <Caution> Attend an image with the Find function
        circle = obj.transform.Find("ProgressCircle").GetComponent<Image>();
        // Hide Canvas
        DisableCanvas();
        // initialization
        isWorking = false;
	}

    void Update()
    {
        // Gauge update process
        if (isWorking) UpdateCircle();
    }

    /// <summary>
    /// Gauge reduction process
    /// </summary>
    private void UpdateCircle()
    {
        // Gauge decreases with time
        circle.fillAmount -= 1.0f / inputTime * Time.deltaTime;
        // Gauge goes to zero
        if (circle.fillAmount <= 0.0f) isWorking = false;
    }

    /// <summary>
    /// Start Gauge process
    /// </summary>
    /// <param name="Time">Time to accept input</param>
    public void ActiveGauge(float Time) 
    { 
        isWorking = true;
        circle.fillAmount = 1.0f;
        inputTime = Time;
    }

    /// <summary>
    /// Below is the implementation of the UI interface
    /// </summary>
	public void EnableCanvas() { canvas.enabled = true; }

    public void DisableCanvas() { canvas.enabled = false; }

    public void FadeOutUI(GameObject UI, int index) { }
}
