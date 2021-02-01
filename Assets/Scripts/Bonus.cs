using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bonus
{
    [SerializeField]
    private int duration;                           // Время, в течение которого бонус можно собрать
    [SerializeField]
    private int reward;                             // Сколько монет дает

    public int Duration
    {
        get { return duration; }
    }
    public int Reward
    {
        get { return reward; }
    }

    public Bonus(int _duration, int _reward)
    {
        duration = _duration;
        reward = _reward;
    }
}
