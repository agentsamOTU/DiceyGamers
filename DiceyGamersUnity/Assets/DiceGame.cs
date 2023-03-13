using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DiceGame : MonoBehaviour
{
    string die1Path;
    string die2Path;
    DiceOdds die1;
    DiceOdds die2;
    // Start is called before the first frame update
    void Start()
    {
        LoadOrCreateFiles();

    }

    // Update is called once per frame
    void Update()
    {
        
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
            die1.Print();
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
            die1.Print();
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

    }
}

public class DiceOdds
{
    public float[] sideOdds;
    public DiceOdds()
    {
        sideOdds = new float[]
        {
            1.0f/6.0f,1.0f/6.0f,1.0f/6.0f,
            1.0f/6.0f,1.0f/6.0f,1.0f/6.0f
        };
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
}
