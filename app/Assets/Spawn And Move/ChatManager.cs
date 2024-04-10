using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dojo;
using Dojo.Starknet;
using Dojo.Torii;
using dojo_bindings;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public bool chatOpen = false;

    public GameManager gameManager;
    public WorldManager worldManager;

    private Transform chatScrollView;
    private TMPro.TMP_InputField chatInput;

    // Start is called before the first frame update
    async void Start()
    {
        chatInput = GetComponentInChildren<TMPro.TMP_InputField>(true);
        chatInput.gameObject.SetActive(false);

        chatScrollView = transform.Find("Scroll View/Viewport/Content");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            chatInput.gameObject.SetActive(true);
            chatInput.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
            chatOpen = true;
        }

        // chat interactions below
        if (!chatOpen) return;
        // if we press enter, send message
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Enum.TryParse(chatInput.text, out Emote emote);

            SendEmote(emote);
            chatInput.gameObject.SetActive(false);
            chatInput.text = "";
            chatOpen = false;
        }

        // if press esc. close chat
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            chatInput.gameObject.SetActive(false);
            chatInput.text = "";
            chatOpen = false;
        }
    }

    async void SendEmote(Emote emote)
    {
        var account = gameManager.burnerManager.CurrentBurner ?? gameManager.masterAccount;

        var typed_data = TypedData.From(new EmoteMessage {
            identity = account.Address,
            emote = emote,
        });

        FieldElement messageHash = typed_data.encode(account.Address);
        Signature signature = account.Signer.Sign(messageHash);

        await worldManager.Publish(typed_data, signature);
    }
}
