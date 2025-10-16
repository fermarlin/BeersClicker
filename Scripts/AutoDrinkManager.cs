using UnityEngine;

public class AutoDrinkManager : MonoBehaviour
{
    private UpgradeManager upgradeMan;
    private BeerClickManager beerClickMan;
    private float autoDrinkBeers=0;
    private float timer=0;

    void Start(){
        upgradeMan = GetComponent<UpgradeManager>();
        beerClickMan = GetComponent<BeerClickManager>();
        upgradeMan.OnHabilityChanged += OnHabilityChangedHandler;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            beerClickMan.beerCount += autoDrinkBeers;
            timer = 0f;
        }
    }



    void OnHabilityChangedHandler(int index, int newValue)
    {
        if(index==2){
            autoDrinkBeers+=1;
        }
    }
}
