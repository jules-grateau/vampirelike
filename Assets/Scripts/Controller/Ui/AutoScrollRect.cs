using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Add the script to your Dropdown Menu Template Object via (Your Dropdown Button > Template)

[RequireComponent(typeof(ScrollRect))]
public class AutoScrollRect : MonoBehaviour
{
    // Sets the speed to move the scrollbar
    public float scrollSpeed = 10f;

    private RectTransform m_SelectedRectTransform;

    RectTransform scrollRectTransform;
    RectTransform contentPanel;

    void Start()
    {
        scrollRectTransform = GetComponent<RectTransform>();
        contentPanel = GetComponent<ScrollRect>().content;
    }

    GameObject lastSelected;

    void Update()
    {
        UpdateScrollToSelected();
    }

    void UpdateScrollToSelected()
    {
        // Get the current selected option from the eventsystem.
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null || selected.transform.parent != contentPanel.transform || selected == lastSelected)
        {
            return;
        }


        m_SelectedRectTransform = selected.GetComponent<RectTransform>();
        float selectedPositionX = Mathf.Abs(m_SelectedRectTransform.anchoredPosition.x) + m_SelectedRectTransform.rect.width;

        // The upper bound of the scroll view is the anchor position of the content we're scrolling.
        float scrollViewMinX = contentPanel.anchoredPosition.x;
        // The lower bound is the anchor position + the height of the scroll rect.
        float scrollViewMaxX = contentPanel.anchoredPosition.x + scrollRectTransform.rect.width;

        // If the selected position is below the current lower bound of the scroll view we scroll down.
        if (selectedPositionX > scrollViewMaxX)
        {
            float newX = scrollRectTransform.rect.width - selectedPositionX;
            contentPanel.anchoredPosition = new Vector2(newX, contentPanel.anchoredPosition.y);
        } else if(selectedPositionX < scrollViewMinX)
        {
            float newX = selectedPositionX - scrollRectTransform.rect.width;
            contentPanel.anchoredPosition = new Vector2(newX, contentPanel.anchoredPosition.y);
        }

        Debug.Log(m_SelectedRectTransform.anchoredPosition);
        Debug.Log($"Selected Position X :{selectedPositionX}");
        Debug.Log($"Scroll View Min X : {scrollViewMinX}");
        Debug.Log($" Scroll View Max X : {scrollViewMaxX}");

       lastSelected = selected;
    }
}