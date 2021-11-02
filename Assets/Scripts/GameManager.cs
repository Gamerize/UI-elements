using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int m_RowCount;
    private int m_ColumnCount;
    [SerializeField] private int m_NumberOfMines;

    private List<Vector2Int> m_MineLocations;
    private List<int> m_GridMap;
    private List<TileButton> m_TileButtons;
    private int m_NumberOfTilesLeft;

    [Header("Game Panel")]
    [SerializeField] private GameObject m_GameWindow;
    [SerializeField] private GameObject m_RowPanel;
    [SerializeField] private GameObject m_GameTile;

    [Header("Canvas")]
    [SerializeField] private GameObject m_StartGameCanvas;
    [SerializeField] private GameObject m_GameCanvas;
    [SerializeField] private GameObject m_EndGameCanvas;

    [Header("Input Field")]
    [SerializeField] private InputField m_InputFieldRowCount;
    [SerializeField] private InputField m_InputFieldColumnCount;

    private void Start()
    {
        //set it so that only the StartGameCanvas is on
        m_StartGameCanvas.SetActive(true);
        m_EndGameCanvas.SetActive(false);
        m_EndGameCanvas.SetActive(false);

        //Init the lists
        m_MineLocations = new List<Vector2Int>();
        m_GridMap = new List<int>();
        m_TileButtons = new List<TileButton>();

        //set the number of tiles left to 0
        m_NumberOfTilesLeft = 0;
    }

    private void CreateGameBoard()
    {
        //List used to make the game grid. This is used so that we can remove the locations to create mines
        List<Vector2Int> tempGrid = new List<Vector2Int>();

        //loop through the rows and columns to create the grid
        for(int i = 0; i < m_RowCount; i++)
        {
            for (int j = 0; i < m_ColumnCount; j++)
            {
                //add the x and y pos to the grid
                tempGrid.Add(new Vector2Int(j, i));
                m_GridMap.Add(0);
            }
        }

        //loop through the amount of the mines we need 
        for (int i = 0; i < m_NumberOfMines; i++)
        {
            //get a random number from 0 - the max amount of tiles
            int index = Random.Range(0, tempGrid.Count);
            //use the random number to get a location off the temp grid
            m_MineLocations.Add(tempGrid[index]);
            //remove the location from the temp grid so that we don't use it again
            tempGrid.RemoveAt(index);
        }

        //Create the Grid
        //loop through the row count
        for (int i = 0; i < m_RowCount; i++)
        {
            //create a new row gameObject
            GameObject tempRow = Instantiate(m_RowPanel);
            //set its parent to be the game window
            tempRow.transform.SetParent(m_GameWindow.transform, false);
            //loop through the colmuns for that row
            for(int j = 0; j < m_ColumnCount; j++)
            {
                //Create a new tile
                GameObject tempTile = Instantiate(m_GameTile);
                //set its Parent to be the row we just created
                tempTile.transform.SetParent(tempRow.transform, false);
                //get the TileButton script
                TileButton tempButton = tempTile.GetComponent<TileButton>();
                //Init the tile button and give it its location
                tempButton.Init(j, i);
                //assign the onClick event so tha6t we can use the button
                AddButtonListener(tempButton.m_Button, (i * m_ColumnCount) + j);
                //add the button to the tileButtonlist
                m_TileButtons.Add(tempButton);
                //increase the amount of tiles left
                m_NumberOfTilesLeft++;
            }
        }
    }

    private void AddButtonListener(Button button, int index)
    {
        button.onClick.AddListener(() => OnGameTileButtonDown(index));
    }

    private void CheckForEmptyuCells(int x, int y)
    {

    }

    private void GameOver(bool won)
    {
        m_GameCanvas.SetActive(false);
        m_EndGameCanvas.SetActive(true);
    }

#region UI Functions
    public void OnRowCountChanged()
    {
        int.TryParse(m_InputFieldRowCount.text, out m_RowCount);
    }

    public void OnColumnCountChanged()
    {
        int.TryParse(m_InputFieldRowCount.text, out m_ColumnCount);
    }

    public void StartGameButton()
    {
        //Turn the Start Canvas off and the Game Canvas on
        m_StartGameCanvas.SetActive(false);
        m_GameCanvas.SetActive(true);
    }

    private void OnGameTileButtonDown(int index)
    {
        //we try to activate the button if this returns true it means that the button hasn't been pressed before and it wasn't a mine
        if (m_TileButtons[index].ActivateButton(m_GridMap[index]))
        {
            //decrease the amounts of Tiles left
            m_NumberOfTilesLeft--;
            //start checking for empty tiles
            CheckforEmptyCells(m_TileButtons[index].m_XPos, m_TileButtons[index].m_YPos);
            if (m_NumberOfTilesLeft - m_NumberOfMines <= 0) ;
            {
                GameOver(true);
            }
        }
        else
        {
            //check to see if the player has hit a bomb
            if(m_GridMap[index] == -1)
            {
                GameOver(false);
            }
        }
    }
#endregion
}
