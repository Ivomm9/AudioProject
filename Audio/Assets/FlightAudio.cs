using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class FlightAudio : MonoBehaviour
{
    [SerializeField] private AudioSource m_PSpeedSource;
    private CancellationTokenSource _cancellationTokenSource;
    private bool isTaskRunning = false;
    [SerializeField] private AnimationCurve m_SpeedCurve;
    [SerializeField] FlyBehaviour flyBehaviour;
    bool once = false;
    bool onceHold = true;
    void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();

    }

    void Update()
    {
        if (flyBehaviour.fly)
        {
            once = true; 

            if (onceHold)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = new CancellationTokenSource();

                    PSpeedAudio(_cancellationTokenSource.Token, true, 1).Forget();
                }
                onceHold = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();

                PSpeedAudio(_cancellationTokenSource.Token, true, 1).Forget();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();

                PSpeedAudio(_cancellationTokenSource.Token, false, 0.5f).Forget();
            }
        }
        else
        {
            if (once)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();
                PSpeedAudio(_cancellationTokenSource.Token, false, 0.5f).Forget();
                once = false;
                onceHold = true;
            }

        }
    }

    private async UniTask PSpeedAudio(CancellationToken _cancellationToken, bool start, float duration)
    {
        if (m_PSpeedSource == null) return;

        isTaskRunning = true;

        if (start)
        {
            float initialVolume = m_PSpeedSource.volume;
            float initialPitch = m_PSpeedSource.pitch;
           // m_PSpeedSource.Play();

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float ratio = t / duration;
                m_PSpeedSource.volume = Mathf.Lerp(initialVolume, 1f, ratio);
                m_PSpeedSource.pitch = Mathf.Lerp(initialPitch, 1.5f, ratio);

                if (_cancellationToken.IsCancellationRequested)
                {
                    isTaskRunning = false;
                    return;
                }
                await UniTask.Yield(cancellationToken: _cancellationToken);
            }

            m_PSpeedSource.volume = 1f;
            m_PSpeedSource.pitch = 1.5f;
        }
        else
        {
            float startVolume = m_PSpeedSource.volume;
            float startPitch = m_PSpeedSource.pitch;
            float endTime = Time.time + duration;

            while (Time.time < endTime)
            {
                float ratio = 1 - ((endTime - Time.time) / duration);
                m_PSpeedSource.volume = Mathf.Lerp(startVolume, 0f, ratio);
                m_PSpeedSource.pitch = Mathf.Lerp(startPitch, 1f, ratio);

                if (_cancellationToken.IsCancellationRequested)
                {
                    isTaskRunning = false;
                    return;
                }
                await UniTask.NextFrame(cancellationToken: _cancellationToken);
            }

            m_PSpeedSource.volume = 0f;
            m_PSpeedSource.pitch = 1f;
      //      m_PSpeedSource.Stop();
        }

        isTaskRunning = false;
    }
}