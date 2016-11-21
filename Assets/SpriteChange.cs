using UnityEngine;
using System.Collections;

public class SpriteChange : MonoBehaviour {

    public GameObject[] backgrounds; // background image

    public Sprite[] blockForest; // Variantions for forest
    public Sprite[] blockFullForest; // Variations for forest
    public Sprite[] blockCave; // Variantions for forest
    public Sprite[] blockFullCave; // Variantions for forest
    public Sprite[] blockColiseum; // Variantions for forest
    public Sprite[] blockFullColiseum; // Variantions for forest

    private Sprite[] blocksV;
    private Sprite[] blocksFullV;

    private GameObject bg;
    private GameObject[][] gridBlocks;

    void Start()
    {
        bg= new GameObject();
    }

    public void SetSelection(int round)
    {
        Destroy(bg);
        bg = new GameObject();
        bg.name = "Background";
        bg.transform.parent = transform;
        if(round == 1)
        {
            GameObject background = Instantiate(backgrounds[0], new Vector3(10, 9.5f, 0), transform.rotation) as GameObject;
            background.transform.parent = bg.transform;
            blocksV = blockForest;
            blocksFullV = blockFullForest;
        }
        else if(round == 2)
        {
            GameObject background = Instantiate(backgrounds[1], new Vector3(10, 9.5f, 0), transform.rotation) as GameObject;
            background.transform.parent = bg.transform;
            blocksV = blockCave;
            blocksFullV = blockFullCave;
        }
        else if(round == 3)
        {
            GameObject background = Instantiate(backgrounds[2], new Vector3(10, 9.5f, 0), transform.rotation) as GameObject;
            background.transform.parent = bg.transform;
            blocksV = blockColiseum;
            blocksFullV = blockFullColiseum;
        }
    }

    public void ChangeSprites(GameObject[][] grid, GameObject[] rows, GameObject[] bottom)
    {
        gridBlocks = grid;
        for (int x = 0; x < gridBlocks.Length; x++)
        {
            for (int y = 0; y < gridBlocks[x].Length; y++)
            {
                if (gridBlocks[x][y] != null && gridBlocks[x][y].name.Contains("Half"))
                    ChangeHalf(x, y);
                else if (gridBlocks[x][y] != null && gridBlocks[x][y].name.Contains("Full"))
                    ChangeFull(x, y);
            }
        }
        HandleBorder(rows, bottom);
    }

    /** Function change the sprite of the platform on the requested cell
* @Param : xPos : line of the tested block
* @Param : yPos : row of the tested block
*/
    private void ChangeHalf(int x, int y)
    {
        if (x == 0)
        {
            if (y > 0)
            {
                if (gridBlocks[x + 1][y] == null && gridBlocks[x][y - 1] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[1];
                else if (gridBlocks[x + 1][y] == null && gridBlocks[x][y - 1] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[4];
            }
            if (gridBlocks[x + 1][y] != null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[6];
        }
        else if (x == 19)
        {
            if (y > 0)
            {
                if (gridBlocks[x - 1][y] == null && gridBlocks[x][y - 1] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[0];
                else if (gridBlocks[x - 1][y] == null && gridBlocks[x][y - 1] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[3];
            }
            if (gridBlocks[x - 1][y] != null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[6];
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
                else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[6];
            }
            else if (y == 0 || (y > 0 && gridBlocks[x][y - 1] != null))
            {
                if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[3];
                else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[4];
                else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] == null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[5];
                else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] != null)
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[6];
            }

        }
    }

    /** Function change the sprite of the platform on the requested cell
    * @Param : xPos : line of the tested block
    * @Param : yPos : row of the tested block
    */
    private void ChangeFull(int x, int y)
    {
        //Premiere colonne
        if (x == 0)
        {
            if (gridBlocks[x + 1][y] == null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[3];
            else
            {
                if (gridBlocks[x + 1][y].name.Contains("Half"))
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
                if (gridBlocks[x - 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[2];
                else
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[3];
            }
            // Rien à gauche, quelque chose à droite  
            else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] != null)
            {
                if (gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[5];
                else
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[0];

            }
            // Quelque chose à gauche et à droite
            else if (gridBlocks[x - 1][y] != null && gridBlocks[x + 1][y] != null)
            {
                if (gridBlocks[x - 1][y].name.Contains("Half") && gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[7];
                else if (gridBlocks[x - 1][y].name.Contains("Full") && gridBlocks[x + 1][y].name.Contains("Half"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[3];
                else if (gridBlocks[x - 1][y].name.Contains("Half") && gridBlocks[x + 1][y].name.Contains("Full"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[0];
                else if (gridBlocks[x - 1][y].name.Contains("Full") && gridBlocks[x + 1][y].name.Contains("Full"))
                    gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[8];
            }
            //Rien à gauche et à droite
            else if (gridBlocks[x - 1][y] == null && gridBlocks[x + 1][y] == null)
                gridBlocks[x][y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[6];
        }
    }

     /** Function that will change the border sprites
     *
     */
    private void HandleBorder(GameObject[] boundsRows, GameObject[] boundsBottom)
    {
        int yInt = 0;
        //ROWS
        for (int y = 0; y < boundsRows.Length; y++)
        {
            if (y < 20)
            {
                if (gridBlocks[0][y] != null && gridBlocks[0][y].transform.name.Contains("Half"))
                    boundsRows[y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[4];
                else if (gridBlocks[0][y] != null && gridBlocks[0][y].transform.name.Contains("Full"))
                    boundsRows[y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[8];
            }
            else
            {
                if (gridBlocks[19][yInt] != null && gridBlocks[19][yInt].transform.name.Contains("Half"))
                    boundsRows[y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[1];
                else if (gridBlocks[19][yInt] != null && gridBlocks[19][yInt].transform.name.Contains("Full"))
                    boundsRows[y].GetComponentInChildren<SpriteRenderer>().sprite = blocksFullV[8];
                yInt++;
            }
        }
        //BOTTOM
        for (int x = 0; x < boundsBottom.Length; x++)
        {
            if ( x < 20 && gridBlocks[x][0] != null)
                boundsBottom[x].GetComponentInChildren<SpriteRenderer>().sprite = blocksV[7];
        }
    }

    public Sprite GetSprite(int index) { return blocksV[index]; }
    public Sprite GetSpriteFull(int index) { return blocksFullV[index]; }
}
