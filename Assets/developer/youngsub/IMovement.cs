using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement 
{
    public void Move(Vector3 dir);
    public void LookRotate(Vector3 dir);
}
