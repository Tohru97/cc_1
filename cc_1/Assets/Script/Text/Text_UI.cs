using Febucci.UI;
using TMPro;
using UnityEngine;

public class Text_UI : MonoBehaviour
{
    private TMP_Text _textMeshPro;
    private TextAnimator_TMP _textAnimator;

    private void Awake()
    {
        _textMeshPro = GetComponent<TMP_Text>();
        _textAnimator = GetComponent<TextAnimator_TMP>();
    }

    public void ShowText(string text)
    {
        if (_textMeshPro == null)
            _textMeshPro = GetComponent<TMP_Text>();


        if(_textAnimator == null)
            _textAnimator = GetComponent<TextAnimator_TMP>();

        _textAnimator.SetText(text);
    }
}
