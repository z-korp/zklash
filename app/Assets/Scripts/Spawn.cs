using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Role role;
    private GameObject mob;
    
    void Update()
    {
        if (role != Role.None)
        {
            if (mob != null)
            {
                Destroy(mob);
            }
            mob = MobFactory.instance.Create(role, transform);
            role = Role.None;
        }
    }

    public void SetRole(Role _role)
    {
        role = _role;
    }
}
