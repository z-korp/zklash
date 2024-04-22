using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "Mob/MobData")]
public class MobData : ScriptableObject
{
    public int id;
    public string title;
    public string powerLV1;
    public string powerLV2;
    public string powerLV3;
    public string element;
    public int health;
    public int damage;
    public int xp;
    public Sprite image;
    public Role role;

}
