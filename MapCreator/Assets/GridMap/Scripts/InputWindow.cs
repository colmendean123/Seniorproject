using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class InputWindow : MonoBehaviour
{
    private Button continueButton;
    private TMP_InputField widthInput;
    private TMP_InputField heightInput;
    public string width, height;


    private void Start()
    {
        continueButton = transform.Find("ContinueButton").GetComponent<Button>();
        widthInput = transform.Find("WidthInput").GetComponent<TMP_InputField>();
        heightInput = transform.Find("HeightInput").GetComponent<TMP_InputField>();
        Show();
        fileSetUp();
    }

    private void fileSetUp()
    {
        if (File.Exists("./MapConFig"))
            return;
        else
            System.IO.Directory.CreateDirectory("./MapConFig");
    }

    public void onContinue()
    {
        storeValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void storeValues()
    {
        string w = widthInput.text;
        string h = heightInput.text;
        using (var sw = new StreamWriter("./MapConFig/MapConfig.txt"))
        {
            sw.Write(w);
            sw.WriteLine();
            sw.Write(h);
        }
    }

    public string GetWidth()
    {
        return width;
    }

    public string GetHeight()
    {
        return height;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
