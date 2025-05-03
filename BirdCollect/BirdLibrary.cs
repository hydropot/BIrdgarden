using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BirdLibrary", menuName = "BirdLibrary/New BirdLibrary")]
public class BirdLibrary : ScriptableObject
{
    public List<Bird> birdList = new List<Bird>();
}

