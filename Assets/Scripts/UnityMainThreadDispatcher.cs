﻿using HoloIslandVis;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : SingletonComponent<UnityMainThreadDispatcher>
{
    private ConcurrentQueue<IEnumerator> _executionQueue;

    private void Start()
        => _executionQueue = new ConcurrentQueue<IEnumerator>();

    private void Update()
    {
        IEnumerator coroutine;
        if(!_executionQueue.IsEmpty && _executionQueue.TryDequeue(out coroutine))
            StartCoroutine(coroutine);
    }

    public void Enqueue(Action action)
        => _executionQueue.Enqueue(actionWrapper(action));

    public void Enqueue<T>(Action<T> action, T arg)
        => _executionQueue.Enqueue(actionWrapper(action, arg));

    private IEnumerator actionWrapper(Action action)
    {
        action.Invoke();
        yield return null;
    }

    private IEnumerator actionWrapper<T>(Action<T> action, T arg)
    {
        action.Invoke(arg);
        yield return null;
    }
}
