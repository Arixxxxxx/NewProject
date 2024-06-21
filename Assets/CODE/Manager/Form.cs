using UnityEngine;

public enum RankType
{
    Rare,
    Epic,
    Legent,
}
public enum ItemTag
{
    Atk,
    Critical,
    QuestGold,
    CriticalDmg,
    FeverTime,
    QuestDiscount,
    WeaponDiscount,
    AtkSpeed,
    GetStar,
    KillGold,
}

public enum ProductTag
{ 
    Gold,
    Star,
    Ruby,
    Money,
    MiniGameTicket
}

public enum SpMissionTag
{ 
    Quest,
    Weapon,
    Relic
}

public enum PetType
{ 
    Bomb,
    Panda,
    Necromancer,
}

public enum DailyMissionTag
{ 
    VisitShop,
    UseRuby,
    KillMonster,
    DialymissionClear
}

public enum WeeklyMissionTag
{ 
    DailyMissionAllClear,
    Reincarnation,
    QuestLvUp,
    WeaponUpgrade
}

public interface IClickLvUpAble
{
    void ClickUp()
    {
        Debug.Log("Å¬¸¯Áß~");
    }
}
