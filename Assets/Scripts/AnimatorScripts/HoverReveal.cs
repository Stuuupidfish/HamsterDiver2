using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverReveal : MonoBehaviour
{
    private ObjectAnimator objAnimator;
    [SerializeField] private GameObject obj;
    private bool isHovered;
    // Start is called before the first frame update
    void Start()
    {
        objAnimator = GetComponent<ObjectAnimator>();
        isHovered = objAnimator.IsHovered;
    }

    // Update is called once per frame
    void Update()
    {
        isHovered = objAnimator.IsHovered;
        //Debug.Log(isHovered);
        if (isHovered)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }
}
