// Generated by dojo-bindgen on Fri, 5 Apr 2024 08:03:07 +0000. Do not modify this file manually.
using UnityEngine;
using System;
using Dojo;
using Dojo.Starknet;
using Dojo.Torii;


// Model definition for `zklash::models::team::Team` model
public class Team : ModelInstance
{
    [ModelField("player_id")]
    public FieldElement player_id;

    [ModelField("id")]
    public uint id;

    [ModelField("gold")]
    public uint gold;

    [ModelField("health")]
    public uint health;

    [ModelField("level")]
    public byte level;

    [ModelField("character_uuid")]
    public byte character_uuid;

    [ModelField("size")]
    public byte size;

    [ModelField("battle_id")]
    public byte battle_id;

    [ModelField("foe_squad_id")]
    public byte foe_squad_id;

    public override string ToString()
    {
        return $"Player ID: {player_id.Hex()}, Gold: {gold}, Level: {level} Health: {health} Size: {size} Battle ID: {battle_id} Foe Squad ID: {foe_squad_id}";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnUpdate(Model model)
    {
        //Debug.Log($"Team updated: {model}");
        base.OnUpdate(model);
    }
}
