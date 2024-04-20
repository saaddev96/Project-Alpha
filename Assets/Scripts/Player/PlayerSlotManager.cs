using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlotManager : MonoBehaviour
{
    [SerializeField] private slot bareHand;
    [SerializeField] private slot primary;
    [SerializeField] private slot secondery;
}
[System.Serializable]
public class slot
{
    public Item item;
}