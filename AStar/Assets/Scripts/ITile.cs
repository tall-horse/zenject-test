using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    (int X, int Z) Coordinates { get; }
    int Cost { get; }
}
