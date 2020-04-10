using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User
{

    public string nome="";
    public int punti=0;

    public User(string userName,int userPunteggio)
    {
        this.nome = userName;
        this.punti = userPunteggio;
    }

    public User()
    {

    }

}
