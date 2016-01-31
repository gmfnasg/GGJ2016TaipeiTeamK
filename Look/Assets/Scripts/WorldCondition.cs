using System;
using System.Collections.Generic;

public enum ObjectId
{
    Ball = 0 ,
    Candle , 
    Book ,
    Doll ,


    MagicLight , 
    Door ,

    NumCondObject = 4,
}

public enum CondDir
{
    Front = 0,
    Right = 1,
    Back  = 2,
    Left  = 3,
    Top    = 4,
    Bottom = 5,
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


public class Utility
{
    public static int[] makeRandSeq(System.Random rand, int num, int start)
    {
        int[] result = new int[num];
        for (int i = 0; i < num; ++i)
        {
            result[i] = start + i;
        }
        for (int i = 0; i < num; ++i)
        {
            for (int j = 0; j < num; ++j)
            {
                int temp = result[i];
                int idx = rand.Next() % num;
                result[i] = result[idx];
                result[idx] = temp;
            }
        }
        return result;
    }

    public static bool[] makeRandBool(System.Random rand , int num , int numTrue )
    {
        bool[] result = new bool[num];
        for (int i = 0; i < num; ++i)
        {
            result[i] = (i < numTrue);
        }
        for (int i = 0; i < num; ++i)
        {
            for (int j = 0; j < num; ++j)
            {
                bool temp = result[i];
                int idx = rand.Next() % num;
                result[i] = result[idx];
                result[idx] = temp;
            }
        }
        return result;
    }
}

public struct WallInfo
{
    public WallName name;
    public ColorId color;
}

public struct ObjectInfo
{
    public ObjectId id;
    public ColorId color;
    public int num;
}

public class WorldCondition
{
    public WallInfo[] wall = new WallInfo[4];
    public int indexWallHaveLight;
    public int valueForNumberWall;

    public List<ObjectInfo> objects = new List<ObjectInfo>();

    const int MaxCondObjectNum = 10;

    bool[] bTopFireLighting = new bool[4];

    public WorldCondition()
    {
        wall[0].name = WallName.Bed;
    }

    public int getObjectNum( ObjectId id )
    {
        for( int i = 0 ; i < (int)ObjectId.NumCondObject ; ++i )
        {
            if (objects[i].id == id)
                return objects[i].num;
        }
        return 0;
    }
    public int getTopFireLightingNum() 
    {
        int result = 0;
        for( int i = 0 ; i < 4 ; ++i )
        {
            if (bTopFireLighting[i])
                ++result;
        }
        return result;
    }

    public bool isTopFireLighting( WallName nearWallName , CondDir dir , bool bFaceFront )
    {
        int idx = getWallIndex(nearWallName) + (int)dir;
        if (bFaceFront == false)
            idx += 2;
        return bTopFireLighting[idx % 4];
    }

    public bool isTopFireLighting(int idx)
    {
        return bTopFireLighting[idx];
    }

    public ColorId getObjectColor(ObjectId id)
    {
        for (int i = 0; i < (int)ObjectId.NumCondObject; ++i)
        {
            if (objects[i].id == id)
                return objects[i].color;
        }
        return ColorId.White;
    }

    public int getRelDirWallIndex( int idx , CondDir dir )
    {
        return (idx + (int)dir) % 4;
    }

    public bool checkVaild( WallName a , WallName b , CondDir dir )
    {
        int idxA = getWallIndex(a);
        int idxB = getWallIndex(b);
        return getRelDirWallIndex(idxA, dir) == idxB;
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
    public WallName getRelDirWall( WallName name , CondDir dir )
    {
        int idx = getWallIndex(name);
        idx = getRelDirWallIndex(idx, dir);
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

        indexWallHaveLight = rand.Next() % 4;
        valueForNumberWall = 1 + rand.Next() % 99;

        for(int i = 0 ; i < (int)ObjectId.NumCondObject ; ++i )
        {
            addObject( (ObjectId)i , 1 + rand.Next() % (MaxCondObjectNum - 1) , ColorId.White );
        }
        addObject(ObjectId.Door, 1, (ColorId)(rand.Next() % (int)ColorId.Num));
        addObject(ObjectId.MagicLight, 1, (ColorId)(rand.Next() % (int)ColorId.Num));
    }

    void addObject( ObjectId id , int num , ColorId color )
    {
        ObjectInfo info;
        info.id = id;
        info.num = num;
        info.color = ColorId.White;
        objects.Add(info);
    }
}