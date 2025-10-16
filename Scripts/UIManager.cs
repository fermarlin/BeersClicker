using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI beersTxt;
    private BeerClickManager beerClickMan;

    void Start(){
        beerClickMan = GetComponent<BeerClickManager>();
    }

    void Update(){
        beersTxt.text = beerClickMan.beerCount.ToString("F1");
    }
}
