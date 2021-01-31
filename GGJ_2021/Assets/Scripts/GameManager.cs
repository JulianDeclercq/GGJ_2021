using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject _arena;

    [SerializeField]
    private Material _targetMaterial;

    [SerializeField]
    private float _pushCooldown;

    [SerializeField]
    private Text _previewCounterText;

    [SerializeField]
    private float _waitForSpawnDelay = 3f;

    [SerializeField]
    private List<GameObject> _enableAfterSpawnDelay;

    [SerializeField]
    private GameObject _winScreen;

    public bool InputEnabled = false;

    private float _currentPushCooldown = 0f;


    public float CooldownPercentage
    {
        get => _currentPushCooldown / _pushCooldown;
    }

    public bool OnCooldown
    {
        get => _currentPushCooldown > 0f;
    }

    private List<Package> _packages = new List<Package>();
    
    private bool _previewing = false;
    private int _previewsLeft = 4;

    void Start()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError($"GameManager instance already exists, destroying object {gameObject.name}");
            Destroy(this);
            return;
        }

        if (!_arena.activeSelf)
        {
            Debug.LogError($"Inactive arena selected!");
        }

        // retrieve all packages from the arena
        for (int i = 0; i < _arena.transform.childCount; ++i)
        {
            var child = _arena.transform.GetChild(i);
            Package p = child.GetComponent<Package>();
            if (p == null)
            {
                Debug.LogError($"Child of arena did not have a package component {child.name}");
                continue;
            }

            _packages.Add(p);
        }

        // select a random winner, winner can only be of type 5 (only winner texture)
        var possibleWinners = _packages.Where(x => x.Type == 5).ToList();
        Package winner = possibleWinners[Random.Range(0, possibleWinners.Count)];

        winner.gameObject.name = "WINNER";
        winner.Winner = true;
        winner.Renderer.material = _targetMaterial;

        //StartCoroutine(FlashWinner(startDelay: 5f, duration: 2f));
        UpdatePreviewCounter();

        StartCoroutine(WaitForSpawn(_waitForSpawnDelay));
    }

    private void UpdateAllRenderers(bool state, bool excludeWinner = true)
    {
        foreach (Package p in _packages)
        {
            if (p == null) // TODO: does this happen when destroyed by the void or just never?
                continue;

            if (excludeWinner && p.Winner)
                continue;
            
            p.Renderer.enabled = state;
        }
    }

    // TODO: restructure this so it's not in the gamemanager <:)
    public void ResetPushCooldown()
    {
        _currentPushCooldown = _pushCooldown; 
    }

    private void FixedUpdate()
    {
        if (_currentPushCooldown > 0)
        {
            _currentPushCooldown -= Time.fixedDeltaTime;
            if (_currentPushCooldown < 0)
                _currentPushCooldown = 0;
        }
    }

    private void Update()
    {
        // Reset if ctrl + R is pressed
        bool ctrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (ctrlPressed && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // load different levels as player
        if (ctrlPressed)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneManager.LoadScene(1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                SceneManager.LoadScene(2);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                SceneManager.LoadScene(3);
            }
        }

        if (!InputEnabled)
            return;

        if (!_previewing)
        {
            if (Input.GetKeyDown(KeyCode.P) && _previewsLeft > 0)
                StartCoroutine(FlashWinner());
        }
    }
    
    private IEnumerator FlashWinner(float startDelay = 0f, float duration = 2f)
    {
        yield return new WaitForSeconds(startDelay);

        _previewing = true;
        --_previewsLeft;
        UpdatePreviewCounter();

        UpdateAllRenderers(false);

        yield return new WaitForSeconds(duration);

        UpdateAllRenderers(true);

        _previewing = false;
    }

    private IEnumerator WaitForSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var go in _enableAfterSpawnDelay)
            go.SetActive(true);

        InputEnabled = true;
    }

    private void UpdatePreviewCounter()
    {
        _previewCounterText.text = $"Press P for preview ({_previewsLeft})";
    }

    public void WinScreen()
    {
        _winScreen.SetActive(true);
    }
}
