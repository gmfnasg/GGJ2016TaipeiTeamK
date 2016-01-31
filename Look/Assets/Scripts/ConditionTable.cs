using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public enum CondExprElemntType
{
    WallName,
    Object,
    Color,
    IntValue,
    Dir,
    FaceFront ,
}

public struct CondExprElement
{
    public CondExprElemntType type;
    public int meta;
}


public class CondExpression
{
    public CondExprElement[] elements;
    public int IdxContent;

    public virtual bool isVaild( WorldCondition worldCond){ return false; }
    public virtual void generate(System.Random rand){}
    public virtual void generateVaild(System.Random rand, WorldCondition worldCond) { }
    public virtual String getContent(){ return ""; }

    static public String toString( CondExprElement ele )
    {
        switch( ele.type )
        {
        case CondExprElemntType.Dir: return toString( (CondDir)ele.meta );
        case CondExprElemntType.WallName: return toString( (WallName)ele.meta );
        case CondExprElemntType.FaceFront:
                if (ele.meta == 0)
                    return "背對";
                return "面向";
        }
        return "Error Cond Element";
    }
    static public String toString( WallName name )
    {
        switch(name)
        {
        case WallName.Bed: return "床";
        case WallName.Door: return "門";
        case WallName.Clock: return "時鐘";
        case WallName.Number: return "數字牆";
        }
        return "Error Wall Name";
    }
    static public String toString( ObjectId id )
    {
        return "Error Object";
    }
    static public String toString( CondDir dir )
    {
        switch( dir )
        {
        case CondDir.Top: return "上";
        case CondDir.Left: return "左";
        case CondDir.Right: return "右";
        case CondDir.Bottom: return "下";
        case CondDir.Front: return "前";
        case CondDir.Back: return "後";
        }

        return "Error dir";
    }

}

class WallDirCondExpression : CondExpression
{
    private static CondDir[] dirMap = { CondDir.Left , CondDir.Right };
    public override bool isVaild( WorldCondition worldCond)
    {
        if (IdxContent == 0)
        {
            return worldCond.checkVaild((WallName)elements[0].meta, (WallName)elements[1].meta, (CondDir)elements[2].meta);
        }
        else
        {
            CondDir dir = (CondDir)elements[4].meta;
            int curIdx = worldCond.getWallIndex((WallName)elements[0].meta);
            for (int i = 1; i < 4; ++i)
            {
                int nextIdx = worldCond.getRelDirWallIndex( curIdx , dir );
                if (nextIdx != worldCond.getWallIndex((WallName)elements[0].meta))
                    return false;
            }
            return true;
        }
    }
    public override void generate(System.Random rand)
    {
        IdxContent = rand.Next() % 2;
        if (IdxContent == 0 )
        {
            int[] walllId = Utility.makeRandSeq( rand , (int)WallName.Num , 0 );

            elements = new CondExprElement[3];
            elements[0].type = CondExprElemntType.WallName;
            elements[0].meta = walllId[0];
    
            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = walllId[1];
  
            elements[2].type = CondExprElemntType.Dir;
            elements[2].meta = (int)dirMap[ rand.Next() % 2 ];
        }
        else
        {

            int[] walllId = Utility.makeRandSeq(rand, (int)WallName.Num, 0);

            elements = new CondExprElement[5];
            elements[0].type = CondExprElemntType.WallName;
            elements[0].meta = walllId[0];
            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = walllId[1];
            elements[2].type = CondExprElemntType.WallName;
            elements[2].meta = walllId[2];
            elements[3].type = CondExprElemntType.WallName;
            elements[3].meta = walllId[3];

            elements[4].type = CondExprElemntType.Dir;
            elements[4].meta = (int)dirMap[rand.Next() % 2];
        }
    }


    public override void generateVaild(System.Random rand ,  WorldCondition worldCond )
    {
        IdxContent = rand.Next() % 2;
        if (IdxContent == 0)
        {
            elements = new CondExprElement[3];
            elements[0].type = CondExprElemntType.WallName;
            elements[0].meta = rand.Next()%4;

            elements[2].type = CondExprElemntType.Dir;
            elements[2].meta = (int)dirMap[rand.Next() % 2];

            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = (int)worldCond.getRelDirWall( (WallName)elements[0].meta , (CondDir)elements[2].meta );
        }
        else
        {
            elements = new CondExprElement[5];
            int idx = rand.Next() % 4;
            CondDir dir = dirMap[rand.Next() % 2];

            elements[4].type = CondExprElemntType.Dir;
            elements[4].meta = (int)dir;

            for( int i = 0 ; i < 4 ; ++i )
            {
                elements[i].type = CondExprElemntType.WallName;
                elements[i].meta = (int)worldCond.wall[idx].name;
                idx = worldCond.getRelDirWallIndex(idx, dir);
            }
        }
    }
    public override String getContent()
    { 
        if (IdxContent==0)
        {
            return toString( elements[0] ) + "在" + toString( elements[1] ) + "的" + toString( elements[2] ) + "邊";
        }

        return toString(elements[0]) + "開始往" + toString(elements[4]) + "順序是"
            + toString(elements[1]) + "、"
            + toString(elements[2]) + "、"
            + toString(elements[3]);
    }

}


public class TopLightCondExpression : CondExpression
{

    static CondDir[] dirMap = { CondDir.Front, CondDir.Back, CondDir.Left, CondDir.Right };
    public override bool isVaild(WorldCondition worldCond)
    {
        if (IdxContent == 0)
        {
            return worldCond.isTopFireLighting((WallName)elements[1].meta, (CondDir)elements[0].meta, elements[2].meta != 0);
        }
        else 
        {
            return elements[0].meta == worldCond.getTopFireLightingNum();
        }
    }
    public override void generate(System.Random rand)
    {
        IdxContent = rand.Next() % 2;

        if (IdxContent == 0)
        {
            elements = new CondExprElement[3];
            elements[0].type = CondExprElemntType.FaceFront;
            elements[0].meta = rand.Next() % 2;

            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = rand.Next() % 4;

            elements[2].type = CondExprElemntType.Dir;
            elements[2].meta = (int)dirMap[ rand.Next() % dirMap.Length ];
        }
        else
        {
            elements = new CondExprElement[1];
            elements[0].type = CondExprElemntType.IntValue;
            elements[0].meta = rand.Next() % 5;
        }
        
    }
    public override void generateVaild(System.Random rand, WorldCondition worldCond)
    {
        IdxContent = rand.Next() % 2;

        if (IdxContent == 0)
        {
            elements = new CondExprElement[3];
            elements[0].type = CondExprElemntType.FaceFront;
            elements[0].meta = rand.Next() % 2;

            elements[2].type = CondExprElemntType.Dir;
            elements[2].meta = (int)dirMap[ rand.Next() % dirMap.Length ];

            List<int> idxLighting = new List<int>();
            for (int i = 0; i < 4; ++i )
            {
                if ( worldCond.isTopFireLighting(i) )
                {
                    idxLighting.Add(i);
                }
            }
            int idx = idxLighting[rand.Next() % idxLighting.Count];
            if (idx == 0)
                idx += 2;
            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = (int)worldCond.wall[ ( idx - elements[2].meta + 4 ) % 4 ].name;

        }
        else
        {
            elements = new CondExprElement[1];
            elements[0].type = CondExprElemntType.IntValue;
            elements[0].meta = worldCond.getTopFireLightingNum();
        }
    }
    public override String getContent()
    {
        if (IdxContent == 0)
        {
            return toString( elements[0] ) + toString( elements[1] ) + "向上看，燈亮的是" + toString( elements[2] );
        }
        else
        {
            return "有" + elements[0].meta.ToString() + "個燈亮";
        }
    }
}

public class WallColorCondExpression : CondExpression
{
    public override bool isVaild(WorldCondition worldCond) 
    {
        int idx = worldCond.getWallIndex((WallName)elements[1].meta);
        if (elements[0].meta == 0)
        {
            idx = (idx + 2) % 4;
        }
        return elements[2].meta == (int)worldCond.wall[idx].color;
    }
    public override void generate(System.Random rand) 
    {
        elements = new CondExprElement[3];
        elements[0].type = CondExprElemntType.FaceFront;
        elements[0].meta = rand.Next() % 2;

        elements[1].type = CondExprElemntType.WallName;
        elements[1].meta = rand.Next() % 4;

        elements[2].type = CondExprElemntType.Color;
        elements[2].meta = rand.Next() % (int)ColorId.Num;
    }
    public override void generateVaild(System.Random rand, WorldCondition worldCond) 
    {
        elements = new CondExprElement[3];
        elements[0].type = CondExprElemntType.FaceFront;
        elements[0].meta = rand.Next() % 2;

        elements[1].type = CondExprElemntType.WallName;
        elements[1].meta = rand.Next() % 4;

        int idx = worldCond.getWallIndex((WallName)elements[1].meta);
        if (elements[0].meta == 0)
        {
            idx = ( idx + 2 ) % 4;
        }
        elements[2].type = CondExprElemntType.Color;
        elements[2].meta = (int)worldCond.wall[idx].color;
    }
    public override String getContent() 
    {
        return toString(elements[0]) + toString(elements[1]) + "那面牆，顏色是" + toString( elements[2] ); 
    }
}

public class ObjectNumCondExpression : CondExpression
{
    public override bool isVaild(WorldCondition worldCond)
    {
        return elements[1].meta == worldCond.getObjectNum((ObjectId)elements[0].meta);
    }
    public override void generate(System.Random rand)
    {
        elements = new CondExprElement[2];
        elements[0].type = CondExprElemntType.Object;
        elements[0].meta = rand.Next() % (int)ObjectId.NumCondObject;

        elements[1].type = CondExprElemntType.IntValue;
        elements[1].meta = rand.Next() % 10;
    }
    public override void generateVaild(System.Random rand, WorldCondition worldCond)
    {
        elements = new CondExprElement[2];
        elements[0].type = CondExprElemntType.Object;
        elements[0].meta = rand.Next() % (int)ObjectId.NumCondObject;

        elements[1].type = CondExprElemntType.IntValue;
        elements[1].meta = worldCond.getObjectNum((ObjectId)elements[0].meta);
    }
    public override String getContent()
    {
        return "整個場景有" + toString(elements[1]) + "個" + toString(elements[0]);
    }
}

public class ObjectColorCondExpression : CondExpression
{

    ObjectId[] objectMap = { ObjectId.Door, ObjectId.MagicLight };
    public override bool isVaild(WorldCondition worldCond)
    {
        return elements[1].meta == (int)worldCond.getObjectColor((ObjectId)elements[0].meta);
    }
    public override void generate(System.Random rand)
    {
        elements = new CondExprElement[2];
        elements[0].type = CondExprElemntType.Object;
        elements[0].meta = (int)objectMap[ rand.Next() % objectMap.Length ];

        elements[1].type = CondExprElemntType.Color;
        elements[1].meta = rand.Next() % (int)ColorId.Num;
    }
    public override void generateVaild(System.Random rand, WorldCondition worldCond)
    {
        elements = new CondExprElement[2];
        elements[0].type = CondExprElemntType.Object;
        elements[0].meta = (int)objectMap[rand.Next() % objectMap.Length];

        elements[1].type = CondExprElemntType.Color;
        elements[1].meta = (int)worldCond.getObjectColor((ObjectId)elements[0].meta);
    }
    public override String getContent()
    {
        return "整個場景有" + toString(elements[1]) + "個" + toString(elements[0]);
    }
}


public class WallNumberValueCondExpression : CondExpression
{
    public override bool isVaild(WorldCondition worldCond)
    {
        return elements[0].meta == worldCond.valueForNumberWall;
    }
    public override void generate(System.Random rand)
    {
        elements = new CondExprElement[1];
        elements[0].type = CondExprElemntType.IntValue;
        elements[0].meta = 1 + rand.Next() % 99;
    }
    public override void generateVaild(System.Random rand, WorldCondition worldCond)
    {
        elements = new CondExprElement[1];
        elements[0].type = CondExprElemntType.IntValue;
        elements[0].meta = worldCond.valueForNumberWall;
    }
    public override String getContent()
    {
        return "在牆上的數字為" + elements[0].meta.ToString();
    }
}

public class Condition
{
    public const int TotalExprNum = 4;
    CondExpression[] exprList = new CondExpression[ TotalExprNum ];

    public bool bVaild;

    public String getContent(int idxExpr)
    {
        return exprList[idxExpr].getContent();
    }

    public CondExpression CreateExpression( int idx )
    {
        switch( idx )
        {
        case 0: return new WallDirCondExpression();
        case 1: return new WallColorCondExpression();
        case 2: return new ObjectNumCondExpression();
        case 3: return new ObjectColorCondExpression();
        case 4: return new TopLightCondExpression();
        case 5: return new WallNumberValueCondExpression();
        }
        return null;
    }

    public void generateVaild( System.Random rand , WorldCondition worldCond )
    {
        for( int i = 0 ; i < TotalExprNum ; ++i )
        {
            CondExpression expr = CreateExpression(i);
            if (expr!=null)
            {
                expr.generateVaild(rand, worldCond);
            }
            exprList[i] = expr;
        }
        bVaild = true;
    }

    public void generateRandom(System.Random rand , WorldCondition worldCond , int invVaildFactor)
    {
        int numInvaild = 1 + (TotalExprNum - 1);

        bool[] invaildMap = Utility.makeRandBool(rand, TotalExprNum, numInvaild); 
        for (int i = 0; i < TotalExprNum; ++i)
        {
            CondExpression expr = CreateExpression(i);
            if (expr != null)
            {
               if ( invaildMap[i] )
               {
                    do
                    {
                        expr.generate(rand);
                    }
                    while (expr.isVaild(worldCond) == true);
                }
                else
                {
                    expr.generateVaild( rand , worldCond );
                }
            }
            exprList[i] = expr;
        }

        bVaild = false;
    }
}

public class ConditionTable
{

    public void generate( System.Random rand , WorldCondition worldCond , int num , int numVaild )
    {
        numSelection = num;
        numConditionVaild = numVaild;

        conditions = new Condition[numSelection];

        bool[] vaildMap = Utility.makeRandBool( rand , num , numConditionVaild );
        for( int i = 0 ; i < numSelection ; ++i )
        {
            conditions[i] = new Condition();
            if ( vaildMap[i] )
            {
                conditions[i].generateRandom( rand , worldCond , 10 );
            }
            else
            {
                conditions[i].generateVaild( rand , worldCond );
            }
        }
    }

    public Condition getCondition(int idx) { return conditions[idx]; }
    public bool isVaildCondition(int idx)
    {
        return conditions[idx].bVaild;
    }
    
    public int numConditionVaild;
    public int numSelection;
    Condition[] conditions;
}