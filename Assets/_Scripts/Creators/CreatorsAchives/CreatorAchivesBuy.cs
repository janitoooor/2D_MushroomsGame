using System.Collections.Generic;
using UnityEngine;

class CreatorAchivesBuy : CreatorItems
{
    [SerializeField] private List<AchivementItemBuy> _achivesItemBuy;

    [SerializeField] private Transform _transform;

    private readonly List<AchivementItemBuy> _createdAchives = new List<AchivementItemBuy>();
    public List<AchivementItemBuy> CreatedAchivesBuy { get => _createdAchives; }

    private void Start()
    {
        InstantiateStoreItems(_achivesItemBuy, _transform);

        foreach (var item in _createdItems)
            if (_createdItems != null)
                _createdAchives.Add((AchivementItemBuy)item);

        foreach (var item in _createdAchives)
            if (_createdItems != null)
                item.AchivesCreated += ChangeAchivement;

        SetStartAchivesItems();
        ActiveStartItems();
    }

    private protected override void ActiveStartItems()
    {
        foreach (var item in _createdAchives)
        {
            if (!item.ItemIsGetValue)
            {
                item.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void SetStartAchivesItems()
    {
        foreach (var item in _createdAchives)
            if (item.ItemIsGetValue)
                item.gameObject.SetActive(false);
    }

    private void ChangeAchivement(AchivementItemBuy achivementBuy)
    {
        achivementBuy.gameObject.SetActive(false);

        for (int i = 0; i < _createdAchives.Count - 1; i++)
            if (achivementBuy == _createdAchives[i])
                _createdAchives[i + 1].gameObject.SetActive(true);
    }
}
