using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.PostProcessing;

public class InitialStartSettings : MonoBehaviour {
    [SerializeField]
    Options.Settings temp;
    [SerializeField]
    private PostProcessingProfile CurrentProfile;
    [SerializeField]
    private AudioMixerGroup Master;
    [SerializeField]
    private string DataFileName;

    private void Start()
    {
        if (File.Exists(DataFileName))
        {
            Options.Settings set = Utils.GetOptionsFromFile(DataFileName);
            temp = set;
            Master.audioMixer.SetFloat("Volume", set.AudioLevel);
            CurrentProfile.motionBlur.enabled = set.MotionBlur;
        }
    }

}
