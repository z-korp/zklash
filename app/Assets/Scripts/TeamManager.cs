using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zKlash.Game.Items;
using zKlash.Game.Roles;
using GameCharacter = zKlash.Game.Character;

[System.Serializable] // Makes the struct visible in the Unity Inspector, allowing for easier debugging and setup
public class TeamSpot
{
    public bool IsAvailable;
    public Role Role;
    public GameObject mob;

    public TeamSpot(bool isAvailable, Role role = Role.None)
    {
        IsAvailable = isAvailable;
        Role = role;
        mob = null;
    }
}

public class TeamManager : MonoBehaviour
{
    public TeamSpot[] TeamSpots = new TeamSpot[4]; // 4 spots for team members

    public static TeamManager instance;

    public List<Transform> targetsTeam;

    public List<Transform> targetsShopTeam;

    private Vector3 offset = new(0, 0.5f, 0);

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
            if (TeamSpots[i].mob != null)
            {
                HideXpCanvas(TeamSpots[i].mob);
                TeamSpots[i].mob.GetComponent<MobMovement>().speed = 6;
                TeamSpots[i].mob.GetComponent<MobMovement>().Move(targetsTeam[i]);
            }
        }
    }

    public void TPTeamToShop()
    {
        for (int i = 0; i < TeamSpots.Length; i++)
        {
            if (TeamSpots[i].mob != null)
            {
                // Hide team still alive
                HideXpCanvas(TeamSpots[i].mob);
                TeamSpots[i].mob.SetActive(false);

                // Move Team to shop
                TeamSpots[i].mob.transform.position = targetsShopTeam[i].position + offset;
                HideXpCanvas(TeamSpots[i].mob, false);

                // Show team again
                TeamSpots[i].mob.SetActive(true);
            }
        }
    }

    public void ResetStatCharacter()
    {
        for (int i = 0; i < TeamSpots.Length; i++)
        {
            if (TeamSpots[i].mob != null)
            {
                MobController mobController = TeamSpots[i].mob.GetComponent<MobController>();
                if (mobController != null && mobController.Character != null)
                {
                    GameCharacter character = mobController.Character;
                    mobController.ConfigureCharacter(character.RoleInterface, character.Level, character.ItemInterface, character.XP);
                }

            }
        }

    }


    public GameObject GetMemberFromTeam(int index)
    {
        return TeamSpots[index].mob;
    }

    // Call this method to fill a spot with an entity
    public bool FillSpot(int spotIndex, Role _role, GameObject mob)
    {
        if (spotIndex < 0 || spotIndex >= TeamSpots.Length || !TeamSpots[spotIndex].IsAvailable)
        {
            return false; // Spot index is out of range or spot is not available
        }

        TeamSpots[spotIndex] = new TeamSpot(false, _role); // Fill the spot
        TeamSpots[spotIndex].mob = mob;
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
}

