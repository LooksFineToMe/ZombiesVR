using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VR_InputModule : BaseInputModule
{
    public Camera m_Camera;
    public SteamVR_Input_Sources m_TargetSource;
    public SteamVR_Action_Boolean m_ClickAction;

    private GameObject m_CurrentObject = null;
    private PointerEventData m_data = null;

    protected override void Awake()
    {
        base.Awake();

        m_data = new PointerEventData(eventSystem);
    }
    public override void Process()
    {
        //reset data, set camera
        m_data.Reset();
        m_data.position = new Vector2(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);

        //raycast
        eventSystem.RaycastAll(m_data, m_RaycastResultCache);
        m_data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_data.pointerCurrentRaycast.gameObject;

        //clear raycast
        m_RaycastResultCache.Clear();

        //hoverstate
        HandlePointerExitAndEnter(m_data, m_CurrentObject);

        //press
        if (m_ClickAction.GetStateDown(m_TargetSource))
        {
            ProcessPress(m_data);
        }
        if (m_ClickAction.GetStateUp(m_TargetSource))
        {
            ProcessRelease(m_data);
        }
        //release

    }
    public PointerEventData GetData()
    {
        return m_data;
    }
    private void ProcessPress(PointerEventData data)
    {
        //set raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        //check for object get the down handler, call
        GameObject newPointerpress = ExecuteEvents.ExecuteHierarchy(m_CurrentObject, data, ExecuteEvents.pointerDownHandler);

        //if no down handler, try and get click handler
        if (newPointerpress == null)
        {
            newPointerpress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);
        }

        //set data
        data.pressPosition = data.position;
        data.pointerPress = newPointerpress;
        data.rawPointerPress = m_CurrentObject;


    }
    private void ProcessRelease(PointerEventData data)
    {
        //execute pointer up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        //check for click handler
        GameObject pointerUphandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);

        //check if actual
        if (data.pointerPress == pointerUphandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        //clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        //reset data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
