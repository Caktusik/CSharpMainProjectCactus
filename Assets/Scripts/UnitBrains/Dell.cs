using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UnitBrains;
using Model;
using Model.Runtime.Projectiles;
using Model.Runtime.ReadOnly;
using UnitBrains.Pathfinding;
using UnityEngine;
using Utilities;
using Unit = Model.Runtime.Unit;

namespace Assets.Scripts.UnitBrains
{
    internal class Dell
    {
        public Dell() { }
        public List<Cletka> delliter(List<Cletka> list)
        {
            List<Cletka> list2 = new List<Cletka>();
            foreach (Cletka c in list)
            {
                foreach(Cletka c2 in list)
                {
                    if (c2 == c)
                    {
                        list2.Add(c);
                    }
                }
            }
            foreach (Cletka c in list2)
            {
                list.Remove(c);
            }
            return list2;
        }
    }
}
