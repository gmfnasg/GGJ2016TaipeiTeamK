using System;

public enum ObjectId
{
    Bed = 0,
    Clock,
    Desk,

    Wall,
    Window,
    Lamp,
}

public enum CondDir
{
    Top = 0,
    Bottom,
    Left,
    Right,
}


public enum WallName
{
    Bed,
    Number,
    Door,
    Clock,

    Num ,
}
public enum ColorId
{
    Red = 0,
    Yellow,
    Green,
    White,

    Num,
}

public struct WallInfo
{
    public WallName name;
    public ColorId color;
}

public enum CondExprElemntType
{
    WallName,
    Object,
    Color,
    Number,
    Dir,
}

public struct CondExprElement
{
    public CondExprElemntType type;
    public int meta;
}

public enum CondExprTemplate
{
    TempA,
    TempB,
    Num,
}


public class Utility
{
    public static int[] makeRandSeq(System.Random rand, int num, int start)
    {
        int[] result = new int[num];
        for (int i = 1; i < num; ++i)
        {
            result[i] = start + i;
        }
        for (int i = 1; i < num; ++i)
        {
            for (int j = 1; i < num; ++j)
            {
                int temp = result[i];
                int idx = rand.Next() % num + 1;
                result[i] = result[idx];
                result[idx] = temp;
            }
        }
        return result;
    }
}

class WorldCondition
{
    public WallInfo[] wall = new WallInfo[4];
    WorldCondition()
    {
        wall[0].name = WallName.Bed;
    }

    public int getNeighborWallIndex( int idx , CondDir dir )
    {
        if (dir == CondDir.Right)
        {
            return (idx + 1) % 4;
        }
        return (idx + 3) % 4;
    }
    public bool checkVaild( WallName a , WallName b , CondDir dir )
    {
        int idxA = getWallIndex(a);
        int idxB = getWallIndex(b);
        return getNeighborWallIndex(idxA, dir) == idxB;
    }
    public int getWallIndex(WallName name)
    {
        for(int i=0;i<4;++i)
        {
            if (wall[i].name == name)
                return i;
        }
        return -1;
    }
    public WallName getNeighborWall( WallName name , CondDir dir )
    {
        int idx = getWallIndex(name);
        idx = getNeighborWallIndex(idx, dir);
        return wall[idx].name;
    }


    public void generate(System.Random rand)
    {
        int[] wallId = Utility.makeRandSeq(rand, 3, 1);
        for (int i = 0; i < 4; ++i)
        {
            if (i == 0)
            {
                wall[i].name = WallName.Bed;
            }
            else
            {
                wall[i].name = (WallName)wallId[i - 1];
            }
            wall[i].color = (ColorId)(rand.Next() % (int)ColorId.Num);
        }
    }
}