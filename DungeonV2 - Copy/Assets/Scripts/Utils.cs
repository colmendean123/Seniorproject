using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Utils
    {
        public static int Vertical, Horizontal, Columns, Rows;


        public static Vector3 GridToWorldPosition(int x, int y)
        {
            return new Vector3(y - (Horizontal), x - (Vertical));
        }

    }
}
