using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public Dictionary<int, GameObject> team = new Dictionary<int, GameObject>();

    public static TeamManager instance;

    public List<Transform> targetsTeam;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of TeamManager found!");
            return;
        }
        instance = this;
    }

    void Update()
    {
        // Vérifier si la touche 'P' est pressée
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintDictionary(team);
        }
    }

    // Méthode pour imprimer le dictionnaire
    void PrintDictionary(Dictionary<int, GameObject> dict)
    {
        Debug.Log("Dictionary:");
        foreach (KeyValuePair<int, GameObject> entry in dict)
        {
            Debug.Log($"Key: {entry.Key}, Value: {entry.Value}");
        }
    }

    public void AddTeamMember(int teamId, GameObject mobPrefab)
    {
        if (team != null)
            team.Add(teamId, mobPrefab);
    }

    public void MoveTeam()
    {
        foreach (KeyValuePair<int, GameObject> entry in team)
        {
            HideXpCanvas(entry.Value);
            entry.Value.GetComponent<MobMovement>().speed = 6;
            entry.Value.GetComponent<MobMovement>().Move(targetsTeam[entry.Key]);
        }
    }

    public GameObject GetMemberFromTeam(int key)
    {
        if (team.TryGetValue(key, out GameObject foundGameObject))
        {
            return foundGameObject;
        }
        else
        {
            Debug.LogError("GameObject not found for key: " + key);
            return null;
        }
    }

    private void HideXpCanvas(GameObject go)
    {
        Transform canvasXpTransform = go.transform.Find("CanvasXP"); // Adjust path if nested deeper
        if (canvasXpTransform != null)
        {
            Canvas canvas = canvasXpTransform.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.enabled = false; // Disable rendering for this canvas
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
}

