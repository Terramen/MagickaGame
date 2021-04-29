using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//Manages and shows scores of each player in the room
//Declares winner--> Player with the highest score

public class ScoreAddManager : MonoBehaviourPunCallbacks
{
    
    
    #region PUBLIC VARIABLES
    public Text playerA_NameText;
    public Text playerB_NameText;
    public Text playerA_score;
    public Text playerB_score;

    public GameObject winPanel;
    public Text winText;

    public const int ScoreForWin = 4; // If smone reach this number - this player win game

    public GameObject mainCamera;
    #endregion

    private PlayerDetails playerA = new PlayerDetails();
    private PlayerDetails playerB = new PlayerDetails();
    private int localPlayerScore = 0;
    private int playerA_Score = 0;
    private int playerB_Score = 0;
    void Start()
    {
        //Show names of active players on ScoreBoard
        SetPlayerNames();
    }
    void Update()
    {
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            playerA_Score = ScoreExtensions.GetScore(PhotonNetwork.PlayerList[0]);
            playerB_Score = ScoreExtensions.GetScore(PhotonNetwork.PlayerList[1]);

            playerA_score.text = playerA_Score.ToString();
            playerB_score.text = playerB_Score.ToString();
        
            if(playerA_Score > ScoreForWin || playerB_Score > ScoreForWin)
            {
                winPanel.SetActive(true);
                if (playerA_Score > playerB_Score)
                {
                    winText.text = $"Winner: {PhotonNetwork.PlayerList[1].NickName}";
                }
                else
                {
                    winText.text = $"Winner: {PhotonNetwork.PlayerList[0].NickName}";
                }

                ScoreExtensions.SetScore(PhotonNetwork.PlayerList[0], 0);
                ScoreExtensions.SetScore(PhotonNetwork.PlayerList[1], 0);
            }
        }

    }

    public void SetPlayerNames()
    {
        //Local Player 
        playerA_NameText.text = PhotonNetwork.PlayerList[0].NickName;

        //Other Player
        playerB_NameText.text = PhotonNetwork.PlayerList[1].NickName;
    }


    //Call this function when Answer is correctn 
    //& increase scores and display it
    public void UpdateScore(int value)
    {
        localPlayerScore = localPlayerScore + value;

        ScoreExtensions.SetScore(PhotonNetwork.LocalPlayer, localPlayerScore);

        playerA_Score = ScoreExtensions.GetScore(PhotonNetwork.PlayerList[0]);
        playerB_Score = ScoreExtensions.GetScore(PhotonNetwork.PlayerList[1]);

        playerA_score.text = playerA_Score.ToString();
        playerB_score.text = playerB_Score.ToString();

    }

    
    public void MenuScene()
    {
        mainCamera.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        
    }
    public const string PlayerScoreProp = "score";
    
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("MenuScene");
    }
}

public static class ScoreExtensions
{
    public static void SetScore(this Player player, int newScore)
    {
        Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
        score[PunPlayerScores.PlayerScoreProp] = newScore;

        player.SetCustomProperties(score);  // this locally sets the score and will sync it in-game asap.
    }

    public static void AddScore(this Player player, int scoreToAddToCurrent)
    {
        int current = player.GetScore();
        current = current + scoreToAddToCurrent;

        Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
        score[PunPlayerScores.PlayerScoreProp] = current;

        player.SetCustomProperties(score);  // this locally sets the score and will sync it in-game asap.
    }

    public static int GetScore(this Player player)
    {
        object score;
        if (player.CustomProperties.TryGetValue(PunPlayerScores.PlayerScoreProp, out score))
        {
            return (int)score;
        }

        return 0;
    }
}


[Serializable]
public class PlayerDetails
{
    public string PlayerName;
    public int Score;
    
}