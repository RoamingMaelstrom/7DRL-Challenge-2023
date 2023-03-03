using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class VendorManager : MonoBehaviour
{

    [SerializeField] Collider2DSOEvent spawnVendorEvent;
    [SerializeField] Collider2DCollider2DSOEvent enterVendorEvent;
    [SerializeField] GameObject vendorAreaPrefab;
    [SerializeField] List<GameObject> vendorShipPrefabs;
    [SerializeField] float baseDistanceFromPlayer;
    [SerializeField] [Range(0f, 1f)] float distanceRandomness;

    [SerializeField] Collider2D playerCollider;

    private void Start() 
    {
        if (enterVendorEvent) enterVendorEvent.AddListener(DestroyObjective);
        if (spawnVendorEvent) 
        {
            spawnVendorEvent.AddListener(CreateNewObjective);
            spawnVendorEvent.Invoke(playerCollider);
        }
    }

    // Creates a new target at a random distance from the player, and makes it a child of this object. Does dependency injection too.
    public void CreateNewObjective(Collider2D player)
    {
        float distanceFromPlayer = GetSpawnDistanceRelativeToPlayer(playerCollider);

        Vector2 objectiveDir = Random.insideUnitCircle.normalized;
        Vector3 objectivePos = player.transform.position + (new Vector3(objectiveDir.x, objectiveDir.y, 0) * distanceFromPlayer);
        GameObject newObjective = Instantiate(vendorAreaPrefab, objectivePos, Quaternion.identity, transform);

        VendorMain vendorMain = newObjective.GetComponent<VendorMain>();

        SpawnRandomVendorShip(newObjective.transform); 

    }

    private float GetSpawnDistanceRelativeToPlayer(Collider2D playerCollider)
    {
        return baseDistanceFromPlayer;
    }

    private void SpawnRandomVendorShip(Transform objectiveTransform)
    {
        GameObject npcShip = Instantiate(
         vendorShipPrefabs[Random.Range(0, vendorShipPrefabs.Count)],
         objectiveTransform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0),
         Quaternion.Euler(0, 0, Random.Range(0, 7) * 45),
         objectiveTransform
         );

        npcShip.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f) * Random.Range(0.7f, 0.9f);
    }

    // Wrapper method that ensures compatability with objectReachedEvent.
    public void DestroyObjective(Collider2D player, Collider2D objective)
    {
        DestroyObjective(objective.gameObject);
    }

    // Destroys the current target. Assumes that the objectives parent does not need to be destroyed.
    public void DestroyObjective(GameObject objective)
    {
        Destroy(objective);
    }


}
