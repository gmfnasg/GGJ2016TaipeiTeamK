using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

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


        }
        return false; 
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
            int[] walllId = Utility.makeRandSeq(rand, (int)WallName.Num, 0);

            elements = new CondExprElement[3];
            elements[0].type = CondExprElemntType.WallName;
            elements[0].meta = rand.Next()%4;

            elements[2].type = CondExprElemntType.Dir;
            elements[2].meta = (int)dirMap[rand.Next() % 2];

            elements[1].type = CondExprElemntType.WallName;
            elements[1].meta = (int)worldCond.getNeighborWall( (WallName)elements[0].meta , (CondDir)elements[2].meta );
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
                idx = worldCond.getNeighborWallIndex(idx, dir);
            }
        }
    }
    public override String getContent()
    { 
        if (IdxContent==0)
        {
            return toString( elements[0] ) + "在" + toString( elements[0] ) + "的" + toString( elements[2] ) + "邊";
        }

        return toString(elements[0]) + "開始往" + toString(elements[4]) + "順序是"
            + toString(elements[1]) + "、"
            + toString(elements[2]) + "、"
            + toString(elements[3]);
    }

}


public class Condition
{

    private const int TotalExprNum = 1;
    CondExpression[] exprList = new CondExpression[ TotalExprNum ];

    public String getContent(int idxExpr)
    {
        return exprList[idxExpr].getContent();
    }
    public CondExpression CreateExpression( int idx )
    {
        switch( idx )
        {
        case 0: return new WallDirCondExpression();
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
    }

    public void generateRandom(System.Random rand , WorldCondition worldCond , int invVaildFactor)
    {
        int numInvaild = 1 + (TotalExprNum - 1);
        for (int i = 0; i < TotalExprNum; ++i)
        {
            CondExpression expr = CreateExpression(i);
            if (expr != null)
            {
                bool bGenInvaild = false;
                if ( numInvaild > 0 )
                {
                    if (numInvaild <= TotalExprNum - i )
                    {
                        bGenInvaild = true;
                
                    }
                    else if ( ( rand.Next() % ( TotalExprNum - i ) ) < numInvaild )
                    {
                        bGenInvaild = true;
                    }
               }
               if (bGenInvaild )
               {
                    do
                    {
                        expr.generate(rand);
                    }
                    while (expr.isVaild(worldCond) == true);
                    --numInvaild;
                }
                else
                {
                    expr.generateVaild( rand , worldCond );
                }
                expr.generate(rand);
            }
            exprList[i] = expr;
        }
    }
}

public class ConditionTable
{

    public void generate( System.Random rand , WorldCondition worldCond , int num )
    {
        numSelection = num;

        conditions = new Condition[numSelection];

        int idxVaild = rand.Next() % numSelection;
        for( int i = 0 ; i < numSelection ; ++i )
        {
            conditions[i] = new Condition();
            if ( i == idxVaild )
            {
                conditions[i].generateRandom( rand , worldCond , 10 );
            }
            else
            {
                conditions[i].generateVaild( rand , worldCond );
            }
        }
    }


    int numSelection;
    Condition[] conditions;
}