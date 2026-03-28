using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound; //Move up and down
    [SerializeField] private AudioClip interactSound; //Choose option
    private RectTransform rect;
    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Interact();
            
    } 

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition = 0;

        //Assign the Y position of the current option to the arrow
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }
    
    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);

        //Acces the button component of the current option and call the interact function
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}