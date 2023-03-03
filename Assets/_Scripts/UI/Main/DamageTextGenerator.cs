using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using TMPro;

public class DamageTextGenerator : MonoBehaviour
{
    [SerializeField] GameObjectFloatSOEvent enemyDamageEvent;
    [SerializeField] GameObject textPrefab;

    private void Awake() 
    {
        enemyDamageEvent.AddListener(GenerateDamageText);
    }

    void GenerateDamageText(GameObject damagedObject, float damageValue)
    {
        if (!damagedObject.activeInHierarchy) return;

        string damageTextValue = ((int)damageValue).ToString();

        GameObject newText = Instantiate(textPrefab, damagedObject.transform.position, Quaternion.identity, transform);

        TextMesh textMesh = newText.transform.GetChild(0).GetComponent<TextMesh>();
        textMesh.text = damageTextValue;
        textMesh.fontSize = Mathf.Clamp(32 + (int)damageValue / 20, 32, 80);

        newText.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-4f, 4f), Random.Range(5f, 14f));
    }

    
}
