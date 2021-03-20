using UnityEngine;
public class GoalTileComponent : MonoBehaviour, ITileComponent
{
    [SerializeField]
    GoalTile tile;
    public void SetTile(Tile tile)
    {
        this.tile = tile as GoalTile;
        if (this.tile != null)
        {
            CorrectRotation();
        }
    }

    void CorrectRotation()
    {
        float zRotation = 0;
        switch (tile.Direction)
        {
            case Directions.Direction.North:
                zRotation = 0;
                break;
            case Directions.Direction.South:
                zRotation = 180;
                break;
            case Directions.Direction.West:
                zRotation = 270;
                break;
            case Directions.Direction.East:
                zRotation = 90;
                break;
            default:
                throw new System.Exception("Unknown Direction");
        }
        transform.Rotate(new Vector3(0, 0, 1), zRotation);
        
    }
}