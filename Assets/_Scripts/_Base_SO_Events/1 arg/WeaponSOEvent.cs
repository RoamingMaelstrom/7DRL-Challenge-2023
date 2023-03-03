using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SOEvents
{
[CreateAssetMenu(fileName = "WeaponSOEvent", menuName = "SOEvent/1arg/WeaponSOEvent", order = 20)]
public class WeaponSOEvent : SOEvent<Weapon>{}
}