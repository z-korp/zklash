using UnityEngine;
using System;
using zKlash.Game.Items;
using ItemEnum = zKlash.Game.Items.Item;
using zKlash.Game.Roles;

namespace zKlash.Game
{
    public class Character
    {
        private int _health;
        private int _attack;
        private int _absorb;
        private int _stun;

        public int Level { get; private set; }

        public int Health
        {
            get => _health;
            private set => _health = value;
        }

        public int Attack
        {
            get
            {
                if (_stun > 0)
                {
                    _stun--;  // Decrease stun count and return 0 attack
                    return 0;
                }
                return _attack;  // Return normal attack if not stunned
            }
            private set => _attack = value;
        }

        public int Damage => _attack;

        public int Absorb
        {
            get => _absorb;
            private set => _absorb = value;
        }

        public int Stun
        {
            get => _stun;
            set => _stun += value;
        }

        public IRole Role { get; private set; }
        public IItem Item { get; private set; }

        public Character(Role roleType, int level, ItemEnum item)
        {
            Item = ItemFactory.GetItem(ItemEnum.None);
            Role = RoleFactory.GetRole(roleType);

            Level = level;

            _health = Role.Health(Phase.OnHire, level);
            _attack = Role.Attack(Phase.OnHire, level);
            _absorb = Role.Absorb(Phase.OnHire, level);
            _stun = 0;

            Equip(item);
        }

        /*public void FromGameObject(GameObject gameObject)
        {
            Health = gameObject.GetComponent<MobStat>().health;
            Attack = gameObject.GetComponent<MobStat>().damage;
            //Absorb = gameObject.GetComponent<MobStat>().absorb;
            //Stun = gameObject.GetComponent<MobStat>().stun;
        }*/

        public bool IsDead()
        {
            return Health == 0;
        }

        public int TakeDamage(int damage)
        {
            if (damage > 0)
            {
                damage -= Math.Min(damage, Absorb);
                Absorb = 0;
            }
            damage = Math.Min(damage, Health);
            Health -= damage;
            return damage;
        }

        public void ApplyBuff(Buff buff)
        {
            Health += buff.Health;
            Attack += buff.Attack;
            Absorb += buff.Absorb;
        }

        public void ApplyDebuff(Buff buff)
        {
            Health -= buff.Health;
            Attack -= buff.Attack;
            Absorb -= buff.Absorb;
        }

        public (int, int, Buff) Talent(Phase phase)
        {
            // [Effect] Update the character's buff
            Buff buff = new Buff
            {
                Health = Role.Health(phase, Level),
                Attack = Role.Attack(phase, Level),
                Absorb = Role.Absorb(phase, Level),
            };
            ApplyBuff(buff);

            int damage = Role.Damage(phase, Level);
            int stun = Role.Stun(phase, Level);
            Buff nextBuff = new Buff
            {
                Health = Role.NextHealth(phase, Level),
                Attack = Role.NextAttack(phase, Level),
                Absorb = Role.NextAbsorb(phase, Level),
            };
            return (damage, stun, nextBuff);
        }

        public int Usage(Phase phase)
        {
            // [Effect] Update the item's effect
            Buff buff = new Buff
            {
                Health = Item.Health(phase),
                Attack = Item.Attack(phase),
                Absorb = Item.Absorb(phase),
            };
            ApplyBuff(buff);

            Item = ItemFactory.GetItem(Item.Usage(phase));
            // [Effect] Return the item damage
            return Item.Damage(phase);
        }


        public void Unequip()
        {
            // [Effect] Update the item's effect
            Buff buff = new Buff
            {
                Health = Item.Health(Phase.OnEquip),
                Attack = Item.Attack(Phase.OnEquip),
                Absorb = Item.Absorb(Phase.OnEquip),
            };
            ApplyDebuff(buff);
            Item = ItemFactory.GetItem(ItemEnum.None);
        }

        public void Equip(ItemEnum item)
        {
            // [Effect] Remove the previous item's effect
            Unequip();
            Item = ItemFactory.GetItem(item);

            if (item != ItemEnum.None)
            {
                Talent(Phase.OnEquip);
            }

            // [Effect] Equip and apply the new item's effect
            Buff buff = new Buff
            {
                Health = Item.Health(Phase.OnEquip),
                Attack = Item.Attack(Phase.OnEquip),
                Absorb = Item.Absorb(Phase.OnEquip),
            };
            Debug.Log("MMMMMMMMMMMMM Applying buff: " + buff.Health + " " + buff.Attack + " " + buff.Absorb);
            ApplyBuff(buff);
        }
    }
}