using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class DespawnFarAwayEnemies : MonoBehaviour
{
    [SerializeField] GameObjectFloatSOEvent despawnEvent;
    public float farDistanceDespawn = 60f;

    List<GameObject> farDistanceEnemies = new List<GameObject>();

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.gameObject.activeInHierarchy) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

        // Handles REGULAR despawning enemies.
        if (other.transform.parent.GetComponent<EnemyDespawnType>().despawnType == DespawnType.REGULAR) 
         despawnEvent.Invoke(other.transform.parent.gameObject, 0);
        // Inserts enemy body into check for FAR despawn condition List.
        else if (other.transform.parent.GetComponent<EnemyDespawnType>().despawnType == DespawnType.FAR) farDistanceEnemies.Add(other.transform.parent.gameObject); 
    }

    private void FixedUpdate() {
        // Deals with FAR despawn type enemies.
        for (int i = farDistanceEnemies.Count - 1; i >= 0; i--)
        {
            if ((farDistanceEnemies[i].transform.position - transform.position).sqrMagnitude >= farDistanceDespawn * farDistanceDespawn) 
             despawnEvent.Invoke(farDistanceEnemies[i].transform.parent.gameObject, 0);
             farDistanceEnemies.RemoveAt(i);
        }
    }
}
