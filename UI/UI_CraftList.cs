using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// UI_CraftList 类用于管理工艺列表界面，实现 IPointerDownHandler 接口以处理鼠标按下事件
public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    // 序列化字段，用于保存工艺项目父对象的 Transform 组件
    [SerializeField] private Transform craftStotParent;
    // 序列化字段，用于保存工艺项目槽预制体
    [SerializeField] private GameObject craftSlotPrefab;

    // 序列化字段，用于保存可以制作的装备项目数据列表
    [SerializeField] private List<ItemData_Equipment> craftEquipment;
    //[SerializeField] private List<UI_CraftSlot> craftSlots;

    // Start 方法在脚本实例化时调用，初始化工艺列表并设置默认的工艺窗口
    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
        //AssignCraftSlots();
    }

    // // AssignCraftSlots 方法用于将所有子对象的 UI_CraftSlot 组件添加到 craftSlots 列表中
    // private void AssignCraftSlots()
    // {
    //     for (int i = 0; i < craftStotParent.childCount; i++)
    //     {
    //         craftSlots.Add(craftStotParent.GetChild(i).GetComponent<UI_CraftSlot>());
    //     }
    // }

    // SetupCraftList 方法用于设置工艺列表，销毁旧的槽对象并创建新的槽对象
    public void SetupCraftList()
    {
        for (int i = 0; i < craftStotParent.childCount; i++)//craftSlots.Count
        {
            Destroy(craftStotParent.GetChild(i).gameObject);
            //Destroy(craftSlots[i].gameObject);
        }

        //craftSlots = new List<UI_CraftSlot>();

        for(int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftStotParent);
            //craftSlots.Add(newSlot.GetComponent<UI_CraftSlot>());
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    // OnPointerDown 方法在鼠标按下时调用，设置工艺列表
    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    // SetupDefaultCraftWindow 方法用于设置默认的工艺窗口显示第一个可制作的装备项目
    public void SetupDefaultCraftWindow()
    {
        if(craftEquipment[0]!= null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}