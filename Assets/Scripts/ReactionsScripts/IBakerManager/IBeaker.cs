using UnityEngine;

public interface IBeaker
{
    MeshRenderer MainLiquid { get; }
    MeshRenderer[] LayeredLiquids { get; }
    Vector3 ReactionPosition { get; }
    void ResetBeaker();
}