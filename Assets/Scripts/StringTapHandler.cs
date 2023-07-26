using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;  

public class StringTapHandler : MonoBehaviour
{
    [SerializeField]
    public AudioClip[] clip;
    [SerializeField]
    public Transform[] fretPoss;
    //highestx = 0 means no pressed
    public float highestX = -1;
    private int activeTouch = -1;

    private AudioSource AudioSrc;
    [SerializeField]
    private BoxCollider2D fretCollider;
    [SerializeField]
    private BoxCollider2D strumCollider;
    [SerializeField]
    private BoxCollider2D bendCollider;

    private LineRenderer LR;
    private float initY;
    private bool initalized = false;

    // Start is called before the first frame update
    void Start()
    {
        AudioSrc= GetComponent<AudioSource>();
        LR = GetComponent<LineRenderer>();
        HandleAspectRatios();
        StartCoroutine(a());
    }

    void HandleAspectRatios()
    {
        var pos = LR.GetPosition(2);
        var newx = (Camera.main.aspect) * Camera.main.orthographicSize;
        pos.x = -newx;
        LR.SetPosition(0, pos);
        pos.x = newx;
        LR.SetPosition(2, pos);
        LR.SetPosition(1, pos);

    }

    IEnumerator a()
    {
        yield return new WaitForSeconds(3);
        initY = LR.GetPosition(0).y;
        initalized= true;
    }

    // Update is called once per frame
    void Update()
    {
        checkTapAndUpdateActiveFret();
        handleLineRenBend();
        AdjustPitch();
        vibrateString();
        //CheckStrum();
    }
    private void vibrateString()
    {
        if (AudioSrc.isPlaying)
        {
            var p = LR.GetPosition(0);
            p.y += Mathf.Sin(Time.time*100)/50.0f;
            LR.SetPosition(0, p);
        }
        else if (initalized)
        {
            var p = LR.GetPosition(0);
            p.y = initY;
            LR.SetPosition(0, p);
        }
    }

    private void handleLineRenBend()
    {

        if (activeTouch >= 0)
        {
            var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(activeTouch).position);
            wp.z = 0;
            Debug.Log(activeTouch + " " + wp + " " + LR.positionCount);
            LR.SetPosition(1, wp);
        }
        else
        {
            LR.SetPosition(1, LR.GetPosition(2));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered Trigger");
        AudioSrc.Play();
    }



    void checkTapAndUpdateActiveFret()
    {
        
        
        for (int j = 0; j < Input.touchCount; j++)
        {
            var touch = Input.GetTouch(j);
            Vector3 wp = Camera.main.ScreenToWorldPoint(touch.position);
            //highestx = 0 means no pressed

            if (fretCollider.OverlapPoint(wp))
            {
                //highestx = 0 means no pressed
                highestX = 1;
                activeTouch = j;
                for (int i = 0; i < fretPoss.Length; i++)
                {
                    if (wp.x < fretPoss[i].position.x && highestX < (i+2))
                    {
                        highestX = i+2;
                        activeTouch = j;
                    }
                }

            }
        }

        var touchingFret = false;
        foreach (var t in Input.touches)
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(t.position);
            if (bendCollider.OverlapPoint(wp) && highestX>0)
                touchingFret = true;
        }

        if (!touchingFret)
            activeTouch = -1;

        if (Input.touchCount == 0 && activeTouch==-1)
        {
            highestX = 0;
        }

        if (clip.Length > 1)
        {

            if (highestX >= 5)
            {
                AudioSrc.clip = clip[1];
                highestX -= 5;
            }
            else
                AudioSrc.clip = clip[0];
        }
        
    }

    void AdjustPitch()
    {
        //AudioSrc.pitch = 1;
        //highestx = 0 means no pressed
        //Debug.Log(highestX);
        //Debug.Log(activeTouch);
        AudioSrc.pitch = Mathf.Pow(1.059463094f, (highestX));
        if (activeTouch >= 0)
        {
            var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(activeTouch).position);
            float delx = Mathf.Abs(fretPoss[0].position.y - wp.y);
            delx /= 1.5f;
            AudioSrc.pitch = AudioSrc.pitch * Mathf.Pow(1.059463094f, delx);
        }

    }

}
