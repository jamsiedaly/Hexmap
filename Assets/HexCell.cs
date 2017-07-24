using UnityEngine;

public class HexCell : MonoBehaviour {

    public HexCoordinates coordinates;

    public RectTransform uiRect;

    public HexGridChunk chunk;

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform.localPosition = position;
            if (elevation <= 2)
            {
                Color color = new Color(0.2F, 0.2F, 0.8F, 0.9F);
                this.Color = color;
            }
            else if (elevation >= 7)
            {
                Color color = new Color(0.9F, 0.9F, 0.9F, 0.8F);
                this.Color = color;
            }
            else if (elevation >= 6)
            {
                Color color = new Color(0.4F, 0.6F, 0.5F, 0.8F);
                this.Color = color;
            }
            else
            {
                Color color = new Color(0.2F, 0.8F, 0.2F, 1.0F);
                this.Color = color;
            }
            //Vector3 uiPosition = uiRect.localPosition;
            //uiPosition.z = elevation * -HexMetrics.elevationStep;
            //GenuiRect.localPosition = uiPosition;

            Refresh();
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            if (color == value)
            {
                return;
            }
            color = value;
            Refresh();
        }
    }

    public Color color;

    void Refresh()
    {
        if (chunk)
        {
            chunk.Refresh();
            for (int i = 0; i < neighbors.Length; i++)
            {
                HexCell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != chunk)
                {
                    neighbor.chunk.Refresh();
                }
            }
        }
    }

    public int elevation;
    //int elevation = int.MinValue;

    [SerializeField]
    HexCell[] neighbors;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public HexEdgeType GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(
            elevation, neighbors[(int)direction].elevation
        );
    }

    public HexEdgeType GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(
            elevation, otherCell.elevation
        );
    }

}
