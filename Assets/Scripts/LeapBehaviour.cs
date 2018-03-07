using UnityEngine;
using System.Collections.Generic;
using Leap.Unity;
using Leap;

public class LeapBehaviour : MonoBehaviour
{
    LeapProvider provider;
    public CycleMultiImages ci;
    public float LeapMultiplier = 10.0f;
    public float yCutoff = 0.2f;

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

            if (frame.Hands[0].PalmPosition.y > yCutoff)
            {
                ci.BlendY((frame.Hands[0].PalmPosition.y - yCutoff) * LeapMultiplier);
            }
            else
            {
                ci.BlendY((frame.Hands[0].PalmPosition.y - yCutoff) * LeapMultiplier);
            }

            if (frame.Hands[0].PalmPosition.x > 0.0f)
            {
                ci.BlendX((frame.Hands[0].PalmPosition.x) * LeapMultiplier);
            }
            else
            {
                ci.BlendX((frame.Hands[0].PalmPosition.x) * LeapMultiplier);
            }
        } else
        {
            ci.hasUser = false;
        }
    }
}