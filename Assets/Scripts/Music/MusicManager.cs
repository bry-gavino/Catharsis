using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public static MusicManager MusicInstance;

    [Tooltip("Add an audio source component to the game object that uses this script")]
    private AudioSource _audioSource;

    [SerializeField] [Tooltip("A list of audio tracks to loop through and play.")]
    private AudioClip[] tracks;

    private bool playing;

    /**
     * Only one music manager at a time.
     */
    private void Awake() {
        if (MusicInstance != null && MusicInstance != this) {
            Destroy(this.gameObject);
            return;
        }

        MusicInstance = this;
        _audioSource = GetComponent<AudioSource>();
        playing = true;
        StartCoroutine(PlayMusicLoop());
    }

    /**
     * Given an audio clip, plays the sound. Can be called by other objects that reference this Music Manager.
     */
    public void playClip(AudioClip sound, float scale) {
        _audioSource.PlayOneShot(sound, scale);
    }

    /**
     * Loops through the current tracks of music.
     */
    IEnumerator PlayMusicLoop() {
        yield return null;
        while (playing) {
            for (int i = 0; i < tracks.Length; i++) {
                _audioSource.clip = tracks[i];
                _audioSource.Play();
                while (_audioSource.isPlaying) {
                    yield return null;
                }
            }
        }
    }
}