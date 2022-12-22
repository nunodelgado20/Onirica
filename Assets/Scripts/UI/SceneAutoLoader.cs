using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class SceneAutoLoader : MonoBehaviour
{
    [SerializeField] private float _waitTime = 12f;
    private LoadScene _loadScene;

    private void Awake()
    {
        _loadScene = GetComponent<LoadScene>();
    }

    private void Start()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return new WaitForSeconds(_waitTime);
        _loadScene.Load();
    }


}