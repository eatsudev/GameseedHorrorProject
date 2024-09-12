using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Plantation;

public class Plantation : Base_Interactable_Structure
{
    public static Plantation instance;
    public Transform plantPoint;
    public GameObject currPlantGO;
    
    public Flower flowerPrefab; 

    [Serializable]
    public class PlantStage
    {
        public GameObject plantStageModelPrefabs;
        public float growTime;
    }

    public List<PlantStage> plantStages;

    private int currStage;

    private Flower flowerToHarvest;

    private bool isWatered;
    private bool hasBeenHarvested;
    private bool seedIsPlanted = false;
    private bool isReadyToHarvest = false;

    private void Awake()
    {
        if (instance == null)
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

    }

    public override void Interact()
    {
        base.Interact();

        if (hasBeenHarvested)
        {
            Player_Hold_Manager.instance.WarningOnItem("Can no longer be used...");
            return;
        }

        if (Player_Hold_Manager.instance.IsHoldingItem())
        {
            Base_Holdable_Items heldItem = Player_Hold_Manager.instance.GetHeldItem();
            Seed seed = heldItem.GetComponent<Seed>();
            Water_Bucket bucket = heldItem.GetComponent<Water_Bucket>();
            if (seed)
            {
                if (!seedIsPlanted)
                {
                    StartGrowngSeed();

                    
                }
                else if(seedIsPlanted && !isWatered && !isReadyToHarvest)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Planted Seed need Water to Grow");
                    return;
                }
                else if(seedIsPlanted && isWatered && !isReadyToHarvest)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Seed is already Growing");
                    return;
                }
                else if(seedIsPlanted && isReadyToHarvest)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Flower is Ready to Harvest");
                    return;
                }
            }
            else if (bucket)
            {
                if(!seedIsPlanted)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Need Seed to Water");
                    return;
                }
                else if (seedIsPlanted && !isWatered && !isReadyToHarvest)
                {
                    if (bucket.IsFilledWithWater())
                    {
                        WateringSeed();
                        bucket.EmptyWater();

                        Player_Hold_Manager.instance.WarningOnItem("Planted Seed is Watered");
                    }
                    else
                    {
                        Player_Hold_Manager.instance.WarningOnItem("Bucket is Empty");
                    }
                    
                    return;
                }
                else if (seedIsPlanted && isWatered && !isReadyToHarvest)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Seed is already Growing");
                    return;
                }
                else if (seedIsPlanted && isReadyToHarvest)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Flower is Ready to Harvest");
                    return;
                }
            }
            else
            {
                if (!seedIsPlanted)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Need Seed to Grow");
                    return;
                }
                else if (seedIsPlanted && !isWatered && !isReadyToHarvest)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Planted Seed need Water to Grow");
                    return;
                }
                else if (seedIsPlanted && isWatered && !isReadyToHarvest)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Seed is not yet Grown");
                    return;
                }
                else if (seedIsPlanted && isReadyToHarvest)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Flower is Ready to Harvest");
                    return;
                }
            }
        }
        else
        {
            if (!seedIsPlanted)
            {
                Player_Hold_Manager.instance.WarningOnItem("Need Seed to Grow");
                return;
            }
            else if (seedIsPlanted && !isWatered && !isReadyToHarvest)
            {
                Player_Hold_Manager.instance.WarningOnItem("Planted Seed need Water to Grow");
                return;
            }
            else if (seedIsPlanted && !isReadyToHarvest)
            {
                Player_Hold_Manager.instance.WarningOnItem("Seed is not yet Grown");
                return;
            }
            if (seedIsPlanted && isReadyToHarvest)
            {
                Harvest();
                Player_Hold_Manager.instance.WarningOnItem("Flower is Harvested");
                return;
            }
        }
    }

    private void StartGrowngSeed()
    {
        currStage = 0;
        seedIsPlanted = true;
        isReadyToHarvest = false;
        Player_Hold_Manager.instance.ItemUsed();
        StartCoroutine(GrowSeed());
    }

    private void WateringSeed()
    {
        isWatered = true;
    }

    private IEnumerator GrowSeed()
    {
        foreach(PlantStage plantStage in plantStages)
        {
            currStage++;
            Destroy(currPlantGO);
            
            

            currPlantGO = Instantiate(plantStage.plantStageModelPrefabs, plantPoint.position, Quaternion.identity, plantPoint);
            currPlantGO.transform.position = plantPoint.position;

            if (currStage == plantStages.Count - 1)
            {
                break;
            }

            while (!isWatered && currStage == 1)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(plantStage.growTime);
        }

        isReadyToHarvest = true;
    }

    private void Harvest()
    {
        Destroy(currPlantGO);

        currPlantGO = Instantiate(plantStages[currStage].plantStageModelPrefabs, plantPoint.position, Quaternion.identity, plantPoint);
        currPlantGO.transform.position = plantPoint.position;


        flowerToHarvest = Instantiate(flowerPrefab, plantPoint.position, Quaternion.identity, plantPoint);
        Player_Hold_Manager.instance.PickUpItem(flowerToHarvest);

        hasBeenHarvested = true;

        currStage = 0;
        seedIsPlanted = false;
        isWatered = false;
        isReadyToHarvest = false;
    }
}
