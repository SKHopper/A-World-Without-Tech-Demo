using System.Collections;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

//fullscreen uGUI flash effect
//lerps image between first colour and transparent colour
public class DamageEffect : MonoBehaviour
{
    Image parentImage;

    float startTime;
    float duration = 0.25f;
    //duration/2 for formulae
    float halfLength;
    UnityEngine.Color oldRGB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentImage = GetComponent<Image>();
        halfLength = duration/2;
        oldRGB = parentImage.color;
    }

    // Update is called once per frame
    void Update() {}

    //setup and begin animation
    public void StartEffect()
    {
        startTime = Time.time;
        StartCoroutine(RunAnimation());
    }

    //
    //out opacity = y, max opacity = c1, duration = c2 = 0.25, x = progress
    // y = c1 - m(x - c2/2)^2
    //out = max - 16(progress - duration/2)^2
    IEnumerator RunAnimation()
    {
        float progress = 0;
        float outOpacity;

        while (progress <= duration)
        {
            progress = Time.time - startTime;
            
            outOpacity = 1f - 16f * (progress - halfLength) * (progress - halfLength);
            parentImage.color = new UnityEngine.Color(oldRGB.r, oldRGB.g, oldRGB.b, outOpacity);
            yield return new WaitForEndOfFrame();
        }
        parentImage.color = oldRGB;
    }
}
