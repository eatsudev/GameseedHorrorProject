using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Well : Base_Interactable_Structure
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        base.Interact();

        Base_Holdable_Items playerHeldItem = Player_Hold_Manager.instance.GetHeldItem();

        if (!playerHeldItem) return;

        Water_Bucket bucket = playerHeldItem.GetComponent<Water_Bucket>();

        if (bucket)
        {
            GetWater(bucket);
        }
        else
        {
            Player_Hold_Manager.instance.WarningOnItem("Need Bucket to Get Water");
            return;
        }
    }

    private void GetWater(Water_Bucket bucket)
    {
        if (bucket.IsFilledWithWater())
        {
            Player_Hold_Manager.instance.WarningOnItem("Bucket is Already Full");
            return;
        }
        bucket.FillWithWater();
        Player_Hold_Manager.instance.WarningOnItem("Bucket is Filled");
    }
}
