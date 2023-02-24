using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill1 : Skill
{
    public override void Use()
    {
        Debug.Log("Use DASH Skill");
        _invocator = GameObject.Find("Main");
        _invocator.GetComponent<Animator>().Play("Attacking");
    }
}
