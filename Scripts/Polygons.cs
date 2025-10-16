using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Polygons
{
    private static readonly int[][,] polygons = new int[][,]
    {
        new int[,]
        {
            {0, 0, 0},
            {0, 1, 0},
            {0, 0, 0}
        },
        new int[,]
        {
            {0, 0, 0},
            {1, 1, 0},
            {0, 0, 0}
        },
        new int[,]
        {
            {0, 0, 0},
            {0, 1, 1},
            {0, 0, 0}
        },
        new int[,]
        {
            {0, 1, 0},
            {0, 1, 0},
            {0, 0, 0}
        },
        new int[,]
        {
            {0, 0, 0},
            {1, 1, 1},
            {0, 0, 0}
        },
        new int[,]
        {
            {0, 1, 0},
            {0, 1, 0},
            {0, 1, 0}
        },
        new int[,]
        {
            {1, 0, 0},
            {1, 0, 0},
            {1, 1, 0}
        },
        new int[,]
        {
            {0, 0, 1},
            {0, 0, 1},
            {0, 1, 1}
        },
        new int[,]
        {
            {1, 1, 1},
            {1, 0, 0},
            {1, 0, 0}
        },
        new int[,]
        {
            {1, 1, 1},
            {0, 0, 1},
            {0, 0, 1}
        },
        new int[,]
        {
            {0, 1, 1},
            {1, 1, 0},
            {0, 0, 0}
        },
        new int[,]
        {
            {1, 1, 0},
            {0, 1, 1},
            {0, 0, 0}
        },
        new int[,]
        {
            {0, 1, 0},
            {1, 1, 1},
            {0, 0, 0}
        },
        new int[,]
        {
            {0, 0, 0},
            {1, 1, 1},
            {0, 1, 0}
        },
        new int[,]
        {
            {0, 1, 0},
            {1, 1, 0},
            {0, 1, 0}
        },
        new int[,]
        {
            {0, 1, 0},
            {0, 1, 1},
            {0, 1, 0}
        },
        new int[,]
        {
            {0, 1, 1},
            {0, 1, 1},
            {0, 0, 0}
        },
        new int[,]
        {
            {1, 1, 0},
            {1, 1, 0},
            {0, 0, 0}
        },
        new int[,]
        {
            {0, 1, 0},
            {1, 1, 1},
            {0, 1, 0}
        }
    };

    static Polygons()
    {
        foreach (var polygon in polygons)
        {
            ReverseRows(polygon);
        }
    }
    public static int[,] Get(int index) => polygons[index];
    public static int Length => polygons.Length;
    
    private static void ReverseRows(int[,] polygon){
        var polygonRows = polygon.GetLength(0);
        var polygonColumns = polygon.GetLength(1);
        for (var r = 0; r < polygonRows / 2; r++)
        {
            var topRow = r;
            var bottomRow = polygonRows - r - 1;
            for (var c = 0; c < polygonColumns; c++)
            {
                (polygon[bottomRow, c], polygon[topRow, c]) = (polygon[topRow, c], polygon[bottomRow, c]);
            }
        }
    }
}
