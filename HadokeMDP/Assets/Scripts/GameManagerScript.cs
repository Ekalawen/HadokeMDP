using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public static float frameRate = 0.5f; // En millisecondes

    public PlayerScript player1;
    public PlayerScript player2;

    public Strategie strategie1;
    public Strategie strategie2;

    public bool isDisplayedOnScreen;


    float lastFrame;

	// Use this for initialization
	void Start () {
        lastFrame = Time.time;
        player1.setInitialPosition(3);
        player2.setInitialPosition(7);

        strategie1.Load();
        strategie2.Load();
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
            strategie1.Unload();
            strategie2.Unload();

            QuitGame();
        }
	}

    void applyRound()
    {
        MyInput m1 = strategie1.GetInput();
        MyInput m2 = strategie2.GetInput();
        Debug.Log("m1 = " + m1);
        Debug.Log("m2 = " + m2);

        player1.ApplyInput(m1, player2.position);
        player2.ApplyInput(m2, player1.position);
        Debug.Log("player1 = " + player1);
        Debug.Log("player2 = " + player2);

        bool player1IsDammaged = player1.ApplyDammage(player2);
        bool player2IsDammaged = player2.ApplyDammage(player1);
        if (player1IsDammaged)
            StartCoroutine(CSetDammage(player1));
        if (player2IsDammaged)
            StartCoroutine(CSetDammage(player2));

        if (isDisplayedOnScreen)
        {
            player1.Display();
            player2.Display();
        }
    }

    IEnumerator CSetDammage(PlayerScript player) {
        yield return new WaitForSeconds(0.20f);
        player.etat = PlayerScript.Etat.DAMMAGED;
        player.Display();
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
}
