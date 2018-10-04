using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPopur : MonoBehaviour
{
    [SerializeField] private Image[] imagesIcons;
    [SerializeField] private Text[] textsLables;
    [SerializeField] private Text currentTextLabe;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button useButton;

    private string currItem;
    public void refresh()
    {
        List<string> itemlist = Managers.inventory.getItemList();
        int len = imagesIcons.Length;
        for (int i = 0; i < len; i++)
        {
            if (i < itemlist.Count)
            {
                imagesIcons[i].gameObject.SetActive(true);
                textsLables[i].gameObject.SetActive(true);

                string item = itemlist[i];
                Sprite sprt = Resources.Load<Sprite>("Icons/" + item);
                imagesIcons[i].sprite = sprt;
                imagesIcons[i].SetNativeSize();

                int count = Managers.inventory.getItemCount(item);
                string message = "x" + count;
                if (item == Managers.inventory.equippedItem)
                    message = "Equiped\n" + message;

                textsLables[i].text = message;
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((BaseEventData data) =>
                {
                    OnItem(item);
                });
                EventTrigger thetrigger = imagesIcons[i].GetComponent<EventTrigger>();
                thetrigger.triggers.Clear();
                thetrigger.triggers.Add(entry);
            }
            else
            {
                imagesIcons[i].gameObject.SetActive(false);
                textsLables[i].gameObject.SetActive(false);
            }
        }
        if (!itemlist.Contains(currItem))
            currItem = null;

        if (currItem == null)
        {
            currentTextLabe.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            currentTextLabe.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            if (currItem == "health")
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }
            currentTextLabe.text = currItem + ":";
        }
    }
    public void OnItem(string item)
    {
        currItem = item;
        refresh();
    }
    public void OnEquip()
    {
        Managers.inventory.EquipItem(currItem);
        refresh();
    }
    public void OnUse()
    {
        Managers.inventory.ConsumeItem(currItem);
        if (currItem == "health")
        {
            Managers.player.changeHealth(25);
        }
        refresh();
    }
}
