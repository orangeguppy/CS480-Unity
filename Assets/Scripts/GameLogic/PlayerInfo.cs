using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public static int Money;
    public static int Lives;

    [Header("Player stats")]
    public int startingMoney = 500;
    public int startingLives = 20;

    [Header("ui")]
    public TextMeshProUGUI moneyUI;
    public TextMeshProUGUI livesUI;


    void Start()
    {
        Money = startingMoney;
        Lives = startingLives;
    }

    // Update is called once per frame
    void Update()
    {
        moneyUI.text = "$" + Money.ToString();
        livesUI.text = Lives.ToString();
    }
}
