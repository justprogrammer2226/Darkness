using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TypeWriterEffect : MonoBehaviour
{
    [Range(0, 1)] public float delay;

    private Text _text;
    private string _message;

    private void Awake()
    {
        _text = GetComponent<Text>();
        _message = _text.text;
        _text.text = "";
        StartCoroutine(PlayText());
    }

    private IEnumerator PlayText()
    {
        Debug.Log("Старт");
        foreach (char c in _message)
        {
            _text.text += c;
            yield return new WaitForSeconds(delay);
        }
        Debug.Log("Конец");
    }
}
