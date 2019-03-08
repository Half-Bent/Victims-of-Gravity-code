using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextController : MonoBehaviour {

    private static DamageText damageTextPrefab;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if(!damageTextPrefab)
            damageTextPrefab = Resources.Load<DamageText>("DamageParent");
    }

	public static void CreateDamageText(string value, Transform location)
    {
        DamageText instance = Instantiate(damageTextPrefab);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetValue(value);
    }
}
