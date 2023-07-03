using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{

    // ½Ì±ÛÅæ
   public static ItemDatabase instance;
    private void Awake()
    {
        instance = this; 
    }

    // item class¿¡ ÀÖ´Â DB Á¤º¸
    public List<Item> itemDB = new List<Item>();
}
