using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UnitBrains
{
    internal class Cletka
    {
        public Vector2Int Pos;
        public string Direction;
        public Cletka(Vector2Int Pos, string Direction) 
        { 
            this.Pos = Pos;
            this.Direction = Direction;

        }
    }
}
