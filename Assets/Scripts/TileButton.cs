using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    [SerializeField] private Sprite m_MineSprite;
    public Button m_Button { get; private set; }
    public Text m_Text { get; private set; }
    public bool m_pressed { get; private set; }
    public int m_XPos { get; private set; }
    public int m_YPos { get; private set; }

    public void Init(int x, int y)
    {
        //set the x and y
        m_XPos = x;
        m_YPos = y;
        //set the game object's name to its position
        gameObject.name = x.ToString() + " - " + y.ToString();
        //get the reference to the button
        m_Button = GetComponent<Button>();
        //get reference to the text
        m_Text = GetComponentInChildren<Text>();
        //ensure the button knows it haven't been pressed
        m_pressed = false;
    }

    /// <summary>
    /// Return true if we need to look for empty spaces
    /// </summary>
    /// <param name="value">the value pulled from the grid map</param>
    public bool ActivateButton(int value)
    {
        if(!m_pressed)
        {
            if(value == -1)//If a bomb
            {
                //set the sprite to be a bomb
                m_Button.image.sprite = m_MineSprite;
                //set pressed to true
                m_pressed = true;
                return false;
            }
            else
            {
                //grey out the button
                m_Button.image.color = Color.grey;

                //if the button has a value display the text
                if(value > 0)
                {
                    m_Text.text = value.ToString();
                }

                m_pressed = true;
                return true;
            }
        }
        return false;
    }
}
