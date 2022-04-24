using UnityEngine;
using  System.Collections;

namespace HW1
{
    public class Unit : MonoBehaviour
    {
        private int _health;
        private Coroutine _doHeal;
    
        [SerializeField]private float _timeRemainedForHeal=3f;
        [SerializeField] private int _healRate = 5;
        [SerializeField] private float _healingFrequency = 0.5f;
        [SerializeField] private int _playerStartHP = 50;
        [SerializeField] private int _maxPlayerHealth = 100;
    
        private float _baseTimeRemainedForHeal;
        private bool _acceptingHeal;

        private void Awake()
        {
            _baseTimeRemainedForHeal = _timeRemainedForHeal;
            _health = _playerStartHP;
        }

        private void Start()
        {
            ReceiveHealing();
        }

        public void ReceiveHealing(/*int healRate*/)
        {
            if(_acceptingHeal) return;
            
            #if UNITY_EDITOR
            Debug.Log("Start healing");
            #endif
            
            _doHeal = StartCoroutine(DoHeal(_healRate));
            _acceptingHeal=true;
        }
    
        private IEnumerator DoHeal(int healRate)
        {
            while  (_timeRemainedForHeal>0 && _health<_maxPlayerHealth)
            {
                if((_health+healRate)>_maxPlayerHealth)
                {
                    _health=_maxPlayerHealth;
                    StopHeal();
                }
                else _health+=healRate;
            
                #if UNITY_EDITOR
                Debug.Log($"Player have now {_health} HP");
                #endif
                
                _timeRemainedForHeal-=_healingFrequency;
                yield return new WaitForSecondsRealtime(_healingFrequency);
            }
            StopHeal();
        }

        private void StopHeal()
        {
            _timeRemainedForHeal=_baseTimeRemainedForHeal;
            _acceptingHeal=false;

            StopCoroutine(_doHeal);
            #if UNITY_EDITOR
            Debug.Log("Stop healing");
            #endif
        }
    }
}