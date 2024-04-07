using Newtonsoft.Json.Linq; // Make sure to include this
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEventDetails { }

public class Fighter : IEventDetails
{
    public string PlayerId;
    public uint TeamId;
    public uint BattleId;
    public uint CharacterId;
    public uint Index;
    public uint Role;
    public uint Item;
    public uint Xp;
    public uint Level;
    public uint Health;
    public uint Attack;
    public uint Absorb;
    public uint Stun;
}

public class Hit : IEventDetails
{
    public string PlayerId;
    public uint TeamId;
    public uint BattleId;
    public uint Tick;
    public uint FromCharacterId;
    public uint ToCharacterId;
    public uint Damage;
}

public class Stun : IEventDetails
{
    public string PlayerId;
    public uint TeamId;
    public uint BattleId;
    public uint Tick;
    public uint FromCharacterId;
    public uint ToCharacterId;
    public uint Value;
}

public class Absorb : IEventDetails
{
    public string PlayerId;
    public uint TeamId;
    public uint BattleId;
    public uint Tick;
    public uint CharacterId;
    public uint Value;
}


public class Usage : IEventDetails
{
    public string PlayerId;
    public uint TeamId;
    public uint BattleId;
    public uint Tick;
    public uint CharacterId;
    public uint Item;
    public uint NewItem;
    public uint Health;
    public uint Attack;
    public uint Absorb;
    public uint Damage;
}


public class Talent : IEventDetails
{
    public string PlayerId;
    public uint TeamId;
    public uint BattleId;
    public uint Tick;
    public uint CharacterId;
    public uint Role;
    public uint Health;
    public uint Attack;
    public uint Absorb;
    public uint Damage;
    public uint Stun;
    public uint NextHealth;
    public uint NextAttack;
    public uint NextAbsorb;
}


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

    public IEventDetails ProcessNode(string id, string[] keys, string[] data, string createdAt, string transactionHash)
    {
        if (keys.Length < 1)
        {
            Debug.Log($"Event ID: {id} has no keys.");
            return null;
        }

        // Assuming GetEventNameFromHex is implemented to map the hex value to a readable event name
        string eventName = GetEventNameFromHex(keys[0]);

        Debug.Log($"Processing event: {eventName}");
        Debug.Log($"Event ID: {id}");
        Debug.Log($"Keys: {string.Join(", ", keys)}");
        Debug.Log($"Data: {string.Join(", ", data)}");
        Debug.Log($"Created At: {createdAt}");
        Debug.Log($"Transaction Hash: {transactionHash}");

        switch (eventName)
        {
            case "Hit":
                var hitEvent = ParseHitEvent(keys, data);
                // Now, do something with hitEvent
                Debug.Log($"Processed a Hit event with damage: {hitEvent.Damage}");
                return hitEvent;
            case "Stun":
                var stunEvent = ParseStunEvent(keys, data);
                // Process stunEvent as needed
                Debug.Log($"Processed a Stun event with value: {stunEvent.Value}");
                return stunEvent;
            case "Absorb":
                var absorbEvent = ParseAbsorbEvent(keys, data);
                // Process absorbEvent as needed
                Debug.Log($"Processed an Absorb event with value: {absorbEvent.Value}");
                return absorbEvent;
            case "Usage":
                var usageEvent = ParseUsageEvent(keys, data);
                // Process usageEvent as needed
                Debug.Log($"Processed a Usage event with item: {usageEvent.Item}");
                return usageEvent;
            case "Talent":
                var talentEvent = ParseTalentEvent(keys, data);
                // Process talentEvent as needed
                Debug.Log($"Processed a Talent event with role: {talentEvent.Role}");
                return talentEvent;
            case "Fighter":
                var fighterEvent = ParseFighterEvent(keys, data);
                // Process fighterEvent as needed
                Debug.Log($"Processed a Fighter event with character ID: {fighterEvent.CharacterId}");
                return fighterEvent;
            default:
                Debug.LogError($"Unknown event type: {eventName}");
                return null;
        }
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

    public Fighter ParseFighterEvent(string[] keys, string[] data)
    {
        return new Fighter
        {
            PlayerId = keys[1], // Player ID from keys[1]
            TeamId = Convert.ToUInt32(keys[2], 16), // Team ID from keys[2]
            BattleId = Convert.ToUInt32(keys[3], 16), // Battle ID from keys[3]
            CharacterId = Convert.ToUInt32(data[0], 16), // Start of data array
            Index = Convert.ToUInt32(data[1], 16),
            Role = Convert.ToUInt32(data[2], 16),
            Item = Convert.ToUInt32(data[3], 16),
            Xp = Convert.ToUInt32(data[4], 16),
            Level = Convert.ToUInt32(data[5], 16),
            Health = Convert.ToUInt32(data[6], 16),
            Attack = Convert.ToUInt32(data[7], 16),
            Absorb = Convert.ToUInt32(data[8], 16),
            Stun = Convert.ToUInt32(data[9], 16),
        };
    }

    public Hit ParseHitEvent(string[] keys, string[] data)
    {

        Debug.Log($"Player ID: {keys[1]}");
        var player_id = keys[1];
        Debug.Log($"Team ID: {keys[2]}");
        var team_id = Convert.ToUInt32(keys[2], 16);
        Debug.Log($"Battle ID: {keys[3]}");
        var battle_id = Convert.ToUInt32(keys[3], 16);
        Debug.Log($"Tick: {data[0]}");
        var tick = Convert.ToUInt32(data[0], 16);
        Debug.Log($"From Character ID: {data[1]}");
        var from_character_id = Convert.ToUInt32(data[1], 16);
        Debug.Log($"To Character ID: {data[2]}");
        var to_character_id = Convert.ToUInt32(data[2], 16);
        Debug.Log($"Damage: {data[3]}");
        var damage = Convert.ToUInt32(data[3], 16);


        return new Hit
        {
            PlayerId = keys[1],
            TeamId = Convert.ToUInt32(keys[2], 16),
            BattleId = Convert.ToUInt32(keys[3], 16),
            Tick = Convert.ToUInt32(data[0], 16),
            FromCharacterId = Convert.ToUInt32(data[1], 16),
            ToCharacterId = Convert.ToUInt32(data[2], 16),
            Damage = Convert.ToUInt32(data[3], 16)
        };
    }

    public Usage ParseUsageEvent(string[] keys, string[] data)
    {
        return new Usage
        {
            PlayerId = keys[1],
            TeamId = Convert.ToUInt32(keys[2], 16),
            BattleId = Convert.ToUInt32(keys[3], 16),
            Tick = Convert.ToUInt32(data[0], 16),
            CharacterId = Convert.ToUInt32(data[1], 16),
            Item = Convert.ToUInt32(data[2], 16),
            NewItem = Convert.ToUInt32(data[3], 16),
            Health = Convert.ToUInt32(data[4], 16),
            Attack = Convert.ToUInt32(data[5], 16),
            Absorb = Convert.ToUInt32(data[6], 16),
            Damage = Convert.ToUInt32(data[7], 16)
        };
    }

    public Talent ParseTalentEvent(string[] keys, string[] data)
    {
        return new Talent
        {
            PlayerId = keys[1],
            TeamId = Convert.ToUInt32(keys[2], 16),
            BattleId = Convert.ToUInt32(keys[3], 16),
            Tick = Convert.ToUInt32(data[0], 16),
            CharacterId = Convert.ToUInt32(data[1], 16),
            Role = Convert.ToUInt32(data[2], 16),
            Health = Convert.ToUInt32(data[3], 16),
            Attack = Convert.ToUInt32(data[4], 16),
            Absorb = Convert.ToUInt32(data[5], 16),
            Damage = Convert.ToUInt32(data[6], 16),
            Stun = Convert.ToUInt32(data[7], 16),
            NextHealth = Convert.ToUInt32(data[8], 16),
            NextAttack = Convert.ToUInt32(data[9], 16),
            NextAbsorb = Convert.ToUInt32(data[10], 16)
        };
    }

    public Absorb ParseAbsorbEvent(string[] keys, string[] data)
    {
        return new Absorb
        {
            PlayerId = keys[1],
            TeamId = Convert.ToUInt32(keys[2], 16),
            BattleId = Convert.ToUInt32(keys[3], 16),
            Tick = Convert.ToUInt32(data[0], 16),
            CharacterId = Convert.ToUInt32(data[1], 16),
            Value = Convert.ToUInt32(data[2], 16)
        };
    }

    public Stun ParseStunEvent(string[] keys, string[] data)
    {
        return new Stun
        {

            PlayerId = keys[1], // Assuming player_id is a string.
            TeamId = Convert.ToUInt32(keys[2], 16), // Assuming team_id is parsable directly to uint.
            BattleId = Convert.ToUInt32(keys[3], 16), // Assuming battle_id is parsable directly to uint.
            Tick = Convert.ToUInt32(data[0], 16), // The first element in the data array represents the tick.
            FromCharacterId = Convert.ToUInt32(data[1], 16), // The second element represents from_character_id.
            ToCharacterId = Convert.ToUInt32(data[2], 16), // The third element represents to_character_id.
            Value = Convert.ToUInt32(data[3], 16) // The fourth element represents the stun value.
        };
    }
}
