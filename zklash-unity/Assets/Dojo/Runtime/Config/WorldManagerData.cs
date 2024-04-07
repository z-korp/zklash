using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dojo
{
    [CreateAssetMenu(fileName = "WorldManagerData", menuName = "ScriptableObjects/WorldManagerData", order = 2)]
    public class WorldManagerData : ScriptableObject
    {
        [Header("RPC")]
        public string toriiUrl = "https://api.cartridge.gg/x/zklash/torii";
        public string rpcUrl = "https://api.cartridge.gg/x/zklash/katana";
        public string relayUrl = "/ip4/127.0.0.1/tcp/9090";
        public string relayWebrtcUrl;
        public uint limit = 100;
        [Header("World")]
        public string worldAddress = "0x11aa9b474f093e27b773aaec3c0bae571140e825d27ac433698d3584c8d4af9";
    }
}
