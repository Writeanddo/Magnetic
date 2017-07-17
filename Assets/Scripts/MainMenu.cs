using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to allow buttons to trigger the singleton instance of the gamemanager
/// </summary>
public class MainMenu : MonoBehaviour
{
	public void StartGame()
    {
        GameManager.instance.StartGame();
    }

    public void Exit()
    {
        GameManager.instance.Exit();
    }
}
