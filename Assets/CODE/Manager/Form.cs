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

public enum ProductTag
{ 
    Gold,
    Star,
    Ruby
}

public interface ITypeGetable
{
    Vector2 GetMyType();
}
