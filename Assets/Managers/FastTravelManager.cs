using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FastTravelPoint
{
    public string SceneName;
    public Vector2 SaveLocation;
    public bool Unlocked = false;
}
public class FastTravelManager : MonoBehaviour
{
    public List<FastTravelPoint> FastTravel;

    public void UnlockSaveLocation(string Name)
    {
        for (int i = 0; i < FastTravel.Count; i++)
        {
            if (FastTravel[i].SceneName == Name)
            {
                FastTravel[i].Unlocked = true;
                break;
            }
        }
    }

    public bool isUnlocked(string Name)
    {
        for (int i = 0; i <= FastTravel.Count; i++)
        {
            if (FastTravel[i].SceneName == Name)
            {
                if (FastTravel[i].Unlocked == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

}
