using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public static int Money;
    public static int Lives;
    public static int LessonScore;
    public static int EndlessScore;
    public int HighScore;

    [Header("Player stats")]
    public int startingMoney = 500;
    public int startingLives = 20;
    private int startingScore = 0;

    [Header("ui")]
    public TextMeshProUGUI moneyUI;
    public TextMeshProUGUI livesUI;


    void Start()
    {
        Money = startingMoney;
        Lives = startingLives;
        LessonScore = startingScore;
        EndlessScore = startingScore;

        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        moneyUI.text = "$" + Money.ToString();
        livesUI.text = Lives.ToString();
        // Debug.Log(EndlessScore);
    }
}
