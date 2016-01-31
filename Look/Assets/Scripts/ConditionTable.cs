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

    public virtual bool testVaild( WorldCondition worldCond){ return false; }
    public virtual void generate(System.Random rand){}
    public virtual void generateVaild(System.Random rand, WorldCondition worldCond) { }
    public virtual String getContent(){ return ""; }

    static public String toString( ColorId id )
    {
        switch( id )
        {
         case ColorId.Red: return "紅";
         case ColorId.White: return "白";
         case ColorId.Yellow: return "黃";
         case ColorId.Green: return "綠";
        }
        return "Error Color";
    }

    static public String toString(ObjectId id)
    {
        switch (id)
        {
            case ObjectId.Ball: return "球";
            case ObjectId.Candlestick: return "燭台";
            case ObjectId.Book: return "書";
            case ObjectId.Doll: return "兔娃娃";
            case ObjectId.Lantern: return "燈籠";
            case ObjectId.MugCube: return "方塊";
        }
        return "Error Object";
    }


    static public String toString( CondExprElement ele )
    {
        switch( ele.type )
        {
        case CondExprElemntType.Dir: return toString( (CondDir)ele.meta );
        case CondExprElemntType.WallName: return toString( (WallName)ele.meta );
        case CondExprElemntType.Color: return toString((ColorId)ele.meta);
        case CondExprElemntType.Object: return toString((ObjectId)ele.meta);
        case CondExprElemntType.IntValue: return ele.meta.ToString();
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
    public override bool testVaild( WorldCondition worldCond)
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
    public override bool testVaild(WorldCondition worldCond)
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
            return "有" + toString( elements[0] ) + "個燈亮";
        }
    }
}

public class WallColorCondExpression : CondExpression
{
    public override bool testVaild(WorldCondition worldCond) 
    {
        if (IdxContent == 0)
        {
            int idx = worldCond.getWallIndex((WallName)elements[1].meta);
            if (elements[0].meta == 0)
            {
                idx = (idx + 2) % 4;
            }
            return elements[2].meta == (int)worldCond.wall[idx].color;
        }

        return false;
    }
    public override void generate(System.Random rand)
    {
        //IdxContent = rand.Next() % 2;
        IdxContent = 0;
        if (IdxContent == 0)
        {
            elements = new CondExprElement[3];
            elements[0].type = CondExprElemntType.FaceFront;
            elements[0].meta = rand.Next() % 2;

            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = rand.Next() % 4;

            elements[2].type = CondExprElemntType.Color;
            elements[2].meta = rand.Next() % (int)ColorId.Num;
        }
        else
        {
            elements = new CondExprElement[5];
            elements[0].type = CondExprElemntType.FaceFront;
            elements[0].meta = rand.Next() % 2;

            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = rand.Next() % 4;

            elements[2].type = CondExprElemntType.Color;
            elements[2].meta = rand.Next() % (int)ColorId.Num;

            elements[3].type = CondExprElemntType.Dir;
            if ( (rand.Next() % 2 ) == 0 )
                elements[3].meta = (int)CondDir.Left;
            else
                elements[3].meta = (int)CondDir.Right;

            elements[4].type = CondExprElemntType.IntValue;
            elements[4].meta = 1 + (rand.Next() % 3);
        }
    }
    public override void generateVaild(System.Random rand, WorldCondition worldCond) 
    {
        //IdxContent = rand.Next() % 2;
        IdxContent = 0;
        if (IdxContent == 0)
        {
            elements = new CondExprElement[3];
            elements[0].type = CondExprElemntType.FaceFront;
            elements[0].meta = rand.Next() % 2;

            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = rand.Next() % 4;

            int idx = worldCond.getWallIndex((WallName)elements[1].meta);
            idx = worldCond.getRelDirIndex(idx, CondDir.Front, elements[0].meta != 0);
            elements[2].type = CondExprElemntType.Color;
            elements[2].meta = (int)worldCond.wall[idx].color;
        }
        else
        {
            elements = new CondExprElement[3];
            elements[0].type = CondExprElemntType.FaceFront;
            elements[0].meta = rand.Next() % 2;

            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = rand.Next() % 4;


            int idx = worldCond.getWallIndex((WallName)elements[1].meta);
            idx = worldCond.getRelDirIndex(idx, CondDir.Front, elements[0].meta != 0);

            elements[3].type = CondExprElemntType.Dir;
            if ( (rand.Next() % 2 ) == 0 )
                elements[3].meta = (int)CondDir.Left;
            else
                elements[3].meta = (int)CondDir.Right;

            elements[4].type = CondExprElemntType.IntValue;
            elements[4].meta = 1 + (rand.Next() % 3);

            elements[2].type = CondExprElemntType.Color;
            elements[2].meta = (int)worldCond.wall[idx].color;
        }
    }
    public override String getContent() 
    {
        if (IdxContent == 0)
        {
            return toString(elements[0]) + toString(elements[1]) + "那面牆，顏色是" + toString(elements[2]);
        }

        return toString(elements[0]) + toString(elements[1]) + "向" + toString(elements[3]) + "的第" + toString(elements[4]) + toString(elements[2]);
    }
}

public class ObjectNumCondExpression : CondExpression
{
    public override bool testVaild(WorldCondition worldCond)
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
    public override bool testVaild(WorldCondition worldCond)
    {
        return elements[1].meta == (int)worldCond.getObjectColor(objectMap[elements[0].meta]);
    }
    public override void generate(System.Random rand)
    {
        elements = new CondExprElement[2];
        elements[0].type = CondExprElemntType.IntValue;
        elements[0].meta = rand.Next() % objectMap.Length;

        elements[1].type = CondExprElemntType.Color;
        elements[1].meta = rand.Next() % (int)ColorId.Num;
    }
    public override void generateVaild(System.Random rand, WorldCondition worldCond)
    {
        elements = new CondExprElement[2];
        elements[0].type = CondExprElemntType.IntValue;
        elements[0].meta = rand.Next() % objectMap.Length;

        elements[1].type = CondExprElemntType.Color;
        elements[1].meta = (int)worldCond.getObjectColor(objectMap[elements[0].meta]);
    }
    public override String getContent()
    {
        if (elements[0].meta == 0)
        {
            return "魔法陣的蠟燭火焰顏色是" +  toString(elements[1]);
        }
        else
        {
            return "門的顏色是" + toString(elements[1]);
        }
    }
}


public class WallNumberValueCondExpression : CondExpression
{
    public override bool testVaild(WorldCondition worldCond)
    {
        return ( ( 1 << elements[0].meta ) & worldCond.valuePropertyFlag ) != 0;
    }
    public override void generate(System.Random rand)
    {
        elements = new CondExprElement[1];
        elements[0].type = CondExprElemntType.IntValue;
        elements[0].meta = rand.Next() % (int)ValueProperty.Num;
    }
    public override void generateVaild(System.Random rand, WorldCondition worldCond)
    {
        List<int> props = new List<int>();
        for (int i = 0; i < (int)ValueProperty.Num; ++i )
        {
            if ( ( (1 << i) & worldCond.valuePropertyFlag ) != 0 )
                props.Add(i);
        }
        elements = new CondExprElement[1];
        elements[0].type = CondExprElemntType.IntValue;
        elements[0].meta = props[ rand.Next() % props.Count ];

    }
    public override String getContent()
    {
        switch( (ValueProperty)elements[0].meta )
        {
        case ValueProperty.PrimeNumber: return "在牆上的數字是質數";
        case ValueProperty.DividedBy4: return "在牆上的數字是四的倍數";
        case ValueProperty.DSumIsBiggerThan10: return "在牆上的數字，十位數字加個位數字超過10";
        case ValueProperty.DOIsBiggerThanDT: return "在牆上的數字，個位數字比十位數字大";
        }
        return "Error Content";
    }
}

public class Condition
{
    public const int TotalExprNum = 6;
    CondExpression[] exprList;

    public ObjectId targetId;

    public bool bVaild;

    public int getExprissionNum() { return exprList.Length; }

    public String getTarget() { return CondExpression.toString(targetId); }
    public String getContent(int idxExpr)
    {
        return exprList[idxExpr].getContent();
    }
    public bool testVaild( WorldCondition worldCond , int idxExpr )
    {
        return exprList[idxExpr].testVaild(worldCond);
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

    public void generateVaild(System.Random rand, WorldCondition worldCond, int numExpr)
    {
        int idx = 0;
        exprList = new CondExpression[numExpr];
        bool[] useExprMap = Utility.makeRandBool(rand, TotalExprNum, numExpr); 
        for( int i = 0 ; i < TotalExprNum ; ++i )
        {
            if (useExprMap[i] == false)
                continue;

            CondExpression expr = CreateExpression(i);
            if (expr!=null)
            {
                expr.generateVaild(rand, worldCond);
            }
            exprList[idx] = expr;
            ++idx;
        }
        bVaild = true;
    }

    public void generateRandom(System.Random rand, WorldCondition worldCond, int numExpr, int numInvaild)
    {
        bool[] invaildMap = Utility.makeRandBool(rand, TotalExprNum, numInvaild);
        bool[] useExprMap = Utility.makeRandBool(rand, TotalExprNum, numExpr );

        int idx = 0;
        exprList = new CondExpression[numExpr];
        for (int i = 0; i < TotalExprNum; ++i)
        {
            if (useExprMap[i] == false)
                continue;

            CondExpression expr = CreateExpression(i);
            if (expr != null)
            {
               if ( invaildMap[i] )
               {
                    do
                    {
                        expr.generate(rand);
                    }
                    while (expr.testVaild(worldCond) == true);
                }
                else
                {
                    expr.generateVaild( rand , worldCond );
                }
            }
            exprList[idx] = expr;
            ++idx;
        }

        bVaild = false;
    }
}

public class ConditionTable
{

    public void generate( System.Random rand , WorldCondition worldCond , int numSel , int numExpr , int numVaild )
    {
        numSelection = numSel;
        numConditionVaild = numVaild;

        conditions = new Condition[numSelection];

        int[] objectIdMap = Utility.makeRandSeq( rand , 6 , 0 );
        bool[] vaildMap = Utility.makeRandBool( rand , numSelection , numConditionVaild );
        for( int i = 0 ; i < numSelection ; ++i )
        {
            conditions[i] = new Condition();
            if ( vaildMap[i] )
            {
                int numInvaild = 1;
                int additionInvaild = numExpr - 1;
                if (additionInvaild > 0)
                    numInvaild += rand.Next() % additionInvaild;
                conditions[i].generateRandom(rand, worldCond, numExpr, numInvaild );
            }
            else
            {
                conditions[i].generateVaild( rand , worldCond , numExpr );
            }

            conditions[i].targetId = (ObjectId)objectIdMap[i];
        }
    }

    public bool isVaildObject( ObjectId id )
    {
        for( int i = 0 ; i < numSelection ; ++i )
        {
            if (conditions[i].targetId == id )
            {
                return conditions[i].bVaild;
            }
        }
        return false;
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