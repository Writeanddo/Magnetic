using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// These are the buttons that take the player to a specific level
/// The name of the button represents the level and scene to show/take the player to
/// The component then updates it graphics based on the state (enabled/disabled)
/// and the level number it is
/// </summary>
public class LevelSelectButton : MonoBehaviour
{
    /// <summary>
    /// A list containing the graphics that represents each level
    /// Level counts starts at 1
    /// Item 0 is reserved for the "locked" graphic
    /// </summary>
    [SerializeField]
    List<Sprite> buttonSprites;

    /// <summary>
    /// The name of the scene associated with this level
    /// </summary>
    public string SceneName
    {
        get { return this.name; }
    }

    /// <summary>
    /// Extracts the level number from the scene name
    /// </summary>
    int levelNum = 0;
    int LevelNum
    {
        get {

            if(this.levelNum == 0) {
                string num = Regex.Match(this.SceneName, @"\d+").Value;

                if(!string.IsNullOrEmpty(num)) {
                    this.levelNum = int.Parse(num);
                }
            }
            
            return this.levelNum;
        }
    }

    /// <summary>
    /// Disable self if not yet unlocked
    /// </summary>
    void Awake()
    {
        if(!GameManager.instance.IsLevelUnlocked(this.SceneName)) {
            GetComponent<Image>().enabled = false;
            GetComponent<Button>().interactable = false;
            return;
        }

        // Updates the image displayed ("Image.sprite") to show the level is available
        // Updates the sprite states of this button to match the level number
        SpriteState states = new SpriteState();
        states.disabledSprite = this.buttonSprites[0];
        states.highlightedSprite = this.buttonSprites[this.LevelNum];
        states.pressedSprite = this.buttonSprites[this.LevelNum];

        GetComponent<Image>().sprite = this.buttonSprites[this.LevelNum];
        GetComponent<Button>().spriteState = states;
    }
}
