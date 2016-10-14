using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneratorV2 : MonoBehaviour
{
    //Number of blocks to place
    public int numberOfShapes;

    // All the kind of blocks we want in the game
    public GameObject shape1;
    public GameObject shape2;
    public GameObject shape3;
    public GameObject shape4;
    public GameObject shape5;

    private List<GameObject> shapes;
    private List<GameObject> blocksToPlace;
    private int[][] gridIndexes;

    // Use this for initialization
    void Start ()
    {
        InstantiateElements();
        PlaceBlocks();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void InstantiateElements()
    {
        shapes = new List<GameObject>();
        shapes.Add(shape1);
        shapes.Add(shape2);
        shapes.Add(shape3);
        shapes.Add(shape4);
        shapes.Add(shape5);

        blocksToPlace = new List<GameObject>();
        for( int currentNumber = 0; currentNumber <= numberOfShapes; currentNumber++)
            blocksToPlace.Add( Instantiate( shapes[Random.Range(0,5)], gameObject.transform) as GameObject);

        gridIndexes = new int[21][];
        for (int i = 0; i < 21; i++)
            gridIndexes[i] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    }

    private void PlaceBlocks()
    {
        int randomBlock = Random.Range(0, blocksToPlace.Count);
        Vector3 blockPosition = new Vector3();
        do
        {
            blockPosition = new Vector3(Random.Range(0, 17), Random.Range(0, 15), 0);
        }
        while( gridIndexes[(int)blockPosition.x][(int)blockPosition.y] != 0);
        if( BlockFit( blocksToPlace[randomBlock], blockPosition))
        {
            FillGridIndexes(blocksToPlace[randomBlock], blockPosition);
            blocksToPlace[randomBlock].transform.position = blockPosition;
            blocksToPlace.RemoveAt(randomBlock);
        }
        else
        {
            GameObject rip = blocksToPlace[randomBlock];
            blocksToPlace.RemoveAt(randomBlock);
            Destroy(rip);

        }
        if(blocksToPlace.Count != 0)
            PlaceBlocks();
    }

    private void FillGridIndexes(GameObject block, Vector3 basePosition)
    {
        List<List<int>> positions = block.GetComponent<Shape>().GetCollision();
        for (int i = 0; i < positions.Count; i++)
        {
            List<int> grood = positions[i];
            for (int j = 0; j < grood.Count; j++)
            {
                gridIndexes[(int)basePosition.x + i][(int)basePosition.y + j] = grood[j];
            }
        }
    }


    private bool BlockFit(GameObject shape, Vector3 refVector)
    {
        print("2.Testing if the selected block fit");
        List<List<int>> platformCollisions = shape.GetComponent<Shape>().GetCollision();
        for (int i = 0; i < platformCollisions.Count; i++)
        {
            List<int> grood = platformCollisions[i];
            for (int j = 0; j < grood.Count; j++)
            {
                if ((gridIndexes[(int)refVector.x + i][(int)refVector.y + j]) == 1 || (gridIndexes[(int)refVector.x + i][(int)refVector.y + j]) == 2)
                {
                    print("2..It does not fit");
                    return false;
                }
            }
        }
        return true;
    }
}
