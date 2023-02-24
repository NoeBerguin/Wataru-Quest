using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSkill : Skill
{

    public override void Update()
    {

    }
    // Start is called before the first frame update
    public override void Use()
    {
        _invocator.GetComponent<Animator>().Play("Attacking");
        GameObject spell = Instantiate(_skillPrefab, GameObject.Find("Main").transform.position, Quaternion.identity);
        // spell.GetComponent<SpikeSpell>().direction = GameObject.Find("Main").transform.position - _invocator.transform.position;
        // spell.GetComponent<SpikeSpell>().invocator = _invocator;
    }

}
