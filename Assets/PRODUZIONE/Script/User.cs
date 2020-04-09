using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User
{

    public string userName="";
    public int userPunteggio=0;

    public User(string userName,int userPunteggio)
    {
        this.userName = userName;
        this.userPunteggio = userPunteggio;
    }

    public User()
    {

    }

}
