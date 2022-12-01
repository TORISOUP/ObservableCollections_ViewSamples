using System;

namespace ViewSamples
{
    public readonly struct Monster : IEquatable<Monster>
    {
        public MonsterType MonsterType { get; }
        public string Name { get; }

        public Monster(MonsterType monsterType, string name)
        {
            MonsterType = monsterType;
            Name = name;
        }

        public bool Equals(Monster other)
        {
            return MonsterType == other.MonsterType && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Monster other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)MonsterType, Name);
        }
    }

    public enum MonsterType
    {
        Fire,
        Water,
        Grass
    }
}