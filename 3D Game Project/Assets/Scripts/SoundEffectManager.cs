using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    
    public AudioSource BoomSF;
    public AudioSource CannonSF;
    public AudioSource ChickenSF;
    public AudioSource HitSF;

    // NOTE: Tick "Play on Awake" on AudioSource gameObject to play on start of the Game

    public void PlayBoomSF() {
        BoomSF.Play();
    }

    public void PlayCannonSF() {
        CannonSF.Play();
    }

    public void PlayChickenSF() {
        ChickenSF.Play();
    }

    public void PlayHitSF() {
        HitSF.Play();
    }



}
