using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySystemManager : MonoBehaviour
{
    [ColorHeader("Listening - Add/Remove GravitySources Ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private GravitySourceEventChannelSO askedAddGravitySource;
    [SerializeField] private GravitySourceEventChannelSO askedRemoveGravitySource;

    [ColorHeader("Listening - Add/Remove GravityObject Ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private GravityBodySourceLinkEventChannelSO askedLinkGravityBody;
    [SerializeField] private GravityBodySourceLinkEventChannelSO askedUnlinkGravityBody;

    private Dictionary<GravitySource, HashSet<GravityBody>> sourceToBodyMap = new();
    private Dictionary<GravityBody, int> sourceInteractionCounter = new();

    private void OnEnable()
    {
        askedLinkGravityBody.OnRaised += LinkGravityBody;
        askedUnlinkGravityBody.OnRaised += UnlinkGravityBody;

        askedAddGravitySource.OnRaised += AddGravitySource;
        askedRemoveGravitySource.OnRaised += RemoveGravitySource;
    }

    private void OnDisable()
    {
        askedLinkGravityBody.OnRaised -= LinkGravityBody;
        askedUnlinkGravityBody.OnRaised -= UnlinkGravityBody;
        
        askedAddGravitySource.OnRaised -= AddGravitySource;
        askedRemoveGravitySource.OnRaised -= RemoveGravitySource;
    }

    private void FixedUpdate()
    {
        RebuildGravityForces(Time.fixedDeltaTime);
    }

    private void RebuildGravityForces(float timeStep)
    {
        foreach (var pair in sourceInteractionCounter)
        {
            pair.Key.ResetGravityCache();
        }
        
        foreach (var pair in sourceToBodyMap)
        {
            foreach (var body in pair.Value)
            {
                Vector3 accel = pair.Key.CalculateAcceleration(body);
                body.AddAccel(accel);
            }
        }
        
        foreach (var pair in sourceInteractionCounter)
        {
            pair.Key.FinishCalculatingAccel();
            pair.Key.ApplyGravity(timeStep);
        }
    }
    
    private void AddGravitySource(GravitySource source)
    {
        sourceToBodyMap.TryAdd(source, new HashSet<GravityBody>());
    }
    
    private void RemoveGravitySource(GravitySource source)
    {
        if(sourceToBodyMap.ContainsKey(source))
            sourceToBodyMap.Remove(source);
    }
    
    private void LinkGravityBody(GravityBody body, GravitySource source)
    {
        sourceToBodyMap.TryAdd(source, new HashSet<GravityBody>());
        var set = sourceToBodyMap[source];
        if (!set.Contains(body))
        {
            set.Add(body);
            sourceInteractionCounter.TryAdd(body, 0);
            sourceInteractionCounter[body]++;
        }
    }
    
    private void UnlinkGravityBody(GravityBody body, GravitySource source)
    {
        sourceToBodyMap.TryAdd(source, new HashSet<GravityBody>());
        var set = sourceToBodyMap[source];
        if (set.Contains(body))
        {
            set.Remove(body);
            bool hasKey = sourceInteractionCounter.ContainsKey(body);
            if (hasKey)
            {
                sourceInteractionCounter[body]--;
            }
            // Remove if this body has no interactions left
            if (!hasKey || sourceInteractionCounter[body] <= 0)
            {
                sourceInteractionCounter.Remove(body);
            }
        }
    }
}
