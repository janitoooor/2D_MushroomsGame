using UnityEngine;

public class AuthBonus : MonoBehaviour
{
    public static AuthBonus Instance { get; private set; }

    [SerializeField] private GameObject _button;
    [SerializeField] private long _bonusGems = 100;

    private readonly GemBank _gemBank = GemBank.GetInstance();
    private bool _gemsAdded = false;
    public bool GemsAdded { get => _gemsAdded; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        JsonSaveSystem.Instance.LoadAuthBonus(this);

        if (_gemsAdded)
            _button.SetActive(false);
    }

    public void AuthBonusGems()
    {
        if (_gemsAdded)
            return;

        _gemBank.AddGems(_bonusGems);
        _button.SetActive(false);
        _gemsAdded = true;
        JsonSaveSystem.Instance.SaveAuthBonus(this);
    }

    public void LoadData(bool gemsAdded)
    {
        _gemsAdded = gemsAdded;
    }
}
