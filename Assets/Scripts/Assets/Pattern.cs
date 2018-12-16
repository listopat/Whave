using UnityEngine;

[System.Serializable]
public enum Side
{
    Left,
    Right
}

[System.Serializable]
public class WaveBeat
{
    public int offset;
    public Side side;
}


[CreateAssetMenu]
public class Pattern : ScriptableObject
{
    public WaveBeat[] waves;
    public bool randomizableSpeed;
}
