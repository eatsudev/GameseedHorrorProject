using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress_Manager : MonoBehaviour
{
    public static Progress_Manager instance;

    private int flowerBurned;
    public Ritual_Circle ritual_Circle;

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

    }

    #endregion
}
