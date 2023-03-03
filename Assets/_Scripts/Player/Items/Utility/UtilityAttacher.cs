using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class UtilityAttacher : MonoBehaviour
{
    [SerializeField] GameObjectIntSOEvent createAndAttachUtilityEvent;
    [SerializeField] UpgradeInfoIntSOEvent applyUpgradeEvent;
    [SerializeField] ObjectPoolMain objectPool;
    [SerializeField] ItemProfileContainer utilityProfiles;

    private void Awake() 
    {
        createAndAttachUtilityEvent.AddListener(SetupUtility);
    }

    public void SetupUtility(GameObject parentObject, int utilityTypeID)
    {
        Transform weaponContainer = parentObject.transform.GetChild(0).GetChild(1);

        GameObject utilityObject = objectPool.GetObject("Utilities");

        ItemProfile profile = utilityProfiles.GetProfileWithID(utilityTypeID);

        ConfigureUtility(utilityObject, profile);

        utilityObject.layer = weaponContainer.gameObject.layer;
        utilityObject.transform.parent = weaponContainer;

        applyUpgradeEvent.Invoke(new UpgradeInfo(profile.itemPerks, profile.itemCost), -1);
    }

    void ConfigureUtility(GameObject utilityObject, ItemProfile profile)
    {

        Utility utilityScript = utilityObject.GetComponent<Utility>();
        utilityScript.itemName = profile.itemName;
        utilityScript.itemID = profile.itemID;
        utilityScript.itemLevel = 1;
    }
}
