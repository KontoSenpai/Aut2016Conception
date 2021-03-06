﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelGeneratorV3 : MonoBehaviour {

    public bool generateBlocks; // Know if blocks must be created or not
    public GameObject playerSpawn; // Prefab handling player generation
    public GameObject[] playerObjects; // Prefab of players
    public GameObject pickupSpawn; // Prefab handling pickup generation
    public int nbPlayer = 2; // Number of players
    public int nbPickup = 2; // Number of PickUp
    public int nbTraps = 10; // Number of traps in scene
    public GameObject[] trapObject; // Trap Styles
    private List<GameObject> players;

    public GameObject blocks; // half-block prefabs
    public GameObject blocksF; // block prefabs

    private GameObject[] boundsRows = new GameObject[40];
    private GameObject[] boundsBottom = new GameObject[20];

    private int nombreHalf = 0; // Amount of half block placed
    private int nombreFull = 0; // Amount of full block placed
    public int rowNumber = 16;

    private int[][] gridIndexes; // Table containing state of all cells
    private GameObject[][] gridBlocks; // Table containing all the level blocks references
    private int[] numberOfBlocks; // Block desired on each line

    private GameObject blocksParent;
    private GameObject spawnsParent;
    private GameObject trapsParent;
    private GameObject pickupsParent;
    private int round = 1;

    SpriteChange kappa;

    void Awake()
    {
        players = new List<GameObject>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G) && round < 3)
        {
            round++;
            Refresh(round);
        }
        else if(Input.GetKeyDown(KeyCode.V))
        {
            Refresh(round);
        }
    }

    private IEnumerator ThreadItems(float time, int round)
    {
        if (round == 1)
            SpawnTraps(round);
        else
            SpawnDynamicTraps(round);
        yield return new WaitForSeconds(time);
        StartCoroutine(ThreadPlayers());
    }

    private IEnumerator ThreadPlayers()
    {
        PlayerStarts();
        yield return new WaitForSeconds(1);
        SpawnPickup();
    }

    public void Refresh(int round)
    {
        Destroy(blocksParent);
        Destroy(spawnsParent);
        Destroy(trapsParent);
        Destroy(pickupsParent);
        CreateContent(round);
        players.Clear();
    }

    /** Function called each round to generate a new layout
    *
    */
    public void CreateContent(int round)
    {
        kappa = GetComponent<SpriteChange>();
        kappa.SetSelection(round);
        CreateParents();
        CreateBounds(round);
        if (generateBlocks)
        {
            gridIndexes = new int[20][];
            gridBlocks = new GameObject[20][];
            for (int i = 0; i < 20; i++)
            {
                gridIndexes[i] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                gridBlocks[i] = new GameObject[] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null};
            }
            GenerateRandoms(rowNumber);
            for (int i = 0; i < rowNumber; i++)
                PlaceBlocks(i);
            GetComponent<SpriteChange>().ChangeSprites(gridBlocks, boundsRows, boundsBottom);
        }
        StartCoroutine(ThreadItems(1.5f, round));

		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		if (gameController != null)
        {
			gameController.GetComponent<HUD>().ResetTimer();
			gameController.GetComponent<GameController> ().RoundStart ();
		}
    }

    /**Function to create level boundaries
    *
    */
    private void CreateBounds(int round)
    {
        int indy = 0;
        SpriteChange kappa = GetComponent<SpriteChange>();
        // LEFT COLUMN
        for (int y = 0; y < 20; y++)
        {
            GameObject b = Instantiate(blocksF, new Vector3(-1, y, 0), transform.rotation) as GameObject;
            b.transform.parent = blocksParent.transform;
            if (round == 1)
                if (y >= 18)
                    b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteFull(8);
                else
                    b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteFull(3);
            else
                b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteFullSimple();
            boundsRows[indy] = b;
            indy++;
        }
        // RIGHT COLUMN
        for( int y = 0; y < 20; y++)
        {
            GameObject b = Instantiate(blocksF, new Vector3(20, y, 0), transform.rotation) as GameObject;
            b.transform.parent = blocksParent.transform;
            if( round == 1)
            {
                if(y >= 18)
                    b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteFull(8);
                else
                    b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteFull(0);
            }
            else
                b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteFullSimple();
            boundsRows[indy] = b;
            indy++;
        }
        //BOTTOM ROW
        for( int x = 0; x < 20; x++)
        {
            GameObject b = Instantiate(blocks, new Vector3(x, -0.5f, 0), transform.rotation) as GameObject;
            b.transform.parent = blocksParent.transform;
            if (round == 1)
                b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSprite(6);
            else
                b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteSimple();
            boundsBottom[x] = b;
        }
        //TOP ROW
        for (int x = 0; x < 20; x++)
        {
            GameObject b = Instantiate(blocksF, new Vector3(x, 18, 0), transform.rotation) as GameObject;
            GameObject b2 = Instantiate(blocks, new Vector3(x, 19f, 0), transform.rotation) as GameObject;
            b.transform.parent = blocksParent.transform;
            b2.transform.parent = blocksParent.transform;
            if (round == 1)
            {
                b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteFull(8);
                b2.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSprite(7);
            }
            else
            {
                b.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteFullSimple();
                b2.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteSimple();
            }
        }
        GameObject bl = Instantiate(blocks, new Vector3(-1, -0.5f, 0), transform.rotation) as GameObject;
        bl.transform.parent = blocksParent.transform;
        if (round == 1)
            bl.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSprite(7);
        else
            bl.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteSimple();
        bl = Instantiate(blocks, new Vector3(20, -0.5f, 0), transform.rotation) as GameObject;
        bl.transform.parent = blocksParent.transform;
        if (round == 1)
            bl.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSprite(7);
        else
            bl.GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSpriteSimple();
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

    /** Function to Determine which kind of prefab should be placed
    * @Param xPos : line on the grid
    * @Param yPos : row on the grid
    */
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

    /** Function that gonna place the players on a cell above a block
    *
    */
    private void PlayerStarts()
    {
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
                if( gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] != 4)
                {
                    GameObject spawn = Instantiate(playerSpawn, new Vector3(IntParseFast(splittedString[0]) + .5f, IntParseFast(splittedString[1]), 0), transform.rotation) as GameObject;
                    spawn.transform.parent = spawnsParent.transform;
                    players.Add(spawn.GetComponent<SpawnPlayer>().Spawn(playerObjects[player],player + 1));
                    gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] = 4;
                }
            }
        }
        else
        {
            for (int player = 0; player < nbPlayer; player++)
            {
                GameObject spawn = Instantiate(playerSpawn, new Vector3(player * 10 + 0.5f, player*10 + 0.5f, 0), transform.rotation) as GameObject;
                spawn.transform.parent = spawnsParent.transform;
				spawn.GetComponent<SpawnPlayer>().Spawn(playerObjects[player],player + 1);
            }
        }
    }

    /** Function that spawn pickups
    *
    */
    private void SpawnPickup()
    {
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
            for (int Pickup = 0; Pickup < nbPickup; Pickup++)
            {
                int randomOfPotentials = Random.Range(0, potentials.Count);
                string kappaString = potentials[randomOfPotentials];
                string[] splittedString = kappaString.Split(new char[] { '-' });
                if (gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] != 4)
                {
                    GameObject spawner = Instantiate(pickupSpawn, new Vector3(IntParseFast(splittedString[0]) + .5f, IntParseFast(splittedString[1]), 0), transform.rotation) as GameObject;
                    gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] = 4;
                    spawner.transform.parent = pickupsParent.transform;
                } 
            }
        }
    }

    /** Function that places traps on cellblocks
    *
    */
    private void SpawnTraps(int round)
    {
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
                GameObject t = Instantiate(trapObject[0], new Vector3(IntParseFast(splittedString[0]), IntParseFast(splittedString[1]) - 0.22f, 0), transform.rotation) as GameObject;
                t.transform.parent = trapsParent.transform;
                gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] = 3;
            }
        }
    }

    private void SpawnDynamicTraps(int round)
    {
        if (generateBlocks)
        {
            List<string> potentials = new List<string>();
            List<bool> faceLeft = new List<bool>();
            for (int x = 0; x < gridIndexes.Length; x++)
            {
                for (int y = 0; y < gridIndexes[x].Length; y++)
                {
                    if (gridIndexes[x][y] == 1)
                    {
                        if (round == 2)
                        {
                            if (y > 0 && gridIndexes[x][y - 1] != 1)
                                potentials.Add(x + "-" + y);
                        }
                        else if (round == 3)
                        {
                            if ( x > 1 && gridIndexes[x - 1][y] != 1 && gridIndexes[x - 2][y] != 1)
                            {
                                potentials.Add(x + "-" + y);
                                faceLeft.Add(true);
                            }
                            else if( x < 18 && gridIndexes[x + 1][y] != 1 && gridIndexes[x + 2][y] != 1)
                            {
                                potentials.Add(x + "-" + y);
                                faceLeft.Add(false);
                            }
                        }
                    }
                }
            }
            for (int trap = 0; trap < nbTraps; trap++)
            {
                int randomOfPotentials = Random.Range(0, potentials.Count);
                string kappaString = potentials[randomOfPotentials];
                string[] splittedString = kappaString.Split(new char[] { '-' });
                if (round == 2 && gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] != 4)
                {
                    GameObject t = Instantiate(trapObject[1], new Vector3(IntParseFast(splittedString[0]), IntParseFast(splittedString[1]) - 0.25f, 0), transform.rotation) as GameObject;
                    t.transform.parent = trapsParent.transform;
                    t.GetComponent<DynamicTrapSpawner>().InitializeCave();
                    gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] = 4;
                }
                else if (round == 3 && gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] != 4)
                {
                    GameObject t = Instantiate(trapObject[1], new Vector3(IntParseFast(splittedString[0]), IntParseFast(splittedString[1]), 0), transform.rotation) as GameObject;
                    t.transform.parent = trapsParent.transform;
                    t.GetComponent<DynamicTrapSpawner>().InitializeColiseum(faceLeft[randomOfPotentials]);
                    gridIndexes[IntParseFast(splittedString[0])][IntParseFast(splittedString[1])] = 4;
                }
            }
        }
    }

    /** Function that generate a certain amount of platforms to spawn in each lines
    *
    */
    private void GenerateRandoms(int rowNumber)
    {
        numberOfBlocks = new int[rowNumber];
        for( int i = 0; i < rowNumber; i++)
        {
            numberOfBlocks[i] = Random.Range(5, 8);
        }
    }

    /* GENERATION OF BLOCKS
    *
    */
    private void BlockR(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(blocks, futurePosition, transform.rotation) as GameObject;
        gridBlocks[xPos][yPos].transform.parent = blocksParent.transform;
        
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        NameBlocks(gridBlocks[xPos][yPos]);

        Vector3 otherFuturePosition = new Vector3(xPos + 1, yPos, 0);
        gridBlocks[xPos + 1][yPos] = Instantiate(blocks, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos + 1][yPos].transform.parent = blocksParent.transform;
        
        gridIndexes[xPos + 1][yPos] = 1;
        gridIndexes[xPos + 1][yPos + 1] = 2;
        gridIndexes[xPos + 1][yPos + 2] = 3;
        gridIndexes[xPos + 1][yPos + 3] = 3;

        NameBlocks(gridBlocks[xPos + 1][yPos]);
    }

    private void BlockL(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(blocks, futurePosition, transform.rotation) as GameObject;
        gridBlocks[xPos][yPos].transform.parent = blocksParent.transform;
        
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        NameBlocks(gridBlocks[xPos][yPos]);

        Vector3 otherFuturePosition = new Vector3(xPos - 1, yPos, 0);
        gridBlocks[xPos - 1][yPos] = Instantiate(blocks, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos - 1][yPos].transform.parent = blocksParent.transform;

        gridIndexes[xPos - 1][yPos] = 1;
        gridIndexes[xPos - 1][yPos + 1] = 2;
        gridIndexes[xPos - 1][yPos + 2] = 3;
        gridIndexes[xPos - 1][yPos + 3] = 3;

        NameBlocks(gridBlocks[xPos - 1][yPos]);
    }
    
    private void BlockLR(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(blocks, futurePosition, transform.rotation) as GameObject;
        gridBlocks[xPos][yPos].transform.parent = blocksParent.transform;

        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 2;
        gridIndexes[xPos][yPos + 2] = 3;
        gridIndexes[xPos][yPos + 3] = 3;

        gridBlocks[xPos][yPos].GetComponentInChildren<SpriteRenderer>().sprite = kappa.GetSprite(6);
        NameBlocks(gridBlocks[xPos][yPos]);

        Vector3 otherFuturePosition = new Vector3(xPos + 1, yPos, 0);
        gridBlocks[xPos + 1][yPos] = Instantiate(blocks, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos + 1][yPos].transform.parent = blocksParent.transform;
       
        gridIndexes[xPos + 1][yPos] = 1;
        gridIndexes[xPos + 1][yPos + 1] = 2;
        gridIndexes[xPos + 1][yPos + 2] = 3;
        gridIndexes[xPos + 1][yPos + 3] = 3;

        NameBlocks(gridBlocks[xPos + 1][yPos]);

        otherFuturePosition = new Vector3(xPos - 1, yPos, 0);
        gridBlocks[xPos - 1][yPos] = Instantiate(blocks, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos - 1][yPos].transform.parent = blocksParent.transform;

        gridIndexes[xPos - 1][yPos] = 1;
        gridIndexes[xPos - 1][yPos + 1] = 2;
        gridIndexes[xPos - 1][yPos + 2] = 3;
        gridIndexes[xPos - 1][yPos + 3] = 3;

        NameBlocks(gridBlocks[xPos - 1][yPos]);
    }

    private void BlockU(int xPos, int yPos)
    {
        Vector3 futurePosition = new Vector3(xPos, yPos, 0);
        gridBlocks[xPos][yPos] = Instantiate(blocksF, futurePosition, transform.rotation) as GameObject;
        gridBlocks[xPos][yPos].transform.parent = blocksParent.transform;
       
        Vector3 otherFuturePosition = new Vector3(xPos, yPos + 1, 0);
        gridBlocks[xPos][yPos + 1] = Instantiate(blocks, otherFuturePosition, transform.rotation) as GameObject;
        gridBlocks[xPos][yPos + 1].transform.parent = blocksParent.transform;
        
        gridIndexes[xPos][yPos] = 1;
        gridIndexes[xPos][yPos + 1] = 1;
        gridIndexes[xPos][yPos + 2] = 2;
        gridIndexes[xPos][yPos + 3] = 3;
        gridIndexes[xPos][yPos + 4] = 3;

        NameBlocks(gridBlocks[xPos][yPos + 1], gridBlocks[xPos][yPos]);
    }

    /** Function that ensure that no blocks overlaps
    * @Param : xPos : line of the tested block
    * @Param : yPos : row of the tested block
    */
    private bool TestCollisions(int xPos, int yPos)
    {
        if (gridIndexes[xPos][yPos] == 0) // Sur la position
        {
            if (gridIndexes[xPos][yPos + 1] == 0)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    /** Function that change the name of the block to make easier the sprite change
    * @Param halfblock : Object needing to be renamed
    */
    private void NameBlocks( GameObject halfBlock)
    {
        halfBlock.name = "Half " + nombreHalf;
        nombreHalf++;
    }

    /** Function that change the name of the blocks to make easier the sprite change
    * @Param halfblock : Object needing to be renamed
    * @Param fullblock : Object needing to be renamed
    */
    private void NameBlocks( GameObject halfBlock, GameObject fullBlock)
    {
        halfBlock.name = "Half " + nombreHalf;
        nombreHalf++;
        fullBlock.name = "Full " + nombreFull;
        nombreFull++;
    }

    private void CreateParents()
    {
        blocksParent = new GameObject();
        blocksParent.name = "Blocks";
        blocksParent.transform.parent = transform;
        spawnsParent = new GameObject();
        spawnsParent.name = "Spawns";
        spawnsParent.transform.parent = transform;
        trapsParent = new GameObject();
        trapsParent.name = "Traps";
        trapsParent.transform.parent = transform;
        pickupsParent = new GameObject();
        pickupsParent.name = "Pickups";
        pickupsParent.transform.parent = transform;
    }

    public List<GameObject> GetPlayers() { return players; }

    /** Fast and efficient way to parse
    * @Param value : desired string to convert to an int
    * @Return result : int parsed from value
    */
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
