using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelGeneratorV3 : MonoBehaviour {

    public bool generateBlocks; // Know if blocks must be created or not
    public GameObject playerSpawn; // Prefab handling player generation
    public int nbPlayer = 2; // Number of players
    public int nbTraps = 10; // Number of traps in scene
    public GameObject trapObject; // Trap Styles
    public GameObject[] backgrounds; // Background Style

    public GameObject[] blocks; // half-block prefabs
    public GameObject[] blocksF; // block prefabs

    private GameObject block; // Half-blocks references
    private GameObject blockFull; // Blocks references

    private GameObject[] boundsRows = new GameObject[40];
    private GameObject[] boundsBottom = new GameObject[20];

    public Sprite[] blockForest; // Variantions for forest
    public Sprite[] blockFullForest; // Variations for forest

    private Sprite[] blocksV; // Half-block sprites references
    private Sprite[] blocksFullV; // Block sprites references


    private int rows = 17; // toal rows in level

    private int nombreHalf = 0; // Amount of half block placed
    private int nombreFull = 0; // Amount of full block placed

    private int[][] gridIndexes; // Table containing state of all cells
    private GameObject[][] gridBlocks; // Table containing all the level blocks references
    private int[] numberOfBlocks; // Block desired on each line

    // Use this for initialization
    void Start()
    {
        if(SceneManager.GetActiveScene().name.Contains("Forest"))
        {
            Instantiate(backgrounds[0], new Vector3(10, 9.5f, 0), transform.rotation);
            block = blocks[0];
            blockFull = blocksF[0];
            blocksV = blockForest;
            blocksFullV = blockFullForest;
        }
        CreateBounds();
        if( generateBlocks)
        {
            gridIndexes = new int[20][];
            gridBlocks = new GameObject[20][];
            for (int i = 0; i < 20; i++)
            {
                gridIndexes[i] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                gridBlocks[i] = new GameObject[] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            }
            GenerateRandoms();
            for (int i = 0; i < 17; i++)
                PlaceBlocks(i);
            SwapSprites();
        }
        SpawnTraps();
        PlayerStarts();
    }

    /**Function to create level boundaries
    *
    */
    private void CreateBounds()
    {
        int indy = 0;
        int indx = 0;
        // LEFT COLUMN
        for (int y = 0; y < 20; y++)
        {
            GameObject b = Instantiate(blockFull, new Vector3(-1, y, 0), transform.rotation) as GameObject;
            b.GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[3];
            boundsRows[indy] = b;
            indy++;
        }
        // RIGHT COLUMN
        for( int y = 0; y < 20; y++)
        {
            GameObject b = Instantiate(blockFull, new Vector3(20, y, 0), transform.rotation) as GameObject;
            b.GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[0];
            boundsRows[indy] = b;
            indy++;
        }
        //BOTTOM COLUMN
        for( int x = 0; x < 20; x++)
        {
            GameObject b = Instantiate(blockFull, new Vector3(x, -1, 0), transform.rotation) as GameObject;
            b.GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[0];
            boundsBottom[indx] = b;
            indx++;
        }
        GameObject bl = Instantiate(blockFull, new Vector3(-1, -1, 0), transform.rotation) as GameObject;
        bl.GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[8];
        bl = Instantiate(blockFull, new Vector3(20, -1, 0), transform.rotation) as GameObject;
        bl.GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[8];
    }

    /**Function to generate a blocks on a line of the level grid
    * @param index : y coordinate on the level
    */
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
            }while( !TestCollisions( xPos, index) && tries < 19);

            if (tries < 19)
                ShapePlacement(xPos, index);
            else
                i = 42;
        }
    }

    private void ShapePlacement( int xPos, int yPos)
    {
        float choices = Random.Range(0.0f, 2.0f);
        // Un bloc à droite
        if (choices <= 0.4 && yPos < 19 && gridIndexes[xPos + 1][yPos] == 0)
            BlockR(xPos, yPos);
        // Un bloc dessus
        else if (choices > 0.4 && choices <= 0.8)
            BlockU(xPos, yPos);
        // Un bloc à gauche
        else if ((choices > 0.8 && choices <= 1.2) && xPos >= 1 && gridIndexes[xPos - 1][yPos] == 0)
            BlockL(xPos, yPos);
        // Un bloc à gauche et à droite
        else if ((choices > 1.2 && choices <= 1.5) && (xPos >= 1 && xPos < 19) && (gridIndexes[xPos - 1][yPos] == 0 && gridIndexes[xPos + 1][yPos] == 0))
            BlockLR(xPos, yPos);
    }

    private void PlayerStarts()
    {
        GameObject spawns = new GameObject();
        spawns.name = "Spawns";
        if( generateBlocks)
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
                GameObject spawn = Instantiate(playerSpawn, new Vector3(IntParseFast(splittedString[0]) + .5f, IntParseFast(splittedString[1]), 0), transform.rotation) as GameObject;
                spawn.transform.parent = spawns.transform;
                spawn.GetComponent<SpawnPlayer>().Spawn(player + 1);
            }
        }
        else
        {
            for (int player = 0; player < nbPlayer; player++)
            {
                GameObject spawn = Instantiate(playerSpawn, new Vector3(player * 10 + 0.5f, player*10 + 0.5f, 0), transform.rotation) as GameObject;
                spawn.transform.parent = spawns.transform;
                spawn.GetComponent<SpawnPlayer>().Spawn(player + 1);
            }
        }

    }

    private void SpawnTraps()
    {
        GameObject traps = new GameObject();
        traps.name = "Traps";
        if (generateBlocks)
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
            for (int trap = 0; trap < nbTraps; trap++)
            {
                int randomOfPotentials = Random.Range(0, potentials.Count);
                string kappaString = potentials[randomOfPotentials];
                string[] splittedString = kappaString.Split(new char[] { '-' });
                GameObject t = Instantiate(trapObject, new Vector3(IntParseFast(splittedString[0]), IntParseFast(splittedString[1]) - 0.25f, 0), transform.rotation) as GameObject;
                t.transform.parent = traps.transform;
                gridIndexes[IntParseFast(splittedString[0])] [IntParseFast(splittedString[0])] = 3;
            }
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
        HandleBorder();
    }

    /* UTILS
    *
    */

    private void BlockR(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(block, futurePosition, transform.rotation) as GameObject;
        gridBlocks[xPos][yPos].transform.parent = transform;
        NameBlocks(gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        Vector3 otherFuturePosition = new Vector3(xPos + 1, yPos, 0);
        gridBlocks[xPos + 1][yPos] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos + 1][yPos].transform.parent = transform;
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
        gridBlocks[xPos][yPos].transform.parent = transform;
        NameBlocks(gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        Vector3 otherFuturePosition = new Vector3(xPos - 1, yPos, 0);
        gridBlocks[xPos - 1][yPos] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos - 1][yPos].transform.parent = transform;
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
        gridBlocks[xPos][yPos].transform.parent = transform;
        NameBlocks(gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        Vector3 otherFuturePosition = new Vector3(xPos + 1, yPos, 0);
        gridBlocks[xPos + 1][yPos] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos + 1][yPos].transform.parent = transform;
        NameBlocks(gridBlocks[xPos + 1][yPos]);
        gridIndexes[xPos + 1][yPos] = 1;
        gridIndexes[xPos + 1][yPos + 1] = 2;
        gridIndexes[xPos + 1][yPos + 2] = 3;
        gridIndexes[xPos + 1][yPos + 3] = 3;

        otherFuturePosition = new Vector3(xPos - 1, yPos, 0);
        gridBlocks[xPos - 1][yPos] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos - 1][yPos].transform.parent = transform;
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
        gridBlocks[xPos][yPos].transform.parent = transform;

        Vector3 otherFuturePosition = new Vector3(xPos, yPos + 1, 0);
        gridBlocks[xPos][yPos + 1] = Instantiate(block, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos][yPos + 1].transform.parent = transform;
        NameBlocks(gridBlocks[xPos][yPos + 1], gridBlocks[xPos][yPos]);
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 1;
        gridIndexes[xPos][yPos + 2] = 2;
        gridIndexes[xPos][yPos + 3] = 3;
        gridIndexes[xPos][yPos + 4] = 3;
    }

    private void Block(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(block, futurePosition, transform.rotation) as GameObject;
        gridBlocks[xPos][yPos].transform.parent = transform;
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
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[1];
                if (gridBlocks[x + 1][y] == null && gridBlocks[x][y - 1] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[4];
            }
        }
        else if (x == 19)
        {
            if (y > 0)
            {
                if (gridBlocks[x - 1][y] == null && gridBlocks[x][y - 1] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[0];
                if (gridBlocks[x - 1][y] == null && gridBlocks[x][y - 1] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[3];
            }
        }
        else
        {
            if (y > 0 && gridBlocks[x][y - 1] == null)
            {
                if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[0];
                else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[1];
                else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[2];
            }
            else if ( y == 0 || ( y > 0 && gridBlocks[x][y - 1] != null ))
            {
                if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[3];
                else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[4];
                else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[5];
            }

        }
    }

    private void HandleFullBlocks( int x, int y)
    {
        //Premiere colonne
        if (x == 0)
        {
            if (gridBlocks[x + 1][y] == null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[3];
            else
            {
                if(gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[4];
            }
        }
        //Dernière colonne
        else if (x == 19)
        {
            if (gridBlocks[x - 1][y] == null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[0];
            else
            {
                if (gridBlocks[x - 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[1];
            }
        }
        else
        {
            // Rien à droite, quelque chose à gauche
            if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] == null)
            {
                if( gridBlocks[x - 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[2];
                else
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[3];
            } 
            // Rien à gauche, quelque chose à droite  
            else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] != null)
            {
                if( gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[5];
                else
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[0];

            }
            // Quelque chose à gauche et à droite
            else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] != null)
            {
                if(gridBlocks[x - 1][y].name.Contains("Half") && gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[7];
                else if(gridBlocks[x - 1][y].name.Contains("Full") && gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[3];
                else if (gridBlocks[x - 1][y].name.Contains("Half") && gridBlocks[x + 1][y].name.Contains("Full"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[0];
            }
            //Rien à gauche et à droite
            else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] == null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[6];
        }
    }

    private void HandleBorder()
    {
        int yInt = 0;
        //ROWS
        for( int y = 0; y < boundsRows.Length; y++)
        {
            if( y < 20)
            {
                if(gridIndexes[0][y] == 1 && gridBlocks[0][y].transform.name.Contains("Half"))
                    boundsRows[y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[4];
                else if( gridIndexes[0][y] == 1 && gridBlocks[0][y].transform.name.Contains("Full"))
                    boundsRows[y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[8];
            }
            else
            {
                if (gridIndexes[19][yInt] == 1 && gridBlocks[19][yInt].transform.name.Contains("Half"))
                    boundsRows[y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[1];
                else if (gridIndexes[19][yInt] == 1 && gridBlocks[19][yInt].transform.name.Contains("Full"))
                    boundsRows[y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[8];
                yInt++;
            }
        }
        //BOTTOM
        for( int x = 0; x < boundsBottom.Length; x++)
        {
            if (gridIndexes[x][0] == 1)
                boundsBottom[x].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[8];
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
