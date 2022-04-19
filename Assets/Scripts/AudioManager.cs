using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager>
{
    public static string MIXER_PARAM_MASTER_VOLUME = "Master_Volume";
    public static string MIXER_PARAM_MUSIC_VOLUME = "Music_Volume";
    public static string MIXER_PARAM_SFX_VOLUME = "SFX_Volume";

    public AudioMixer AudioMixer;

    // TODO pool these
    public AudioSource musicEmitter;
    public AudioSource sfxEmitter;

    private float masterVolume = 1;
    private float musicVolume = 1;
    private float sfxVolume = 1;

    public void SetMixerVolume(string key, float value)
    {
        AudioMixer.SetFloat(key, NormalizedToMixerValue(value));
    }
    public float GetMixerVolume(string key)
    {
        AudioMixer.GetFloat(key, out var volume);
        return MixerValueToNormalized(volume);
    }

    // Audio Mixer uses dB
    private float MixerValueToNormalized(float mixerValue)
    {
        return 1f + (mixerValue / 80f);
    }

    private float NormalizedToMixerValue(float normalizedValue)
    {
        return (normalizedValue - 1f) * 80f;
    }

    public void PlayMusic(AudioClipSO audioClipSo)
    {
        if (musicEmitter.clip == audioClipSo.audioClip) 
            return;
        musicEmitter.clip = audioClipSo.audioClip;
        musicEmitter.loop = audioClipSo.loop;
        musicEmitter.volume = GetMixerVolume(MIXER_PARAM_MUSIC_VOLUME);
        musicEmitter.Play();
    }

    public void PlaySfx(AudioClipSO audioClipSo)
    {
        sfxEmitter.clip = audioClipSo.audioClip;
        sfxEmitter.loop = audioClipSo.loop;
        sfxEmitter.volume = GetMixerVolume(MIXER_PARAM_SFX_VOLUME);
        sfxEmitter.Play();
    }
    public void MuteMusic()
    {
        musicVolume = GetMixerVolume(MIXER_PARAM_MUSIC_VOLUME);
        SetMixerVolume(MIXER_PARAM_MUSIC_VOLUME, 0);
    }
    
    public void UnmuteMusic()
    {
        SetMixerVolume(MIXER_PARAM_MUSIC_VOLUME, musicVolume);
    }

    public void MuteSfx()
    {
        sfxVolume = GetMixerVolume(MIXER_PARAM_SFX_VOLUME);
        SetMixerVolume(MIXER_PARAM_SFX_VOLUME, 0);
    }
    
    public void UnmuteSfx()
    {
        SetMixerVolume(MIXER_PARAM_SFX_VOLUME, sfxVolume);
    }

}