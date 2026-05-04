using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private float frequency = 0f;
    private float volume = 0f;
    private float envelope = 0f;
    private float decay = 0.99f;
    private int sampleRate = 48000;
    private float phase = 0f;
    private int waveform = 0; // 0 = Square, 1 = Sawtooth, 2 = Noise
    
    private System.Random rand = new System.Random();
    private float frequencySlide = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        sampleRate = AudioSettings.outputSampleRate;
    }

    public void PlayJump()
    {
        frequency = 250f;
        frequencySlide = 0.05f; // Slide UP
        volume = 0.15f;
        decay = 0.999f;
        envelope = 1f;
        waveform = 0; // Square
    }

    public void PlayDeath()
    {
        frequency = 150f;
        frequencySlide = -0.01f; // Slide DOWN
        volume = 0.25f;
        decay = 0.999f;
        envelope = 1f;
        waveform = 2; // Noise
    }

    public void PlayWin()
    {
        frequency = 440f;
        frequencySlide = 0.01f; // Slide UP fast
        volume = 0.15f;
        decay = 0.9999f;
        envelope = 1f;
        waveform = 0; // Square
    }

    public void PlayError()
    {
        frequency = 100f;
        frequencySlide = -0.01f; // Slide DOWN
        volume = 0.2f;
        decay = 0.999f;
        envelope = 1f;
        waveform = 1; // Sawtooth
    }

    // BGM properties - A nice, relaxing melody (C major scale)
    private float[] bgmMelody = { 
        261.63f, 329.63f, 392.00f, 523.25f, // C E G C (C Major)
        392.00f, 493.88f, 587.33f, 783.99f, // G B D G (G Major)
        440.00f, 523.25f, 659.25f, 880.00f, // A C E A (A Minor)
        349.23f, 440.00f, 523.25f, 698.46f  // F A C F (F Major)
    };
    private int currentNoteIndex = 0;
    private float bgmPhase = 0f;
    private float bgmTime = 0f;
    private float noteDuration = 0.4f; // Slower, more relaxing tempo
    private float bgmVolume = 0.08f;

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            // --- SFX (Channel 1) ---
            float sfxVal = 0f;
            if (envelope > 0.001f)
            {
                float increment = frequency * 2f * Mathf.PI / sampleRate;
                phase += increment;
                
                frequency += frequencySlide;
                if (frequency < 0) frequency = 0;

                if (waveform == 0) sfxVal = Mathf.Sign(Mathf.Sin(phase)); // Square wave
                else if (waveform == 1) sfxVal = (phase % (2f * Mathf.PI)) / Mathf.PI - 1f; // Sawtooth
                else if (waveform == 2) sfxVal = (float)(rand.NextDouble() * 2.0 - 1.0); // Noise

                envelope *= decay;
                if (envelope < 0.001f) envelope = 0f;
                
                sfxVal = sfxVal * volume * envelope;
            }

            // --- BGM (Channel 2) ---
            bgmTime += 1f / sampleRate;
            if (bgmTime >= noteDuration)
            {
                bgmTime = 0f;
                currentNoteIndex = (currentNoteIndex + 1) % bgmMelody.Length;
            }

            // Calculate ADSR envelope for the BGM note to make it sound like plucking/soft synth
            float noteEnvelope = 1f;
            if (bgmTime < 0.05f) noteEnvelope = bgmTime / 0.05f; // Attack
            else noteEnvelope = 1f - ((bgmTime - 0.05f) / (noteDuration - 0.05f)); // Decay

            float currentBgmFreq = bgmMelody[currentNoteIndex];
            float bgmIncrement = currentBgmFreq * 2f * Mathf.PI / sampleRate;
            bgmPhase += bgmIncrement;
            
            // Soft sine wave for a beautiful, gentle background sound
            float bgmVal = Mathf.Sin(bgmPhase);
            bgmVal *= bgmVolume * noteEnvelope;

            // Combine SFX and BGM
            float finalOut = sfxVal + bgmVal;

            for (int c = 0; c < channels; c++)
            {
                data[i + c] = finalOut;
            }
        }
    }
}
