using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerController callingController);
    }
}
