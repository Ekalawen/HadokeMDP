using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public float frameRate = 0.001f; // En millisecondes
    public int nbMatchs;

    public PlayerScript player1;
    public PlayerScript player2;

    public Strategie strategie1;
    public Strategie strategie2;

    public bool isDisplayedOnScreen;


    float lastFrame;
    int nbMatchesDones = 0;

	// Use this for initialization
	void Start () {
        lastFrame = Time.time;

        strategie1.Load();
        strategie2.Load();

        player1.setInitialPosition(3);
        player2.setInitialPosition(7);

	}
	
	// Update is called once per frame
	void Update () {
        if(isDisplayedOnScreen)
        {
            if(isTimeNextFrame())
            {
                applyRound();
            }
        } else
        {
            applyRound();
        }

        if(isFinMatch())
        {
            nbMatchesDones++;
            ResetMatch();
            if (nbMatchesDones >= nbMatchs)
            {
                strategie1.Unload();
                strategie2.Unload();

                QuitGame();
            }
        }
	}

    void applyRound()
    {
        // Récupérer les inputs
        GameState oldState1 = GetGameState(GameState.PlayerNumber.PLAYER_1);
        GameState oldState2 = GetGameState(GameState.PlayerNumber.PLAYER_2);
        MyInput a1 = strategie1.GetInput(oldState1);
        MyInput a2 = strategie2.GetInput(oldState2);

        // Appliquer les inputs
        player1.ApplyInput(a1, player2.position);
        player2.ApplyInput(a2, player1.position);

        // Vérifier si un player a reçu un coup
        bool player1IsDammaged = player1.ApplyDammage(player2);
        bool player2IsDammaged = player2.ApplyDammage(player1);
        float r1 = computeReward(player1IsDammaged, player2IsDammaged);
        float r2 = -r1;

        // On défavorise les échanges de coups ! On veut que les deux joueurs ne prennent pas de coups !
        if(player1IsDammaged && player2IsDammaged) {
            r1 -= 0.01f;
            r2 -= 0.01f;
        }

        // Récompenser les stratégies
        GameState newState1 = GetGameState(GameState.PlayerNumber.PLAYER_1);
        GameState newState2 = GetGameState(GameState.PlayerNumber.PLAYER_2);
        strategie1.Reward(oldState1, a1, newState1, r1);
        strategie2.Reward(oldState2, a2, newState2, r2);

        // Afficher les résultats s'il le faut
        if (isDisplayedOnScreen)
        {
            player1.Display();
            player2.Display();
        }
    }

    float computeReward(bool player1IsDammaged, bool player2IsDammaged)
    {
        float r = 0; // Ce r est le r du joueur 1, r2 = -r
        if (player1IsDammaged) {
            StartCoroutine(CSetDammage(player1));
            r--;
            if(player1.isDead()) {
                r -= 9;
            }
        }
        if (player2IsDammaged) {
            StartCoroutine(CSetDammage(player2));
            r++;
            if(player2.isDead()) {
                r += 9;
            }
        }
        return r;
    }

    IEnumerator CSetDammage(PlayerScript player) {
        player.etat = PlayerScript.Etat.DAMMAGED;
        if (isDisplayedOnScreen)
        {
            yield return new WaitForSeconds(frameRate * 0.4f);
            player.Display();
        }
    }

    bool isTimeNextFrame()
    {
        float now = Time.time;
        if(now - lastFrame > frameRate)
        {
            lastFrame = now;
            return true;
        }
        return false;
    }

    bool isFinMatch()
    {
        return player1.isDead() || player2.isDead();
    }
    
    void QuitGame()
    {
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }

    GameState GetGameState(GameState.PlayerNumber playerNumber)
    {
        PlayerState p1State = player1.GetState();
        PlayerState p2State = player2.GetState();

        return new GameState(p1State, p2State, playerNumber);
    }

    void ResetMatch() {
        if(player2.isDead() && !player1.isDead()) {
            strategie1.RegisterVictory();
        } else {
            strategie1.RegisterDefeat();
        }
        if(player1.isDead() && !player2.isDead()) {
            strategie2.RegisterVictory();
        } else {
            strategie2.RegisterDefeat();
        }
        player1.setInitialPosition(3);
        player2.setInitialPosition(7);
        player1.pv = 10;
        player2.pv = 10;
        Debug.Log(nbMatchesDones + " matchs terminées !");
    }
}
