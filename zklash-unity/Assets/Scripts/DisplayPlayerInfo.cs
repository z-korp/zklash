using UnityEngine;
using UnityEngine.UI;
using Dojo.Starknet;

public class DisplayPlayerInfo : MonoBehaviour
{
    public Text currentPlayerText;

    void UpdatePlayerAddressOnUI(FieldElement playerAddress)
    {
        if(currentPlayerText != null)
        {
            currentPlayerText.text = "Player Address: " + playerAddress.Hex();
        }
    }
}
