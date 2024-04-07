#!/bin/bash
set -euo pipefail
pushd $(dirname "$0")/..

export RPC_URL="https://api.cartridge.gg/x/zklash/katana"

export WORLD_ADDRESS=$(cat ./manifests/dev/manifest.json | jq -r '.world.address')

export ACCOUNT_ADDRESS=$(cat ./manifests/dev/manifest.json | jq -r '.contracts[] | select(.name == "zklash::systems::account::account" ).address')
export BATTLE_ADDRESS=$(cat ./manifests/dev/manifest.json | jq -r '.contracts[] | select(.name == "zklash::systems::battle::battle" ).address')
export MARKET_ADDRESS=$(cat ./manifests/dev/manifest.json | jq -r '.contracts[] | select(.name == "zklash::systems::market::market" ).address')

echo "---------------------------------------------------------------------------"
echo world : $WORLD_ADDRESS
echo " "
echo account : $ACCOUNT_ADDRESS
echo battle : $BATTLE_ADDRESS
echo market : $MARKET_ADDRESS
echo "---------------------------------------------------------------------------"

# enable system -> models authorizations
sozo auth grant --world $WORLD_ADDRESS --wait writer \
  Character,$ACCOUNT_ADDRESS \
  Player,$ACCOUNT_ADDRESS \
  Shop,$ACCOUNT_ADDRESS \
  Team,$ACCOUNT_ADDRESS \
  Character,$BATTLE_ADDRESS \
  Player,$BATTLE_ADDRESS \
  Shop,$BATTLE_ADDRESS \
  Team,$BATTLE_ADDRESS \
  Character,$MARKET_ADDRESS \
  Player,$MARKET_ADDRESS \
  Shop,$MARKET_ADDRESS \
  Team,$MARKET_ADDRESS >/dev/null

echo "Default authorizations have been successfully set."
