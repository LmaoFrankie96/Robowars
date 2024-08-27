using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputNavigator : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        NavigateInput();
    }

    private void NavigateInput()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                    if (selectable != null)
                        selectable.Select();
                }
            }
            else
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                {
                    Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                    if (selectable != null)
                        selectable.Select();
                }
            }
        }
    }
}
