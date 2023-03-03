using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class DropCreator : MonoBehaviour
{
    [SerializeField] GameObjectFloatSOEvent despawnEvent;
    [SerializeField] GameModifiers gameModifiers;
    [SerializeField] ObjectPoolMain objectPool;
    [SerializeField] public List<DropProfile> dropProfiles = new List<DropProfile>();

    [SerializeField] float internalTime = 0;
    [SerializeField] float internalProbabilityModifier = 1f; 

    private void Awake() 
    {
        despawnEvent.AddListener(CheckForDrop);
    }

    private void FixedUpdate() 
    {
        internalTime += Time.fixedDeltaTime;
        if (internalTime > 1000) internalProbabilityModifier = 0.75f;
    }

    public void CheckForDrop(GameObject despawningObject, float DROPFLAG)
    {
        if (DROPFLAG <= 0) return;
        if (!despawningObject.activeInHierarchy) return;
        List<int> itemIDsToDrop = new List<int>();
        DropItems objectDropItems = despawningObject.GetComponent<DropItems>();

        float randNum;

        foreach (var drop in objectDropItems.drops)
        {
            randNum = (Random.Range(0f, 1f) + gameModifiers.GetAdditionValueOfType(UpgradeType.DROP_PROBABILITY_CURRENT_GAME)) * 
             (gameModifiers.GetMultiplicationValueOfType(UpgradeType.DROP_PROBABILITY_CURRENT_GAME) / 2);
            if (randNum >= (1 / internalProbabilityModifier) - drop.dropProbability)
            {
                GenerateDrops(drop.droppableTypeID, drop.quantity, despawningObject.transform.GetChild(0).position);
            }
        }
    }

    public void GenerateDrops(int dropptableTypeID, int quantity, Vector3 spawnPos)
    {
        for (int i = 0; i < quantity; i++)
        {
            CreateAndConfigureDroppedItem(dropptableTypeID, spawnPos);
        }
    }

    public void CreateAndConfigureDroppedItem(int dropptableTypeID, Vector3 spawnPos)
    {
        GameObject droppableObject = objectPool.GetObject("Dropped Items");

        DropProfile profile = GetDropProfile(dropptableTypeID);

        DroppedItemMain droppedItemScript = droppableObject.GetComponent<DroppedItemMain>();
        droppedItemScript.droppableType = profile.droppableType;
        droppedItemScript.value1 = profile.value1;
        droppedItemScript.value2 = profile.value2;

        droppableObject.GetComponent<SpriteRenderer>().sprite = profile.sprite;
        droppableObject.GetComponent<CircleCollider2D>().radius = profile.triggerRadius;

        // If the game becomes too laggy, changing this rigidbody to static could reduce the lag.
        droppableObject.GetComponent<Rigidbody2D>().velocity = GetDropRandomVelocity();

        droppableObject.transform.position = spawnPos;

    }

    private Vector2 GetDropRandomVelocity()
    {
        return Random.insideUnitCircle;
    }

    private DropProfile GetDropProfile(int dropptableTypeID)
    {
        foreach (var profile in dropProfiles)
        {
            if (profile.droppableTypeID == dropptableTypeID) return profile;
        }

        throw new System.Exception(string.Format("Could not find drop with ID {0} in List of Drop Profiles", dropptableTypeID));
    }
}
