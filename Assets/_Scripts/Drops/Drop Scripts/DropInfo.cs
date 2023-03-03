using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropInfo
{
    public int droppableTypeID = 2000;
    public int quantity;
    [Range(0.0f, 1.0f)] public float dropProbability;

    public DropInfo(int DroppableTypeID, int Quantity, float DropProbability)
    {
        droppableTypeID = DroppableTypeID;
        quantity = Quantity;
        dropProbability = DropProbability;
    }
}
