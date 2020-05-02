using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UserPadre
{

    public string id = "";
    public User user;

    public UserPadre(string id, User user)
    {
        this.id = id;
        this.user = user;
    }

    public UserPadre()
    {

    }

}
