using UnityEngine;
using zKlash.Game.Roles;

[CreateAssetMenu(fileName = "MobData", menuName = "Mob/MobData")]
public class MobData : ScriptableObject
{
    public string title;
    public string powerLV1;
    public string powerLV2;
    public string powerLV3;
    public Sprite image;
    public Role role;
}
