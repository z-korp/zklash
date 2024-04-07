using Newtonsoft.Json.Linq; // Make sure to include this
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventParser
{
  private Dictionary<string, string> dataDict = new Dictionary<string, string>
    {
        { "Hit", "0x33f1adaeb6b7468c983c7285a0776514bd4bc3082362e9ead4211d605daf6fa" },
        { "Fighter", "0x2e716cf114cb4ac634249799a5c2f6d92d29e1ffbabe1b53fd81dd04a93343d" },
        { "Stun", "0x2726959cf68d0c5db668fc83c0d2dba1219eac773ca2dcbc132751349bc56b1" },
        { "Absorb", "0x1fa8f09733b3238e24214d0467ad01fb18a7a487aeee6341fb65b8a65a9f0ec" },
        { "Usage", "0x31dc5bb49c81bb56051cf4df0b97da231ab1dc494fd966e87d79dccf76f4244" },
        { "Talent", "0x24adf676f72d49020e56880b277e37210699f6b1c3822f9401e727754aa8a49" }
    };

  public void ProcessNode(string id, string[] keys, string[] data, string createdAt, string transactionHash)
  {
    if (keys.Length == 0)
    {
      Debug.Log($"Event ID: {id} has no keys.");
      return;
    }

    string eventName = GetEventNameFromHex(keys[0]);

    Debug.Log($"Processing event: {eventName}");
    Debug.Log($"Event ID: {id}");
    Debug.Log($"Keys: {string.Join(", ", keys)}");
    Debug.Log($"Data: {string.Join(", ", data)}");
    Debug.Log($"Created At: {createdAt}");
    Debug.Log($"Transaction Hash: {transactionHash}");

    // Additional processing based on eventName or data can be added here
  }

  private string GetEventNameFromHex(string hex)
  {
    foreach (var entry in dataDict)
    {
      if (entry.Value.Equals(hex, StringComparison.OrdinalIgnoreCase))
      {
        return entry.Key;
      }
    }
    return "Unknown";
  }
}
