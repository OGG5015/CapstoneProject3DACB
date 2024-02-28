using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class volumesettings : MonoBehaviour
{

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicslider;

    public void SetMusicVolume()
    {
        float volume = musicslider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume)*20);
    }
}
