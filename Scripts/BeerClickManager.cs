using UnityEngine;

public class BeerClickManager : MonoBehaviour
{
    [HideInInspector]
    public float beerCount=0;
    private float valueToDrink=1;
    private float multiplierToDrink=1;
    private UpgradeManager upgradeMan;

    void Start(){
        upgradeMan = GetComponent<UpgradeManager>();
        upgradeMan.OnHabilityChanged += OnHabilityChangedHandler;
    }

    public void DrinkBeer(){
        beerCount+=valueToDrink*multiplierToDrink;
    }

    void OnHabilityChangedHandler(int index, int newValue)
    {
        Debug.Log($"Habilidad {index} subió a nivel {newValue}");
        if(index==0){
            valueToDrink+=0.1f;
        }
        if(index==1){
            multiplierToDrink+=2f;
        }
    }
}
