using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Bucket : Base_Holdable_Items
{
    public GameObject emptyBucketModelGO;
    public GameObject filledBucketModelGO;

    private bool isFilledWithWater = false;

    public override void Interact()
    {
        base.Interact();
    }

    public bool IsFilledWithWater()
    {
        return isFilledWithWater;
    }
    public void FillWithWater()
    {
        emptyBucketModelGO.SetActive(false);
        filledBucketModelGO.SetActive(true);

        isFilledWithWater = true;
    }

    public void EmptyWater()
    {
        emptyBucketModelGO.SetActive(true);
        filledBucketModelGO.SetActive(false);

        isFilledWithWater = false;
    }
}
