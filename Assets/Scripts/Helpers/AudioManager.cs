using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    // UI
    //  Opening scene
    [SerializeField] private AudioSource[] startButtonPressedSounds;
    [SerializeField] private AudioSource[] startBackgroundMusics;
    private int _startBackgroundIndex = 0;
    //  Lose / Win scenes
    [SerializeField] private AudioSource[] loseBackgroundMusics;
    private int _loseBackgroundIndex = 0;
    [SerializeField] private AudioSource[] winBackgroundMusics;
    private int _winBackgroundIndex = 0;
    [SerializeField] private AudioSource[] restartButtonPressedSounds;
    //  Playing scene
    [SerializeField] private AudioSource[] scoreUpSounds;
    
    // Character
    [SerializeField] private AudioSource[] firstLandingSounds;
    [SerializeField] private AudioSource[] runningSounds;
    private int _runningSoundIndex;
    [SerializeField] private AudioSource[] jumpSounds;
    [SerializeField] private AudioSource[] landOnSinkSounds;
    [SerializeField] private AudioSource[] landOnObjectSounds;
    [SerializeField] private AudioSource[] landInWaterSounds;
    [SerializeField] private AudioSource[] squeezeSounds;
    [SerializeField] private AudioSource[] splashOnSqueezeSounds;
    [SerializeField] private AudioSource[] smashedByObjectSounds;
    [SerializeField] private AudioSource[] drownSounds;
    
    // Environment
    //  Home
    [SerializeField] private AudioSource[] roomToneSounds;
    private int _roomToneIndex = 0;
    [SerializeField] private AudioSource[] peopleFoliesSounds;
    private int _peopleFoliesIndex = 0;
    [SerializeField] private AudioSource[] homeBackgroundMusics;
    private int _homeBackgroundIndex = 0;
    //  Objects
    [SerializeField] private AudioSource[] fallingObjectSounds;
    [SerializeField] private AudioSource[] plateBreakSounds;
    [SerializeField] private AudioSource[] dishWithDishCollisionSounds;
    //  Water
    [SerializeField] private AudioSource[] faucetOpenSounds;
    [SerializeField] private AudioSource[] faucetCloseSounds;
    [SerializeField] private AudioSource[] faucetOpenWaterPressureSounds;
    [SerializeField] private AudioSource[] faucetCloseWaterPressureSounds;
    [SerializeField] private AudioSource[] waterFallOnSinkSounds;
    [SerializeField] private AudioSource[] waterFallOnWaterSounds;
    [SerializeField] private AudioSource[] waterFlowSounds;
    [SerializeField] private AudioSource[] underwaterMovingSounds;
    
    // others
    [SerializeField] private PlayerMovement playerMovement;
    private bool _iswaterFallOnWaterSoundNull;
    private bool _fallOnWaterSoundNull;
    private bool _iswaterFallOnSinkSoundNull;
    private bool _fallOnSinkSoundNull;
    private bool _isfaucetOpenWaterPressureSoundNull;
    private bool _isfaucetOpenSoundNull;
    private bool _isfallingObjectSoundNull;
    private bool _isjumpSoundNull;
    private bool _isrunningSoundNull;
    private bool _isfirstLandingSoundNull;
    private bool _isrestartButtonPressedSoundNull;
    private bool _iswaterFlowSoundNull;
    private bool _isunderwaterMovingSoundNull;
    private bool _isRunningSoundNull;
    private bool _isLandInWaterSoundsNull;
    private bool _isSqueezeSoundsNull;
    private bool _isSmashedByObjectSoundsNull;
    private bool _isHomeBackgroundMusicsNull;
    private bool _isStartButtonPressedSoundsNull;
    private bool _isLoseBackgroundSoundsNull;
    private bool _isWinBackgroundMusicsNull;
    private bool _isScoreUpSoundsNull;
    private bool _isRoomToneSoundsNull;
    private bool _isPeopleFoliesSoundsNull;
    private int _waterFallOnSinkIndex = 0;
    private int _waterFallOnWaterIndex = 0;
    private int _underwaterMovingIndex = 0;


    private static AudioManager Instance { get; set; }

    private void Awake() => Instance = this;

    private void Start()
    {
        _isRunningSoundNull = runningSounds == null || runningSounds.Length == 0;
        _isunderwaterMovingSoundNull = underwaterMovingSounds == null || underwaterMovingSounds.Length == 0;
        _iswaterFlowSoundNull = waterFlowSounds == null || waterFlowSounds.Length == 0;
        _isrestartButtonPressedSoundNull = restartButtonPressedSounds == null || restartButtonPressedSounds.Length == 0;
        _isfirstLandingSoundNull = firstLandingSounds == null || firstLandingSounds.Length == 0;
        _isrunningSoundNull = runningSounds == null || runningSounds.Length == 0;
        _isjumpSoundNull = jumpSounds == null || jumpSounds.Length == 0;
        _isfallingObjectSoundNull = fallingObjectSounds == null || fallingObjectSounds.Length == 0;
        _isfaucetOpenSoundNull = faucetOpenSounds == null || faucetOpenSounds.Length == 0;
        _isfaucetOpenWaterPressureSoundNull = faucetOpenWaterPressureSounds == null || faucetOpenWaterPressureSounds.Length == 0;
        _fallOnSinkSoundNull = waterFallOnSinkSounds == null || waterFallOnSinkSounds.Length == 0;
        _iswaterFallOnSinkSoundNull = waterFallOnSinkSounds == null || waterFallOnSinkSounds.Length == 0;
        _fallOnWaterSoundNull = waterFallOnWaterSounds == null || waterFallOnWaterSounds.Length == 0;
        _iswaterFallOnWaterSoundNull = waterFallOnWaterSounds == null || waterFallOnWaterSounds.Length == 0;
        _isLandInWaterSoundsNull = landInWaterSounds == null || landInWaterSounds.Length == 0;
        _isSqueezeSoundsNull = squeezeSounds == null || squeezeSounds.Length == 0;
        _isSmashedByObjectSoundsNull = smashedByObjectSounds == null || smashedByObjectSounds.Length == 0;
        _isHomeBackgroundMusicsNull = homeBackgroundMusics == null || homeBackgroundMusics.Length == 0.0;
        _isLoseBackgroundSoundsNull = loseBackgroundMusics == null || loseBackgroundMusics.Length == 0;
        _isRoomToneSoundsNull = roomToneSounds == null || roomToneSounds.Length == 0;
        _isPeopleFoliesSoundsNull = peopleFoliesSounds == null || peopleFoliesSounds.Length == 0;
        _isScoreUpSoundsNull = scoreUpSounds == null || scoreUpSounds.Length == 0;
        _isWinBackgroundMusicsNull = winBackgroundMusics == null || winBackgroundMusics.Length == 0;
        _isStartButtonPressedSoundsNull = startButtonPressedSounds == null || startButtonPressedSounds.Length == 0;
        EventManagerScript.Instance.StartListening(EventManagerScript.Win, WinAudio);
        EventManagerScript.Instance.StartListening(EventManagerScript.Lose, LoseAudio);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerHit, PlaySmashedByObject);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDrowned, PlayDrown);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerFirstLand, PlayFirstLanding);
        PlayStartBackground();
    }
    
    private void PlaySound(AudioSource audioSource)
    {
        if (audioSource == null) return;
        audioSource.Play();
    }
    
    private void PlayRandomSound(AudioSource[] audioSources)
    {
        if (audioSources == null || audioSources.Length == 0) return;
        var random = Random.Range(0, audioSources.Length);
        audioSources[random].Play();
    }
    
    private void StopSound(AudioSource audioSource)
    {
        if (audioSource == null) return;
        audioSource.Stop();
    }

    private void WinAudio(object arg0)
    {
        if (homeBackgroundMusics == null || homeBackgroundMusics.Length == 0) return;
        homeBackgroundMusics[_homeBackgroundIndex].Stop();
        
        StopSound(roomToneSounds[_roomToneIndex]);
        StopSound(peopleFoliesSounds[_peopleFoliesIndex]);
        PlayWinBackground();
    }
    
    private void LoseAudio(object arg0)
    {
        if (homeBackgroundMusics == null || homeBackgroundMusics.Length == 0) return;
        homeBackgroundMusics[_homeBackgroundIndex].Stop();
        StopSound(roomToneSounds[_roomToneIndex]);
        StopSound(peopleFoliesSounds[_peopleFoliesIndex]);
        PlayLoseBackground();
    }

    private void LateUpdate()
    {
        if (playerMovement.Running && !_isRunningSoundNull && !runningSounds[_runningSoundIndex].isPlaying)
        {
            PlayRunning();
        }
    }

    private void PlayStartButtonPressedSound()
    {
        PlayRandomSound(startButtonPressedSounds);
    }
    
    private void PlayStartBackgroundMusic()
    {
        if (_isStartButtonPressedSoundsNull) return;
        _startBackgroundIndex = Random.Range(0, startBackgroundMusics.Length);
        PlaySound(startBackgroundMusics[_startBackgroundIndex]);
    }
    
    private void StopStartBackgroundMusic()
    {
        if (_isStartButtonPressedSoundsNull) return;
        StopSound(startBackgroundMusics[_startBackgroundIndex]);
    }
    
    private void PlayLoseBackgroundMusic()
    {
        if (_isLoseBackgroundSoundsNull) return;
        _loseBackgroundIndex = Random.Range(0, loseBackgroundMusics.Length);
        PlaySound(loseBackgroundMusics[_loseBackgroundIndex]);
    }
    
    private void PlayWinBackgroundMusic()
    {
        if (_isWinBackgroundMusicsNull) return;
        _winBackgroundIndex = Random.Range(0, winBackgroundMusics.Length);
        PlaySound(winBackgroundMusics[_winBackgroundIndex]);
    }
    
    private void PlayRestartButtonPressedSound()
    {
        PlayRandomSound(restartButtonPressedSounds);
    }
    
    private void PlayScoreUpSound()
    {
        PlayRandomSound(scoreUpSounds);
    }
    
    private void PlayFirstLandingSound()
    {
        PlayRandomSound(firstLandingSounds);
    }
    
    private void PlayRunningSound()
    {
        if (_isrunningSoundNull) return;
        _runningSoundIndex = Random.Range(0, runningSounds.Length);
        runningSounds[_runningSoundIndex].Play();
    }
    
    private void StopRunningSound()
    {
        if (_isrunningSoundNull || !runningSounds[_runningSoundIndex].isPlaying) return;
        runningSounds[_runningSoundIndex].Stop();
    }
    
    private void PlayJumpSound()
    {
        PlayRandomSound(jumpSounds);
    }
    
    private void PlayLandOnSinkSound()
    {
        PlayRandomSound(landOnSinkSounds);
    }
    
    private void PlayLandOnObjectSound()
    {
        PlayRandomSound(landOnObjectSounds);
    }
    
    private void PlayLandInWaterSound()
    {
        PlayRandomSound(landInWaterSounds);
    }
    
    private void PlaySqueezeSound()
    {
        PlayRandomSound(squeezeSounds);
    }

    private void PlaySplashOnSqueezeSound()
    {
        PlayRandomSound(splashOnSqueezeSounds);
    }
    
    private void PlaySmashedByObjectSound()
    {
        PlayRandomSound(smashedByObjectSounds);
    }
    
    private void PlayDrownSound()
    {
        PlayRandomSound(drownSounds);
    }
    
    private void PlayRoomToneSound()
    {
        if (_isRoomToneSoundsNull) return;
        _roomToneIndex = Random.Range(0, roomToneSounds.Length);
        roomToneSounds[_roomToneIndex].Play();
    }
    
    private void PlayPeopleFoliesSound()
    {
        if (_isPeopleFoliesSoundsNull) return;
        _peopleFoliesIndex = Random.Range(0, peopleFoliesSounds.Length);
        peopleFoliesSounds[_peopleFoliesIndex].Play();
    }
    
    private void PlayHomeBackgroundMusic()
    {
        if (_isHomeBackgroundMusicsNull) return;
        _homeBackgroundIndex = Random.Range(0, homeBackgroundMusics.Length);
        homeBackgroundMusics[_homeBackgroundIndex].Play();
    }
    
    private void PlayFallingObjectSound()
    {
        PlayRandomSound(fallingObjectSounds);
    }
    
    private void PlayPlateBreakSound()
    {
        PlayRandomSound(plateBreakSounds);
    }
    
    private void PlayDishWithDishCollisionSound()
    {
        PlayRandomSound(dishWithDishCollisionSounds);
    }
    
    private void PlayFaucetOpenSound()
    {
        PlayRandomSound(faucetOpenSounds);
    }
    
    private void PlayFaucetCloseSound()
    {
        PlayRandomSound(faucetCloseSounds);
    }
    
    private void PlayFaucetOpenWaterPressureSound()
    {
        PlayRandomSound(faucetOpenWaterPressureSounds);
    }
    
    private void PlayFaucetCloseWaterPressureSound()
    {
        PlayRandomSound(faucetCloseWaterPressureSounds);
    }
    
    private void PlayWaterFallOnSinkSound()
    {
        if (_fallOnSinkSoundNull || waterFallOnSinkSounds[_waterFallOnSinkIndex].isPlaying) return;
        _waterFallOnSinkIndex = Random.Range(0, waterFallOnSinkSounds.Length);
        waterFallOnSinkSounds[_waterFallOnSinkIndex].Play();
    }
    
    private void StopWaterFallOnSinkSound()
    {
        if (_iswaterFallOnSinkSoundNull || !waterFallOnSinkSounds[_waterFallOnSinkIndex].isPlaying) return;
        waterFallOnSinkSounds[_waterFallOnSinkIndex].Stop();
    }
    
    private void PlayWaterFallOnWaterSound()
    {
        if (_fallOnWaterSoundNull || waterFallOnWaterSounds[_waterFallOnWaterIndex].isPlaying) return;
        _waterFallOnWaterIndex = Random.Range(0, waterFallOnWaterSounds.Length);
        waterFallOnWaterSounds[_waterFallOnWaterIndex].Play();
    }
    
    private void StopWaterFallOnWaterSound()
    {
        if (_iswaterFallOnWaterSoundNull || !waterFallOnWaterSounds[_waterFallOnWaterIndex].isPlaying) return;
        waterFallOnWaterSounds[_waterFallOnWaterIndex].Stop();
    }

    
    private void PlayWaterFlowSound()
    {
        PlayRandomSound(waterFlowSounds);
    }
    
    private void PlayUnderwaterMovingSound()
    {
        if (_isunderwaterMovingSoundNull || underwaterMovingSounds[_underwaterMovingIndex].isPlaying) return;
        _underwaterMovingIndex = Random.Range(0, underwaterMovingSounds.Length);
        underwaterMovingSounds[_underwaterMovingIndex].Play();
    }
    
    private void StopUnderwaterMovingSound()
    {
        if (_isunderwaterMovingSoundNull || !underwaterMovingSounds[_underwaterMovingIndex].isPlaying) return;
        underwaterMovingSounds[_underwaterMovingIndex].Stop();
    }

    public static void PlayStartButtonPressed() => Instance.PlayStartButtonPressedSound();
    public static void PlayStartBackground() => Instance.PlayStartBackgroundMusic();
    public static void StopStartBackground() => Instance.StopStartBackgroundMusic();
    public static void PlayLoseBackground(object arg0=null) => Instance.PlayLoseBackgroundMusic();
    public static void PlayWinBackground(object arg0=null) => Instance.PlayWinBackgroundMusic();
    public static void PlayRestartButtonPressed() => Instance.PlayRestartButtonPressedSound();
    public static void PlayScoreUp() => Instance.PlayScoreUpSound();  // todo score not yet implemented
    public static void PlayFirstLanding(object arg0=null) => Instance.PlayFirstLandingSound();
    public static void PlayRunning() => Instance.PlayRunningSound();
    public static void StopRunning() => Instance.StopRunningSound();
    public static void PlayJump() => Instance.PlayJumpSound();
    public static void PlayLandOnSink() => Instance.PlayLandOnSinkSound();
    public static void PlayLandOnObject() => Instance.PlayLandOnObjectSound();
    public static void PlayLandInWater() => Instance.PlayLandInWaterSound();
    public static void PlaySqueeze() => Instance.PlaySqueezeSound();  // todo squeeze not yet implemented
    public static void PlaySplashOnSqueeze() => Instance.PlaySplashOnSqueezeSound();  // todo squeeze not yet implemented
    public static void PlaySmashedByObject(object arg0=null) => Instance.PlaySmashedByObjectSound();
    public static void PlayDrown(object arg0=null) => Instance.PlayDrownSound();
    public static void PlayRoomTone() => Instance.PlayRoomToneSound(); // todo when?
    public static void PlayPeopleFolies() => Instance.PlayPeopleFoliesSound(); // todo when?
    public static void PlayHomeBackground() => Instance.PlayHomeBackgroundMusic();
    public static void PlayFallingObject() => Instance.PlayFallingObjectSound();
    public static void PlayPlateBreak() => Instance.PlayPlateBreakSound();  // todo plate break not yet implemented
    public static void PlayDishWithDishCollision() => Instance.PlayDishWithDishCollisionSound();
    public static void PlayFaucetOpen() => Instance.PlayFaucetOpenSound();
    public static void PlayFaucetClose() => Instance.PlayFaucetCloseSound();
    public static void PlayFaucetOpenWaterPressure() => Instance.PlayFaucetOpenWaterPressureSound();
    public static void PlayFaucetCloseWaterPressure() => Instance.PlayFaucetCloseWaterPressureSound();
    public static void PlayWaterFallOnSink() => Instance.PlayWaterFallOnSinkSound();
    public static void StopWaterFallOnWater() => Instance.StopWaterFallOnWaterSound();
    public static void StopWaterFallOnSink() => Instance.StopWaterFallOnSinkSound();


    public static void PlayWaterFallOnWater() => Instance.PlayWaterFallOnWaterSound();
    public static void PlayWaterFlow() => Instance.PlayWaterFlowSound();
    public static void PlayUnderwaterMoving() => Instance.PlayUnderwaterMovingSound();
    public static void StopUnderwaterMoving() => Instance.StopUnderwaterMovingSound();
}
