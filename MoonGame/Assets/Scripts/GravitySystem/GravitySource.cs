using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GravitySource : MonoBehaviour
{
    [ColorHeader("Invoking - Ask Link/Unlink to gravity body Channels", ColorHeaderColor.TriggeringEvents)] 
    [SerializeField] protected GravityBodySourceLinkEventChannelSO askLinkGravityBody;
    [SerializeField] protected GravityBodySourceLinkEventChannelSO askUnlinkGravityBody;

    [ColorHeader("Invoking - Ask Add/Remove Source Channels", ColorHeaderColor.TriggeringEvents)] 
    [SerializeField] protected GravitySourceEventChannelSO askAddGravitySource;
    [SerializeField] protected GravitySourceEventChannelSO askRemoveGravitySource;

    protected virtual void OnEnable()
    {
        askAddGravitySource.Raise(this);
        gameObject.layer = 8;
    }

    protected virtual void OnDisable()
    {
        askRemoveGravitySource.Raise(this);
    }

    public abstract Vector3 CalculateAcceleration(GravityBody body);
}
