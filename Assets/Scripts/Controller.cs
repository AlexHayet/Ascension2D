using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Controller : NetworkBehaviour
{
    int pointWorth;
    public Slider progSlider;

    public GameObject player;
    public GameObject loadCanvas;
    public List<GameObject> levelList;
    private int currLevelIdx = 0;
    public GameObject gameOver;
    public static event Action GameRestart;

    void Start()
    {
        pointWorth = 0; // 0 points on spawn
        progSlider.value = 0; // 0 points in bar on spawn
        Gem.GemCollect += IncPoints; // Increase points on collection of coin
        LoadLevel.HoldCompletion += LoadNextLevel; // Loads next level when holding e
        PlayerHealth.Death += GameOver; // Game over will pop up once the player dies
        gameOver.SetActive(false); // Game over will not pop up until death
    }

    void IncPoints(int worth) // Increases points in progress bar when collecting coins and allows for going to next level when threshold is met
    {
        pointWorth += worth;
        progSlider.value += pointWorth;

        if (pointWorth >= 100)
        {
            loadCanvas.SetActive(true);
        }
    }

    void LoadLvl(int currLvl) // Loads necessary level (next level or level 1 on death)
    {
        loadCanvas.SetActive(false);

        levelList[currLevelIdx].gameObject.SetActive(false);
        levelList[currLvl].gameObject.SetActive(true);

        //player.transform.position = new Vector3(0, 0, 0);
        var playerObj = NetworkManager.Singleton.LocalClient.PlayerObject; // New position tracker for multiplayer that updates dynamically

        if (playerObj != null)
        {
            playerObj.transform.position = Vector3.zero;
        }

        currLevelIdx = currLvl;
        pointWorth = 0;
        progSlider.value = 0;
    }

    void LoadNextLevel() // Load the next level after collecting enough coins and holding button
    {
        int nextLevelIdx = (currLevelIdx == levelList.Count - 1) ? 0 : currLevelIdx + 1;
        LoadLvl(nextLevelIdx);
    }

    void GameOver() // Game over screen pops up when dead, also freezes time on death
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame() // Restarts the game when the restart button is clicked
    {
        gameOver.SetActive(false);
        LoadLvl(0);
        Time.timeScale = 1;
    }
}
