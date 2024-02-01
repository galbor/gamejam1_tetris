using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    // UI
    //  Opening scene
    [SerializeField] private AudioSource startButtonPressedSound;
    [SerializeField] private AudioSource startBackgroundMusic;
    //  Lose / Win scenes
    [SerializeField] private AudioSource loseBackgroundMusic;
    [SerializeField] private AudioSource winBackgroundMusic;
    [SerializeField] private AudioSource restartButtonPressedSound;
    //  Playing scene
    [SerializeField] private AudioSource scoreUpSound;
    
    // Character
    [SerializeField] private AudioSource firstLandingSound;
    [SerializeField] private AudioSource runningSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landOnSinkSound;
    [SerializeField] private AudioSource landOnObjectSound;
    [SerializeField] private AudioSource landInWaterSound;
    [SerializeField] private AudioSource squeezeSound;
    [SerializeField] private AudioSource splashOnSqueezeSound;
    [SerializeField] private AudioSource smashedByObjectSound;
    [SerializeField] private AudioSource drownSound;
    
    // Environment
    //  Home
    [SerializeField] private AudioSource roomToneSound;
    [SerializeField] private AudioSource peopleFoliesSound;
    [SerializeField] private AudioSource[] homeBackgroundMusics;
    //  Objects
    [SerializeField] private AudioSource fallingObjectSound;
    [SerializeField] private AudioSource plateBreakSound;
    [SerializeField] private AudioSource dishWithDishCollisionSound;
    //  Water
    [SerializeField] private AudioSource faucetOpenSound;
    [SerializeField] private AudioSource faucetCloseSound;
    [SerializeField] private AudioSource faucetOpenWaterPressureSound;
    [SerializeField] private AudioSource faucetCloseWaterPressureSound;
    [SerializeField] private AudioSource waterFallOnSinkSound;
    [SerializeField] private AudioSource waterFallOnWaterSound;
    [SerializeField] private AudioSource waterFlowSound;
    [SerializeField] private AudioSource underwaterMovingSound;
    
    // others
    [SerializeField] private PlayerMovement playerMovement;
    private int _homeBackgroundIndex = 0;
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


    private static AudioManager Instance { get; set; }

    private void Awake() => Instance = this;

    private void Start()
    {
        _isRunningSoundNull = runningSound == null;
        _isunderwaterMovingSoundNull = underwaterMovingSound == null;
        _iswaterFlowSoundNull = waterFlowSound == null;
        _isrestartButtonPressedSoundNull = restartButtonPressedSound == null;
        _isfirstLandingSoundNull = firstLandingSound == null;
        _isrunningSoundNull = runningSound == null;
        _isjumpSoundNull = jumpSound == null;
        _isfallingObjectSoundNull = fallingObjectSound == null;
        _isfaucetOpenSoundNull = faucetOpenSound == null;
        _isfaucetOpenWaterPressureSoundNull = faucetOpenWaterPressureSound == null;
        _fallOnSinkSoundNull = waterFallOnSinkSound == null;
        _iswaterFallOnSinkSoundNull = waterFallOnSinkSound == null;
        _fallOnWaterSoundNull = waterFallOnWaterSound == null;
        _iswaterFallOnWaterSoundNull = waterFallOnWaterSound == null;
        EventManagerScript.Instance.StartListening(EventManagerScript.Win, WinAudio);
        EventManagerScript.Instance.StartListening(EventManagerScript.Lose, LoseAudio);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerHit, PlaySmashedByObject);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDrowned, PlayDrown);
        
        PlayStartBackground();
    }

    private void WinAudio(object arg0)
    {
        homeBackgroundMusics[_homeBackgroundIndex].Stop();
        roomToneSound.Stop();
        peopleFoliesSound.Stop();
        PlayWinBackground();
    }
    
    private void LoseAudio(object arg0)
    {
        homeBackgroundMusics[_homeBackgroundIndex].Stop();
        roomToneSound.Stop();
        peopleFoliesSound.Stop();
        PlayLoseBackground();
    }

    private void LateUpdate()
    {
        if (playerMovement.Running && !_isRunningSoundNull && !runningSound.isPlaying)
        {
            PlayRunning();
        }
    }

    private void PlayStartButtonPressedSound()
    {
        if (startButtonPressedSound == null) return;
        startButtonPressedSound.PlayOneShot(startButtonPressedSound.clip);
    }
    
    private void PlayStartBackgroundMusic()
    {
        if (startBackgroundMusic == null) return;
        startBackgroundMusic.Play();
    }
    
    private void StopStartBackgroundMusic()
    {
        if (startBackgroundMusic == null) return;
        startBackgroundMusic.Stop();
    }
    
    private void PlayLoseBackgroundMusic()
    {
        if (loseBackgroundMusic == null) return;
        loseBackgroundMusic.Play();
    }
    
    private void PlayWinBackgroundMusic()
    {
        if (winBackgroundMusic == null) return;
        winBackgroundMusic.Play();
    }
    
    private void PlayRestartButtonPressedSound()
    {
        if (_isrestartButtonPressedSoundNull) return;
        restartButtonPressedSound.PlayOneShot(restartButtonPressedSound.clip);
    }
    
    private void PlayScoreUpSound()
    {
        if (scoreUpSound == null) return;
        scoreUpSound.PlayOneShot(scoreUpSound.clip);
    }
    
    private void PlayFirstLandingSound()
    {
        if (_isfirstLandingSoundNull) return;
        firstLandingSound.PlayOneShot(firstLandingSound.clip);
    }
    
    private void PlayRunningSound()
    {
        if (_isrunningSoundNull) return;
        runningSound.Play();
    }
    
    private void PlayJumpSound()
    {
        if (_isjumpSoundNull) return;
        jumpSound.PlayOneShot(jumpSound.clip);
    }
    
    private void PlayLandOnSinkSound()
    {
        if (landOnSinkSound == null) return;
        landOnSinkSound.PlayOneShot(landOnSinkSound.clip);
    }
    
    private void PlayLandOnObjectSound()
    {
        if (landOnObjectSound == null) return;
        landOnObjectSound.PlayOneShot(landOnObjectSound.clip);
    }
    
    private void PlayLandInWaterSound()
    {
        if (landInWaterSound == null) return;
        landInWaterSound.PlayOneShot(landInWaterSound.clip);
    }
    
    private void PlaySqueezeSound()
    {
        if (squeezeSound == null) return;
        squeezeSound.PlayOneShot(squeezeSound.clip);
    }

    private void PlaySplashOnSqueezeSound()
    {
        if (splashOnSqueezeSound == null) return;
        splashOnSqueezeSound.PlayOneShot(splashOnSqueezeSound.clip);
    }
    
    private void PlaySmashedByObjectSound()
    {
        if (smashedByObjectSound == null) return;
        smashedByObjectSound.PlayOneShot(smashedByObjectSound.clip);
    }
    
    private void PlayDrownSound()
    {
        if (drownSound == null) return;
        drownSound.PlayOneShot(drownSound.clip);
    }
    
    private void PlayRoomToneSound()
    {
        if (roomToneSound == null) return;
        roomToneSound.Play();
    }
    
    private void PlayPeopleFoliesSound()
    {
        if (peopleFoliesSound == null) return;
        peopleFoliesSound.Play();
    }
    
    private void PlayHomeBackgroundMusic()
    {
        if (homeBackgroundMusics == null || homeBackgroundMusics.Length == 0) return;
        _homeBackgroundIndex = Random.Range(0, homeBackgroundMusics.Length);
        homeBackgroundMusics[_homeBackgroundIndex].Play();
    }
    
    private void PlayFallingObjectSound()
    {
        if (_isfallingObjectSoundNull) return;
        fallingObjectSound.PlayOneShot(fallingObjectSound.clip);
    }
    
    private void PlayPlateBreakSound()
    {
        if (plateBreakSound == null) return;
        plateBreakSound.PlayOneShot(plateBreakSound.clip);
    }
    
    private void PlayDishWithDishCollisionSound()
    {
        if (dishWithDishCollisionSound == null) return;
        dishWithDishCollisionSound.PlayOneShot(dishWithDishCollisionSound.clip);
    }
    
    private void PlayFaucetOpenSound()
    {
        if (_isfaucetOpenSoundNull) return;
        faucetOpenSound.PlayOneShot(faucetOpenSound.clip);
    }
    
    private void PlayFaucetCloseSound()
    {
        if (faucetCloseSound == null) return;
        faucetCloseSound.PlayOneShot(faucetCloseSound.clip);
    }
    
    private void PlayFaucetOpenWaterPressureSound()
    {
        if (_isfaucetOpenWaterPressureSoundNull) return;
        faucetOpenWaterPressureSound.PlayOneShot(faucetOpenWaterPressureSound.clip);
    }
    
    private void PlayFaucetCloseWaterPressureSound()
    {
        if (faucetCloseWaterPressureSound == null) return;
        faucetCloseWaterPressureSound.PlayOneShot(faucetCloseWaterPressureSound.clip);
    }
    
    private void PlayWaterFallOnSinkSound()
    {
        if (_fallOnSinkSoundNull || waterFallOnSinkSound.isPlaying) return;
        waterFallOnSinkSound.PlayOneShot(waterFallOnSinkSound.clip);
    }
    
    private void StopWaterFallOnSinkSound()
    {
        if (_iswaterFallOnSinkSoundNull || !waterFallOnSinkSound.isPlaying) return;
        waterFallOnSinkSound.Stop();
    }
    
    private void PlayWaterFallOnWaterSound()
    {
        if (_fallOnWaterSoundNull || waterFallOnWaterSound.isPlaying) return;
        waterFallOnWaterSound.PlayOneShot(waterFallOnWaterSound.clip);
    }
    
    private void StopWaterFallOnWaterSound()
    {
        if (_iswaterFallOnWaterSoundNull || !waterFallOnWaterSound.isPlaying) return;
        waterFallOnWaterSound.Stop();
    }

    
    private void PlayWaterFlowSound()
    {
        if (_iswaterFlowSoundNull || waterFlowSound.isPlaying) return;
        waterFlowSound.PlayOneShot(waterFlowSound.clip);
    }
    
    private void PlayUnderwaterMovingSound()
    {
        if (_isunderwaterMovingSoundNull || underwaterMovingSound.isPlaying) return;
        underwaterMovingSound.PlayOneShot(underwaterMovingSound.clip);
    }

    public static void PlayStartButtonPressed() => Instance.PlayStartButtonPressedSound();
    public static void PlayStartBackground() => Instance.PlayStartBackgroundMusic();
    public static void StopStartBackground() => Instance.StopStartBackgroundMusic();
    public static void PlayLoseBackground(object arg0=null) => Instance.PlayLoseBackgroundMusic();
    public static void PlayWinBackground(object arg0=null) => Instance.PlayWinBackgroundMusic();
    public static void PlayRestartButtonPressed() => Instance.PlayRestartButtonPressedSound();
    public static void PlayScoreUp() => Instance.PlayScoreUpSound();  // todo score not yet implemented
    public static void PlayFirstLanding() => Instance.PlayFirstLandingSound();
    public static void PlayRunning() => Instance.PlayRunningSound();
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
}
