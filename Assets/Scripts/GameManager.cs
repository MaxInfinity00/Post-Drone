using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _totalPlayTimeInSeconds = 800.0f;

    [SerializeField] private Animation _timerAnimation;

    private DropoffPoint[] _dropOffPoints;
    private bool _shouldTimerStop = false;
    private float _currentTime = 0.0f;
    private float _totalPlayTimeElapsed;
    private Timer _timer = null;
    private string _timerText = "";
    private string _rightHandSide = "";
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        _dropOffPoints = FindObjectsOfType<DropoffPoint>();
        foreach (DropoffPoint dropoffPoint in _dropOffPoints)
        {
            dropoffPoint.PackageDelivered += AddTime;
        }
        _timer = FindObjectOfType<Timer>();
        _currentTime = _totalPlayTimeInSeconds;
    }

    private void SetTimerText()
    {
        if (_currentTime % 60 < 10)
        {
            _rightHandSide = "0" + (int)_currentTime % 60;
        }
        else
        {
            _rightHandSide = ((int)_currentTime % 60).ToString();
        }
        _timerText = (int)_currentTime / 60 + " : " + _rightHandSide;
        _timer.SetText(_timerText);
    }


    public void AddTime(float time)
    {
        _currentTime += time;
        _timerAnimation.Play();
    }

    private void FixedUpdate()
    {
        if (!_shouldTimerStop)
        {
            _currentTime -= Time.fixedDeltaTime;
            _totalPlayTimeElapsed += Time.fixedDeltaTime;
        }
        if (_currentTime < 0)
        {
            _shouldTimerStop = true;
            _currentTime = 0.1f;
            UIManager.instance.WinLoseScreen(false, 0f);
        }
        SetTimerText();
    }

    public int GetTimeElapsed()
    {
        return (int)_totalPlayTimeElapsed;
    }
}