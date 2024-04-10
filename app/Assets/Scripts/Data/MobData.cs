using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "MobData")]
public class MobData : ScriptableObject
{
    public int id;
    public string nameMob;
    public int health;
    public int damage;
    public int xp;

}
