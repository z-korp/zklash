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

        private int _xp;
        private int _lvl;

        private Role _role;
        private ItemEnum _item;

        private const int MaxLevel = 3;  // Max level limit

        public int XP
        {
            get => _xp;
            set
            {
                _xp = value;
                CheckLevelUp();
                CheckMaxLevel();
            }
        }

        public Role RoleInterface
        {
            get => _role;
            private set => _role = value;
        }

        public ItemEnum ItemInterface
        {
            get => _item;
            private set => _item = value;
        }

        public int Level
        {
            get => _lvl;
            private set => _lvl = value;
        }

        public int Health
        {
            get => _health;
            private set => _health = value;
        }

        public int Damage // Attack value during combat
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

        public int Attack
        {
            get => _attack;
            private set => _attack = value;
        }

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

        public Character(Role roleType, int lvl, ItemEnum item, int xp = 0)
        {
            Item = ItemFactory.GetItem(ItemEnum.None);
            Role = RoleFactory.GetRole(roleType);

            _lvl = lvl;
            _xp = xp;

            _health = Role.Health(Phase.OnHire, lvl);
            _attack = Role.Attack(Phase.OnHire, lvl);
            _absorb = Role.Absorb(Phase.OnHire, lvl);
            _stun = 0;

            _role = roleType;
            _item = item;

            Equip(item);
        }

        private void CheckLevelUp()
        {
            while (_xp >= (_lvl + 1) && _lvl < MaxLevel)
            {
                _xp = 0; // Reset XP after leveling up
                _lvl++;
                UpdateStats(); // Update stats based on new level
            }
        }

        private void CheckMaxLevel()
        {
            if (_lvl == MaxLevel)
                _xp = 100;
        }

        public void AddExperience(int amount)
        {
            XP += amount; // Setter handles level checking
        }

        public bool IsMobMaxLevel()
        {
            return _lvl == MaxLevel;
        }

        private void UpdateStats()
        {
            Health = Role.Health(Phase.OnHire, _lvl);
            Attack = Role.Attack(Phase.OnHire, _lvl);
            Absorb = Role.Absorb(Phase.OnHire, _lvl);
        }

        public int ExperienceRequiredForLevel(int level)
        {
            return level + 1; // Each level requires 'current level + 1' XP to level up
        }

        public void Merge(Character other)
        {
            AddExperience(1); // Gain 1 XP for merging
        }

        public bool IsDead()
        {
            return Health == 0;
        }

        public int TakeDamage(int damage)
        {
            if (damage > 0)
            {
                Debug.Log(">>>>>>>>>>>>>>>>>> TakeDamage: " + damage + " " + Absorb + " Math.Min:" + Math.Min(damage, Absorb));
                damage -= Math.Min(damage, Absorb);
                Debug.Log(">>>>>>>>>>>>>>>>>> TakeDamage: " + damage);
                Absorb = 0;
            }
            Health -= Math.Min(damage, Health);
            return damage;
        }

        public void ApplyBuff(Buff buff)
        {
            Debug.Log(">>>>>>>>>>>>>>>>>> ApplyBuff: " + buff.Health + " " + buff.Attack + " " + buff.Absorb);
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
            Debug.Log(">>>>>>>>>>>>>>>>>> buff: " + buff.Health + " " + buff.Attack + " " + buff.Absorb);
            ApplyBuff(buff);

            int damage = Role.Damage(phase, Level);
            int stun = Role.Stun(phase, Level);
            Buff nextBuff = new Buff
            {
                Health = Role.NextHealth(phase, Level),
                Attack = Role.NextAttack(phase, Level),
                Absorb = Role.NextAbsorb(phase, Level),
            };
            Debug.Log(">>>>>>>>>>>>>>>>>> nextBuff: " + nextBuff.Health + " " + nextBuff.Attack + " " + nextBuff.Absorb);
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

            int dmg = Item.Damage(phase);

            Item = ItemFactory.GetItem(Item.Usage(phase));
            // [Effect] Return the item damage
            return dmg;
        }


        public void Unequip()
        {
            Talent(Phase.OnUnequip);

            // [Effect] Update the item's effect
            Buff buff = new Buff
            {
                Health = Item.Health(Phase.OnEquip),
                Attack = Item.Attack(Phase.OnEquip),
                Absorb = Item.Absorb(Phase.OnEquip),
            };
            ApplyDebuff(buff);
            Item = ItemFactory.GetItem(ItemEnum.None);
            _item = ItemEnum.None;
        }

        public void Equip(ItemEnum item)
        {
            // [Effect] Remove the previous item's effect
            if (_item != ItemEnum.None)
                Unequip();

            Item = ItemFactory.GetItem(item);
            _item = item;

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
            ApplyBuff(buff);
        }
    }
}