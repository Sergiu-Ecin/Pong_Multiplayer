using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class PongHUD : NetworkBehaviour
{
    #region Variables
    public TextMeshProUGUI Player1ScoreText;
    public TextMeshProUGUI Player2ScoreText;

    [SyncVar(hook = nameof(HandlePlayer1Score))]
    public int Player1Score = 0;

    [SyncVar(hook = nameof(HandlePlayer2Score))]
    public int Player2Score = 0;
    #endregion


    public void UpdateScore(int player)
    {
        if (player == 1)
        {
            Player1Score++;
        }
        else if (player == 2)
        {
            Player2Score++;
        }
    }


    void HandlePlayer1Score(int oldScore, int newScore)
    {
        Player1ScoreText.text = "Player 1 : " + newScore;
    }

    void HandlePlayer2Score(int oldScore, int newScore)
    {
        Player2ScoreText.text = "Player 2 : " + newScore;
    }

    
}
