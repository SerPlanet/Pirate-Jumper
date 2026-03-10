using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("AudioSources")]
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioSource environmentAudio;
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource UIAudio;
    [SerializeField] private List<AudioSource> coinAudioSource;

    [Header("PublicAudioSources")]
    [SerializeField] private List<AudioClip> collectCoinAudioClip;
    [SerializeField] private AudioClip walkingAudioClip;
    [SerializeField] private List<AudioClip> ambienteMusicClip;
    [SerializeField] private AudioClip switchButtonOn;

    private bool sFXSound, uISound, musikSOund;
    private int nextCoinAudioSource;
     private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    private void Start()
    {
        GameManager.OnGameStateChange += GameStateChanged; 
        musikSOund = SaveManager.Instance.LoadMusicSound();
        musicAudio.mute = musikSOund;
        sFXSound = SaveManager.Instance.LoadSFXSound();
        playerAudio.mute = sFXSound;
        environmentAudio.mute = sFXSound;
        foreach(AudioSource coinAudio in coinAudioSource)
        {
            coinAudio.mute = sFXSound;
        }
        uISound = SaveManager.Instance.LoadUISound();
        ToggleUISound();
        SpawnManager.Instance.AudiManagerRdy();
        
    }
    private void OnDestroy()
    {
         GameManager.OnGameStateChange -= GameStateChanged;
    }

    #region SFXSound
    public void PlayPlayerAudi(AudioClip audioClip, float pitchStart, float pitchEnd)
    {
        playerAudio.clip = audioClip;
        playerAudio.loop = false;
        playerAudio.Play();
    }

    public void PlayenvironmentAudi(AudioClip audioClip)
    {
        environmentAudio.clip = audioClip;
        environmentAudio.Play();
    }

     public void PlayCoinCollectSound()
    {
         AudioSource source = coinAudioSource.Find(a => !a.isPlaying);

        if (source == null)
        {
            // Alle AudioSources sind noch aktiv → optional:
            // - Überspringen (Sound nicht abspielen)
            // - Oder dynamisch eine neue Source hinzufügen
            return;
        }

        //source.pitch = Random.Range(1f, 1.1f);
        int randomCoinSound = Random.Range(0,collectCoinAudioClip.Count);
        source.PlayOneShot(collectCoinAudioClip[randomCoinSound]);
        
    }
    public void PlayWalkingSound()
    {
        playerAudio.clip = walkingAudioClip;
        playerAudio.loop = true;
        playerAudio.Play();
    }

    public void ToggleSound()
    {
        
        sFXSound = !sFXSound;
        if(!sFXSound)
        {
            PlayUIAudi(switchButtonOn, 1.1f,0.9f);
        }
        playerAudio.mute = sFXSound;
        environmentAudio.mute = sFXSound;
        foreach(AudioSource coinAudio in coinAudioSource)
        {
            coinAudio.mute = sFXSound;
        }
        SaveManager.Instance.SaveSFXSound(sFXSound);
    }
    #endregion

      #region UI
    public void PlayUIAudi(AudioClip audioClip, float pitchStart, float pitchEnd)
    {
        UIAudio.clip = audioClip;
        UIAudio.pitch = Random.Range(pitchStart,pitchEnd);
        UIAudio.Play();
    }

    public void ToggleUISound()
    {
        uISound = !uISound;
        UIAudio.mute = uISound;
         SaveManager.Instance.SaveUISound(uISound);
    }
    #endregion
    #region Musik
    public void PlaymusicAudi(AudioClip audioClip)
    {
        musicAudio.clip = audioClip;
        musicAudio.Play();
    }
   
    private void PLayInGameMusic()
    {
        musicAudio.loop = true;
        musicAudio.clip = ambienteMusicClip[0];
        musicAudio.Play();
    }
    private void StopInGameMusic()
    {
        
        musicAudio.Pause();
    }
      private void RepeatInGameMusic()
    {
        musicAudio.UnPause();
    }
     IEnumerator PlayMusic()
    {
        int index = Random.Range(0, ambienteMusicClip.Count);

        while(true) // unendliche Schleife
        {
            AudioClip currentClip = ambienteMusicClip[index];


            musicAudio.clip = currentClip;
            musicAudio.Play();

            yield return new WaitForSeconds(currentClip.length);

            // Index für nächste Runde erhöhen
            index = (index + 1) % ambienteMusicClip.Count; // wiederholt von vorne
        }
    }

    public void ToggleMusik()
    {
        musikSOund = !musikSOund;
        musicAudio.mute = musikSOund;
        if (!musikSOund)
        {
            PlayUIAudi(switchButtonOn, 1.1f,0.9f);
        }
      
        SaveManager.Instance.SaveMusicSound(musikSOund);
    }
    #endregion
    //Public sounds
   


   

    

  

    private void GameStateChanged(GameStates newState)
    {
        switch (newState){
            case(GameStates.StartGame):
                PLayInGameMusic();
            break;
            case(GameStates.GameEnds):
                StopInGameMusic();
            break;
            case(GameStates.GameRunning):
                RepeatInGameMusic();

            break;
            case(GameStates.PauseGame):
                StopInGameMusic();
            break;
            case(GameStates.Loading):
            
            break;
        }
    }
}
