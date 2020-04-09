using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ListaUsers
{
    public List<User> utenti=new List<User>();

    public ListaUsers(List<User> utenti)
    {
        this.utenti = utenti;
    }

  
}
