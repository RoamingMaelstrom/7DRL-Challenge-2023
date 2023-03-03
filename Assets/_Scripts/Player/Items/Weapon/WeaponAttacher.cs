using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class WeaponAttacher : MonoBehaviour
{
    [SerializeField] GameObjectIntSOEvent createAndAttachWeaponEvent;
    [SerializeField] ObjectPoolMain objectPool;
    [SerializeField] WeaponConfigurer weaponConfigurer;


    private void Awake() {
        createAndAttachWeaponEvent.AddListener(SetupWeapon);
    }

    public void SetupWeapon(GameObject parentObject, int weaponTypeID)
    {
        Transform weaponContainer = parentObject.transform.GetChild(0).GetChild(0);

        GameObject weaponObject = objectPool.GetObject("Weapons");

        weaponObject.layer = weaponContainer.gameObject.layer;
        weaponObject.transform.parent = weaponContainer;

        weaponObject = weaponConfigurer.ConfigureWeapon(weaponObject, weaponTypeID);
    }
}
