using UnityEngine;


[RequireComponent(typeof(BasePlayer))]
[RequireComponent(typeof(AudioSource))]
public class PlayerSoundComponent : MonoBehaviour
{
    [SerializeField] private AudioClip[] m_FootstepSounds;    
    [SerializeField] private AudioClip m_JumpSound;           
    [SerializeField] private AudioClip m_LandSound;

    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayFootStepAudio()
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }

    public void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }

    public void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
    }
}
