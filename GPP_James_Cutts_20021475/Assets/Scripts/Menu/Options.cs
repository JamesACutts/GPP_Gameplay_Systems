using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedRes;
    public TMP_Text resolutionLabel;

    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;
        
        if(QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;

        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;

        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedRes = i;

                UpdateResLabel();
            }
        }

        if(!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);

            selectedRes = resolutions.Count - 1;

            UpdateResLabel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResLeft()
    {
        selectedRes--;
        if(selectedRes < 0)
        {
            selectedRes = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedRes++;
        if(selectedRes > resolutions.Count - 1)
        {
            selectedRes = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedRes].horizontal.ToString() + " X " + resolutions[selectedRes].vertical.ToString();
    }

    public void ApplyGrphics()
    {
       // Screen.fullScreen = fullscreenTog.isOn;
        if(vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreenTog.isOn);
    }
}
[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}