using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 角色类 管理装备和武器
/// </summary>
public class CharacterSlot : Slot
{

    public Equipment.EquipmentType m_equipmentType;
    public Weapon.WeaponType m_weaponType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        //鼠标右键  角色穿戴物品
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if ((transform.childCount > 0) && !InventoryManager.Instance.IsPickedItem)
            {
                ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
                Character.Instance.PutOff(itemUI);
            }
        }

        ItemUI pickedItem = InventoryManager.Instance.PickedItem;
        //left 鼠标左键
        if (eventData.button != PointerEventData.InputButton.Left) return;
        //点击卡槽不为空
        if (transform.childCount > 0)
        {
            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
            //手上物品为空  捡起
            if (!InventoryManager.Instance.IsPickedItem)
            {
                InventoryManager.Instance.PickupItem(currentItem.m_item, currentItem.m_amount);
                InventoryManager.Instance.m_changeSlot = this;
                DestroyImmediate(currentItem.gameObject);
            }
            //手上物品不为空  存放/替换
            else
            {
                if (!IsRightItem(pickedItem.m_item))
                {
                    return;
                }
                //手上的物品与卡槽内的物品id相同  存放
                if (currentItem.m_item.m_ID == InventoryManager.Instance.PickedItem.m_item.m_ID)
                {
                    if (currentItem.m_item.m_capacity > currentItem.m_amount)
                    {
                        //当前物品槽剩余的空间
                        int amountRemain = currentItem.m_item.m_capacity - currentItem.m_amount;

                        //可以完全放下
                        if (amountRemain > pickedItem.m_amount)
                        {
                            InventoryManager.Instance.PutDownItem();
                            currentItem.AddAmount(pickedItem.m_amount);
                        }
                        else
                        {
                            currentItem.AddAmount(amountRemain);
                            InventoryManager.Instance.RemoveItemByAmount(amountRemain);

                        }
                    }
                }
                //id不同  交换
                else
                {
                    //点击的物品和捡起的物品id不一致
                    Item item = currentItem.m_item;
                    int amount = currentItem.m_amount;
                    currentItem.SetItem(pickedItem.m_item, pickedItem.m_amount);
                    pickedItem.SetItem(item, amount);
                }
            }
        }
        else
        {
            //卡槽为空  手上有物品  存放
            if (InventoryManager.Instance.IsPickedItem)
            {
                if(!IsRightItem(pickedItem.m_item))
                {
                    return;
                }
                StoreItem(pickedItem.m_item, InventoryManager.Instance.PickedItem.m_amount);
                InventoryManager.Instance.PutDownItem();
            }
        }

        Character.Instance.UpdateProperty();
    }

    public bool IsRightItem(Item item)
    {
        if (item is Equipment)
        {
            if (((Equipment)(item)).m_equipType != m_equipmentType || m_equipmentType == Equipment.EquipmentType.none)
            {
                return false;
            }
        }
        else if (item is Weapon)
        {
            if (((Weapon)(item)).m_weaponType != m_weaponType || m_weaponType == Weapon.WeaponType.none)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

}
