using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player's Stats")]
    public int health = 5;
    public int magCount;

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        print("Game is Over");
    }
}
