using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a button selecting a level to play
/// </summary>
public class LevelSelectButton : MonoBehaviour
{
    /// <summary>
    /// The name of the scene associated with this level
    /// </summary>
    [SerializeField]
    string sceneName;
    public string SceneName
    {
        get { return this.sceneName; }
    }

    /// <summary>
    /// Disable self if not yet unlocked
    /// </summary>
    void Awake()
    {
        if(!GameManager.instance.IsLevelUnlocked(this.sceneName)) {
            GetComponent<Image>().enabled = false;
            GetComponent<Button>().interactable = false;
        }
    }
}
