using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementController : MonoBehaviour
{
    [SerializeField] Sprite[] element;
    [SerializeField] Image[] elementImage;
    [SerializeField] List<GameObject> elementGo = new List<GameObject>();

    public void InitElement(List<Elements> elements)
    {
        for (int i = 0; i < elementGo.Count; i++)
        {
            if (elements.Count > i)
            {
                elementGo[i].SetActive(true);
                elementImage[i].sprite = element[(int)elements[i]];
            }
            else
                elementGo[i].SetActive(false);
        }
    }
    public void CheckInputElement(List<Elements> elements, Elements currentElement)
    {
        int elemetnNumber = elements.Count;
        bool s = false;
        if (elements.Count > 0 &&
        elemetnNumber < 4 &&
        elements[elemetnNumber - 1] != currentElement &&
        !ComboElements(elements))
        {
            elements[elemetnNumber - 1] = CombineElements(elements[elemetnNumber - 1], currentElement);
            s = true;
        }
        if (elements.Count < 3 && !s)
        {
            elements.Add(currentElement);
        }
    }
    private bool ComboElements(List<Elements> SpellElements)
    {
        bool s = false;
        if (SpellElements[SpellElements.Count - 1] == Elements.LAVA ||
            SpellElements[SpellElements.Count - 1] == Elements.ELECTRO ||
            SpellElements[SpellElements.Count - 1] == Elements.FROST)
            return s = true;
        else return s = false;
    }
    private Elements CombineElements(Elements first, Elements second)
    {
        if (first == Elements.FIRE && second == Elements.EARTH || second == Elements.FIRE && first == Elements.EARTH)
        {
            return Elements.LAVA;
        }
        else if (first == Elements.WATER && second == Elements.EARTH || first == Elements.EARTH && second == Elements.WATER)
        {
            return Elements.FROST;
        }
        return Elements.ELECTRO;
    }
}
public enum Elements
{
    FIRE, EARTH, WATER, ELECTRO, LAVA, FROST
}