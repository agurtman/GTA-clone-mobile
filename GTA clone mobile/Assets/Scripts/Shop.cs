using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] List<GameObject> canvas;
    [SerializeField] List<Text> costText = new List<Text>();
    [SerializeField] GameObject currentSkin;
    public Skin[] skin;
    bool isOpen;
    PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        for (var i = 0; i < skin.Length; i++)
        {
            costText[i].text = skin[i].price.ToString() + " Gold";
        }
    }

    public void OpenShop()
    {
        if (!isOpen)
        {
            canvas[0].SetActive(false);
            canvas[1].SetActive(true);
            isOpen = true;
        }
        else
        {
            canvas[1].SetActive(false);
            canvas[0].SetActive(true);
            isOpen = false;
        }
    }

    public void BuyAmmo(int count)
    {
        if (playerController.money >= count * 2)
        {
            playerController.AddAmmo(count);
            playerController.GetMoney(-count * 2);
        }
    }

    public void BuySkin(int count)
    {
        if (playerController.money >= skin[count].price && !skin[count].isBuy)
        {
            costText[count].text = "Sold";
            currentSkin.SetActive(false);
            skin[count].skinToBuy.SetActive(true);
            currentSkin = skin[count].skinToBuy;
            skin[count].isBuy = true;
            playerController.GetMoney(-skin[count].price);
        }
        if (skin[count].isBuy)
        {
            currentSkin.SetActive(false);
            skin[count].skinToBuy.SetActive(true);
            currentSkin = skin[count].skinToBuy;
        }
    }
}

[System.Serializable]
public class Skin
{
    public GameObject skinToBuy;
    public int price;
    public bool isBuy;
}