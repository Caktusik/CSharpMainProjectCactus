using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            

                IncreaseTemperature();       
            
                for(int i=0; i< GetTemperature() && GetTemperature() < OverheatTemperature ; i++)
                {
                    var projectile = CreateProjectile(forTarget);
                    AddProjectileToList(projectile, intoList);

                }
        }
        
        public override Vector2Int GetNextStep()
        {
            Vector2Int Target = new Vector2Int();
            Target = SelectTargets()[0];
            //4. В методе GetNextStep() нужно описать получение цели из списка целей. Если целей там нет или цель в области атаки, нужно вернуть позицию юнита.
            if (IsTargetInRange(Target)) 
            { 
                return unit.Pos;

            }
            //5. Если цель есть, но вне области атаки, в GetNextStep() вызвать у текущей позиции метод CalcNextStepTowards(), передав туда цель. 
            else 
            {
                return unit.Pos.CalcNextStepTowards(Target); 

            }

        }

        protected override List<Vector2Int> SelectTargets()
        {
            //1. Вместо достижимых целей получи все с помощью метода GetAllTargets().
            var AllTargets = GetAllTargets();

            //2a. Создаем новое поле для хранения целей, к которым нужно идти, но которые вне зоны досягаемости.
            List<Vector2Int> NoReachableTargets = new List<Vector2Int>();

            //2b. Записываем самую опасную цель в эту коллекцию. Если цель в зоне досягаемости, то добавляем в result.
            List<Vector2Int> result = new List<Vector2Int>();
            float max = float.MaxValue;
            var nearest = new Vector2Int();
            result.Clear();
            foreach (var target in AllTargets)
            {
                if (DistanceToOwnBase(target) < max)
                {
                    max = DistanceToOwnBase(target);
                    nearest=target;
                }
            }
                result.Clear();
                result.Add(nearest);
            
            //3. Если целей нет, добавляем в цели базу противника.
            if (result.Count==0)
            {
                result.Clear();
                result.Add(runtimeModel.RoMap.Bases[IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId]);
            }
            return result;
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
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
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}