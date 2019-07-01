using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TypeWriterEffect : MonoBehaviour
{
    [TextArea(2, 5)] public string text;
    [Range(0, 1)] public float delay;

    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
        Type();
    }

    public void Type() => StartCoroutine(TypeText());

    private IEnumerator TypeText()
    {
        _text.text = "";
        foreach (char c in text)
        {
            _text.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
