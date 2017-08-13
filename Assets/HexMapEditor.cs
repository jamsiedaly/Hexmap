using UnityEngine;

public class HexMapEditor : MonoBehaviour
{

    public Color[] colors;

    public HexGrid hexGrid;

    private Color activeColor;

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
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            EditCell(hexGrid.GetCell(hit.point));
        }
    }

    int activeElevation;

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    void EditCell(HexCell cell)
    {
        cell.Color = activeColor;
//        cell.Elevation = activeElevation;
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }
}
