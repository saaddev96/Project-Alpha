using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UiManager : Singleton<UiManager>
{
    [Header("Interaction UI")]
    [SerializeField] private GameObject interactBG;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image interactFill;
    [SerializeField] private TextMeshProUGUI interactTextCounter;

    public GameObject InteractBG => interactBG;
    public TextMeshProUGUI InteractText => interactText;
    public Image InteractFill => interactFill;
    public TextMeshProUGUI InteractTextCounter => interactTextCounter;
}
