
using UnityEngine;
using UnityEngine.UI;

public class VolumeController: MonoBehaviour {
    private float savedVolume = 1.0f;

    private AudioSource audio;

    public Slider volumeSlider;


    void Start () {  
        audio = this.GetComponent<AudioSource> ();
        savedVolume = volumeSlider.value;
    }
    
    public void UpdateVolume () {
        audio.volume = volumeSlider.value;
    }

    public void Mute () {
        if (audio.volume > 0) {
            savedVolume = volumeSlider.value;
            volumeSlider.value = 0;
        } else {
            volumeSlider.value = savedVolume;
        }
    }
}