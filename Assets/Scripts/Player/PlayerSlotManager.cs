using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlotManager : MonoBehaviour
{
    public static PlayerSlotManager Instance;
    [SerializeField] private slot bareHand;
    [SerializeField] private slot primary;
    [SerializeField] private slot secondery;

    private InputReader _inputReader;
    private Item currentItem;
    private Item PreviousItem;
    private Item defaultItem => bareHand.item;

    public Item CurrentItem => currentItem;
    private void Awake()
    {
        if(Instance== null)
        {
            Instance = this;
        }
        _inputReader = GetComponent<InputReader>();
    }
    private void OnEnable()
    {
        _inputReader.OnprimarySelectedEvent+= SetPrimaryItem;
        _inputReader.OnBareHandsSelectedEvent += SetDefaultItem;
    }
    private void OnDisable()
    {
        _inputReader.OnprimarySelectedEvent -= SetPrimaryItem;
        _inputReader.OnBareHandsSelectedEvent -= SetDefaultItem;
    }
    private void Start()
    {
        SetDefaultItem();
    }

    IEnumerator SetSlotItem(Item item)
    {
        if (currentItem == null)
        {
            
            currentItem = item;
            yield return StartCoroutine(currentItem.DrawItem());
            currentItem.OnActive();
       
        }
        else if (currentItem != item)
        {
            PreviousItem = currentItem;
            yield return StartCoroutine(currentItem.HolsterItem());
            currentItem.OnInactive();
            currentItem = item;
            yield return StartCoroutine(currentItem.DrawItem());
            currentItem.OnActive();
        }
            yield return null ;
    }
    void SetPrimaryItem()
    {
     
       StartCoroutine(SetSlotItem(primary.item));
    }
    void SetDefaultItem()
    {
        StartCoroutine(SetSlotItem(bareHand.item));
    }
   
}


[System.Serializable]
public class slot
{
    public Item item;
}