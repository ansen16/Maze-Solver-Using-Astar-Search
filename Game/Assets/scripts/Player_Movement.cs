using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// for sort order
public class V2Comparer : IComparer<Vector2>
{
    public int Compare(Vector2 left, Vector2 right)
    {
        var dif = left - right;
        if (dif.x == 0 && dif.y == 0)
            return 0;
        else if (dif.x == 0)
            return (int)Mathf.Sign(dif.y);
        return (int)Mathf.Sign(dif.x);
    }
}

public class Player_Movement : MonoBehaviour
{
    // Initialization 
    public Rigidbody rb;
    public GameObject tile;
    int rows;
    int columns;
    public Vector2 start;
    public Vector2 end;
    float TimeToGo;
    float Fraction_of_way_there;
    List<Vector2> Obstacles = new List<Vector2>();
    List<string> moves = new List<string>();
    bool flag = true;
    Vector3 started;
    

    // Heuristic Function
    int manhattan_dist(Vector2 a, Vector2 b)
    {
        int dist = (int)((a.x - b.x) + (a.y - b.y));
        return Mathf.Abs(dist);
    }

    // A STAR SEARCH 
    List<Vector2> AStarSearch(Vector2 start, Vector2 end)
    {
        List<Vector2> path = new List<Vector2>();
        Dictionary<Vector2, int> g_cost = new Dictionary<Vector2, int>();
        Dictionary<Vector2, int> f_cost = new Dictionary<Vector2, int>();

        g_cost[start] = 0;
        f_cost[start] = manhattan_dist(start, end);

        SortedSet<Vector2> closed = new SortedSet<Vector2>(new V2Comparer());
        SortedSet<Vector2> open = new SortedSet<Vector2>(new V2Comparer());

        open.Add(start);

        Dictionary<Vector2?, Vector2> came_from = new Dictionary<Vector2?, Vector2>();

        while (open.Count > 0)
        {
            Vector2? current = null;
            int temp_f_cost = 0;
            foreach (Vector2 point in open)
            {
                if (current == null || f_cost[point] < temp_f_cost)
                {
                    temp_f_cost = f_cost[point];
                    current = point;
                }
            }


            
            if (current == end)
            {
                path.Add((Vector2)current);

                //Add each step to path
                while (came_from.ContainsKey(current))
                {
                    current = came_from[current];
                    path.Add((Vector2)current);
                }
                path.Reverse();
                return path;
            }

            closed.Add((Vector2)current);
            open.Remove((Vector2)current);

            // add neighbours of current into open list
            List<Vector2> neighbours = new List<Vector2>();

            Vector2 temp = (Vector2) current;

            Vector2 up = new Vector2((int)temp.x, (int)temp.y + 1);
            Vector2 down = new Vector2((int)temp.x, (int)temp.y - 1);
            Vector2 left = new Vector2((int)temp.x - 1, (int)temp.y);
            Vector2 right = new Vector2((int)temp.x + 1, (int)temp.y);

                if (up.x >= 0 && up.x < columns && up.y >= 0 && up.y < rows)
                    neighbours.Add(up);
                if (down.x >= 0 && down.x < columns && down.y >= 0 && down.y < rows)
                    neighbours.Add(down);
                if (left.x >= 0 && left.x < columns && left.y >= 0 && left.y < rows)
                    neighbours.Add(left);
                if (right.x >= 0 && right.x < columns && right.y >= 0 && right.y < rows)
                    neighbours.Add(right);

                foreach (Vector2 neighbour in neighbours)
                {
      
                    if (!(closed.Contains(neighbour)))
                    {
                        int neighbour_gcost = 1;
                        if (Obstacles.Contains(neighbour))
                            neighbour_gcost = 1000;
                        neighbour_gcost += g_cost[temp];
                        if (!open.Contains(neighbour))
                        {
                            open.Add(neighbour);
                            came_from[neighbour] = temp;
                            g_cost[neighbour] = neighbour_gcost;
                            f_cost[neighbour] = g_cost[neighbour] + manhattan_dist(neighbour, end);
                        }   
                        else if (neighbour_gcost < g_cost[neighbour])
                        {
                            came_from[neighbour] = temp;
                            g_cost[neighbour] = neighbour_gcost;
                            f_cost[neighbour] = g_cost[neighbour] + manhattan_dist(neighbour, end);
                        }
                    }
                }
            
            
        }
        return path;
    }
    void CreateMoves(List<Vector2> path)
        {
            Vector2 prev = path[0];

            for (var i = 1; i < path.Count; i++)
            {

                if (path[i].x == prev.x)
                {
                    if (path[i].y > prev.y)
                    {
                        moves.Add("right");
                    }
                    else
                    {
                        moves.Add("left");
                    }
                }
                else
                {
                    if (path[i].x > prev.x)
                    {
                        moves.Add("up");
                    }
                    else
                    {
                        moves.Add("down");
                    }
                }
                prev = path[i];
            }
        }

    

        // Start is called before the first frame update
        void Start()
        {
            GameObject maze = GameObject.Find("Maze_Generator");
            Maze_Generator obj = maze.GetComponent<Maze_Generator>();
            Obstacles = obj.obstacles;
            rows = obj.rows;
            columns = obj.columns;

            List <Vector2> path = AStarSearch(start, end);

            CreateMoves(path);

            transform.position = new Vector3(start.y * 6 + 2,0, start.x * 6 + 3);

            TimeToGo = Time.fixedTime + 1.5f;

        }

        

        void Go_up()
        {
            Fraction_of_way_there += 1f;
            transform.position = Vector3.Lerp(started, started + new Vector3(0, 0, 6), Fraction_of_way_there);
            Instantiate(tile,started, Quaternion.identity);
            moves.RemoveAt(0);

        }
        void Go_down()
        {
            Fraction_of_way_there += 1f;
            transform.position = Vector3.Lerp(started, started + new Vector3(0, 0, -6), Fraction_of_way_there);
            Instantiate(tile, started, Quaternion.identity);
            moves.RemoveAt(0);
        }
        void Go_left()
        {
            Fraction_of_way_there += 1f;
            transform.position = Vector3.Lerp(started, started + new Vector3(-6, 0, 0), Fraction_of_way_there);
            Instantiate(tile, started, Quaternion.identity);
            moves.RemoveAt(0);
        }
        void Go_right()
        {
            Fraction_of_way_there += 1f;
            transform.position = Vector3.Lerp(started, started + new Vector3(6, 0, 0), Fraction_of_way_there);
            Instantiate(tile, started, Quaternion.identity);
            moves.RemoveAt(0);
        }


        // Update is called once per frame
        void Update()
        {
            if (Time.fixedTime >= TimeToGo)
            {
                if (flag)
                {
                    if (moves.Count == 0)
                    {
                        flag = false;
                    }
                    foreach (var name in moves.ToList())
                    {
                        started = transform.position;

                        if (name == "up")
                        {
                            Go_up();
                            break;
                        }
                        else if (name == "down")
                        {
                            Go_down();
                            break;
                        }
                        else if (name == "left")
                        {
                            Go_left();
                            break;
                        }
                        else if (name == "right")
                        {
                            Go_right();
                            break;
                        }


                    }
                    
                    Instantiate(tile,new Vector3(transform.position.x,0,transform.position.z), Quaternion.identity);

            }
                
                TimeToGo = Time.fixedTime + 1.0f;

            }
        }
        
    
}
