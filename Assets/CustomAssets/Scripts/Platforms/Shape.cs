using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shape : MonoBehaviour
{
    public int shapeType;
    private int[][] collisions;
    private List<List<int>> gridX;
    private List<int> bords;
    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    private List<List<int>> Type1()
    {
        List<int> bords = new List<int>();
        bords.Add(2);
        bords.Add(2);
        gridX = new List<List<int>>();
        List<int> gridY = new List<int>();
        gridY.Add(1);
        for( int i = 0; i < 3; i++)
            gridY.Add(2);
        gridX.Add(bords);
        gridX.Add(gridY);
        gridX.Add(bords);
        return gridX;
    }

    private List<List<int>> Type2()
    {
        List<int> bords = new List<int>();
        bords.Add(2);
        bords.Add(2);
        gridX = new List<List<int>>();
        List<int> gridY = new List<int>();
        gridY.Add(1);
        gridX.Add(bords);
        for (int i = 0; i < 3; i++)
            gridY.Add(2);
        for (int i = 0; i < 2; i++)
            gridX.Add(gridY);
        gridX.Add(bords);
        return gridX;
    }

    private List<List<int>> Type3()
    {
        List<int> bords = new List<int>();
        bords.Add(2);
        bords.Add(2);
        gridX = new List<List<int>>();
        gridX.Add(bords);
        List<int> gridY = new List<int>();
        gridY.Add(1);
        for (int i = 0; i < 3; i++)
            gridY.Add(2);
        for (int i = 0; i < 3; i++)
            gridX.Add(gridY);
        gridX.Add(bords);
        return gridX;
    }

    private List<List<int>> Type4()
    {
        List<int> bords = new List<int>();
        bords.Add(2);
        bords.Add(2);
        gridX = new List<List<int>>();
        gridX.Add(bords);
        List<int> gridY1 = new List<int>();
        gridY1.Add(1);
        for( int i = 0; i < 3; i++)
        {
            gridY1.Add(2);
        }
        gridY1.Add(0);

        List<int> gridY2 = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            if( i < 2)
                gridY2.Add(1);
            else
                gridY2.Add(2);
        }

        List<int> gridY3 = new List<int>();
        gridY3 = gridY1;

        gridX.Add(gridY1);
        gridX.Add(gridY2);
        gridX.Add(gridY3);
        gridX.Add(bords);
        return gridX;
    }

    private List<List<int>> Type5()
    {
        List<int> bords = new List<int>();
        bords.Add(2);
        bords.Add(2);
        gridX = new List<List<int>>();
        List<int> gridY1 = new List<int>();
        gridY1.Add(0);
        gridY1.Add(1);
        for (int i = 0; i < 3; i++)
        {
            gridY1.Add(2);
        }
        gridY1.Add(0);
        List<int> gridY2 = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
                gridY2.Add(1);
            else
                gridY2.Add(2);
        }
        List<int> gridY3 = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            if (i == 0)
                gridY3.Add(0);
            else if (i < 5)
                gridY3.Add(1);
            else
                gridY3.Add(3);
        }
        gridX.Add(bords);
        gridX.Add(gridY1);
        gridX.Add(gridY2);
        gridX.Add(gridY3);
        gridX.Add(bords);
        return gridX;
    }

    private void DisplayContent()
    {
        for( int i = 0; i < gridX.Count; i++)
        {
            List<int> grood = gridX[i];
            for( int j = 0; j < grood.Count; j++)
            {
                print("LUL " + grood[j]);
            }
        }
    }

    public List<List<int>> GetCollision()
    {
        switch (shapeType)
        {
            case 0:
                return Type1();
            case 1:
                return Type2();
            case 2:
                return Type3();
            case 3:
                return Type4();
            case 4:
                return Type5();
        }
        return gridX;
    }
}
