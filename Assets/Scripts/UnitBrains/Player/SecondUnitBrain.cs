using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Model;
using Model.Runtime.Projectiles;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Utilities;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;


        public Vector2Int MostDangerTarget = new Vector2Int();
        public Vector2Int UnitPositionHodNazad = new Vector2Int();
        static int counter = -1;
        int UnitID = counter++;
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {


            IncreaseTemperature();

            for (int i = 0; i < GetTemperature() && GetTemperature() < OverheatTemperature; i++)
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);

            }
        }

        public override Vector2Int GetNextStep()
        {
            
            if (UnitPositionHodNazad == unit.Pos) { return base.GetNextStep(); }
            else if (SelectTargets().Count > 0) { return unit.Pos; }
            else {UnitPositionHodNazad=unit.Pos; return unit.Pos.CalcNextStepTowards(MostDangerTarget); }
            
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override List<Vector2Int> SelectTargets()
        {
            List<Vector2Int> result = new List<Vector2Int>();
            List<Vector2Int> AllTargets = new List<Vector2Int>();
            int Target = UnitID;
            foreach(var target in GetAllTargets()) { AllTargets.Add(target); }

            AllTargets.Remove((runtimeModel.RoMap.Bases[IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId]));
            SortByDistanceToOwnBase(AllTargets);

            while (Target > 2) { Target -= 3; }
            while (Target > AllTargets.Count-1) { Target -= 1; }

            if (AllTargets.Count > 0) {  result.Add(AllTargets[Target]); }
            else { result.Add(runtimeModel.RoMap.Bases[IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId]); }

            if (IsTargetInRange(result[0])) { return result;  }
            else {  MostDangerTarget =result[0] ; result.Clear(); return result; }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown / 10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if (_overheated) return (int)OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }

    }
}