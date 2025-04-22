using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiplomaForm : MonoBehaviour
{

    public TMP_InputField nameField;
    public TMP_Text DiplomaNameTextField;
    public Button setNameButton;

    public void SetNameText()
    {
        DiplomaNameTextField.text = nameField.text;
        nameField.gameObject.SetActive(false);
        setNameButton.gameObject.SetActive(false);
    }


}
