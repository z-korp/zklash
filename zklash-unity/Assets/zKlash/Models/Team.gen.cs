// Generated by dojo-bindgen on Fri, 5 Apr 2024 08:03:07 +0000. Do not modify this file manually.
using UnityEngine;
using System;
using Dojo;
using Dojo.Starknet;
using Dojo.Torii;


// Model definition for `zklash::models::team::Team` model
public class Team : ModelInstance {
    [ModelField("player_id")]
    public FieldElement player_id;

    [ModelField("id")]
    public uint id;

    [ModelField("seed")]
    public FieldElement seed;

    [ModelField("nonce")]
    public FieldElement nonce;

    [ModelField("gold")]
    public uint gold;

    [ModelField("health")]
    public uint health;

    [ModelField("level")]
    public byte level;

    [ModelField("character_count")]
    public byte character_count;

    [ModelField("battle_id")]
    public byte battle_id;

    public override string ToString()
    {
        return $"Player ID: {player_id.Hex()}, Gold: {gold}, Level: {level} Health: {health} Character Count: {character_count} Battle ID: {battle_id}";
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
    }

    public override void OnUpdate(Model model)
    {
        Debug.Log($"Team updated: {model}");
        base.OnUpdate(model);
    }
}
        