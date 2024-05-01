#!/bin/bash
set -euo pipefail
pushd $(dirname "$0")/..

export STARKNET_RPC_URL="https://api.cartridge.gg/x/zklash/katana"

export DOJO_WORLD_ADDRESS=$(cat ./manifests/dev/manifest.json | jq -r '.world.address')

export ACCOUNT_ADDRESS=$(cat ./manifests/dev/manifest.json | jq -r '.contracts[] | select(.name == "zklash::systems::account::account" ).address')
export BATTLE_ADDRESS=$(cat ./manifests/dev/manifest.json | jq -r '.contracts[] | select(.name == "zklash::systems::battle::battle" ).address')
export MARKET_ADDRESS=$(cat ./manifests/dev/manifest.json | jq -r '.contracts[] | select(.name == "zklash::systems::market::market" ).address')

echo "---------------------------------------------------------------------------"
echo world : $DOJO_WORLD_ADDRESS
echo " "
echo account : $ACCOUNT_ADDRESS
echo battle : $BATTLE_ADDRESS
echo market : $MARKET_ADDRESS
echo "---------------------------------------------------------------------------"

# enable system -> models authorizations
MODELS=("Character" "Foe" "League" "Player" "Registry" "Shop" "Slot" "Squad" "Team")
ACTIONS=($ACCOUNT_ADDRESS $BATTLE_ADDRESS $MARKET_ADDRESS)

command="sozo auth grant --world $DOJO_WORLD_ADDRESS --wait writer "
for model in "${MODELS[@]}"; do
    for action in "${ACTIONS[@]}"; do
        command+="$model,$action "
    done
done
eval "$command"

echo "Default authorizations have been successfully set."
