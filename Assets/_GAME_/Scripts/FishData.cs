using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishData
{
    public string uid;
    public FishType fishType;
    public float vel; // Velocity
    public float resistence; // Chance to die from natural causes
    public int age; // Has higher chance to die as it gets older
    public float size; // The bigger the fish, the more it eats
    public bool isSwimming;

    public FishData(FishType fishType, float vel, float resistence, int age, float size)
    {
        uid = System.Guid.NewGuid().ToString();
        this.fishType = fishType;
        this.vel = vel;
        this.resistence = resistence;
        this.age = age;
        this.size = size;
    }


    public FishData DeepCopy()
    {
        return new FishData(this.fishType, this.vel, this.resistence, this.age, this.size);
    }

}

public enum FishType
{
    Tuna = 0,
    PufferFish = 1,
    Sardine = 2
}