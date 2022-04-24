using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private int _health;
    private Coroutine _doHeal;
    private float _timeRemained=3f;
    private bool _acceptingHeal=false;

    public void ReceiveHealing(/*int healRate*/)
    {
        if(_acceptingHeal) return;

        _doHeal = StartCoroutine(DoHeal(5));
        _acceptingHeal=true;
    }
    
    private IENumerator DoHeal(int healRate)
    {
        while  (_timeRemained>0 && _health<100)
        {
            if((_health+healRate)>100)
                {
                    _health=100;
                    StopHeal();
                }
            else _health+=healRate;
            
            _timeRemained-=0.5f;
            yield return new WaitForSecondsRealtime(0.5f);
        }
        StopHeal();
    }

    private void StopHeal()
    {
        _timeRemained=3f;
        _acceptingHeal=false;

        StopCoroutine(DoHeal);
    }
}