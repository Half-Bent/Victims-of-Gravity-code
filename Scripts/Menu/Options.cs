using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;

public class Options : MonoBehaviour {

    [System.Serializable]
    public class Settings
    {
        public float AudioLevel;
        public int maxScore;
        public bool MotionBlur;

        public Settings()
        {
            AudioLevel = 10f;
            maxScore = 0;
            MotionBlur = true;
        }
    }

    [SerializeField]
    private AudioMixerGroup Master;
    [SerializeField]
    private GameObject volumeSlider;
    [SerializeField]
    private string FileName;
    [SerializeField]
    private PostProcessingProfile currentProfile;

    private Settings settings;
    

    private void Awake()
    {
        if (File.Exists(FileName))
        {
            settings = Utils.GetOptionsFromFile(FileName);
        }
        else
        {
            settings = new Settings()
            {
                AudioLevel = 10f,
                maxScore = 0
            };
        }
    }



    // Use this for initialization
    void Start () {
        
        float volume;
        VolumeChange(settings.AudioLevel);
        if(!Master.audioMixer.GetFloat("Volume", out volume))
        {
            Debug.Log("Wrong name");
        }

        volumeSlider.GetComponent<Slider>().value = volume;
	}
	
    public void VolumeChange(float value)
    {
        Master.audioMixer.SetFloat("Volume", value);
        settings.AudioLevel = value;
    }

   public void SavesSettings()
    {
        Utils.WriteToFile(settings, FileName);
        currentProfile.motionBlur.enabled = settings.MotionBlur;
    }

    public void OnmotionBlurrChange(bool value)
    {
        settings.MotionBlur = value;
    }
}