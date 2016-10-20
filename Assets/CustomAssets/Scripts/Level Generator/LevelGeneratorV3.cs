using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneratorV3 : MonoBehaviour {

    public GameObject block;
    public GameObject blockFull;
    public Sprite[] blockVariations;
    public Sprite[] blockFullVariations;

    public GameObject playerSpawn;
    public int nbPlayer;

    private int rows = 17;

    private int nombreHalf = 0;
    private int nombreFull = 0;

    private int[][] gridIndexes;
    private GameObject[][] gridBlocks;
    private int[] numberOfBlocks;

    // Use this for initialization
    void Start ()
    {
        gridIndexes = new int[20][];
        gridBlocks = new GameObject[20][];
        for (int i = 0; i < 20; i++)
        {
            gridIndexes[i] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            gridBlocks[i] = new GameObject[] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null};
        }
        GenerateRandoms();
        for( int i = 0; i < 17; i++)
            PlaceBlocks( i);
        SwapSprites();
        PlayerStarts();
    }

    private void PlaceBlocks( int index)
    {
        int xPos;
        for ( int i = 0; i <= numberOfBlocks[index]; i++)
        {
            int tries = 0;
            do
            {
                xPos = Random.Range(0, 19);
                tries++;
            } while ( !TestCollisions( xPos, index) && tries < 19);

            if (tries < 19)
            {
                ShapePlacement(xPos, index);
            }
            else
                i = 42;

        }
    }

    private void ShapePlacement( int xPos, int yPos)
    {
        float choices = Random.Range(0.0f, 2.0f);
        // Un bloc à droite
        if( choices <= 0.4 && yPos < 19 && gridIndexes[xPos + 1][yPos] == 0 )
        {
            BlockR(xPos, yPos);
        }
        // Un bloc dessus
        else if( choices > 0.4 && choices <= 0.8)
        {
            BlockU(xPos, yPos);
        }
        // Un bloc à gauche
        else if( ( choices > 0.8 && choices <= 1.2) && xPos >= 1 && gridIndexes[xPos - 1][yPos] == 0 )
        {
            BlockL(xPos, yPos);
        }
        // Un bloc à gauche et à droite
        else if( (choices > 1.2 && choices <= 1.5)  && ( xPos >= 1 && xPos < 19) && (gridIndexes[xPos - 1][yPos] == 0 && gridIndexes[xPos + 1][yPos] == 0))
        {
            BlockLR(xPos, yPos);
        }
        // Rien de spécial
        else
        {

        }

    }

    private void PlayerStarts()
    {
        List<string> potentials = new List<string>();
        for (int x = 0; x < gridIndexes.Length; x++)
        {
            for (int y = 0; y < gridIndexes[x].Length; y++)
            {
                if (gridIndexes[x][y] == 2)
                    potentials.Add(x + "-" + y);
            }
        }

        for (int player = 0; player < nbPlayer; player++)
        {
            int randomOfPotentials = Random.Range(0, potentials.Count);
            string kappaString = potentials[randomOfPotentials];
            string[] splittedString = kappaString.Split(new char[] { '-' });
            GameObject spawn = Instantiate(playerSpawn, new Vector3(IntParseFast(splittedString[0])+.5f, IntParseFast(splittedString[1]), 0), transform.rotation) as GameObject;
            spawn.GetComponent<SpawnPlayer>().Spawn(player+1);
        }
    }

    private void SwapSprites()
    {
        for (int x = 0; x < gridBlocks.Length; x++)
        {
            for (int y = 0; y < gridBlocks[x].Length; y++)
            {
                if (gridBlocks[x][y] != null && gridBlocks[x][y].name.Contains("Half"))
                    HandleHalfBlocks(x, y);
                else if (gridBlocks[x][y] != null && gridBlocks[x][y].name.Contains("Full"))
                    HandleFullBlocks(x, y);
            }
        }
    }
    /** UTILS
    *
    */

    private void BlockR(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(block, futurePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        Vector3 otherFuturePosition = new Vector3(xPos + 1, yPos, 0);
        gridBlocks[xPos + 1][yPos] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos + 1][yPos]);
        gridIndexes[xPos + 1][yPos] = 1;
        gridIndexes[xPos + 1][yPos + 1] = 2;
        gridIndexes[xPos + 1][yPos + 2] = 3;
        gridIndexes[xPos + 1][yPos + 3] = 3;
    }

    private void BlockL(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(block, futurePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        Vector3 otherFuturePosition = new Vector3(xPos - 1, yPos, 0);
        gridBlocks[xPos - 1][yPos] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos - 1][yPos]);
        gridIndexes[xPos - 1][yPos] = 1;
        gridIndexes[xPos - 1][yPos + 1] = 2;
        gridIndexes[xPos - 1][yPos + 2] = 3;
        gridIndexes[xPos - 1][yPos + 3] = 3;
    }
    
    private void BlockLR(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(block, futurePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        Vector3 otherFuturePosition = new Vector3(xPos + 1, yPos, 0);
        gridBlocks[xPos + 1][yPos] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos + 1][yPos]);
        gridIndexes[xPos + 1][yPos] = 1;
        gridIndexes[xPos + 1][yPos + 1] = 2;
        gridIndexes[xPos + 1][yPos + 2] = 3;
        gridIndexes[xPos + 1][yPos + 3] = 3;

        otherFuturePosition = new Vector3(xPos - 1, yPos, 0);
        gridBlocks[xPos - 1][yPos] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos - 1][yPos]);
        gridIndexes[xPos - 1][yPos] = 1;
        gridIndexes[xPos - 1][yPos + 1] = 2;
        gridIndexes[xPos - 1][yPos + 2] = 3;
        gridIndexes[xPos - 1][yPos + 3] = 3;
    }

    private void BlockU(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(blockFull, futurePosition, transform.rotation) as GameObject;
        Vector3 otherFuturePosition = new Vector3(xPos, yPos + 1, 0);
        gridBlocks[xPos][yPos + 1] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos][yPos + 1], gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 2;
        gridIndexes[xPos][yPos + 1] = 1;
        gridIndexes[xPos][yPos + 2] = 2;
        gridIndexes[xPos][yPos + 3] = 3;
        gridIndexes[xPos][yPos + 4] = 3;
    }

    private void Block(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(block, futurePosition, transform.rotation) as GameObject;
        NameBlocks(gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;
    }

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
        if (gridIndexes[xPos][yPos] == 0) // Sur la position
        {
            if (gridIndexes[xPos][yPos + 1] == 0)
                return true;
            else
                return false;
            /*
            if (xPos == 0) // Premiere colone
            {
                if (gridIndexes[xPos + 1][yPos + 1] == 0) // Haut-Droite
                    return true;
                else
                    return false;
            }
            else if (xPos == 19) // Dernière colone
            {
                if (gridIndexes[xPos - 1][yPos + 1] == 0)// Haut-Gauche
                    return true;
                else
                    return false;
            }
            else if (gridIndexes[xPos + 1][yPos + 1] == 0 && gridIndexes[xPos - 1][yPos + 1] == 0)
                return true;
            else
                return false;
                */
        }
        else
            return false;
        //return true;
    }

    private void HandleHalfBlocks(int x, int y)
    {
        if (x == 0)
        {
            if (y > 0)
            {
                if (gridBlocks[x + 1][y] == null && gridBlocks[x][y - 1] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[1];
                if (gridBlocks[x + 1][y] == null && gridBlocks[x][y - 1] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[4];
            }
        }
        else if (x == 19)
        {
            if (y > 0)
            {
                if (gridBlocks[x - 1][y] == null && gridBlocks[x][y - 1] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[0];
                if (gridBlocks[x - 1][y] == null && gridBlocks[x][y - 1] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[3];
            }
        }
        else
        {
            if (y > 0 && gridBlocks[x][y - 1] == null)
            {
                if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[0];
                else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[1];
                else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[2];
            }
            else if ( y == 0 || ( y > 0 && gridBlocks[x][y - 1] != null ))
            {
                if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[3];
                else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[4];
                else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockVariations[5];
            }

        }
    }

    private void HandleFullBlocks( int x, int y)
    {
        //Premiere colonne
        if (x == 0)
        {
            if (gridBlocks[x + 1][y] == null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[3];
            else
            {
                if(gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[4];
            }
        }
        //Dernière colonne
        else if (x == 19)
        {
            if (gridBlocks[x - 1][y] == null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[0];
            else
            {
                if (gridBlocks[x - 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[1];
            }
        }
        else
        {
            // Rien à droite, quelque chose à gauche
            if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] == null)
            {
                if( gridBlocks[x - 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[2];
                else
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[3];
            } 
            // Rien à gauche, quelque chose à droite  
            else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] != null)
            {
                if( gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[5];
                else
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[0];

            }
            // Quelque chose à gauche et à droite
            else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] != null)
            {
                if(gridBlocks[x - 1][y].name.Contains("Half") && gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[7];
                else if(gridBlocks[x - 1][y].name.Contains("Full") && gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[3];
                else if (gridBlocks[x - 1][y].name.Contains("Half") && gridBlocks[x + 1][y].name.Contains("Full"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[0];
            }
            //Rien à gauche et à droite
            else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] == null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blockFullVariations[6];
        }
    }

    private void NameBlocks( GameObject halfBlock)
    {
        halfBlock.name = "Half " + nombreHalf;
        nombreHalf++;
    }

    private void NameBlocks( GameObject halfBlock, GameObject fullBlock)
    {
        halfBlock.name = "Half " + nombreHalf;
        nombreHalf++;
        fullBlock.name = "Full " + nombreFull;
        nombreFull++;
    }

    private static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }
}
