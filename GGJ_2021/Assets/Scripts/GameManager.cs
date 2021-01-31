using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _arena;

    [SerializeField]
    private Material _targetMaterial;

    private List<GameObject> _packages = new List<GameObject>();
    private List<Renderer> _packageRenderers = new List<Renderer>();

    private int _winner = -1;

    void Start()
    {
        // retrieve all packages from the arena
        for (int i = 0; i < _arena.transform.childCount; ++i)
        {
            var child = _arena.transform.GetChild(i);
            if (child.GetComponent<Package>() == null)
                continue;
            
            _packages.Add(child.gameObject);
            _packageRenderers.Add(child.GetComponent<Renderer>());
        }

        // select a random winner
        _winner = Random.Range(0, _packages.Count - 1);
        _packages[_winner].gameObject.name = "WINNER";
        _packages[_winner].GetComponent<Package>().Winner = true;
        _packages[_winner].GetComponent<Renderer>().material = _targetMaterial;

        StartCoroutine(FlashWinner(startDelay: 5f, duration: 2f));
    }

    private IEnumerator FlashWinner(float startDelay, float duration)
    {
        yield return new WaitForSeconds(startDelay);

        UpdateAllRenderers(false);

        yield return new WaitForSeconds(duration);

        UpdateAllRenderers(true);
    }

    private void UpdateAllRenderers(bool state, bool excludeWinner = true)
    {
        for (int i = 0; i < _packageRenderers.Count; ++i)
        {
            if (excludeWinner && i == _winner)
                continue;

            if (_packageRenderers[i] != null)
                _packageRenderers[i].enabled = state;
        }
    }
}
