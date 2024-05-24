using System.Collections;
using System.Collections.Generic;
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
using UnitBrains.Player;
using static UnityEngine.GraphicsBuffer;
public class ThirdUnitBrain : DefaultPlayerUnitBrain
{
    public override string TargetUnitName => "Ironclad Behemoth";
    private bool IsMuving = true;
    private bool IsShuting = false;
    private bool IsTimerGooingMuvToShut = false;
    private bool IsTimerGooingShutToMuv = false;
    public Vector2Int MostDangerTarget = new Vector2Int();
    float TimerMaxCount = 0.3f;
    float Timer = 0.3f;
    enum mod{stop,go,shut}
    
    mod Mod = mod.go;
    protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
    {   
        if (Mod==mod.shut)
        {
            var projectile = CreateProjectile(forTarget);
            AddProjectileToList(projectile, intoList);
        }
    }

    public override Vector2Int GetNextStep()
    {
        
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            Timer = TimerMaxCount;
            Debug.Log("������ ������");
        }

        if (SelectTargets().Count > 0&&Mod!=mod.shut)
        {
            Mod = mod.stop;
        }
            
        else if (SelectTargets().Count == 0 && Mod != mod.go) 
        {
            Mod = mod.stop;
        }
        if (Mod == mod.stop)
        {
            Timer -= Time.deltaTime;
            if(Timer <= 0)
            {
                Timer = TimerMaxCount;
                if (SelectTargets().Count > 0) { Mod = mod.shut; }
                else { Mod = mod.go; }
                Debug.Log("������������ ������");
            }
        }




        if (Mod!=mod.go) { return unit.Pos; }
        else if (SelectTargets().Count > 0) { return unit.Pos; }
        else { return unit.Pos.CalcNextStepTowards(MostDangerTarget); }
    }

    protected override List<Vector2Int> SelectTargets()
    {
        
        List<Vector2Int> AllTargets =new List<Vector2Int>();
        List<Vector2Int> result = new List<Vector2Int>();

        foreach (var target in GetAllTargets()) { AllTargets.Add(target); }
        AllTargets.Remove((runtimeModel.RoMap.Bases[IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId]));
        SortByDistanceToOwnBase(AllTargets);

        if (AllTargets.Count > 0) { result.Add(AllTargets[0]); }
        else { result.Add(runtimeModel.RoMap.Bases[IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId]); }

        if (IsTargetInRange(result[0])) { return result; }
        else { MostDangerTarget = result[0]; result.Clear(); return result; }
    }
}
