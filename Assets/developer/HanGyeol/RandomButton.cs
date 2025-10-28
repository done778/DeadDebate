using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomButton : MonoBehaviour
{
    [SerializeField] private Transform[] slots;
    [SerializeField] private GameObject[] buttonPrefabs;


    public void Refresh()
    {
        ClearSlots();

        for (int i = 0; i < slots.Length; i++)
        {
            int r = Random.Range(0, buttonPrefabs.Length);
            var go = Instantiate(buttonPrefabs[r], slots[i]);

           
            var rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.localScale = Vector3.one;
            }
        }
    }

    private void ClearSlots()
    {
        foreach (var s in slots)
            for (int i = s.childCount - 1; i >= 0; i--)
                Destroy(s.GetChild(i).gameObject);
    }
}


