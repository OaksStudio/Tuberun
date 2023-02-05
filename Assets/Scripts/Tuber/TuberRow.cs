using System;
using System.Collections;
using System.Collections.Generic;
using Jozi.Pools;
using Sirenix.OdinInspector;
using UnityEngine;

public enum FarmType { Limited, Continuous }

public class TuberRow : MonoBehaviour
{
    public int ID => _ID;
    private int _ID;

    [Header("Setup")]
    public int PoolExtra = 10;

    public Transform RowHolder;
    public float RowOffset = 1;

    public bool Clear => _tubers.Count <= 0 && _selectedTuber == null;
    public Tuber SelectedTuber => _selectedTuber;

    private Queue<Tuber> _tubers;
    private long _currentSlot = 0;
    [ReadOnly, SerializeField] private Tuber _selectedTuber;
    private bool _stoppedRow;

    public Action OnClearRow;
    public Action<Tuber> OnReleaseTuber;

    [Header("Gizmos")]
    public Color color = Color.green;
    public Vector3 size = Vector3.one;

    public void Setup(int id, List<SOTuber> tubers, FarmType farmType, int rowSize = 10)
    {
        _ID = id;
        _tubers = new Queue<Tuber>();

        foreach (var tuber in tubers)
        {
            PoolCommand.Warm(tuber.TuberPrefab.gameObject, rowSize + PoolExtra);
        }

        for (int i = 0; i < rowSize; i++)
        {
            GenerateTuber(tubers);
        }

        GameManager.Instance.Pullers[ID].OnPull += TryPullerPull;

        _stoppedRow = false;
    }

    private void OnDestroy()
    {
        GameManager.Instance.Pullers[ID].OnPull -= TryPullerPull;
    }

    private void GenerateTuber(List<SOTuber> tubers)
    {
        int randomTuberIndex = UnityEngine.Random.Range(0, tubers.Count);
        CreateTuber(tubers[randomTuberIndex]);
    }

    private void CreateTuber(SOTuber tubers)
    {
        Tuber tuber = PoolCommand.GetObject(tubers.TuberPrefab.gameObject).GetComponent<Tuber>();
        tuber.Setup(ID, tubers);

        if (_stoppedRow)
        {
            tuber.StopTuber();
        }

        _tubers.Enqueue(tuber);

        Vector3 position = RowHolder.transform.position + Vector3.right * (float)_currentSlot * RowOffset;
        tuber.transform.position = position;

        _currentSlot++;
    }

    public Tuber GetTuber()
    {
        if (_selectedTuber) return _selectedTuber;

        _selectedTuber = _tubers.Dequeue();

        _selectedTuber.OnRelease += RemoveTubber;
        return _selectedTuber;
    }

    public void RemoveTubber(Tuber tuber)
    {
        if (tuber.GetInstanceID() != _selectedTuber.GetInstanceID()) return;

        OnReleaseTuber?.Invoke(tuber);

        tuber.OnRelease -= RemoveTubber;
        _selectedTuber = null;

        if (_tubers.Count <= 0)
        {
            OnClearRow?.Invoke();
        }
    }

    private void TryPullerPull(InputActions direction, float pullForce)
    {
        if (PauseManager.Instance.IsPaused) return;
        GetTuber().TryPull(direction, pullForce);
    }


    public void StopRow()
    {
        if (_stoppedRow) return;
        _stoppedRow = true;

        _selectedTuber?.StopTuber();

        for (int i = 0; i < _tubers.Count; i++)
        {
            Tuber tuber = _tubers.Dequeue();
            tuber.StopTuber();
            _tubers.Enqueue(tuber);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 lastPosition;
        bool started = false;
        for (int i = 0; i < 10; i++)
        {
            Vector3 position = RowHolder.transform.position + Vector3.right * (float)i * RowOffset;

            if (!started) started = true;
            Gizmos.color = color;
            Gizmos.DrawWireCube(position, size);

            lastPosition = position;
        }

    }
#endif
}
