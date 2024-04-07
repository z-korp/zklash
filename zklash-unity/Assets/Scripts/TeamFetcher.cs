using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFetcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (var spot in VillageData.Instance.Spots)
        {
            var entity = spot.EntityContained;
            var character = GameManager.Instance.worldManager.Entity(entity).GetComponent<Character>();
            if (character != null)
            {
                if (character.Health > 0)
                {
                    Debug.Log($"XXXXXXXXXXXXXXXXXXXXXXX======> Character: {character.name} is alive");
                }
                else
                {
                    Debug.Log($"XXXXXXXXXXXXXXXXXXXXXXX======> Character: {character.name} is dead");
                    VillageData.Instance.FreeSpot(VillageData.Instance.Spots.IndexOf(spot));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
