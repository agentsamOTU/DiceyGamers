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
        Debug.Log(Roll());
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
            die2.ReTotal();
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
    public int Roll()
    {
        int total=0;
        float random = Random.Range(0.0f, die1.total);
        float running = 0;
        for(int i = 0; i<die1.sideOdds.Length;i++)
        {
            running += die1.sideOdds[i];
            if (random < running)
            {
                total += i + 1;
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
                total += i + 1;
                break;
            }
        }

        return total;
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
