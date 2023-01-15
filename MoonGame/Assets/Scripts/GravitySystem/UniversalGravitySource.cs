using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UniversalGravitySource : GravitySource
{
    [ColorHeader("Listening - On Gravity Body Enable/Disable Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private GravityBodyEventChannelSO onBodyEnabled;
    [SerializeField] private GravityBodyEventChannelSO onBodyDisabled;
    
    [ColorHeader("Config", ColorHeaderColor.Config)] 
    [SerializeField] private Vector3 gravityAccel;

    protected override void OnEnable()
    {
        base.OnEnable();
        onBodyEnabled.OnRaised += AddBody;
        onBodyDisabled.OnRaised += RemoveBody;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        onBodyEnabled.OnRaised -= AddBody;
        onBodyDisabled.OnRaised -= RemoveBody;
    }

    private void AddBody(GravityBody body)
    {
        askLinkGravityBody.Raise(body, this);
    }
    
    private void RemoveBody(GravityBody body)
    {
        askUnlinkGravityBody.Raise(body, this);
    }

    public override Vector3 CalculateAcceleration(GravityBody body)
    {
        return gravityAccel;
    }
}
