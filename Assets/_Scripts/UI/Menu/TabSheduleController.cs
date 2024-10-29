using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabSheduleController : MonoBehaviour
{
    private List<TabInfoWindowController> _tabs;

    private void Start()
    {
        _tabs = GetComponentsInChildren<TabInfoWindowController>().ToList();
        foreach(var tab in _tabs)
        {
            tab.TabClicked += OnTabSelected;
        }
        OnTabSelected(_tabs[0]);
    }

    private void OnTabSelected(TabInfoWindowController tab)
    {
        foreach(var t in _tabs)
        {
            t.Hide();
        }
        tab.Show();

    }


}
