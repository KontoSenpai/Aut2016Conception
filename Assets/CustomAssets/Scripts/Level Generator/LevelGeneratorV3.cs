using UnityEngine;
using System.Collections;

public class LevelGeneratorV3 : MonoBehaviour {

    public GameObject block;
    private int rows = 17;

    private int[][] gridIndexes;
    private int[] numberOfBlocks;

    // Use this for initialization
    void Start ()
    {
        gridIndexes = new int[20][];
        for (int i = 0; i < 20; i++)
            gridIndexes[i] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        GenerateRandoms();
        for( int i = 0; i < 17; i++)
            PlaceBlocks( i);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void PlaceBlocks( int index)
    {
        int xPos;
        for ( int i = 0; i <= numberOfBlocks[index]; i++)
        {
            int tries = 0;
            Vector3 futurePosition = new Vector3(0,0,0);
            do
            {
                xPos = Random.Range(0, 19);
                tries++;
            } while ( !TestCollisions( xPos, index) && tries < 19);

            if (tries < 19)
            {
                futurePosition = new Vector3(xPos, index, 0);
                GameObject placedBlock = Instantiate(block, futurePosition, transform.rotation) as GameObject;
                gridIndexes[xPos][index] = 1;
                gridIndexes[xPos][index + 1] = 1;
                gridIndexes[xPos][index + 2] = 1;
                gridIndexes[xPos][index + 3] = 1;
            }
            else
                i = 42;

        }
    }


    /** UTILS
    *
    */

    private void GenerateRandoms()
    {
        numberOfBlocks = new int[rows];
        numberOfBlocks[0] = Random.Range(1, 4);
        numberOfBlocks[1] = Random.Range(0, 5);
        numberOfBlocks[2] = Random.Range(0, 9);
        numberOfBlocks[3] = Random.Range(5, 8);
        numberOfBlocks[4] = Random.Range(5, 9);
        numberOfBlocks[5] = Random.Range(10, 12);
        numberOfBlocks[6] = Random.Range(5, 8);
        numberOfBlocks[7] = Random.Range(7, 12);
        numberOfBlocks[8] = Random.Range(5, 8);
        numberOfBlocks[9] = Random.Range(5, 8);
        numberOfBlocks[10] = Random.Range(5, 8);
        numberOfBlocks[11] = Random.Range(5, 8);
        numberOfBlocks[12] = Random.Range(5, 8);
        numberOfBlocks[13] = Random.Range(5, 8);
        numberOfBlocks[14] = Random.Range(5, 8);
        numberOfBlocks[15] = Random.Range(5, 8);
        numberOfBlocks[16] = Random.Range(5, 8);
    }

    private bool TestCollisions(int xPos, int yPos)
    {
        if (gridIndexes[xPos][yPos] == 0)
        {
            if (xPos == 0)
            {
                if (gridIndexes[xPos + 1][yPos + 1] == 0)
                    return true;
                else
                    return false;
            }
            else if (xPos == 19)
                if (gridIndexes[xPos - 1][yPos + 1] == 0)
                    return true;
                else
                    return false;
            else if (gridIndexes[xPos + 1][yPos + 1] == 0 && gridIndexes[xPos - 1][yPos + 1] == 0)
                return true;
            else
                return false;
        }
        else
            return false;
    }
}
