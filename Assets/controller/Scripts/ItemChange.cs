using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class ItemChange : MonoBehaviour
    {
        [Header("Item Change")]
        [SerializeField] public Animator ani;
        [SerializeField] bool LoopItems = true;
        [SerializeField, Tooltip("You can add your new item here.")] GameObject[] Items;
        [SerializeField] int ItemIdInt;
        int MaxItems;
        int ChangeItemInt;
        [HideInInspector] public bool DefiniteHide;
        private void Start()
        {
            if (ani == null && GetComponent<Animator>()) ani = GetComponent<Animator>();
            DefiniteHide = false;
            ChangeItemInt = ItemIdInt;
            MaxItems = Items.Length - 1;
            StartCoroutine(ItemChangeObject());
        }
        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                ItemIdInt++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                ItemIdInt--;
            }
            if(Input.GetKeyDown(KeyCode.H))
            {
                if (ani.GetBool("Hide")) Hide(false);
                else Hide(true);
            }
            if (ItemIdInt < 0) ItemIdInt = LoopItems ? MaxItems : 0;
            if (ItemIdInt > MaxItems) ItemIdInt = LoopItems ? 0 : MaxItems;
            if (ItemIdInt != ChangeItemInt)
            {
                ChangeItemInt = ItemIdInt;
                StartCoroutine(ItemChangeObject());
            }
        }
        public void Hide(bool Hide)
        {
            DefiniteHide = Hide;
            ani.SetBool("Hide", Hide);
        }
        IEnumerator ItemChangeObject()
        {
            if(!DefiniteHide) ani.SetBool("Hide", true);
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < (MaxItems + 1); i++)
            {
                Items[i].SetActive(false);
            }
            Items[ItemIdInt].SetActive(true);
            if (!DefiniteHide) ani.SetBool("Hide", false);
        }
    }
}
