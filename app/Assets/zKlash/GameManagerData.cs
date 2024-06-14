using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerData", menuName = "ScriptableObjects/GameManagerData", order = 2)]
public class GameManagerData : ScriptableObject
{
    public string masterPrivateKey;
    public string masterAddress;

    // World
    public string worldContractAddress;

    // Account
    public string accountContractAddress;
    // Battle
    public string battleContractAddress;
    //  Market
    public string marketContractAddress;
}
