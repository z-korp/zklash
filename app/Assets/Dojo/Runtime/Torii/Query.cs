using System;
using System.Linq;
using Dojo.Starknet;
using dojo_bindings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Dojo.Torii
{
#nullable enable

    [Serializable]
    public class Query
    {
        public uint limit;
        public uint offset;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Clause? clause;

        public Query(uint limit, uint offset, Clause? clause = null)
        {
            this.limit = limit;
            this.offset = offset;
            this.clause = clause;
        }

        public dojo.Query ToNative()
        {
            var nativeQuery = new dojo.Query
            {
                limit = limit,
                offset = offset,
                clause = new dojo.COptionClause { tag = dojo.COptionClause_Tag.NoneClause }
            };

            if (clause.HasValue)
            {
                nativeQuery.clause = new dojo.COptionClause
                {
                    tag = dojo.COptionClause_Tag.SomeClause,
                    some = clause.Value.ToNative()
                };
            }

            return nativeQuery;
        }
    }

    [Serializable]
    public struct Clause
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public KeysClause? Keys;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MemberClause? Member;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CompositeClause? Composite;

        public static implicit operator Clause(KeysClause keysClause)
        {
            return new Clause { Keys = keysClause };
        }

        public static implicit operator Clause(MemberClause memberClause)
        {
            return new Clause { Member = memberClause };
        }

        public static implicit operator Clause(CompositeClause compositeClause)
        {
            return new Clause { Composite = compositeClause };
        }

        public dojo.Clause ToNative()
        {
            if (Keys.HasValue)
                return new dojo.Clause { tag = dojo.Clause_Tag.Keys, keys = Keys.Value.ToNative() };
            if (Member.HasValue)
                return new dojo.Clause { tag = dojo.Clause_Tag.CMember, c_member = Member.Value.ToNative() };
            if (Composite.HasValue)
                return new dojo.Clause { tag = dojo.Clause_Tag.Composite, composite = Composite.Value.ToNative() };

            throw new InvalidOperationException("Clause must have one non-null value");
        }
    }

    [Serializable]
    public struct ModelKeysClause
    {
        public string model;
        public FieldElement[] keys;

        public ModelKeysClause(string model, FieldElement[] keys)
        {
            this.model = model;
            this.keys = keys;
        }

        public dojo.ModelKeysClause ToNative()
        {
            return new dojo.ModelKeysClause
            {
                model = model,
                keys = keys.Select(k => k.Inner).ToArray()
            };
        }
    }

    [Serializable]
    public struct EntityKeysClause
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FieldElement[]? HashedKeys;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public KeysClause? Keys;

        public EntityKeysClause(object type) : this()
        {
            if (type is FieldElement[] hashedKeys)
                HashedKeys = hashedKeys;
            else if (type is KeysClause keys)
                Keys = keys;
            else
                throw new ArgumentException("Invalid entity keys clause type");
        }

        public dojo.EntityKeysClause ToNative()
        {
            if (HashedKeys != null)
                return new dojo.EntityKeysClause
                {
                    tag = dojo.EntityKeysClause_Tag.HashedKeys,
                    hashed_keys = HashedKeys.Select(k => k.Inner).ToArray()
                };
            if (Keys.HasValue)
                return new dojo.EntityKeysClause
                {
                    tag = dojo.EntityKeysClause_Tag.EntityKeys,
                    entity_keys = Keys.Value.ToNative()
                };

            throw new InvalidOperationException("EntityKeysClause must have one non-null value");
        }
    }

    [Serializable]
    public struct KeysClause
    {
        public FieldElement?[] keys;
        [JsonConverter(typeof(StringEnumConverter))]
        public dojo.PatternMatching pattern_matching;
        public string[] models;

        public KeysClause(FieldElement?[] keys, dojo.PatternMatching pattern_matching, string[] models)
        {
            this.keys = keys;
            this.pattern_matching = pattern_matching;
            this.models = models;
        }

        public dojo.KeysClause ToNative()
        {
            return new dojo.KeysClause
            {
                keys = keys.Select(k => k is null ?
                    new dojo.COptionFieldElement { tag = dojo.COptionFieldElement_Tag.NoneFieldElement }
                    : new dojo.COptionFieldElement { tag = dojo.COptionFieldElement_Tag.SomeFieldElement, some = k.Inner }).ToArray(),
                pattern_matching = pattern_matching,
                models = models
            };
        }
    }

    [Serializable]
    public struct MemberClause
    {
        public string model;
        public string member;
        [JsonConverter(typeof(StringEnumConverter))]
        public dojo.ComparisonOperator @operator;
        public Value value;

        public MemberClause(string model, string member, dojo.ComparisonOperator @operator, Value value)
        {
            this.model = model;
            this.member = member;
            this.@operator = @operator;
            this.value = value;
        }

        public dojo.MemberClause ToNative()
        {
            return new dojo.MemberClause
            {
                model = model,
                member = member,
                operator_ = @operator,
                value = value.ToNative()
            };
        }
    }

    [Serializable]
    public struct CompositeClause
    {
        public string model;
        [JsonConverter(typeof(StringEnumConverter))]
        public dojo.LogicalOperator @operator;
        public Clause[] clauses;

        public CompositeClause(string model, dojo.LogicalOperator @operator, Clause[] clauses)
        {
            this.model = model;
            this.@operator = @operator;
            this.clauses = clauses;
        }

        public dojo.CompositeClause ToNative()
        {
            return new dojo.CompositeClause
            {
                model = model,
                operator_ = @operator,
                clauses = clauses.Select(c => c.ToNative()).ToArray()
            };
        }
    }

    [Serializable]
    public class Value
    {
        public Primitive primitive_type;
        public ValueType value_type;

        public dojo.Value ToNative()
        {
            return new dojo.Value
            {
                primitive_type = primitive_type.ToNative(),
                value_type = value_type.ToNative()
            };
        }
    }

    [Serializable]
    public struct ValueType
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? String;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? Int;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ulong? UInt;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? VBool;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte[]? Bytes;

        public static implicit operator ValueType(string str)
        {
            return new ValueType { String = str };
        }

        public static implicit operator ValueType(long i)
        {
            return new ValueType { Int = i };
        }

        public static implicit operator ValueType(ulong u)
        {
            return new ValueType { UInt = u };
        }

        public static implicit operator ValueType(bool b)
        {
            return new ValueType { VBool = b };
        }

        public static implicit operator ValueType(byte[] bytes)
        {
            return new ValueType { Bytes = bytes };
        }

        public dojo.ValueType ToNative()
        {
            if (String != null)
                return new dojo.ValueType { tag = dojo.ValueType_Tag.String, @string = String };
            if (Int.HasValue)
                return new dojo.ValueType { tag = dojo.ValueType_Tag.Int, int_ = Int.Value };
            if (UInt.HasValue)
                return new dojo.ValueType { tag = dojo.ValueType_Tag.UInt, u_int = UInt.Value };
            if (VBool.HasValue)
                return new dojo.ValueType { tag = dojo.ValueType_Tag.VBool, v_bool = VBool.Value };
            if (Bytes != null)
                return new dojo.ValueType { tag = dojo.ValueType_Tag.Bytes, bytes = Bytes };

            throw new InvalidOperationException("ValueType must have one non-null value");
        }
    }

    [Serializable]
    public struct Primitive
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte? U8;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ushort? U16;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? U32;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ulong? U64;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FieldElement? U128;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? U256;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? USize;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Bool;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FieldElement? Felt252;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FieldElement? ClassHash;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FieldElement? ContractAddress;

        public dojo.Primitive ToNative()
        {
            if (U8.HasValue)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.U8, u8 = U8.Value };
            if (U16.HasValue)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.U16, u16 = U16.Value };
            if (U32.HasValue)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.U32, u32 = U32.Value };
            if (U64.HasValue)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.U64, u64 = U64.Value };
            if (U128 != null)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.U128, u128 = U128.Inner.data };
            if (U256 != null)
                throw new NotImplementedException("U256 conversion not implemented");
            if (USize.HasValue)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.USize, u_size = USize.Value };
            if (Bool.HasValue)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.Bool, bool_ = Bool.Value };
            if (Felt252 != null)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.Felt252, felt252 = Felt252.Inner };
            if (ClassHash != null)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.ClassHash, class_hash = ClassHash.Inner };
            if (ContractAddress != null)
                return new dojo.Primitive { tag = dojo.Primitive_Tag.ContractAddress, contract_address = ContractAddress.Inner };

            throw new InvalidOperationException("Primitive must have one non-null value");
        }
    }
}