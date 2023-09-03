using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestNPC : MonoBehaviour
{
    private bool _playerNear, _timercontinue, _cooldown, _canCooldown;
    private GameObject _prompt;
    private Slider _promptCooldown;
    private float _timer, _maxTime, _secsToCooldown;
    [SerializeField] private ObjectSettings _settings;
    [SerializeField] private TextAsset _dialogue;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _prompt = transform.GetChild(0).transform.GetChild(0).gameObject;
        _promptCooldown = _prompt.GetComponent<Slider>();
        _maxTime = _settings.MaxTimeToPressDown;
        _prompt.SetActive(false);
        _canCooldown = _settings.CanCooldown;
        _secsToCooldown = _settings.WaitAfterCooldown;
    }

    private void Update()
    {
        _timer = Mathf.Clamp(_timer, 0, _maxTime);
        
        _promptCooldown.value = _timer;
        _promptCooldown.maxValue = _maxTime;

        if (_cooldown || DialogueManager.GetInstance()._dialogueIsPlaying)
        {
            _prompt.SetActive(false);
            return;
        }

        if (_playerNear)
        {
            _prompt.SetActive(true);
        }
        else
        {
            _prompt.SetActive(false);
        }

        if (_timercontinue)
        {
            _timer += 0.3f;
        }
    }

    private void OnEnable()
    {
        PlayerMovement._confirmed += AddTimer;
        PlayerMovement._canceledconfirmed += StopTimer;
    }

    private void OnDisable()
    {
        PlayerMovement._confirmed -= AddTimer;
        PlayerMovement._canceledconfirmed -= StopTimer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNear = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNear = false;
            _timer = 0;
        }
    }

    void AddTimer()
    {
        if (_playerNear)
        {
            _timercontinue = true;
        }
    }

    void StopTimer()
    {
        if (_timer >= _maxTime)
        {
            Announce();
        }
        _timercontinue = false;
        _timer = 0;
    }

    void Announce()
    {
        if (_playerNear)
        {
            DialogueManager.GetInstance().EnterDialogueMode(_dialogue);
        }
        else
        {
            Debug.Log("Player not near");
        }
    }

    IEnumerator CoolingDown()
    {
        _cooldown = true;
        yield return new WaitForSeconds(_secsToCooldown);
        _cooldown = false;
    }
}
