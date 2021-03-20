using UnityEngine;

public static class GridMapHelper
{
    public static string ToString(IGridMap map)
    {
        Vector2Int size = map.Size;
        string outStr = "";
        for (int y = size.y - 1; y >= 0; y--)
        {
            string roofStr = "";
            string centerStr = "";
            string bottomStr = "";
            for (int x = 0; x < size.x; x++)
            {
                Tile tile = map.GetTile(new Vector2Int(x, y));
                if (tile.GetWall(Directions.Direction.North))
                {
                    roofStr += "x..";
                }
                else
                {
                    roofStr += "x--";
                }
                if (tile.GetWall(Directions.Direction.West))
                {
                    centerStr += "...";
                }
                else
                {
                    centerStr += "|..";
                }

                if (y == 0)
                {
                    if (tile.GetWall(Directions.Direction.South))
                    {
                        bottomStr += "x..";
                    }
                    else
                    {
                        bottomStr += "x--";
                    }
                }
            }
            centerStr += "|";
            centerStr += '\n';

            roofStr += "x" + '\n';
            if (y == 0)
            {
                bottomStr += "x" + '\n';
            }
            outStr += roofStr + centerStr + bottomStr;
        }
        return outStr;
    }
}