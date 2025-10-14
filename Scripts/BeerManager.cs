using UnityEngine;
using TMPro;

public class BeerManager : MonoBehaviour
{
    public TextMeshProUGUI beersTxt;
    public int beerCount=0;

    public void DrinkBeer(int value){
        beerCount+=value;
    }

    void Update(){
        beersTxt.text = beerCount.ToString();
    }
}
