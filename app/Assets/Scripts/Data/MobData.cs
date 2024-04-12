using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "Mob/MobData")]
public class MobData : ScriptableObject
{
    public int id;
    public string title;
    public int health;
    public int damage;
    public int xp;
    public Sprite image;
    public Role role;

}
