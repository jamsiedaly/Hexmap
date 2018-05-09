using UnityEngine;

public class HexMapEditor : MonoBehaviour
{

    public Color[] colors;

    public HexGrid hexGrid;

    private Color activeColor;

    enum OptionalToggle
    {
        Ignore, Yes, No
    }

    OptionalToggle riverMode;

    bool isDrag;
    HexDirection dragDirection;
    HexCell previousCell;

    void Awake()
    {
        SelectColor(0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
        else {
            previousCell = null;
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            HexCell currentCell = hexGrid.GetCell(hit.point);
            if (previousCell && previousCell != currentCell)
            {
                ValidateDrag(currentCell);
            }
            else {
                isDrag = false;
            }
            EditCell(currentCell);
            previousCell = currentCell;
        }
        else {
            previousCell = null;
        }
    }

    int activeElevation;

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    void EditCell(HexCell cell)
    {
        if (cell.Elevation > 2 && cell.Elevation < 6) 
            cell.Color = activeColor;
        //cell.Elevation = activeElevation;
        if (riverMode == OptionalToggle.No)
        {
            cell.RemoveRiver();
        }
        else if (isDrag && riverMode == OptionalToggle.Yes)
        {
            HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
            if (otherCell)
            {
                otherCell.SetOutgoingRiver(dragDirection);
            }
        }
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }

    public void SetRiverMode(int mode)
    {
        riverMode = (OptionalToggle)mode;
    }

    void ValidateDrag(HexCell currentCell)
    {
        for (
            dragDirection = HexDirection.NE;
            dragDirection <= HexDirection.NW;
            dragDirection++
        )
        {
            if (previousCell.GetNeighbor(dragDirection) == currentCell)
            {
                isDrag = true;
                return;
            }
        }
        isDrag = false;
    }
}
