//using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DiceGame : MonoBehaviour
{
    public static DiceGame instance { get; private set; }

    [SerializeField] private Texture[] diceSides;
    [SerializeField] private RawImage dice1Rend; //0 - 5 for the dice sides
    [SerializeField] private RawImage dice2Rend; //0 - 5 for the dice sides

    [SerializeField] private int dice1Value = 1;
    [SerializeField] private int dice2Value = 1;

    string die1Path;
    string die2Path;
    DiceOdds die1;
    DiceOdds die2;

    public GameObject winNotif;
    public GameObject loseNotif;

    public int pBet;
    public int pWincon;
    public int pCash = 2000;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuManager.Instance.pCash.text = pCash.ToString();
        LoadOrCreateFiles();
        diceSides = Resources.LoadAll<Texture>("DiceSides/");//loads in all 6 dice sides
    }

    // Update is called once per frame
    void Update()
    {
        dice1Rend.texture = diceSides[dice1Value - 1]; // updates dice 1 sprite
        dice2Rend.texture = diceSides[dice2Value -1];// updates dice 2 sprite
        //Debug.Log(Roll());
    }

    void LoadOrCreateFiles()
    {
        die1Path = Application.dataPath + Path.DirectorySeparatorChar + "die1.json";
        die2Path = Application.dataPath + Path.DirectorySeparatorChar + "die2.json";
        if (File.Exists(die1Path))
        {
            StreamReader sr = new StreamReader(die1Path);
            string temp = sr.ReadToEnd();
            die1 = JsonUtility.FromJson<DiceOdds>(temp);
            sr.Close();
            die1.ReTotal();
            //die1.Print();
        }
        else
        {
            die1 = new DiceOdds();
            string temp = JsonUtility.ToJson(die1);
            StreamWriter sw = new StreamWriter(die1Path);
            sw.Write(temp);
            sw.Close();
        }
        if (File.Exists(die2Path))
        {
            StreamReader sr = new StreamReader(die2Path);
            string temp = sr.ReadToEnd();
            die2 = JsonUtility.FromJson<DiceOdds>(temp);
            sr.Close();
            die2.ReTotal();
            //die1.Print();
        }
        else
        {
            die2 = new DiceOdds();
            string temp = JsonUtility.ToJson(die2);
            StreamWriter sw = new StreamWriter(die2Path);
            sw.Write(temp);
            sw.Close();
        }
    }
    public void Roll()
    {
        if (pBet == 0)
        {
            AudioManager.instance.PlayClip(0);

            float random = Random.Range(0.0f, die1.total);
            float running = 0;
            for (int i = 0; i < die1.sideOdds.Length; i++)
            {
                running += die1.sideOdds[i];
                if (random < running)
                {
                    dice1Value = i + 1;
                    break;
                }
            }

            random = Random.Range(0.0f, die2.total);
            running = 0;
            for (int i = 0; i < die2.sideOdds.Length; i++)
            {
                running += die2.sideOdds[i];
                if (random < running)
                {
                    dice2Value = i + 1;
                    break;
                }
            }

            WinCheck(dice2Value+dice1Value);
        }
    }
    public void WinCheck(int value)
    {
        switch (pWincon) 
        {
            case 0:
                if (value < 7) 
                {
                    StartCoroutine(Win());
                    pCash += pBet * 2;
                    MenuManager.Instance.pCash.text = pCash.ToString();
                }
                break;
                case 1:
                if (value == 7)
                {
                    StartCoroutine(Win());
                    pCash += pBet * 3;
                    MenuManager.Instance.pCash.text = pCash.ToString();
                }
                break;
                case 2:
                if (value > 7)
                {
                    StartCoroutine(Win());
                    pCash += pBet * 2;
                    MenuManager.Instance.pCash.text = pCash.ToString();
                }
                break;
                default:
                pBet = 0;
                MenuManager.Instance.pBet.text = "0";
                StartCoroutine(Lose());
                break;
        }
    }


    public IEnumerator Win()
    {
        winNotif.SetActive(true);
        AudioManager.instance.PlayClip(2);
        yield return new WaitForSeconds(1);
        winNotif.SetActive(false);
    }

    public IEnumerator Lose()
    {
        loseNotif.SetActive(true);
        AudioManager.instance.PlayClip(1);
        yield return new WaitForSeconds(1);
        loseNotif.SetActive(false);
    }
}

public class DiceOdds
{
    public float[] sideOdds;
    public float total=0;
    public DiceOdds()
    {
        sideOdds = new float[]
        {
            1.0f/6.0f,1.0f/6.0f,1.0f/6.0f,
            1.0f/6.0f,1.0f/6.0f,1.0f/6.0f
        };
        foreach(float f in sideOdds)
        {
            total+= f;
        }
    }
    public DiceOdds(float[] sideOdds)
    {
        this.sideOdds = sideOdds;
    }
    public void Print()
    {
        for(int i = 0; i<sideOdds.Length;i++)
        {
            Debug.Log("Side " + i + " Odds : " + sideOdds[i]);
        }
    }
    public void ReTotal()
    {
        total = 0;
        foreach(float f in sideOdds)
        {
            total += f;
        }
    }
}
