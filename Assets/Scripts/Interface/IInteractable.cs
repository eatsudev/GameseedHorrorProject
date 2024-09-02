using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    Base_Holdable_Items Get_Base_Holdable_Item();
    void Interact();
}
