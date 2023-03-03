using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultDropProfile", menuName = "DropProfiles/DefaultDropProfile", order = 0)]
public class DropProfile : ScriptableObject
{
    public int droppableTypeID;
    public string droppableName;
    public Sprite sprite;

    public DroppableType droppableType;

    public float triggerRadius = 1;

    public string value1Destription;
    public float value1;
    public string value2Description;
    public float value2;

}


public enum DroppableType
{
    MONEY
}
