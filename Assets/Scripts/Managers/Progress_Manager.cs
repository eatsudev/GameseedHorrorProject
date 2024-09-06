using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress_Manager : MonoBehaviour
{
    public static Progress_Manager instance;

    private int flowerBurned;

    private Flower flower;
    public Ritual_Circle ritual_Circle;
    public GameObject victoryScreen;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        ritual_Circle = Entities_Manager.Instance.ritual_Circle;
        
    }

    void Update()
    {
        
    }


    #region Changing Progress Variables
    
    public void AddFlowerBurned()
    {
        flowerBurned++;

        Entities_Manager.Instance.enemy.speedModifier += 0.2f;

        if(flowerBurned >= 10)
        {
            Victory();
        }
    }

    #endregion

    #region Getting Progress Variables

    public int FlowerBurned()
    {
        return flowerBurned;
    }

    #endregion

    #region Progress Changer

    private void Victory()
    {
        Destroy(Entities_Manager.Instance.enemy.gameObject);
        InGame_UI_Manager.Instance.victoryScreenUI.ShowVictoryScreen();
    }

    #endregion
}
