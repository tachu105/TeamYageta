using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField] public float volume;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
    private const float FEED_TIME = 2f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        Play(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(AudioClip clip)
    {
        StartCoroutine(PlayCoroutine(clip));
    }

    public void Play(int trackNumber)
    {
        StartCoroutine(PlayCoroutine(audioClips[trackNumber]));
    }


    private IEnumerator PlayCoroutine( AudioClip clip)
    {
        float time = 0f;
        if (audioSource.isPlaying)
        {
            while(time < FEED_TIME)
            {
                audioSource.volume = Mathf.Lerp(volume, 0f, time / FEED_TIME);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        audioSource.Stop();
        audioSource.clip = clip;
        yield return new WaitForEndOfFrame();
        audioSource.Play();

        time = 0f;
        while (time < FEED_TIME)
        {
            audioSource.volume = Mathf.Lerp(0f, volume, time / FEED_TIME);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
