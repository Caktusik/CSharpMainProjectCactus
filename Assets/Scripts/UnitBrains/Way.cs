using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace Assets.Scripts.UnitBrains
{
    internal class Way
    {
        IReadOnlyRuntimeModel runtimeModel;
        Vector2Int targetPos;
        Vector2Int startPos;

        bool IsWayFound=false;

        Vector2Int left = new Vector2Int(-1, 0);
        Vector2Int right = new Vector2Int(1, 0);
        Vector2Int down = new Vector2Int(0,1);
        Vector2Int up = new Vector2Int(0, -1);
        List<Cletka> AlredyChecked = new List<Cletka>();
        public Way(IReadOnlyRuntimeModel runtimeModel, Vector2Int targetPos,Vector2Int startPos)
        {

            this.runtimeModel = runtimeModel;
            this.targetPos = targetPos;
            this.startPos = startPos;
        }

        public bool CheckingOnAnotherCletka(Cletka cletka)
        {

            foreach (Cletka other in AlredyChecked)
            {
                if (other == cletka) return false;
            }
            return true;

        }
        public bool ChecigOnWalkable(Cletka cletka)
        {
            if (runtimeModel.IsTileWalkable(cletka.Pos))
            {
                return true;
            }
            return false;
        }
        public bool ChecihgOnCorrect(Cletka cletka)
        {
            return ChecigOnWalkable(cletka)&&CheckingOnAnotherCletka(cletka);
        }
        public List<Cletka> ChekingNeigbors(Cletka cletka)
        {
            List<Cletka> Result = new List<Cletka>();
            if (ChecihgOnCorrect(new Cletka((cletka.Pos + left), "right"))) { Result.Add(new Cletka((cletka.Pos + left), "right")); }
            if (ChecihgOnCorrect(new Cletka((cletka.Pos + up), "down"))) { Result.Add(new Cletka((cletka.Pos + up), "down")); }
            if (ChecihgOnCorrect(new Cletka((cletka.Pos + right), "left"))) { Result.Add(new Cletka((cletka.Pos + right), "left")); }
            if (ChecihgOnCorrect(new Cletka((cletka.Pos + down), "up"))) { Result.Add(new Cletka((cletka.Pos + down), "up")); }
            return Result;
        }
        public bool IsNeigborAreTarget(Cletka cletka)
        {
            if (cletka.Pos + left == targetPos|| cletka.Pos + right == targetPos|| cletka.Pos + up == targetPos|| cletka.Pos + down == targetPos) { return true; }
            return false;
        }
        public Cletka ooo()
        {
            Dell dell = new Dell();
            AlredyChecked.Add(new Cletka(startPos, "0" ));
            
            for (int i = 0; i < 100; i++)
            {
                int Count = AlredyChecked.Count();
                for (int j = 0; j < Count;j++ )
                {
                    if (IsNeigborAreTarget(AlredyChecked[j]))
                    {
                        return AlredyChecked[j];
                    }
                    else
                    {
                        AlredyChecked.AddRange<Cletka>(ChekingNeigbors(AlredyChecked[j]));                     
                    }
                }
                AlredyChecked=(dell.delliter(AlredyChecked));
            }
            return null;
        }
        public Cletka WayMaker()
        {
            Cletka current;
            if (ooo() != null)
            {
                AlredyChecked = new List<Cletka>();
                current = ooo();
                for (int i = 0; i < 100; i++)
                {
                    if (current.Pos + left == startPos || current.Pos + right == startPos || current.Pos + up == startPos || current.Pos + down == startPos)
                    {
                        return current;
                    }
                    else if (current.Direction == "up") { current.Pos += up; }
                    else if (current.Direction == "down") { current.Pos += down; }
                    else if (current.Direction == "right") { current.Pos += right; }
                    else if (current.Direction == "left") { current.Pos += left; }
                }
            }
            if(ooo()==null) { Debug.Log("А"); }
            return null;
        }
    }
}
