using System;
using System.Collections.Generic;

[Serializable]
public class LeaderboardsModel
{
    public List<LeaderboardItem> Items;
}

[Serializable]
public class LeaderboardItem
{
    public string name;
    public int score;
}
