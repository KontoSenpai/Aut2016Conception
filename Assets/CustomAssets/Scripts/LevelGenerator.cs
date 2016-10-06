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
        shapes.Add( shape1);
        shapes.Add( shape2);
        shapes.Add( shape3);
        shapes.Add( shape4);
        shapes.Add( shape5);
        //shapes[5] = shape6;

        indexes = new int[19][];
        for( int i = 0; i < 19; i++)
            indexes[i] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }


    private void PlayerStarts()
    {

    }

    private bool CompleteLevel()
    {
        // Know if the process of adding a block ended with 
        bool blockFit = false;
        bool rejected = false;

        //The position in the scene( and in the grid) requested
        Vector3 desiredPos = new Vector3();

        //Control of attempts to get the fitting block
        int desiredBlock = -1;
        List<int> indexTried = new List<int>();
        
        if (placedShapes < numberOfShapes)
        {
            do
            {
                rejected = false;
                do
                {
                    print("0.Creating a vector");
                    desiredPos = new Vector3(Random.Range(2, 17), Random.Range(5, 12), 0);
                }
                while (indexes[(int)desiredPos.x][(int)desiredPos.y] != 0);
                // Now we gonna attemp to insertBlocks in the grid and thus, world space
                do
                {
                    desiredBlock = GetBlockIndex(indexTried);
                    if (desiredBlock == -1)
                    {
                        print("0..I don't fit, no block left remaining");
                        rejected = true;
                    }
                    else
                    {
                        blockFit = BlockFit(shapes[desiredBlock], desiredPos);
                        if (!blockFit)
                        {
                            print("0..I don't fit, blocks can be still tried");
                            indexTried.Add(desiredBlock);
                        }
                    }
                }
                while (!blockFit || rejected);
            }
            while (rejected);
            placedShapes++;
            CompleteLevel();
        }
        else
            return true;
        return false;
    }

    /** Function to tell weither it's possible or not to place a block on the grid
    *
    * @Params shape : selected shape for the placement
    * @Params Ref : vector containin selection's root on the grid.
    */
    private bool BlockFit(GameObject shape, Vector3 refVector)
    {
        print("2.Testing if the selected block fit");
        GameObject platform = Instantiate(shape) as GameObject;
        List<List<int>> platformCollisions = platform.GetComponent<Shape>().GetCollision();
        for( int i = 0; i < platformCollisions.Count; i++)
        {
            List<int> grood = platformCollisions[i];
            for(int j = 0; j < grood.Count; j++)
            {
                if ( (indexes[(int)refVector.x + i][(int)refVector.y + j]) == 1 || (indexes[(int)refVector.x + i][(int)refVector.y + j]) == 2)
                {
                    print("2..It does not fit");
                    Destroy(platform);
                    return false;
                }
            }
        }
        FillTheGrid( platformCollisions, refVector);
        platform.transform.position = refVector;
        return true;
    }

    //UTILS
    //
    // Functions that help to structure and read the possibilities
    //
    //
    private int GetBlockIndex(List<int> alreadyTried)
    {
        print("1.Getting a random value");
        int desiredBlock = Random.Range(0, 4);
        List<int> toasted = new List<int>();
        bool rejected = false;

        do
        {
            if (alreadyTried.Count == 0)
            {
                print("1..No block was previously tested");
                return desiredBlock;
            }
            else
            {
                if (toasted.Count == alreadyTried.Count)
                {
                    print("1..Tried all possibilities, can't add at this position");
                    rejected = true;
                }
                else
                {
                    if (alreadyTried.IndexOf(desiredBlock) == -1)
                    {
                        print("1..No correspondance in already tested blocks");
                        return desiredBlock;
                    }
                    else
                    {
                        print("1..The block already has been tried, rerolling for a new value");
                        toasted.Add(desiredBlock);
                        desiredBlock = Random.Range(0, 4);
                    }
                }
            }
        }
        while (rejected == false);
        return -1;
    }

    private void FillTheGrid(List<List<int>> objPos, Vector3 refVector)
    {
        for (int i = 0; i < objPos.Count; i++)
        {
            List<int> grood = objPos[i];
            for (int j = 0; j < grood.Count; j++)
            {
                indexes[(int)refVector.x + i][(int)refVector.y + j] = grood[j];
            }
        }
    }


    private int Toast(List<int> alreadyTried)
    {
        return 0;
        /*
        else
        {
            if( alreadyTried.IndexOf( desiredBlock) == -1)
            {

            }
            do
            {
                for (int i = 0; i < alreadyTried.Count; i++)
                {
                    if (desiredBlock == alreadyTried[i])
                    {
                        print("1..Value already tested");
                        break;
                    }
                    else if (i == alreadyTried.Count)
                    {
                        print("1..No similar value was found");
                        return desiredBlock;
                    }
                }
                toasted.Add(desiredBlock);
                if (toasted.Count == alreadyTried.Count)
                {
                    print("1...Tried all possibilities, canceling to try a new possition on the grid");
                    noBlock = true;
                }
                else
                {
                    print("1...Trying a new number");
                    desiredBlock = Random.Range(0, 4);
                }
            }
            while (!noBlock);
        }
        return -1;*/
    }
}
