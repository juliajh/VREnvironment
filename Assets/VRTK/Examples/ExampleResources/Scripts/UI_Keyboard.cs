namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UI_Keyboard : MonoBehaviour
    {
        [SerializeField]
        private InputField idInput;

        public void ClickKey(string character)
        {
            idInput.text += character;
        }

        public void Backspace()
        {
            if (idInput.text.Length > 0)
            {
                idInput.text = idInput.text.Substring(0, idInput.text.Length - 1);
            }
        }


        /*private void Start()
        {
            idInput = GetComponentInChildren<InputField>();
        }*/

    }
}