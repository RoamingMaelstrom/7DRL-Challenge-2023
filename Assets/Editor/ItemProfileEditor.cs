using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemProfile)), CanEditMultipleObjects]
public class ItemProfileEditor : Editor
{
     
    public SerializedProperty 
        itemID_Prop,
        itemName_Prop,
        itemDescription_Prop,
        itemCost_Prop,

        itemType_Prop,
        itemOffset_Prop,
        sprite_Prop,
        material_Prop,


        weaponType_Prop,
        damageDealerTypeID_Prop,
        baseMaxCd_Prop,

        accuracyCoefficient_Prop,
        targetingType_Prop,


        itemPerks_Prop,
        itemUpgrades_Prop;
     
    void OnEnable () {
        // Setup the SerializedProperties
        itemID_Prop = serializedObject.FindProperty ("itemID");
        itemName_Prop = serializedObject.FindProperty("itemName");
        itemDescription_Prop = serializedObject.FindProperty("itemDescription");
        itemCost_Prop = serializedObject.FindProperty("itemCost");

        itemType_Prop = serializedObject.FindProperty ("itemType"); 
        itemOffset_Prop = serializedObject.FindProperty("itemOffset");
        sprite_Prop = serializedObject.FindProperty("sprite");
        material_Prop = serializedObject.FindProperty("material"); 

        weaponType_Prop = serializedObject.FindProperty("weaponType");
        damageDealerTypeID_Prop = serializedObject.FindProperty("damageDealerTypeID");
        baseMaxCd_Prop = serializedObject.FindProperty("baseMaxCd");

        accuracyCoefficient_Prop = serializedObject.FindProperty("accuracyCoefficient");
        targetingType_Prop = serializedObject.FindProperty("targetingType");  
        
        itemPerks_Prop = serializedObject.FindProperty("itemPerks");
        itemUpgrades_Prop = serializedObject.FindProperty("itemUpgrades");
     
     }
     
     public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(itemID_Prop);         
        EditorGUILayout.PropertyField(itemName_Prop);
        EditorGUILayout.PropertyField(itemDescription_Prop);
        EditorGUILayout.PropertyField(itemCost_Prop);
        
        EditorGUILayout.PropertyField(itemOffset_Prop);
        EditorGUILayout.PropertyField(sprite_Prop);
        EditorGUILayout.PropertyField(material_Prop);
        EditorGUILayout.PropertyField(itemType_Prop);

         
        ItemType st = (ItemType)itemType_Prop.enumValueIndex;
         
        switch( st ) {
        case ItemType.WEAPON:            
            EditorGUILayout.PropertyField(weaponType_Prop, new GUIContent("Weapon Type"));
            EditorGUILayout.PropertyField(damageDealerTypeID_Prop, new GUIContent("Damage Dealer Type ID"));  
            EditorGUILayout.PropertyField(baseMaxCd_Prop, new GUIContent("Weapon Cooldown"));             
            EditorGUILayout.Slider(accuracyCoefficient_Prop, 0f, 1f, new GUIContent("Accuracy Coefficient"));             
            EditorGUILayout.PropertyField(targetingType_Prop, new GUIContent("Targeting Type"));         
            break;
 
        case ItemType.UTILITY:              
            break;    
        }
         

        EditorGUILayout.PropertyField(itemPerks_Prop);
        EditorGUILayout.PropertyField(itemUpgrades_Prop);
         
        serializedObject.ApplyModifiedProperties();
    }
}
