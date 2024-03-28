using UnityEngine;

public enum RankType
{
    Normal,
    Rare,
    Epic,
    Legent,
}
public enum ItemTag
{
    Test
}

public interface ITypeGetable
{
    Vector2 GetMyType();
}
