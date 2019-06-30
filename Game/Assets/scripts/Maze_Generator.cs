using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze_Generator : MonoBehaviour
{
    // Referencing prefabs
    public GameObject Wall_Vertical;
    public GameObject Wall_Horizontal;
    public GameObject player;
    public GameObject obstacle;


    public int rows = 0;
    public int columns = 0;
    public List<Vector2> obstacles = new List<Vector2>();

    // Instantiating prefabs when the game starts
    void Start()
    {
        // Horizontal walls
        for(int i=0;i<=rows;i++)
        {
            if(i==0||i==rows)
            {
                for (int j = 0; j < 2 * columns; j++)
                {
                    if (j % 2 == 0)
                        Instantiate(Wall_Horizontal, new Vector3(6 * (j / 2), 0, 6 * i), Quaternion.identity);
                    else
                        Instantiate(Wall_Horizontal, new Vector3(6 * (j / 2) + 3, 0, 6 * i), Quaternion.identity);
                }

            }
            else
            {
                for (int j = 0; j < 2 * columns; j++)
                {
                    if (j % 2 == 0)
                        Instantiate(Wall_Horizontal, new Vector3(6 * (j / 2), 0, 6 * i), Quaternion.identity);
                    else
                        Instantiate(Wall_Horizontal, new Vector3(6 * (j / 2) + 4, 0, 6 * i), Quaternion.identity);
                }
            }

            
        }

        // Vertical walls
        
        for (int j = 0; j <= columns; j++)
        {
            if(j==0||j==columns)
            {
                for (int i = 0; i < rows * 2; i++)
                {
                    if (i % 2 == 0)
                        Instantiate(Wall_Vertical, new Vector3(6 * j - 1, 0, 6 * (i / 2) + 1), Quaternion.identity);
                    else
                        Instantiate(Wall_Vertical, new Vector3(6 * j - 1, 0, 6 * (i / 2) + 4), Quaternion.identity);
                }
            }
            else
            {
                for (int i = 0; i < rows * 2; i++)
                {
                    if (i % 2 == 0)
                        Instantiate(Wall_Vertical, new Vector3(6 * j - 1, 0, 6 * (i / 2) + 1), Quaternion.identity);
                    else
                        Instantiate(Wall_Vertical, new Vector3(6 * j - 1, 0, 6 * (i / 2) + 5), Quaternion.identity);
                }
            }
            
        }

        Instantiate(Wall_Vertical, new Vector3(6 * columns-1, 0, (6 * rows)-1), Quaternion.identity);


        // obstacles
        foreach ( Vector2 obst in obstacles)
        {
            Instantiate(obstacle, new Vector3(obst.y * 6 + 2,0, obst.x * 6 + 3), Quaternion.identity);
        }

        //camera
        if (rows >= columns)
            GameObject.Find("Main Camera").transform.position = new Vector3(columns * 3, rows * 6 + 20, rows * 3 + 3);
        else
            GameObject.Find("Main Camera").transform.position = new Vector3(columns * 3 + 3, columns * 6 + 20, rows * 3 + 3);
        
    }
}


