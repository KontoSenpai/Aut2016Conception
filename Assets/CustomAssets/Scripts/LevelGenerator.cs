using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

    // All the kind of blocks we want in the game
    public GameObject shape1;
    public GameObject shape2;
    public GameObject shape3;
    public GameObject shape4;
    public GameObject shape5;
    public GameObject shape6;

    private List<GameObject> shapes;
    // Number of shapes we want
    public int numberOfShapes;
    private int placedShapes;

    private int[][] indexes;
    private int currentLine;
    private bool levelCompleted;

	// Use this for initialization
	void Start ()
    {
        InitializeTables();
       CompleteLevel();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    private void InitializeTables()
    {
        shapes = new List<GameObject>();
        shapes.Add(shape1);
        shapes.Add(shape2);
        shapes.Add(shape3);
        /*shapes[3] = shape4;
        shapes[4] = shape5;
        shapes[5] = shape6;*/

        indexes = new int[19][];
        for( int i = 0; i < 19; i++)
            indexes[i] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }


    private void PlayerStarts()
    {

    }



    private bool CompleteLevel()
    {

        List<GameObject> blockTried;
        blockTried = new List<GameObject>();

        int desiredBlock = -1;

        bool good = false;
        desiredBlock = Random.Range(0, 2);
        //Si on à un bloc qui est bon ou pas
        /*while( !good)
        {
            for( int i = 0; i < blockTried.Count-1; i++)
            {
                if (blockTried[i] = shapes[desiredBlock])
                    good = false;
                else
                    good = true;
            }
        }*/
        blockTried.Add(shapes[desiredBlock]);

        if (placedShapes < numberOfShapes)
        {
            GameObject platform = Instantiate(shapes[desiredBlock]) as GameObject;
            platform.transform.position = new Vector3(Random.Range(2, 20), Random.Range(2, 20), 0);
            placedShapes++;
            CompleteLevel();
        }
        else
            return true;
        return false;
    }
}
