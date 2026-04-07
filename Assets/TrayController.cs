using System.Collections.Generic;
using UnityEngine;

public class TrayController : MonoBehaviour
{
    public int rows = 4;
    public int columns = 7;
    public float spacing = 0.2f;

    private List<Transform> slots = new List<Transform>();
    private int index = 0;

    void Start()
    {
        GenerateSlots();
    }

    void GenerateSlots()
    {
        slots.Clear();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 pos = transform.position + new Vector3(c * spacing, 0, r * spacing);

                GameObject slot = new GameObject("Slot_" + slots.Count);
                slot.transform.position = pos;
                slot.transform.parent = transform;

                slots.Add(slot.transform);
            }
        }
    }

    public Vector3 GetNextSlotPosition()
    {
        if (index >= slots.Count)
            return transform.position;

        return slots[index].position;
    }

    public void PlaceObject(PickableObject obj)
    {
        if (index >= slots.Count) return;

        obj.transform.position = slots[index].position;
        obj.transform.SetParent(slots[index]);

        index++;
    }
}