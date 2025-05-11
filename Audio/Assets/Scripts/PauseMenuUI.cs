using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderEnvironment;
    [SerializeField] private Slider sliderPlayer;

    [SerializeField] private AudioMixer audioMixer;
    bool isPaused = false;
    void Start()
    {
        sliderMaster.onValueChanged.AddListener(SetMasterVolume);
        sliderEnvironment.onValueChanged.AddListener(SetEnvironmentVolume);
        sliderPlayer.onValueChanged.AddListener(SetPlayerVolume);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseUI.SetActive(!pauseUI.activeSelf);
            if (isPaused)
            {
                audioMixer.FindSnapshot("Paused").TransitionTo(0.1f);
            }
            else
            {
                audioMixer.FindSnapshot("Default").TransitionTo(0.1f);
            }
        }
    }
    public void SetMasterVolume(float value)
    {
        float adjustedValue = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(adjustedValue) * 20);
    }

    public void SetEnvironmentVolume(float value)
    {
        float adjustedValue = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("EnvironmentVolume", Mathf.Log10(adjustedValue) * 20);
    }

    public void SetPlayerVolume(float value)
    {
        float adjustedValue = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("PlayerVolume", Mathf.Log10(adjustedValue) * 20);
    }
}
