﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIHolder : MonoBehaviour
{
    private List<Image> representImages = new List<Image>();
    private List<ItemType> storedTypes = new List<ItemType>();
    private BasicItem BoundItem;
    private RectTransform _rectTransform;

    private void Start()
    {
        if (!_rectTransform)
        {
            _rectTransform = this.transform.GetComponent<RectTransform>();   
        }
    }

    private void Update()
    {
        if (BoundItem)
        {
            _rectTransform.anchoredPosition =
                UIMethods.GetScreenPosViaWorldPos(UIController.Instance.MainCanvas, BoundItem.transform.position) +
                new Vector2(0, this._rectTransform.sizeDelta.y/2 + 40);
        }
    }

    private void RecalculateSize()
    {
        int imageWidth = 40;
        int padding = 5;
        int spacing = 5;
        var width = imageWidth + padding * 2;
        var height = imageWidth * representImages.Count + spacing * (representImages.Count - 1) + padding * 2;
        if (!_rectTransform)
        {
            _rectTransform = this.GetComponent<RectTransform>();
        }
        this._rectTransform.sizeDelta = new Vector2(width, height);
    }

    private void GenerateImageObject(ItemType itemType)
    {
        var obj = new GameObject();
        obj.transform.SetParent(this.transform);
        var image = obj.AddComponent<Image>();
        representImages.Add(image);
        image.sprite = UIController.Instance.SpriteItemTypeLibrary.GetSpriteBasedOnType(itemType);
    }
    
    public void InitializeUI(BasicItem item)
    {
        BoundItem = item;
        foreach (var itemType in item.requireItemTypes)
        {
            GenerateImageObject(itemType);
        }
        storedTypes = item.requireItemTypes.ToList();
        RecalculateSize();
    }
    
    public void UpdateUI(ItemType satisfiedType)
    {
        for (int i = storedTypes.Count-1; i >= 0; i--)
        {
            if (storedTypes[i] == satisfiedType)
            {
                storedTypes.RemoveAt(i);
                var image = representImages[i];
                Destroy(image.gameObject);
                representImages.RemoveAt(i);
                RecalculateSize();
            }
        }

        if (storedTypes.Count <= 0)
        {
            //使命完成
            Destroy(this.gameObject);
        }
    }
}