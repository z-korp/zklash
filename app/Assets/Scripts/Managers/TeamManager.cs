
using System.Collections.Generic;
using UnityEngine;
using zKlash.Game.Roles;
using GameCharacter = zKlash.Game.Character;

[System.Serializable] // Makes the struct visible in the Unity Inspector, allowing for easier debugging and setup
public class TeamSpot
{
    public bool IsAvailable;
    public Role Role;
    public GameObject Mob;
    public string Entity;

    public TeamSpot(bool isAvailable, Role role = Role.None)
    {
        IsAvailable = isAvailable;
        Role = role;
        Mob = null;
        Entity = "";
    }
}

public class TeamManager : MonoBehaviour
{
    public TeamSpot[] TeamSpots = new TeamSpot[4]; // 4 spots for team members

    public static TeamManager instance;

    public List<Transform> targetsTeam;

    public List<Transform> targetsShopTeam;

    private Vector3 offset = new(0, 0.5f, 0);

    private string pendingEntity = "";

    public string PendingEntity
    {
        get { return pendingEntity; }
        set { pendingEntity = value; }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of TeamManager found!");
            return;
        }
        instance = this;

        // Initialize the team spots
        for (int i = 0; i < TeamSpots.Length; i++)
        {
            TeamSpots[i] = new TeamSpot(true); // Make all spots available
        }
    }

    public void MoveTeam()
    {
        for (int i = 0; i < TeamSpots.Length; i++)
        {
            if (TeamSpots[i].Mob != null)
            {
                HideXpCanvas(TeamSpots[i].Mob);
                TeamSpots[i].Mob.GetComponent<MobMovement>().speed = 6f * TimeScaleController.Instance.speedGame;
                TeamSpots[i].Mob.GetComponent<MobMovement>().Move(targetsTeam[i]);
            }
        }
    }

    public void TPTeamToShop()
    {
        for (int i = 0; i < TeamSpots.Length; i++)
        {
            if (TeamSpots[i].Mob != null)
            {
                // Hide team still alive
                HideXpCanvas(TeamSpots[i].Mob);
                TeamSpots[i].Mob.SetActive(false);

                // Move Team to shop
                TeamSpots[i].Mob.transform.position = targetsShopTeam[i].position + offset;
                TeamSpots[i].Mob.GetComponent<MobMovement>().ResetTarget();
                HideXpCanvas(TeamSpots[i].Mob, false);

                // Show team again
                TeamSpots[i].Mob.SetActive(true);
            }
        }
    }

    public void SaveInfoMobBeforeFight()
    {
        for (int i = 0; i < TeamSpots.Length; i++)
        {
            if (TeamSpots[i].Mob != null)
            {
                var mobItem = TeamSpots[i].Mob.GetComponent<MobItem>();
                SaveItemBeforeFight(mobItem);
            }
        }
    }

    private void SaveItemBeforeFight(MobItem mobItem)
    {
        var itemToSave = mobItem.item;
        mobItem.SaveItemDataBeforeBattle(itemToSave);
    }


    public void ResetStatCharacter()
    {
        for (int i = 0; i < TeamSpots.Length; i++)
        {
            if (TeamSpots[i].Mob != null)
            {
                GameObject mobObject = TeamSpots[i].Mob;
                MobController mobController = mobObject.GetComponent<MobController>();
                if (mobController != null && mobController.Character != null)
                {
                    GameCharacter character = mobController.Character;
                    mobController.ConfigureCharacter(character.RoleInterface, character.Level, character.ItemInterface, character.XP);

                    ItemData battleItem = mobObject.GetComponent<MobItem>().GetBattleItemData();
                    string itemName = PrefabMappings.NameToItemDataMap[character.Item.GetItemType()];

                    if (battleItem != null)
                    {
                        //ItemData item = PrefabUtils.FindScriptableByName(BattleManager.instance.itemDataArray, itemName);
                        var mobItem = mobObject.GetComponent<MobItem>();
                        mobItem.ResetItemDataAfterBattle();
                        //mobObject.GetComponent<MobController>().Character.Equip(battleItem.type);

                    }
                }
            }
        }

    }

    public void ReorganizeTeamSpots()
    {
        for (int i = 0; i < TeamSpots.Length; i++)
        {
            if (!TeamSpots[i].IsAvailable)
            {
                for (int j = 0; j < i; j++)
                {
                    if (TeamSpots[j].IsAvailable)
                    {

                        TeamSpots[j].Mob = TeamSpots[i].Mob;
                        TeamSpots[j].Role = TeamSpots[i].Role;
                        TeamSpots[j].Entity = TeamSpots[i].Entity;
                        TeamSpots[j].IsAvailable = false;
                        TeamSpots[j].Mob.GetComponent<MobDraggable>().index = j;

                        // Marquer l'emplacement original comme disponible
                        TeamSpots[i].Mob = null;
                        TeamSpots[i].Role = Role.None;
                        TeamSpots[i].Entity = null;
                        TeamSpots[i].IsAvailable = true;
                        break;
                    }
                }
            }
        }
    }


    public GameObject GetMemberFromTeam(int index)
    {
        return TeamSpots[index].Mob;
    }

    public string GetEntityFromTeam(int index)
    {
        return TeamSpots[index].Entity;
    }

    public string GetEntityFromTeam(GameObject mob)
    {
        Debug.Log(">>>>>>>>>>> GetEntityFromTeam");
        foreach (var spot in TeamSpots)
        {
            Debug.Log("Spot " + spot.Role);
            if (spot.Mob == mob)
            {
                return spot.Entity;
            }
        }
        Debug.Log("<<<<<<<<<<<<<<");
        return "";
    }

    // Call this method to fill a spot with an entity
    public bool FillSpot(int spotIndex, Role _role, GameObject mob, string entity)
    {
        if (spotIndex < 0 || spotIndex >= TeamSpots.Length || !TeamSpots[spotIndex].IsAvailable)
        {
            return false; // Spot index is out of range or spot is not available
        }

        TeamSpots[spotIndex] = new TeamSpot(false, _role); // Fill the spot
        TeamSpots[spotIndex].Mob = mob;
        TeamSpots[spotIndex].Entity = entity;
        Debug.Log($"Spot {spotIndex} filled with entity: {_role}");
        return true;
    }

    // This function is called when we hire a new mob, it will use the pendingEntity that
    // was set by torii when it fetched the entity that has been hired
    public bool FillNewSpot(int spotIndex, Role _role, GameObject mob, string entity = "")
    {
        bool ret = FillSpot(spotIndex, _role, mob, pendingEntity);
        if (ret)
        {
            pendingEntity = ""; // reset pending entity
        }
        return ret;
    }

    public bool UpdateSpot(int spotIndex, Role _role, GameObject mob, string entity = "")
    {
        TeamSpots[spotIndex] = new TeamSpot(false, _role); // Fill the spot
        TeamSpots[spotIndex].Mob = mob;
        TeamSpots[spotIndex].Entity = entity;
        Debug.Log($"Spot {spotIndex} filled with entity: {_role}");
        return true;
    }

    // Call this method to free up a spot
    public bool FreeSpot(int spotIndex)
    {
        if (spotIndex < 0 || spotIndex >= TeamSpots.Length)
        {
            return false; // Spot index is out of range
        }

        TeamSpots[spotIndex] = new TeamSpot(true); // Make the spot available again
        Debug.Log($"Spot {spotIndex} is now available");
        return true;
    }

    public Role RoleAtIndex(int index)
    {
        if (index < 0 || index >= TeamSpots.Length)
        {
            return Role.None; // Return Role.None if index is out of range
        }

        return TeamSpots[index].Role; // Return the entity contained in the spot
    }

    private void HideXpCanvas(GameObject go, bool hide = true)
    {
        Transform canvasXpTransform = go.transform.Find("CanvasXP"); // Adjust path if nested deeper
        if (canvasXpTransform != null)
        {
            Canvas canvas = canvasXpTransform.GetComponent<Canvas>();
            if (canvas != null)
            {
                if (hide)
                {
                    canvas.enabled = false; // Disable rendering for this canvas
                }
                else
                {
                    canvas.enabled = true;
                }

            }
            else
            {
                Debug.LogError("Canvas component not found on 'CanvasXP' object.");
            }
        }
        else
        {
            Debug.LogError("CanvasXp GameObject not found.");
        }
    }

    public void UpdateFirstAvailableSpot(Role _role)
    {
        foreach (var spot in TeamSpots)
        {
            if (!spot.IsAvailable && spot.Role == Role.None)
            {
                spot.Role = _role; // Assign the entity to this spot
                Debug.Log($"Spot updated with entity: {_role}");
                return; // Exit the method after updating the first matching spot
            }
        }
    }

    public void UpdateMissingEntity(string _entity)
    {
        foreach (var spot in TeamSpots)
        {
            if (!spot.IsAvailable && spot.Entity == "")
            {
                spot.Entity = _entity;
                return; // Exit the method after updating the first matching spot
            }
        }
    }

    public int CountMobInTeam()
    {
        var countTeamSpot = 0;
        foreach (var spot in TeamSpots)
        {
            if (!spot.IsAvailable)
            {
                countTeamSpot++;
            }
        }

        return countTeamSpot;

    }
}

