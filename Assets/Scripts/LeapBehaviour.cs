using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using Leap.Unity;
using Leap;

public class LeapBehaviour : MonoBehaviour
{
    LeapProvider provider;
    public CycleMultiImages ci;
    public float LeapMultiplier = 10.0f;
    public float xCutoff = 0.01f;
    public float yCutoff = 0.01f;
    public float xDeadzone = 0.01f;
    public float yDeadzone = 0.01f;
    public AudioMixer a;
    public AudioMixerSnapshot normal;
    public AudioMixerSnapshot[] soundSnapshots;
    public float[] weights;
    public float mixerSpeed;
    private Vector2 startPos = new Vector2(0, 0.3f);
    private int leftRight;
    private int upDown;

    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        ci = GetComponent<CycleMultiImages>();
    }

    void Update()
    {
        
        Frame frame = provider.CurrentFrame;

        if (frame.Hands.Count > 0)
        {
            ci.hasUser = true;
            //if (startPos != Vector2.zero)
            //{
                Vector2 diff = new Vector2(frame.Hands[0].PalmPosition.x - startPos.x, frame.Hands[0].PalmPosition.y - startPos.y);

                if (Mathf.Abs(diff.x) > xDeadzone)
                {
                    if (diff.x < 0 && frame.Hands[0].PalmPosition.x < xCutoff)
                    {
                        //Debug.Log("left: " + diff.x);
                        ci.BlendX((frame.Hands[0].PalmPosition.x - xCutoff) * LeapMultiplier);
                        leftRight = 1;
                        foreach(AudioSource a in GetComponents<AudioSource>())
                        {
                           a.pitch = Mathf.Lerp(a.pitch, -1f, 0.05f);
                        }
                    }
                    else if (diff.x > 0 && frame.Hands[0].PalmPosition.x > xCutoff)
                    {
                        //Debug.Log("right: " + diff.x);
                        ci.BlendX((frame.Hands[0].PalmPosition.x - xCutoff) * LeapMultiplier);
                        leftRight = 2;
                        foreach (AudioSource a in GetComponents<AudioSource>())
                        {
                           a.pitch = Mathf.Lerp(a.pitch, 1f, 0.05f);
                        }
                    }
                    else
                    {
                        leftRight = 0;
                        foreach (AudioSource a in GetComponents<AudioSource>())
                        {
                            a.pitch = Mathf.Lerp(a.pitch, 1f, 0.05f);
                        }
                    }
                }

                if (Mathf.Abs(diff.y) > yDeadzone)
                {
                    if (diff.y > 0 && frame.Hands[0].PalmPosition.y > yCutoff)
                    {
                        //Debug.Log("up: " + diff.y);
                        ci.BlendY((frame.Hands[0].PalmPosition.y - yCutoff) * LeapMultiplier);
                        upDown = 1;
                    }
                    else if (diff.y < 0 && frame.Hands[0].PalmPosition.y < yCutoff)
                    {
                        //Debug.Log("down: " + diff.y);
                        ci.BlendY((frame.Hands[0].PalmPosition.y - yCutoff) * LeapMultiplier);
                        upDown = 2;
                    }
                    else
                    {
                        upDown = 0;
                    }
                }

                //if (frame.Hands[0].PalmPosition.y > yCutoff)
                //{
                //    ci.BlendY((frame.Hands[0].PalmPosition.y - yCutoff) * LeapMultiplier);
                //    falseColor.TransitionTo(2f);
                //}
                //else 
                //{
                //    ci.BlendY((frame.Hands[0].PalmPosition.y - yCutoff) * LeapMultiplier);
                //    trueColor.TransitionTo(2f);
                //}
                //if (frame.Hands[0].PalmPosition.x > xCutoff)
                //{
                //    ci.BlendX((frame.Hands[0].PalmPosition.x - xCutoff) * LeapMultiplier);
                //    fastTime.TransitionTo(2f);
                //}
                //if (frame.Hands[0].PalmPosition.x < xCutoff)
                //{
                //    ci.BlendX((frame.Hands[0].PalmPosition.x - xCutoff) * LeapMultiplier);
                //    slowTime.TransitionTo(2f);
                //}
            //}
            //else
            //{
            //    startPos = new Vector2(frame.Hands[0].PalmPosition.x, frame.Hands[0].PalmPosition.y);
            //    Debug.Log("start: " + startPos);
            //}
        }
        else// if (startPos != Vector2.zero)
        {
            //Debug.Log("RESET: " + startPos);
            //startPos = Vector2.zero;
            ci.hasUser = false;
            normal.TransitionTo(2f);
            leftRight = 0;
            upDown = 0;
            foreach (AudioSource a in GetComponents<AudioSource>())
            {
                a.pitch = 1f;
            }
        }

        BlendSound(leftRight, upDown);
       
    }

    public void BlendSound(int blendX,int blendY)
    {
        if(blendX == 0 && blendY == 1)
        {
            weights[0] = 1f;
            weights[1] = 0f;
            weights[2] = 0f;
            weights[3] = 0f;
            a.TransitionToSnapshots(soundSnapshots, weights, mixerSpeed);
        }
        else if(blendX == 0 && blendY == 2)
        {
            weights[0] = 0f;
            weights[1] = 1f;
            weights[2] = 0f;
            weights[3] = 0f;
            a.TransitionToSnapshots(soundSnapshots, weights, mixerSpeed);
        }
        else if(blendX == 1 && blendY == 0)
        {
            weights[0] = 0f;
            weights[1] = 0f;
            weights[2] = 1f;
            weights[3] = 0f;
            a.TransitionToSnapshots(soundSnapshots, weights, mixerSpeed);
        }
        else if(blendX == 2 && blendY == 0)
        {
            weights[0] = 0f;
            weights[1] = 0f;
            weights[2] = 0f;
            weights[3] = 1f;
            a.TransitionToSnapshots(soundSnapshots, weights, mixerSpeed);
        }
        else if(blendX == 1 && blendY == 1)
        {
            weights[0] = 0.5f;
            weights[1] = 0f;
            weights[2] = 0.5f;
            weights[3] = 0f;
            a.TransitionToSnapshots(soundSnapshots, weights, mixerSpeed);
        }
        else if(blendX == 2 && blendY == 1)
        {
            weights[0] = 0.5f;
            weights[1] = 0f;
            weights[2] = 0f;
            weights[3] = 0.5f;
            a.TransitionToSnapshots(soundSnapshots, weights, mixerSpeed);
        }
        else if(blendX == 1 && blendY == 2)
        {
            weights[0] = 0f;
            weights[1] = 0.5f;
            weights[2] = 0.5f;
            weights[3] = 0f;
            a.TransitionToSnapshots(soundSnapshots, weights, mixerSpeed);
        }
        else if(blendX == 2 && blendY == 2)
        {
            weights[0] = 0f;
            weights[1] = 0.5f;
            weights[2] = 0f;
            weights[3] = 0.5f;
            a.TransitionToSnapshots(soundSnapshots, weights, mixerSpeed);
        }
    }
}