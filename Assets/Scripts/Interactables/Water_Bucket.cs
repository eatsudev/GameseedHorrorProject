using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Bucket : Base_Holdable_Items
{
    public GameObject currModel;

    public GameObject emptyBucketPrefab;
    public GameObject filledBucketPrefab;

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
        GameObject newGameObject = Instantiate(filledBucketPrefab, currModel.transform.position, currModel.transform.rotation, currModel.transform.parent);

        Destroy(currModel);

        currModel = newGameObject;

        isFilledWithWater = true;
    }

    public void EmptyWater()
    {
        GameObject newGameObject = Instantiate(emptyBucketPrefab, currModel.transform.position, currModel.transform.rotation, currModel.transform.parent);

        Destroy(currModel);

        currModel = newGameObject;

        isFilledWithWater = false;
    }
}
