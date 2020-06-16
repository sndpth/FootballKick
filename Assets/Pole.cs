using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pole : MonoBehaviour
{
    public GameObject Arms;
    public Animator ArmsAnimation;
    public int Randomizer;
    public Color32 SelectedColor, UnslectedColor;
    public float TimeCounter,MaxCounter;
    public SpriteRenderer ArmSpriteRenderer1, ArmSpriteRenderer2;
    public Collider2D ArmCollider1, ArmCollider2;

    private float _randomChangerFloat;
    public static Pole Instance { get; set; }

    public static Pole GetInstance()
    {
        return Instance == null ? FindObjectOfType<Pole>() : Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchOrbs();
        ArmsAnimation.Play("PoleArms",0,Random.Range(0,10f));

    }

    // Update is called once per frame
   

    public void SwitchOrbs()
    {
        if (Arms.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color ==SelectedColor)
        {
//            Debug.Log("Set player image");
            ArmSpriteRenderer1.color = UnslectedColor;
            ArmCollider1.isTrigger = false;
            
            
            ArmSpriteRenderer2.color = SelectedColor;
            ArmCollider2.isTrigger = true;
        }
        else
        {
            ArmSpriteRenderer1.color = SelectedColor;
            ArmCollider1.isTrigger = true;
            
           ArmSpriteRenderer2.color = UnslectedColor;
           ArmCollider2.isTrigger = false;
        }
    }


    
}