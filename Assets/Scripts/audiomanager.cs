using UnityEngine;

public class audiomanager : MonoBehaviour
{
    [Header("-----------Audio Source-----------")]
    [SerializeField] AudioSource musicsource;
    [SerializeField] AudioSource backgroundsource;

    [Header("-----------Audio Clip-------------")]
    public AudioClip music;
    public AudioClip background;


    private void Start()
    {
        musicsource.clip = background;
        musicsource.Play();
    }
}
